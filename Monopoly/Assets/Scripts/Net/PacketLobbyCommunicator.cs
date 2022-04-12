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

    public class PacketLobbyCommunicator : MonoBehaviour
    {

        public delegate void PacketDelegate<T>(T packet);

        public event PacketDelegate<PacketException>            OnError;
        public event PacketDelegate<PacketLaunchGame>           OnGameStart;
        public event PacketDelegate<PacketBroadcastUpdateLobby> OnLobbyUpdate;

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
            case PacketLaunchGame packet:
                OnGameStart(packet); break;
            case PacketBroadcastUpdateLobby packet:
                OnLobbyUpdate(packet); break;
            }
        }

    }

}
