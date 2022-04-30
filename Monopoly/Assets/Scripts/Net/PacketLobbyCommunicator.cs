/*
 * PacketLobbyCommunicator.cs
 * Asynchronous packet communication handler for the lobby.
 * 
 * Date created : 12/04/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Monopoly.Net.Packets;
using Monopoly.Runtime;

namespace Monopoly.Net
{

    public class PacketLobbyCommunicator
    {

        public delegate void PacketDelegate<T>(T packet);

        public event PacketDelegate<PacketException>            OnError;
        public event PacketDelegate<PacketAppletPrepare>        OnAppletPrepare;
        public event PacketDelegate<PacketCreateGameSucceed>
                                                         OnCreateGameSucceed;
        public event PacketDelegate<PacketEnterRoomSucceed>
                                                         OnEnterRoomSucceed;
        public event PacketDelegate<PacketLeaveRoomSucceed>
                                                         OnLeaveRoomSucceed;
        public event PacketDelegate<PacketLaunchGame>           OnGameStart;
        public event PacketDelegate<PacketBroadcastUpdateLobby> OnLobbyUpdate;
        public event PacketDelegate<PacketBroadcastUpdateRoom>  OnRoomUpdate;
        public event PacketDelegate<PacketBroadcastNewRoomToLobby>
                                                         OnBroadcastCreateGame;
        public event PacketDelegate<PacketStatusRoom>
                                                         OnRoomModify;
        public event PacketDelegate<PacketNewHost>
                                                         OnNewHost;

        private PacketSocket socket;

        public PacketLobbyCommunicator(PacketSocket socket)
        {
            if (socket == null)
            {
                throw new System.NullReferenceException(
                    "Packet communicator instantiated with null socket!");
            }
            this.socket = socket;
            socket.Sock.OnMessage += (data) => ReceivePacket(data);
        }

        public void DoLaunchGame()
        {
            PacketLaunchGame packet = new PacketLaunchGame();
            SendPacket(packet);
        }

        public void DoCreateGame(string playerId, int maxPlayers,
                                 string password, string gameName,
                                 bool privateGame, int startBalance,
                                 bool auctions, bool doubleGo,
                                 int turnTime, int maxRounds,
                                 bool canBuyFirstCircle)
        {
            PacketCreateGame packet =
                new PacketCreateGame(playerId, maxPlayers, password, gameName,
                                     privateGame, startBalance, auctions,
                                     doubleGo, turnTime, maxPlayers,
                                     canBuyFirstCircle);
            SendPacket(packet);
        }

        public void DoEnterRoom(string lobbyToken, string password)
        {
            PacketEnterRoom packet =
                new PacketEnterRoom(lobbyToken, password);
            SendPacket(packet);
        }

        public void DoLeaveRoom(string playerId)
        {
            PacketLeaveRoom packet = new PacketLeaveRoom(playerId);
            SendPacket(packet);
        }

        private async void SendPacket(Packet packet)
        {
#if UNITY_EDITOR
            Debug.Log("WebSocket send: " + packet.Serialize());
#endif
            try
            {
                await socket.Sock.SendText(packet.Serialize());
            }
            catch (System.Exception)
            {
                //Debug.LogException(e);
                Debug.LogWarning("WebSocket died. Will now quit the lobby...");
                ClientLobbyState.current.Crash();
            }
        }

        private void ReceivePacket(byte[] data)
        {
            if (data == null)
            {
                Debug.LogWarning("Received null packet?");
                return;
            }
            string stringData = System.Text.Encoding.UTF8.GetString(data);
            Packet p = Packet.Deserialize(stringData);
            if (p == null)
                return;
            switch (p)
            {
            case PacketException packet:
                OnError(packet); break;
            case PacketAppletPrepare packet:
                OnAppletPrepare(packet); break;
            case PacketCreateGameSucceed packet:
                OnCreateGameSucceed(packet); break;
            case PacketEnterRoomSucceed packet:
                OnEnterRoomSucceed(packet); break;
            case PacketLeaveRoomSucceed packet:
                OnLeaveRoomSucceed(packet); break;
            case PacketLaunchGame packet:
                OnGameStart(packet); break;
            case PacketBroadcastUpdateLobby packet:
                OnLobbyUpdate(packet); break;
            case PacketBroadcastUpdateRoom packet:
                OnRoomUpdate(packet); break;
            case PacketBroadcastNewRoomToLobby packet:
                OnBroadcastCreateGame(packet); break;
            case PacketStatusRoom packet:
                OnRoomModify(packet); break;
            case PacketNewHost packet:
                OnNewHost(packet); break;
            }
        }

    }

}
