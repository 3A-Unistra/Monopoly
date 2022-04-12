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

        public IEnumerator ConnectWithPort(string address, int port,
                                           string token,
                                           MonoBehaviour connector,
                                           ConnectMode mode)
        {
            StartCoroutine(Connect(string.Format("{0}:{1}", address, port),
                           token, connector, mode));
            yield break;
        }

        public IEnumerator Connect(string loc, string token,
                                   MonoBehaviour connector, ConnectMode mode)
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
            MenuConnect connectConnector = null;
            MainMenu mainConnector = null;
            if (mode == ConnectMode.BYIP)
                connectConnector = (MenuConnect) connector;
            else
                mainConnector = (MainMenu) connector;
            if (socket.HasError())
            {
                if (mode == ConnectMode.BYIP)
                    connectConnector.DisplayError("connection_fail");
                else
                    mainConnector.DisplayError("connection_fail");
                Debug.LogWarning("Error occured opening lobby state!");
                Destroy(this);
                yield break;
            }
            RegisterSocket(sock);

            UIDirector.IsMenuOpen = false;
            if (mode == ConnectMode.BYIP)
                lobbyInstance = Instantiate(connectConnector.LobbyMenuPrefab,
                                            connector.transform.parent);
            else
                lobbyInstance = Instantiate(mainConnector.LobbyMenuPrefab,
                                            connector.transform.parent);
            Destroy(connector.gameObject);
        }

        public void RegisterSocket(PacketSocket sock)
        {
            this.sock = sock;
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
