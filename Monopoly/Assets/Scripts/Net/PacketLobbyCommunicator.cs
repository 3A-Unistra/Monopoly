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

namespace Monopoly.Net
{

    public class PacketLobbyCommunicator
    {

        public delegate void PacketDelegate<T>(T packet);

        public event PacketDelegate<PacketException>            OnError;
        public event PacketDelegate<PacketCreateGameSucceed>
                                                         OnCreateGameSucceed;
        public event PacketDelegate<PacketLaunchGame>           OnGameStart;
        public event PacketDelegate<PacketBroadcastUpdateLobby> OnLobbyUpdate;
        public event PacketDelegate<PacketBroadcastNewRoomToLobby>
                                                         OnBroadcastCreateGame;

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

        private async void SendPacket(Packet packet)
        {
            await socket.Sock.SendText(packet.Serialize());
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
                return; /* TODO: Error? */
            switch (p)
            {
            case PacketException packet:
                OnError(packet); break;
            case PacketCreateGameSucceed packet:
                OnCreateGameSucceed(packet); break;
            case PacketLaunchGame packet:
                OnGameStart(packet); break;
            case PacketBroadcastUpdateLobby packet:
                OnLobbyUpdate(packet); break;
            case PacketBroadcastNewRoomToLobby packet:
                OnBroadcastCreateGame(packet); break;
            }
        }

    }

}
