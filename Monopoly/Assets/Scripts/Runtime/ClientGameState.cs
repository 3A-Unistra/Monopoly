/*
 * ClientGameState.cs
 * Client game state handler.
 * 
 * Date created : 15/03/2022
 * Author       : Finn Rayment <rayment@etu.unistra.fr>
 *              : Rayan Marmar <rayan.marmar@etu.unistra.fr>
 *              : Christophe Pierson <christophe.pierson@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Monopoly.Classes;
using Monopoly.Graphics;
using Monopoly.Net;
using Monopoly.Net.Packets;
using Monopoly.UI;
using Monopoly.Util;

namespace Monopoly.Runtime
{

    public class ClientGameState : MonoBehaviour
    {

        public static ClientGameState current;
        public static List<Dictionary<string, int>> squareData;
        public static List<Dictionary<string, int>> cardData;

        public GameObject MainMenuPrefab;
        public GameObject ExchangePrefab;
        public GameObject AuctionPrefab;

        public GameObject[] piecePrefabs;
        public GameObject boardObject;

        public TMP_Text chatBox;
        public ScrollRect chatScroller;
        public UIPlayerInfo playerInfo;

        public TMP_Text parkingMoneyText;
        private int parkingMoney;

        public TMP_Text actionText;
        private Coroutine actionEnumeration;

        public TokenCard tokenCard;

        public Board Board { get; private set; }
        public List<Player> players;
        private List<PlayerPiece> playerPieces;
        public Player myPlayer;
        private Player playerTurn;
        private bool playerDoneMove;

        private string clientUUID;
        private string token;

        public bool CanRollDice { get; set; }
        public bool CanPerformAction { get; set; }

        public static bool IsMenuOpen = false;

        private bool isGameStart = false;
        private bool isFirstGameStartThrow = true;

        private PacketCommunicator comm;
        private PacketSocket sock;

        public Button actionEndButton;
        public Button rollDiceButton;
        public Button exchangeButton;
        public Button buyPropertyButton;
        public Button auctionButton;
        public Button exitPrisonMoneyButton;
        public Button exitPrisonCardButton;

        public Canvas canvas;
        public Canvas canvasPause;

        public GameObject waitBlock;
        public TMP_Text waitText;

        public Timeout timeout;
        private Dictionary<string, int> timeouts;

        private MenuExchange currentExchange;
        private MenuAuction currentAuction;

        void Awake()
        {
            if (current != null)
            {
                Debug.LogError("Cannot create two concurrent gamestates!");
                Destroy(this);
            }
            current = this;
            currentExchange = null;
            actionEnumeration = null;

            squareData =
                JsonLoader.LoadJsonAsset<List<Dictionary<string, int>>>
                ("Data/squares");
            cardData =
                JsonLoader.LoadJsonAsset<List<Dictionary<string, int>>>
                ("Data/cards");

            InitGame();
            Debug.Log("Initialised gamestate.");
        }
        
        void Start()
        {
            // let stuff in the scene load, then run this code:
            StartCoroutine(OpenGameSocket());

            HideAllInteractButtons();
            rollDiceButton.onClick.AddListener(DoRollDice);
            buyPropertyButton.onClick.AddListener(DoBuyProperty);
            auctionButton.onClick.AddListener(DoAuction);
            actionEndButton.onClick.AddListener(DoActionEnd);
            exchangeButton.onClick.AddListener(DoExchange);
            exitPrisonMoneyButton.onClick.AddListener(DoExitPrisonMoney);
            exitPrisonCardButton.onClick.AddListener(DoExitPrisonCard);
            actionText.text = "";
            waitText.text = StringLocaliser.GetString("waiting_for_players");
        }

        void OnDestroy()
        {
            if (current == this)
            {
                current = null;
                SquareCollider.ResetColliders(); // reset graphical board
                if (sock != null)
                {
                    sock.Close();
                    sock = null;
                }
                Debug.Log("Successfully destroyed gamestate.");
            }
        }

        public void Crash()
        {
#if UNITY_WEBGL
            // not much we can do in webgl, so bye byeeeeeeee
            Application.Quit();
#else
            // if we crash in the binary we can just return to the main menu
            // TODO: add error message in menu scene
            LoadHandler.LoadScene("Scenes/MenuScene");
#endif
        }

        void OnApplicationQuit()
        {
            if (sock != null)
            {
                sock.Close();
                sock = null;
            }
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

        private IEnumerator OpenGameSocket()
        {
            if (ClientLobbyState.token != null)
            {
                // we have arrived from the lobby, so now we need to open the
                // game socket
                string address = ClientLobbyState.address;
                int port = ClientLobbyState.port;
                ClientLobbyState.ConnectMode mode = ClientLobbyState.connectMode;
                Dictionary<string, string> par = new Dictionary<string, string>();
                if (mode == ClientLobbyState.ConnectMode.ONLINE)
                    par.Add("token", ClientLobbyState.token);
                else
                    par.Add("token", ClientLobbyState.clientUUID);
                string gameToken;
                gameToken = ClientLobbyState.currentLobby;
                PacketSocket socket =
                    PacketSocket.CreateSocket(address, port, par, gameToken, false);
                socket.Connect();
                // wait for the socket to open or die
                yield return new WaitUntil(delegate
                {
                    return socket.HasError() || socket.IsOpen();
                });
                if (socket.HasError())
                {
                    //if (mode == ClientLobbyState.ConnectMode.BYIP)
                    //connectConnector.DisplayError("connection_fail");
                    //else
                    //loginConnector.DisplayError("connection_fail");
                    Debug.LogWarning("Error occured opening game state!");
                    Crash();
                    yield break; // unreachable
                }
                RegisterSocket(ClientLobbyState.clientUUID,
                               ClientLobbyState.token,
                               socket);

                UIDirector.IsMenuOpen = false;
                UIDirector.IsGameMenuOpen = false;
            }
        }

        public Dictionary<string, int> GetSquareDataIndex(int idx)
        {
            return squareData.First<Dictionary<string, int>>(
                (x) => x.ContainsKey("id") && x["id"] == idx
            );
        }

        public void LogMessage(string msg)
        {
            // TODO: Fix update scrollbar.
            if (chatBox.text.Length > 0)
                chatBox.text += "<br>";
            chatBox.text += msg;
            StartCoroutine(ScrollUpdate());
            Debug.Log(msg);
        }

        private IEnumerator ScrollUpdate()
        {
            yield return new WaitForEndOfFrame();
            chatScroller.verticalNormalizedPosition = 0;
        }
        private void LogAction(string msg)
        {
            if (actionEnumeration != null)
                StopCoroutine(actionEnumeration);
            actionEnumeration = StartCoroutine(ShowAction(msg));
        }

        private IEnumerator ShowAction(string msg)
        {
            actionText.text = msg;
            yield return new WaitForSeconds(5);
            actionText.text = "";
            actionEnumeration = null;
        }

        public static string PlayerNameLoggable(Player player)
        {
            return string.Format("<color=#db1b4e>{0}</color>", player.Name);
        }

        public static string OwnableNameLoggable(OwnableSquare square)
        {
            string name = null;
            string color = "ffffff";
            if (square.IsProperty())
            {
                name = StringLocaliser.GetString(
                    string.Format("property{0}", square.Id));
                Color c = PropertySquare.GetColorIndex(square.Id);
                color = string.Format("{0:X02}{1:X02}{2:X02}",
                    (int) (c.r*255), (int)(c.g * 255), (int)(c.b * 255));
            }
            else if (square.IsCompany())
            {
                name = StringLocaliser.GetString(
                    string.Format("museum{0}", square.Id));
            }
            else if (square.IsStation())
            {
                name = StringLocaliser.GetString(
                    string.Format("station{0}", square.Id));
            }
            return string.Format("<color=#{0}>{1}</color>", color, name);
        }

        private void InitGame()
        {
            Board = new Board();
            players = new List<Player>();
            playerPieces = new List<PlayerPiece>();
            CanRollDice = false;
            CanPerformAction = false;
            UpdateParkingMoney(0);
        }

        public void RegisterSocket(string uuid, string token, PacketSocket sock)
        {
            this.clientUUID = uuid;
            this.token = token;
            this.sock = sock;
            comm = new PacketCommunicator(sock);
            comm.OnError += OnError;
            comm.OnMessage += OnMessage;
            comm.OnBuyHouse += OnBuyHouse;
            comm.OnSellHouse += OnSellHouse;
            comm.OnBuyProperty += OnBuyProperty;
            comm.OnBalanceUpdate += OnBalanceUpdate;
            comm.OnMortgage += OnMortgageProperty;
            comm.OnUnmortgage += OnUnmortgageProperty;
            comm.OnMove += OnMove;
            comm.OnDisconnect += OnDisconnect;
            comm.OnReconnect += OnReconnect;
            comm.OnGameStart += OnGameStart;
            comm.OnGameStartDice += OnGameStartDice;
            comm.OnGameStartDiceResult += OnGameStartDiceResult;
            comm.OnRoundStart += OnRoundStart;
            comm.OnRoundDiceResult += OnRoundDiceResult;
            comm.OnActionStart += OnActionStart;
            comm.OnActionTimeout += OnActionTimeout;
            comm.OnRoundRandomCard += OnRoundRandomCard;
            comm.OnEnterPrison += OnEnterPrison;
            comm.OnExitPrison += OnExitPrison;
            comm.OnExchange += OnExchange;
            comm.OnExchangePlayerSelect += OnExchangeSelectPlayer;
            comm.OnExchangeTradeSelect += OnExchangeSelectTrade;
            comm.OnExchangeSend += OnExchangeSend;
            comm.OnExchangeAccept += OnExchangeAccept;
            comm.OnExchangeDecline += OnExchangeDecline;
            comm.OnExchangeCounter += OnExchangeCounter;
            comm.OnExchangeCancel += OnExchangeCancel;
            comm.OnAuctionStart += OnAuction;
            comm.OnAuctionBid += OnAuctionBid;
            comm.OnAuctionEnd += OnAuctionEnd;
        }

        public Player GetPlayer(string uuid)
        {
            foreach (Player p in players)
            {
                if (p.Id.Equals(uuid))
                    return p;
            }
            return null;
        }

        public int GetPlayerIndex(string uuid)
        {
            for (int i = 0; i < players.Count; ++i)
            {
                if (players[i].Id.Equals(uuid))
                    return i;
            }
            return -1;
        }

        private int GetPlayerPieceIndex(string uuid)
        {
            for (int i = 0; i < players.Count; ++i)
            {
                if (playerPieces[i].playerUUID.Equals(uuid))
                    return i;
            }
            return -1;
        }

        public void ManuallyRegisterPlayer(Player p, bool me)
        {
            players.Add(p);
            int prefabIndex = p.CharacterIdx % piecePrefabs.Length;
            GameObject playerPiece =
                Instantiate(piecePrefabs[prefabIndex], boardObject.transform);
            PlayerPiece pp = playerPiece.GetComponent<PlayerPiece>();
            pp.playerUUID = p.Id;
            pp.playerIndex = p.CharacterIdx;
            pp.MoveToPosition(0, true, null);
            playerDoneMove = true;
            playerPieces.Add(pp);
            playerInfo.AddPlayer(p, me);
            if (me)
                myPlayer = p;
        }

        private void HideAllInteractButtons()
        {
            actionEndButton.gameObject.SetActive(false);
            rollDiceButton.gameObject.SetActive(false);
            exchangeButton.gameObject.SetActive(false);
            buyPropertyButton.gameObject.SetActive(false);
            auctionButton.gameObject.SetActive(false);
            exitPrisonMoneyButton.gameObject.SetActive(false);
            exitPrisonCardButton.gameObject.SetActive(false);
        }

        private void UpdateParkingMoney(int amount)
        {
            parkingMoney = amount;
            parkingMoneyText.text = string.Format(
                StringLocaliser.GetString("money_format"), parkingMoney);
        }

        public void DoRollDice()
        {
            if (comm == null)
                return;
            if (isGameStart)
            {
                isGameStart = false;
                rollDiceButton.gameObject.SetActive(false);
                comm.DoGameStartDiceThrow(clientUUID);
            }
            else
            {
                comm.DoRoundDiceChoice(clientUUID, PacketRoundDiceChoice.DiceChoice.ROLL_DICE);
            }
            PlayerField pinfo = playerInfo.GetPlayerField(myPlayer);
            pinfo.Dice.RollDice();
        }

        public void DoExitPrisonMoney()
        {
            if (comm == null)
                return;
            comm.DoRoundDiceChoice(clientUUID, PacketRoundDiceChoice.DiceChoice.JAIL_PAY);
        }

        public void DoExitPrisonCard()
        {
            if (comm == null)
                return;
            comm.DoRoundDiceChoice(clientUUID, PacketRoundDiceChoice.DiceChoice.JAIL_CARD);
        }

        public void DoBuyProperty()
        {
            if (comm != null)
                comm.DoBuyProperty(clientUUID, myPlayer.Position);
        }

        public void DoBuyHouse()
        {
            if (comm != null)
                comm.DoBuyHouse(clientUUID, myPlayer.Position);
        }

        public void DoMortgageProperty()
        {
            if (comm != null)
                comm.DoMortgageProperty(clientUUID, myPlayer.Position);
        }

        public void DoUnmortgageProperty()
        {
            if (comm != null)
                comm.DoUnmortgageProperty(clientUUID, myPlayer.Position);
        }

        public void DoSellHouse()
        {
            if (comm != null)
                comm.DoSellHouse(clientUUID, myPlayer.Position);
        }

        public void DoMessage(string message)
        {
            if (comm != null)
                comm.DoMessage(clientUUID, message);
        }

        public void DoActionEnd()
        {
            if (comm != null)
                comm.DoEndAction();
        }

        public void DoExchange()
        {
            if (comm != null)
                comm.DoExchange(clientUUID);
        }

        public void DoExchangeSend()
        {
            if (comm != null)
                comm.DoSendExchange();
        }

        public void DoExchangeAccept()
        {
            if (comm != null)
                comm.DoAcceptExchange();
        }

        public void DoExchangeCounter()
        {
            if (comm != null)
                comm.DoCounterExchange();
        }

        public void DoExchangeRefuse()
        {
            if (comm != null)
                comm.DoDeclineExchange();
        }

        public void DoExchangeCancel()
        {
            if (comm != null)
                comm.DoCancelExchange();
        }

        public void DoExchangeSelectPlayer(string uuid)
        {
            if (comm != null)
                comm.DoExchangePlayerSelect(clientUUID, uuid);
        }

        public void DoExchangeSelectTrade(bool recipient,
            int val, PacketActionExchangeTradeSelect.SelectType type)
        {
            if (comm != null)
                comm.DoExchangeTradeSelect(clientUUID, recipient, val, type);
        }

        public void DoAuction()
        {
            if (comm != null)
                comm.DoAuctionProperty(clientUUID, myPlayer.Position, 0);
        }

        public void DoAuctionBid(int bidAmount)
        {
            if (comm != null)
                comm.DoBidAuction(clientUUID, bidAmount);
        }

        public void OnExchange(PacketActionExchange packet)
        {
            Player p = Player.PlayerFromUUID(players, packet.PlayerId);
            if (p == null)
            {
                Debug.LogWarning(string.Format("Could not find player '{0}'!",
                                               packet.PlayerId));
                return;
            }
            GameObject exchangeMenu =
                Instantiate(ExchangePrefab, canvas.transform);
            currentExchange = exchangeMenu.GetComponent<MenuExchange>();
            currentExchange.playerPrimary = p;
            currentExchange.playerList = players;
        }

        public void OnExchangeSelectPlayer(PacketActionExchangePlayerSelect packet)
        {
            Player p = Player.PlayerFromUUID(players, packet.SelectedPlayerId);
            if (p == null)
            {
                Debug.LogWarning(string.Format("Could not find player '{0}'!",
                                               packet.SelectedPlayerId));
                return;
            }
            if (currentExchange != null)
                currentExchange.PopulateRight(p);
        }

        public void OnExchangeSelectTrade(PacketActionExchangeTradeSelect packet)
        {
            if (currentExchange == null)
                return;
            switch (packet.ExchangeType)
            {
            case PacketActionExchangeTradeSelect.SelectType.MONEY:
                if (packet.AffectsRecipient)
                    currentExchange.SetMoneyRight(packet.Value);
                else
                    currentExchange.SetMoneyLeft(packet.Value);
                break;
            case PacketActionExchangeTradeSelect.SelectType.PROPERTY:
                currentExchange.ToggleSelectProperty
                    (packet.Value, packet.AffectsRecipient);
                break;
            // TODO: implement the rest
            }
        }

        public void OnExchangeSend(PacketActionExchangeSend packet)
        {
            if (currentExchange == null)
                return;
            currentExchange.Swap();
        }

        public void OnExchangeAccept(PacketActionExchangeAccept packet)
        {
            Destroy(currentExchange.gameObject);
            currentExchange = null;
            UIDirector.IsGameMenuOpen = false;
        }

        public void OnExchangeDecline(PacketActionExchangeDecline packet)
        {
            Destroy(currentExchange.gameObject);
            currentExchange = null;
            UIDirector.IsGameMenuOpen = false;
        }

        public void OnExchangeCounter(PacketActionExchangeCounter packet)
        {
            if (currentExchange == null)
                return;
            // TODO: IMPLEMENT
        }

        public void OnExchangeCancel(PacketActionExchangeCancel packet)
        {
            if (currentExchange == null)
                return;
            Destroy(currentExchange.gameObject);
            currentExchange = null;
            UIDirector.IsGameMenuOpen = false;
        }

        public void OnAuction(PacketActionAuctionProperty packet)
        {
            Player p = Player.PlayerFromUUID(players, packet.PlayerId);
            if (p == null)
            {
                Debug.LogWarning(string.Format("Could not find player '{0}'!",
                                               packet.PlayerId));
                return;
            }
            GameObject auctionMenu =
                Instantiate(AuctionPrefab, canvas.transform);
            currentAuction = auctionMenu.GetComponent<MenuAuction>();
            currentAuction.Index = packet.Property;
            currentAuction.CurrentBid = packet.MinBid;
            currentAuction.TimeoutDuration = timeouts["AUCTION_TOUR_WAIT"];
        }

        public void OnAuctionBid(PacketAuctionBid packet)
        {
            if (currentAuction == null)
                return;
            currentAuction.Bid(packet.BidderId, packet.NewPrice);
        }

        public void OnAuctionEnd(PacketAuctionEnd packet)
        {
            if (currentAuction != null)
            {
                Destroy(currentAuction.gameObject);
                currentAuction = null;
                UIDirector.IsGameMenuOpen = false;
            }
            // TODO: show message and update timeout
        }

        public void OnError(PacketException packet)
        {
            // TODO: add an error message and whatnot
            Crash();
        }

        public void OnMessage(PacketChat packet)
        {
            Player p = Player.PlayerFromUUID(players, packet.PlayerId);
            if (p == null)
            {
                Debug.LogWarning(string.Format("Could not find player '{0}'!",
                                               packet.PlayerId));
                return;
            }
            LogMessage(string.Format("{0}: {1}",
                       PlayerNameLoggable(p), packet.Message));
        }

        public void OnBuyHouse(PacketActionBuyHouseSucceed packet)
        {
            Player p = Player.PlayerFromUUID(players, packet.PlayerId);
            if (p == null)
            {
                Debug.LogWarning(string.Format("Could not find player '{0}'!",
                                               packet.PlayerId));
                return;
            }
            Square s = Board.GetSquare(packet.HouseId);
            if (s.IsProperty())
            {
                PropertySquare ps = (PropertySquare)s;
                if (ps.Owner != null && ps.Owner.Id.Equals(packet.PlayerId))
                {
                    if (Board.BuyHouse(ps, p))
                    {
                        ++SquareCollider.Colliders[packet.HouseId].houseLevel;
                        //SquareCollider.Colliders[packet.HouseId].UpdateHouses();
                        LogAction(string.Format(
                            StringLocaliser.GetString("on_buy_house"),
                            PlayerNameLoggable(p),
                            OwnableNameLoggable(ps)));
                    }
                }
            }
            if (BoardCardDisplay.current.rendering)
                BoardCardDisplay.current.Redraw();
        }

        public void OnSellHouse(PacketActionSellHouseSucceed packet)
        {
            Player p = Player.PlayerFromUUID(players, packet.PlayerId);
            if (p == null)
            {
                Debug.LogWarning(string.Format("Could not find player '{0}'!",
                                               packet.PlayerId));
                return;
            }
            Square s = Board.GetSquare(packet.HouseId);
            if (s.IsProperty())
            {
                PropertySquare ps = (PropertySquare)s;
                if (ps.Owner != null && ps.Owner.Id.Equals(packet.PlayerId))
                {
                    if (Board.SellHouse(ps, p))
                    {
                        --SquareCollider.Colliders[packet.HouseId].houseLevel;
                        //SquareCollider.Colliders[packet.HouseId].UpdateHouses();
                        LogAction(string.Format(
                            StringLocaliser.GetString("on_sell_house"),
                            PlayerNameLoggable(p),
                            OwnableNameLoggable(ps)));
                    }
                }
            }
            if (BoardCardDisplay.current.rendering)
                BoardCardDisplay.current.Redraw();
        }

        public void OnBuyProperty(PacketActionBuyPropertySucceed packet)
        {
            Player p = Player.PlayerFromUUID(players, packet.PlayerId);
            if (p == null)
            {
                Debug.LogWarning(string.Format("Could not find player '{0}'!",
                                               packet.PlayerId));
                return;
            }
            Square s = Board.GetSquare(packet.Property);
            if (s.IsOwnable())
            {
                OwnableSquare os = (OwnableSquare)s;
                if (os.Owner == null)
                {
                    Board.BoardBank.BuyProperty(p, os);
                    SquareCollider.Colliders[os.Id].SetSphereChild(p);
                    LogAction(string.Format(
                        StringLocaliser.GetString("on_buy_property"),
                        PlayerNameLoggable(p),
                        OwnableNameLoggable(os)));
                }
            }
            buyPropertyButton.gameObject.SetActive(false);
            auctionButton.gameObject.SetActive(false);
            if (BoardCardDisplay.current.rendering)
                BoardCardDisplay.current.Redraw();
        }

        public void OnEnterPrison(PacketPlayerEnterPrison packet)
        {
            Player p = Player.PlayerFromUUID(players, packet.PlayerId);
            if (p == null)
            {
                Debug.LogWarning(string.Format("Could not find player '{0}'!",
                                               packet.PlayerId));
                return;
            }
            LogAction(string.Format(
                StringLocaliser.GetString("on_enter_prison"),
                PlayerNameLoggable(p)));
            p.EnterPrison();
            playerPieces[GetPlayerPieceIndex(p.Id)].
                MoveToPosition(p.Position, true, null);
            playerDoneMove = true;
        }

        public void OnExitPrison(PacketPlayerExitPrison packet)
        {
            Player p = Player.PlayerFromUUID(players, packet.PlayerId);
            if (p == null)
            {
                Debug.LogWarning(string.Format("Could not find player '{0}'!",
                                               packet.PlayerId));
                return;
            }
            LogAction(string.Format(
                StringLocaliser.GetString("on_exit_prison"),
                PlayerNameLoggable(p)));
            exitPrisonMoneyButton.gameObject.SetActive(false);
            exitPrisonCardButton.gameObject.SetActive(false);
            p.ExitPrison();
        }

        public void OnMortgageProperty(PacketActionMortgageSucceed packet)
        {
            Player p = Player.PlayerFromUUID(players, packet.PlayerId);
            if (p == null)
            {
                Debug.LogWarning(string.Format("Could not find player '{0}'!",
                                               packet.PlayerId));
                return;
            }
            Square s = Board.GetSquare(packet.Property);
            if (s.IsOwnable())
            {
                OwnableSquare os = (OwnableSquare)s;
                if (os.Owner != null && os.Owner.Id.Equals(packet.PlayerId))
                {
                    os.MortgageProperty();
                    LogAction(string.Format(
                        StringLocaliser.GetString("on_mortgage_property"),
                        PlayerNameLoggable(p),
                        OwnableNameLoggable(os)));
                }
            }
            if (BoardCardDisplay.current.rendering)
                BoardCardDisplay.current.Redraw();
        }

        public void OnUnmortgageProperty(PacketActionUnmortgageSucceed packet)
        {
            Player p = Player.PlayerFromUUID(players, packet.PlayerId);
            if (p == null)
            {
                Debug.LogWarning(string.Format("Could not find player '{0}'!",
                                               packet.PlayerId));
                return;
            }
            Square s = Board.GetSquare(packet.Property);
            if (s.IsOwnable())
            {
                OwnableSquare os = (OwnableSquare)s;
                if (os.Owner != null && os.Owner.Id.Equals(packet.PlayerId))
                {
                    os.UnmortgageProperty();
                    LogAction(string.Format(
                        StringLocaliser.GetString("on_unmortgage_property"),
                        PlayerNameLoggable(p),
                        OwnableNameLoggable(os)));
                }
            }
            if (BoardCardDisplay.current.rendering)
                BoardCardDisplay.current.Redraw();
        }

        public void OnBalanceUpdate(PacketPlayerUpdateBalance packet)
        {
            StartCoroutine(OnBalanceUpdateEnumerator(packet));
        }

        private IEnumerator OnBalanceUpdateEnumerator(
            PacketPlayerUpdateBalance packet)
        {
            yield return new WaitUntil(() => playerDoneMove);
            Player p = Player.PlayerFromUUID(players, packet.PlayerId);
            if (p == null)
            {
                Debug.LogWarning(string.Format("Could not find player '{0}'!",
                                               packet.PlayerId));
                yield break;
            }
            p.Money = packet.NewBalance;
            playerInfo.SetMoney(p, p.Money);
            if (packet.Reason.Equals("tax_square") ||
                packet.Reason.Equals("jail_leave_pay"))
            {
                // add to free parking pot
                UpdateParkingMoney(parkingMoney +
                    Mathf.Abs(packet.OldBalance - packet.NewBalance));
            }
            else if (packet.Reason.Equals("parking_square"))
            {
                // take all from free parking pot
                UpdateParkingMoney(0);
            }
        }

        public void OnMove(PacketPlayerMove packet)
        {
            Player p = Player.PlayerFromUUID(players, packet.MovingPlayerId);
            if (p == null)
            {
                Debug.LogWarning(string.Format("Could not find player '{0}'!",
                                               packet.MovingPlayerId));
                return;
            }
            buyPropertyButton.gameObject.SetActive(false);
            auctionButton.gameObject.SetActive(false);
            if (packet.DestinationSquare >= 0 && packet.DestinationSquare <= 39)
            {
                p.Position = packet.DestinationSquare;
                playerDoneMove = false;
                playerPieces[GetPlayerPieceIndex(p.Id)].
                    MoveToPosition(p.Position, packet.Instant,
                                   OnMoveAnimateCallback);
            }
        }

        private void OnMoveAnimateCallback()
        {
            playerDoneMove = true;
        }

        public void OnDisconnect(PacketPlayerDisconnect packet)
        {
            Player p = Player.PlayerFromUUID(players, packet.PlayerId);
            // TODO: Reason
            if (p == null)
            {
                Debug.LogWarning(string.Format("Could not find player '{0}'!",
                                               packet.PlayerId));
                return;
            }
            if (p == myPlayer)
            {
                // I have lost connection, so I need to die and return to the
                // menu
                Crash();
            }
            LogMessage(string.Format(
                StringLocaliser.GetString("on_disconnect"),
                    PlayerNameLoggable(p)));
        }

        public void OnReconnect(PacketPlayerReconnect packet)
        {
            Player p = Player.PlayerFromUUID(players, packet.PlayerId);
            // TODO: Reason
            if (p == null)
            {
                Debug.LogWarning(string.Format("Could not find player '{0}'!",
                                               packet.PlayerId));
                return;
            }
            LogMessage(string.Format(
                StringLocaliser.GetString("on_reconnect"),
                    PlayerNameLoggable(p)));
        }

        public void OnRoundStart(PacketRoundStart packet)
        {
            Player p = Player.PlayerFromUUID(players, packet.PlayerId);
            playerTurn = p;
            playerInfo.SetActive(p);
            if (p == myPlayer)
            {
                CanRollDice = true;
                Debug.Log(string.Format("Your turn to roll the dice. ({0})",
                                        clientUUID));
                // TODO: MESSAGE
                exitPrisonMoneyButton.gameObject.SetActive(p.InJail);
                exitPrisonCardButton.gameObject.SetActive(p.InJail);
                rollDiceButton.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log(string.Format("Player {0} to roll the dice.",
                                        clientUUID));
                HideAllInteractButtons();
                // show the dice rolling automatically
                PlayerField pinfo = playerInfo.GetPlayerField(p);
                pinfo.Dice.RollDice();
            }
            LogAction(string.Format(
                StringLocaliser.GetString("on_round_start"),
                    PlayerNameLoggable(p)));
        }

        public void OnRoundDiceResult(PacketRoundDiceResults packet)
        {
            rollDiceButton.gameObject.SetActive(false);
            exitPrisonMoneyButton.gameObject.SetActive(false);
            exitPrisonCardButton.gameObject.SetActive(false);
            Player p = Player.PlayerFromUUID(players, packet.PlayerId);
            PlayerField pinfo = playerInfo.GetPlayerField(p);
            switch (packet.Reason)
            {
            case PacketRoundDiceResults.ResultEnum.JAIL_CARD_CHANCE:
                Debug.Log(string.Format(
                    "Player {0} used a chance jail card.", packet.PlayerId));
                pinfo.Dice.HideDice();
                break;
            case PacketRoundDiceResults.ResultEnum.JAIL_CARD_COMMUNITY:
                Debug.Log(string.Format(
                    "Player {0} used a community jail card.",
                    packet.PlayerId));
                pinfo.Dice.HideDice();
                break;
            case PacketRoundDiceResults.ResultEnum.JAIL_PAY:
                Debug.Log(string.Format(
                    "Player {0} paid to be released from jail.",
                    packet.PlayerId));
                pinfo.Dice.HideDice();
                break;
            case PacketRoundDiceResults.ResultEnum.ROLL_DICE:
                string msg = string.Format(
                    StringLocaliser.GetString("on_dice_throw"),
                        PlayerNameLoggable(p),
                        packet.DiceResult1 + packet.DiceResult2);
                if (packet.DiceResult1 == packet.DiceResult2)
                {
                    msg += string.Format(" {0}",
                        StringLocaliser.GetString("doubles"));
                }
                pinfo.Dice.RevealDice(packet.DiceResult1, packet.DiceResult2);
                LogAction(msg);
                break;
            }
        }

        public void OnRoundRandomCard(PacketRoundRandomCard packet)
        {
            StartCoroutine(OnRoundRandomCardEnumeration(packet));
        }

        private IEnumerator OnRoundRandomCardEnumeration(
            PacketRoundRandomCard packet)
        {
            yield return new WaitUntil(() => playerDoneMove);
            TokenCard.CardType type =
                packet.IsCommunity ? TokenCard.CardType.COMMUNITY :
                                     TokenCard.CardType.CHANCE;
            string message = StringLocaliser.GetString("card" + packet.CardId);
            try
            {
                int val;
                Card.CardType cardType = (Card.CardType) cardData[packet.CardId - 1]["type"];
                // get card data for formatting
                if (cardType == Card.CardType.GOTO_POSITION)
                {
                    val = 200; // these cards typically say to pass go for $200
                    message = string.Format(message, val);
                }
                else if (cardType == Card.CardType.GIVE_BOARD_HOUSES)
                {
                    // make repairs $25 per house, $100 per property thingy
                    val = 25;
                    int val2 = 100;
                    message = string.Format(message, val, val2);
                }
                else
                {
                    val = cardData[packet.CardId - 1]["value"];
                    message = string.Format(message, val);
                }
            }
            catch (System.Exception)
            {
                // die softly and cry
            }
            tokenCard.ShowCard(type, packet.CardId, message);
        }

        public void OnGameStart(PacketGameStart packet)
        {
            canvas.GetComponent<CanvasGroup>().alpha = 1.0f;
            Destroy(waitBlock);
            LogAction(StringLocaliser.GetString("game_start"));
            foreach (PacketGameStateInternal playerData in packet.Players)
            {
                Player player = new Player(playerData.PlayerId, playerData.PlayerName, playerData.Money, playerData.Piece);
                ManuallyRegisterPlayer(player, playerData.PlayerId.Equals(clientUUID));
                playerInfo.SetMoney(player, player.Money);

                if (player != myPlayer)
                {
                    PlayerField pinfo = playerInfo.GetPlayerField(player);
                    pinfo.Dice.RollDice();
                }
            }
            // setup the timeouts
            timeouts = packet.Timeouts;
        }

        public void OnGameStartDice(PacketGameStartDice packet)
        {
            isGameStart = true;
            rollDiceButton.gameObject.SetActive(true);
            timeout.SetTime(timeouts["START_DICE_WAIT"]);
            timeout.Restart();
        }

        public void OnGameStartDiceResult(PacketGameStartDiceResults packet)
        {
            foreach (PacketGameStartDiceResultsInternal result in packet.DiceResult)
            {
                PlayerField pinfo = playerInfo.GetPlayerField(result.PlayerId);
                pinfo.Dice.RevealDice(result.Dice1, result.Dice2);
                if (result.Win)
                {
                    // TODO: hide dice animations and what not
                    Player p = Player.PlayerFromUUID(players, result.PlayerId);
                    if (p == null)
                    {
                        Debug.LogWarning(string.Format("Could not find player '{0}'!",
                                                       result.PlayerId));
                        return;
                    }
                    LogAction(string.Format(
                        StringLocaliser.GetString("on_game_start_dice_result"),
                        PlayerNameLoggable(p), result.Dice1, result.Dice2));
                    break;
                }
            }
            timeout.Hide();
        }

        public void OnActionStart(PacketActionStart packet)
        {
            timeout.SetTime(timeouts["ACTION_TIMEOUT_WAIT"]);
            timeout.Restart();

            if (myPlayer != playerTurn)
                return;

            rollDiceButton.gameObject.SetActive(false);
            exchangeButton.gameObject.SetActive(true);
            actionEndButton.gameObject.SetActive(true);

            Square square = Board.GetSquare(myPlayer.Position);
            if (square.IsOwnable())
            {
                OwnableSquare os = (OwnableSquare)square;
                if (os.Owner == null)
                {
                    buyPropertyButton.gameObject.SetActive(true);
                    auctionButton.gameObject.SetActive(true);
                }
            }
            CanPerformAction = true;

            if (BoardCardDisplay.current.rendering)
                BoardCardDisplay.current.Redraw();

            Debug.Log("Turn started.");
        }

        public void OnActionTimeout(PacketActionTimeout packet)
        {
            CanPerformAction = false;
            // TODO: do the same thing for auctions
            if (currentExchange != null)
            {
                Destroy(currentExchange.gameObject);
                currentExchange = null;
            }
            UIDirector.IsGameMenuOpen = false;
            HideAllInteractButtons();
            if (BoardCardDisplay.current.rendering)
                BoardCardDisplay.current.Redraw();
            timeout.Hide();
            Debug.Log("Turn ended.");
        }

        public void OnDefeat(PacketPlayerDefeat packet)
        {
            Player p = Player.PlayerFromUUID(players, packet.PlayerId);
            if (p == null)
            {
                Debug.LogWarning(string.Format("Could not find player '{0}'!",
                                               packet.PlayerId));
                return;
            }
            if (p == myPlayer)
            {
                LogAction(string.Format(
                    StringLocaliser.GetString("on_defeat_me")));
                // FIXME: DISABLE EVERYTHING OR LEAVE ON DEFEAT
            }
            else
            {
                LogAction(string.Format(
                    StringLocaliser.GetString("on_defeat"),
                    PlayerNameLoggable(p)));
            }
        }

    }

}
