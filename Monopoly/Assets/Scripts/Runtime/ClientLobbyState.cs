/*
 * ClientLobbyState.cs
 * Client lobby state handler.
 * 
 * Date created : 11/04/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Monopoly.Net;
using Monopoly.Net.Packets;
using Monopoly.UI;

namespace Monopoly.Runtime
{

    public class ClientLobbyState : MonoBehaviour
    {

        public static ClientLobbyState current;

        public GameObject Canvas;
        public GameObject MainMenuPrefab;

        // TODO: pass around
        private string clientUUID;
        private string token;

        private PacketLobbyCommunicator comm;
        private PacketSocket sock;

        private GameObject lobbyInstance;

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

        void OnDestroy()
        {
            if (sock != null)
                sock.Close();
        }

        // TODO: CARRY ON THE USER ID TO GAME STATE, STATIC????
        public IEnumerator Connect(string address, int port,
                                   string userId, string token,
                                   MonoBehaviour connector, ConnectMode mode)
        {
            if (address == null)
            {
                Debug.LogWarning("Cannot load lobby state from null url!");
                Destroy(this.gameObject);
                yield break;
            }
            else if (port < 0 || port >= 65536)
            {
                Debug.LogWarning("Cannot load lobby state from invalid port!");
                Destroy(this.gameObject);
                yield break;
            }
            else if (token == null && mode == ConnectMode.ONLINE)
            {
                Debug.LogWarning("Cannot load lobby state from null token!");
                Destroy(this.gameObject);
                yield break;
            }
            // let stuff in the scene load, then run this code:
            Dictionary<string, string> par = new Dictionary<string, string>();
            if (mode == ConnectMode.ONLINE)
                par.Add("token", token);
            else
                par.Add("token", userId);
            PacketSocket socket =
                PacketSocket.CreateSocket(address, port, par, true, false);
            socket.Connect();
            // wait for the socket to open or die
            yield return new WaitUntil(delegate
            {
                return socket.HasError() || socket.IsOpen();
            });
            MenuConnect connectConnector = null;
            MenuLogin loginConnector = null;
            if (mode == ConnectMode.BYIP)
                connectConnector = (MenuConnect) connector;
            else
                loginConnector = (MenuLogin) connector;
            if (socket.HasError())
            {
                if (mode == ConnectMode.BYIP)
                    connectConnector.DisplayError("connection_fail");
                else
                    loginConnector.DisplayError("connection_fail");
                Debug.LogWarning("Error occured opening lobby state!");
                Destroy(this.gameObject);
                yield break;
            }
            RegisterSocket(socket, userId, token);

            UIDirector.IsMenuOpen = false;
            if (mode == ConnectMode.BYIP)
                lobbyInstance = Instantiate(connectConnector.LobbyMenuPrefab,
                                            connector.transform.parent);
            else
                lobbyInstance = Instantiate(loginConnector.LobbyMenuPrefab,
                                            connector.transform.parent);
            Destroy(connector.gameObject);
        }

        public void RegisterSocket(PacketSocket sock, string uuid, string token)
        {
            this.sock = sock;
            this.clientUUID = uuid;
            this.token = token;
            comm = new PacketLobbyCommunicator(sock);
            comm.OnError += OnError;
        }

        public void OnError(PacketException packet)
        {
            // TODO: implement webgl
#if UNITY_WEBGL
#else
            GameObject mainMenu = Instantiate(MainMenuPrefab, Canvas.transform);
            MainMenu menuScript = mainMenu.GetComponent<MainMenu>();
            menuScript.DisplayError(string.Format("error_{0}", packet.Code));
            if (lobbyInstance != null)
                Destroy(lobbyInstance.gameObject);
            Destroy(gameObject);
#endif
        }

    }

}
