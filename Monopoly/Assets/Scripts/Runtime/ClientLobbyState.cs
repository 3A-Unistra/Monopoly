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

        // the following fields are passed through to the game state for use
        // in both the WebGL and binary versions of the game
        public static string clientUUID;
        public static string clientUsername;
        public static int clientPiece;
        public static string token;
        public static string currentLobby;
        public static string address;
        public static int port;
        public static ConnectMode connectMode;
        public static bool secureMode = false;

        public static string desiredClientUsername;

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
            if (current == this)
                current = null;
        }

        public void Crash()
        {
            /* close all menus and return to main */
            // TODO: show error?
            if (MenuLobby.current != null)
                Destroy(MenuLobby.current.gameObject);
            if (MenuCreate.current != null)
                Destroy(MenuCreate.current.gameObject);

            GameObject mainMenu =
                Instantiate(RuntimeData.current.MainMenuPrefab, Canvas.transform);
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
                PacketSocket.CreateSocket(address, port, par, null, secureMode);
            socket.Connect();
            // wait for the socket to open or die
            yield return new WaitUntil(delegate
            {
                return socket.HasError() ||
                       socket.HasTLSError() ||
                       socket.IsOpen();
            });
            MenuConnect connectConnector = null;
            MenuLogin loginConnector = null;
            if (mode == ConnectMode.BYIP)
            {
                connectConnector = (MenuConnect) connector;
                ClientLobbyState.token = userId;
                ClientLobbyState.clientUsername = userId;
            }
            else
            {
                loginConnector = (MenuLogin) connector;
                ClientLobbyState.token = token;
                ClientLobbyState.clientUsername = userId;
            }
            if (socket.HasError() || socket.HasTLSError())
            {
                string err = socket.HasTLSError() ?
                    "connection_tls" : "connection_fail";
                if (mode == ConnectMode.BYIP)
                    connectConnector.DisplayError(err);
                else
                    loginConnector.DisplayError(err);
                Debug.LogWarning("Error occured opening lobby state!");
                Destroy(this.gameObject);
                yield break;
            }
            RegisterSocket(socket, userId);
            ClientLobbyState.address = address;
            ClientLobbyState.port = port;

            UIDirector.IsMenuOpen = false;
            if (mode == ConnectMode.BYIP)
                lobbyInstance = Instantiate(RuntimeData.current.LobbyMenuPrefab,
                                            connector.transform.parent);
            else
                lobbyInstance = Instantiate(RuntimeData.current.LobbyMenuPrefab,
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
            comm.DoCreateGame(playerId, desiredClientUsername, maxPlayers,
                              password, gameName, privateGame, startBalance,
                              auctions, doubleGo, turnTime, maxRounds,
                              canBuyFirstCircle);
        }

        public void DoJoinGame(string lobbyToken, string password)
        {

            comm.DoEnterRoom(lobbyToken, desiredClientUsername, password);
        }

        public void DoLeaveGame(string lobbyToken)
        {
            comm.DoLeaveRoom(clientUUID);
        }
        
        public void DoRoomModify(string lobbyName, int nbPlayers, 
            int maxPlayers, List<PacketStatusInternal> players, bool auction, 
            bool doubleOnGo, bool buying, int maxTurns, 
            int timeout, int balance)
        {
            if (comm != null)
                comm.DoRoomModify(lobbyName, nbPlayers, maxPlayers, players, 
                    auction, doubleOnGo, buying, maxTurns, timeout, balance);
        }

        public void OnError(PacketException packet)
        {
            // TODO: don't return to main menu for less fatal errors
            UIDirector.IsMenuOpen = false;
            UIDirector.IsUIBlockingNet = true;
            GameObject mainMenu = Instantiate(RuntimeData.current.MainMenuPrefab, Canvas.transform);
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
            GameObject CreateMenu = Instantiate(RuntimeData.current.CreateMenuPrefab,
                                                Canvas.transform);
            MenuCreate menuScript = CreateMenu.GetComponent<MenuCreate>();
            menuScript.UpdateFields(true, clientUUID);
            menuScript.EnableEdits(true);
            menuScript.CurrentHost = clientUUID;
            currentLobby = packet.GameToken;
            clientPiece = packet.PieceId;
            clientUsername = packet.Username;
            if (MenuLobby.current != null)
                Destroy(MenuLobby.current.gameObject);
        }

        public void OnEnterRoomSucceed(PacketEnterRoomSucceed packet)
        {
            UIDirector.IsMenuOpen = false;
            UIDirector.IsUIBlockingNet = true;
            clientUsername = packet.Username;
            GameObject CreateMenu =
                Instantiate(RuntimeData.current.CreateMenuPrefab,
                            Canvas.transform);
            MenuCreate createScript = CreateMenu.GetComponent<MenuCreate>();
            createScript.UpdateFields(false, packet.HostId);
            createScript.EnableEdits(false);
            currentLobby = packet.LobbyToken;
            if (MenuLobby.current != null)
                Destroy(MenuLobby.current.gameObject);
        }

        public void OnLeaveRoomSucceed(PacketLeaveRoomSucceed packet)
        {
            UIDirector.IsMenuOpen = false;
            UIDirector.IsUIBlockingNet = true;
            GameObject lobbyMenu =
                Instantiate(RuntimeData.current.LobbyMenuPrefab,
                            Canvas.transform);
            currentLobby = null;
            if (MenuCreate.current != null)
                Destroy(MenuCreate.current.gameObject);
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
            yield return new WaitUntilForSeconds(
                () => MenuCreate.current != null, 0.5f);
            if (MenuCreate.current == null)
                yield break;
            MenuCreate.current.EnableEdits(false, false);
            MenuCreate.current.UpdateParticipants(packet.Players);
            MenuCreate.current.SetName(packet.GameName);
            //MenuCreate.current.SetPrivacy();
            MenuCreate.current.SetAuctionSwitch(packet.EnableAuctions);
            //MenuCreate.current.SetBotsNumber(packet.b);
            MenuCreate.current.SetBuyingSwitch(packet.EnableFirstTourBuy);
            MenuCreate.current.SetNbTurns(packet.MaxRounds);
            MenuCreate.current.SetPlayerNumber(packet.MaxNumberPlayers);
            MenuCreate.current.SetStartingBalance(packet.StartingBalance);
            MenuCreate.current.SetTurnTime(packet.TurnTimeout);
            MenuCreate.current.SetDoubleOnStartSwitch(packet.EnableDoubleOnGo);
            MenuCreate.current.EnableEdits(true, true);
            MenuCreate.current.UpdateFields(null);
        }

        public void OnNewHost(PacketNewHost packet)
        {
            if (MenuCreate.current != null)
            {
                MenuCreate.current.UpdateFields(packet.PlayerId.Equals(clientUUID),
                                                packet.PlayerId);
                MenuCreate.current.EnableEdits(packet.PlayerId.Equals(clientUUID));
            }
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
