using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Monopoly.Net;
using Monopoly.UI;

namespace Monopoly.Runtime
{

    public class ClientLobbyState : MonoBehaviour
    {

        public static ClientLobbyState current;

        private string loc;
        private string token;

        private PacketCommunicator comm;
        private PacketSocket sock;

        public enum ConnectMode
        {
            ONLINE, BYIP
        }

        void Awake()
        {
            if (current != null)
            {
                Debug.LogError("Cannot create two concurrent lobby states!");
                Destroy(this);
            }
            current = this;
            Debug.Log("Initialised lobby state.");
        }

        public IEnumerator Connect(string loc, string token, MenuConnect connector,
                                   ConnectMode mode)
        {
            if (loc == null)
            {
                Debug.LogWarning("Cannot load lobby state from null url!");
                Destroy(this);
                yield break;
            }
            else if (token == null)
            {
                Debug.LogWarning("Cannot load lobby state from null token!");
                Destroy(this);
                yield break;
            }
            // let stuff in the scene load, then run this code:
            Dictionary<string, string> par = new Dictionary<string, string>();
            par.Add("token", token);
            PacketSocket socket = PacketSocket.CreateSocket(loc, par, true, false);
            socket.Connect();
            // wait for the socket to open or die
            yield return new WaitUntil(delegate
            {
                return socket.HasError() || socket.IsOpen();
            });
            if (socket.HasError())
            {
                connector.DisplayError("connection_fail");
                Debug.LogWarning("Error occured opening lobby state!");
                Destroy(this);
                yield break;
            }
            sock = socket;
            comm = new PacketCommunicator(sock);
            GameObject lobbyMenu = Instantiate(connector.LobbyMenuPrefab,
                                               transform.parent);
            Destroy(connector.gameObject);
        }

        void OnDestroy()
        {
            if (sock != null)
                sock.Close();
        }

    }

}
