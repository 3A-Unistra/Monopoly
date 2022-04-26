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
        public GameObject CreateMenuPrefab;
        public GameObject LobbyMenuPrefab;

        // the following fields are passed through to the game state for use
        // in both the WebGL and binary versions of the game
        public static string clientUUID;
        public static string clientUsername;
        public static string token;
        public static string currentLobby;
        public static string address;
        public static int port;
        public static ConnectMode connectMode;

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
            token = null;
            current = this;
            UIDirector.IsUIBlockingNet = false;
            Debug.Log("Initialised lobby state.");
        }

        void OnDestroy()
        {
            //MenuLobby.lobbyElements.Clear();
            if (sock != null)
                sock.Close();
        }

        public void Crash()
        {
            /* close all menus and return to main */
            // TODO: show error?
            if (MenuLobby.current != null)
                Destroy(MenuLobby.current.gameObject);
            if (MenuCreate.current != null)
                Destroy(MenuCreate.current.gameObject);

            GameObject mainMenu = Instantiate(MainMenuPrefab, Canvas.transform);
        }

        void Update()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            if (sock != null && !UIDirector.IsUIBlockingNet)
            {
                sock.Sock.DispatchMessageQueue();
            }
#endif
        }

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
                PacketSocket.CreateSocket(address, port, par, null, false);
            socket.Connect();
            // wait for the socket to open or die
            yield return new WaitUntil(delegate
            {
                return socket.HasError() || socket.IsOpen();
            });
            MenuConnect connectConnector = null;
            MenuLogin loginConnector = null;
            if (mode == ConnectMode.BYIP)
            {
                connectConnector = (MenuConnect)connector;
                ClientLobbyState.token = userId;
                ClientLobbyState.clientUsername = userId;
            }
            else
            {
                loginConnector = (MenuLogin) connector;
                ClientLobbyState.token = token;
                // TODO: CHANGE TO THE ACTUAL USERNAME
                ClientLobbyState.clientUsername = userId;
            }
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
            RegisterSocket(socket, userId);
            ClientLobbyState.address = address;
            ClientLobbyState.port = port;

            UIDirector.IsMenuOpen = false;
            if (mode == ConnectMode.BYIP)
                lobbyInstance = Instantiate(connectConnector.LobbyMenuPrefab,
                                            connector.transform.parent);
            else
                lobbyInstance = Instantiate(loginConnector.LobbyMenuPrefab,
                                            connector.transform.parent);
            Destroy(connector.gameObject);
        }

        public void RegisterSocket(PacketSocket sock, string uuid)
        {
            this.sock = sock;
            ClientLobbyState.clientUUID = uuid;
            comm = new PacketLobbyCommunicator(sock);
            comm.OnError += OnError;
            comm.OnCreateGameSucceed += OnCreateGameSucceed;
            comm.OnEnterRoomSucceed += OnEnterRoomSucceed;
            comm.OnLeaveRoomSucceed += OnLeaveRoomSucceed;
            comm.OnDeleteRoomSucceed += OnDeleteRoomSucceed;
            comm.OnBroadcastCreateGame += OnBroadcastCreateGame;
            comm.OnLobbyUpdate += OnLobbyUpdate;
            comm.OnRoomUpdate += OnRoomUpdate;
            comm.OnAppletPrepare += OnAppletPrepare;
            comm.OnRoomModify += OnRoomModify;
            comm.OnNewHost += OnNewHost;
        }

        public void DoLaunchGame()
        {
            comm.DoLaunchGame();
        }

        public void DoCreateGame(string playerId, int maxPlayers,
                                 string password, string gameName,
                                 bool privateGame, int startBalance,
                                 bool auctions, bool doubleGo,
                                 int turnTime, int maxRounds,
                                 bool canBuyFirstCircle)
        {
            comm.DoCreateGame(playerId, maxPlayers, password, gameName,
                              privateGame, startBalance, auctions,
                              doubleGo, turnTime, maxRounds,
                              canBuyFirstCircle);
        }

        public void DoJoinGame(string lobbyToken, string password)
        {

            comm.DoEnterRoom(lobbyToken, password);
        }

        public void DoLeaveGame(string lobbyToken)
        {
            comm.DoLeaveRoom(clientUUID);
            /* leave lobby immediately because there's no point forcing the user
               to wait for the succeed packet */
            /*UIDirector.IsMenuOpen = false;
            GameObject lobbyMenu = Instantiate(LobbyMenuPrefab, Canvas.transform);
            currentLobby = null;
            if (MenuCreate.current != null)
                Destroy(MenuCreate.current.gameObject);*/
        }

        public void DoDeleteGame(string lobbyToken)
        {
            comm.DoDeleteRoom(clientUUID);
            /* leave lobby immediately because there's no point forcing the user
               to wait for the succeed packet */
            UIDirector.IsMenuOpen = false;
            UIDirector.IsUIBlockingNet = false;
            GameObject lobbyMenu = Instantiate(LobbyMenuPrefab, Canvas.transform);
            currentLobby = null;
            if (MenuCreate.current != null)
                Destroy(MenuCreate.current.gameObject);
        }

        public void OnError(PacketException packet)
        {
            // TODO: don't return to main menu for less fatal errors
            UIDirector.IsMenuOpen = false;
            UIDirector.IsUIBlockingNet = true;
            GameObject mainMenu = Instantiate(MainMenuPrefab, Canvas.transform);
            MainMenu menuScript = mainMenu.GetComponent<MainMenu>();
            menuScript.DisplayError(string.Format("error_{0}", packet.Code));
            if (lobbyInstance != null)
                Destroy(lobbyInstance.gameObject);
            Destroy(gameObject);
        }

        public void OnCreateGameSucceed(PacketCreateGameSucceed packet)
        {
            UIDirector.IsMenuOpen = false;
            UIDirector.IsUIBlockingNet = true;
            GameObject CreateMenu = Instantiate(CreateMenuPrefab,
                                                Canvas.transform);
            MenuCreate menuScript = CreateMenu.GetComponent<MenuCreate>();
            menuScript.UpdateFields(true);
            currentLobby = packet.GameToken;
            if (MenuLobby.current != null)
                Destroy(MenuLobby.current.gameObject);
        }

        public void OnEnterRoomSucceed(PacketEnterRoomSucceed packet)
        {
            UIDirector.IsMenuOpen = false;
            UIDirector.IsUIBlockingNet = true;
            GameObject CreateMenu = Instantiate(CreateMenuPrefab, Canvas.transform);
            MenuCreate createScript = CreateMenu.GetComponent<MenuCreate>();
            createScript.UpdateFields(false);
            currentLobby = packet.LobbyToken;
            if (MenuLobby.current != null)
                Destroy(MenuLobby.current.gameObject);
        }

        public void OnLeaveRoomSucceed(PacketLeaveRoomSucceed packet)
        {
            UIDirector.IsMenuOpen = false;
            UIDirector.IsUIBlockingNet = true;
            GameObject lobbyMenu = Instantiate(LobbyMenuPrefab, Canvas.transform);
            currentLobby = null;
            if (MenuCreate.current != null)
                Destroy(MenuCreate.current.gameObject);
        }

        public void OnDeleteRoomSucceed(PacketDeleteRoomSucceed packet)
        {
            /*UIDirector.IsMenuOpen = false;
            GameObject lobbyMenu = Instantiate(LobbyMenuPrefab, Canvas.transform);
            currentLobby = null;
            if (MenuCreate.current != null)
                Destroy(MenuCreate.current.gameObject);*/
        }

        public void OnBroadcastCreateGame(PacketBroadcastNewRoomToLobby packet)
        {
            if (MenuLobby.current != null)
                MenuLobby.current.CreateLobbyButton(
                    packet.GameName, packet.LobbyToken,
                    packet.NumberPlayers, packet.MaxPlayers, true);
        }

        public void OnLobbyUpdate(PacketBroadcastUpdateLobby packet)
        {
            if (MenuLobby.current != null)
                MenuLobby.current.UpdateLobby(packet);
        }

        public void OnRoomUpdate(PacketBroadcastUpdateRoom packet)
        {
            if (MenuCreate.current != null)
                MenuCreate.current.UpdateLobby(packet);
        }

        public void OnRoomModify(PacketStatusRoom packet)
        {
            StartCoroutine(OnRoomModifyEnumerator(packet));
        }

        private IEnumerator OnRoomModifyEnumerator(PacketStatusRoom packet)
        {
            // this packet comes way too fast so we need to do a little delay
            // to allow the menu to open. if it still doesn't load in time then
            // tough shit, your computer is a potato
            yield return new WaitForSeconds(0.5f);
            // FIXME: fix the updates and let them be sent off too!!!!!
            if (MenuCreate.current == null)
                yield break;
            foreach (string username in packet.Players)
            {
                MenuCreate.current.ManagePlayerList(
                    PacketBroadcastUpdateRoom.UpdateReason.NEW_PLAYER,
                    username);
            }
            MenuCreate.current.SetName(packet.GameName);
            //MenuCreate.current.SetPrivacy();
            MenuCreate.current.SetAuctionSwitch(packet.EnableAuctions);
            //MenuCreate.current.SetBotsNumber(packet.);
            MenuCreate.current.SetBuyingSwitch(packet.EnableFirstTourBuy);
            MenuCreate.current.SetNbTurns(packet.MaxRounds);
            MenuCreate.current.SetPlayerNumber(packet.MaxNumberPlayers);
            MenuCreate.current.SetStartingBalance(packet.StartingBalance);
            MenuCreate.current.SetTurnTime(packet.TurnTimeout);
            MenuCreate.current.SetDoubleOnStartSwitch(packet.EnableDoubleOnGo);
        }

        public void OnNewHost(PacketNewHost packet)
        {
            if (MenuCreate.current != null)
                MenuCreate.current.UpdateFields(packet.PlayerId.Equals(clientUUID));
        }

        public void OnAppletPrepare(PacketAppletPrepare packet)
        {
            /* lobby handler is finished, lets quit and start the game socket */
            if (sock != null)
                sock.Close();
            UIDirector.IsMenuOpen = false;
            LoadHandler.LoadScene("Scenes/BoardScene");
        }

    }

}
