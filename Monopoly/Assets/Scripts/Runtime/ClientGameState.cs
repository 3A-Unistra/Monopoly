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

using Monopoly.Classes;
using Monopoly.Graphics;
using Monopoly.Net;
using Monopoly.Net.Packets;
using Monopoly.Util;

namespace Monopoly.Runtime
{

    public class ClientGameState : MonoBehaviour
    {

        public static ClientGameState current;
        public List<Dictionary<string, int>> squareData;

        public GameObject[] piecePrefabs;
        public GameObject boardObject;

        public TMP_Text chatBox;

        public Board Board { get; private set; }
        private List<Player> players;
        private List<PlayerPiece> playerPieces;
        private string clientUUID;

        public bool CanRollDice { get; set; }
        public bool CanPerformAction { get; set; }

        private PacketCommunicator comm;
        private PacketSocket sock;

        private void LoadGameData()
        {
            squareData =
                JsonLoader.LoadJsonAsset<List<Dictionary<string, int>>>
                ("Data/squares");
        }

        void Awake()
        {
            if (current != null)
            {
                Debug.LogError("Cannot create two concurrent gamestates!");
                Destroy(this);
            }
            current = this;
            LoadGameData();
            InitGame();
            Debug.Log("Initialised gamestate.");
        }

        void Start()
        {
            // let stuff in the scene load, then run this code:
            /*
            Dictionary<string, string> par = new Dictionary<string, string>();
            par.Add("token", "283e3f3e-3411-44c5-9bc5-037358c47100");
            PacketSocket socket = PacketSocket.CreateSocket("127.0.0.1", 8000, par, false);
            socket.Connect();
            sock = socket;*/
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
            if (sock != null)
            {
#if !UNITY_WEBGL || UNITY_EDITOR
                sock.Sock.DispatchMessageQueue();
#endif
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
            // TODO: Update scrollbar.
            if (chatBox.text.Length > 0)
                chatBox.text += "<br>";
            chatBox.text += msg;
            Debug.Log(msg);
        }

        public string PlayerNameLoggable(Player player)
        {
            return string.Format("<color=#db1b4e>{0}</color>", player.Name);
        }

        public string OwnableNameLoggable(OwnableSquare square)
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
        }

        public void RegisterSocket(string uuid, PacketSocket sock)
        {
            this.clientUUID = uuid;
            this.sock = sock;
            comm = new PacketCommunicator(sock);
            comm.OnBuyHouse += OnBuyHouse;
            comm.OnSellHouse += OnSellHouse;
            comm.OnBuyProperty += OnBuyProperty;
            comm.OnBalanceUpdate += OnBalanceUpdate;
            comm.OnMortgage += OnMortgageProperty;
            comm.OnUnmortgage += OnUnmortgageProperty;
            comm.OnMove += OnMove;
            comm.OnDisconnect += OnDisconnect;
            comm.OnReconnect += OnReconnect;
            comm.OnRoundStart += OnRoundStart;
            comm.OnRoundDiceResult += OnRoundDiceResult;
            comm.OnActionStart += OnActionStart;
            comm.OnActionTimeout += OnActionTimeout;
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

        public void ManuallyRegisterPlayer(Player p)
        {
            players.Add(p);
            int prefabIndex = p.CharacterIdx % piecePrefabs.Length;
            GameObject playerPiece =
                Instantiate(piecePrefabs[prefabIndex], boardObject.transform);
            PlayerPiece pp = playerPiece.GetComponent<PlayerPiece>();
            pp.playerUUID = p.Id;
            pp.playerIndex = players.Count - 1;
            pp.SetPosition(0);
            playerPieces.Add(pp);
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
                        SquareCollider.Colliders[packet.HouseId].UpdateHouses();
                        LogMessage(string.Format(
                            StringLocaliser.GetString("on_buy_house"),
                            PlayerNameLoggable(p),
                            OwnableNameLoggable(ps)));
                    }
                }
            }
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
                        SquareCollider.Colliders[packet.HouseId].UpdateHouses();
                        LogMessage(string.Format(
                            StringLocaliser.GetString("on_sell_house"),
                            PlayerNameLoggable(p),
                            OwnableNameLoggable(ps)));
                    }
                }
            }
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
                    LogMessage(string.Format(
                        StringLocaliser.GetString("on_buy_property"),
                        PlayerNameLoggable(p),
                        OwnableNameLoggable(os)));
                }
            }
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
                    LogMessage(string.Format(
                        StringLocaliser.GetString("on_mortgage_property"),
                        PlayerNameLoggable(p),
                        OwnableNameLoggable(os)));
                }
            }
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
                    LogMessage(string.Format(
                        StringLocaliser.GetString("on_unmortgage_property"),
                        PlayerNameLoggable(p),
                        OwnableNameLoggable(os)));
                }
            }
        }

        public void OnBalanceUpdate(PacketPlayerUpdateBalance packet)
        {
            Player p = Player.PlayerFromUUID(players, packet.PlayerId);
            if (p == null)
            {
                Debug.LogWarning(string.Format("Could not find player '{0}'!",
                                               packet.PlayerId));
                return;
            }
            p.Money = packet.NewBalance;
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
            if (packet.DestinationSquare >= 0 && packet.DestinationSquare <= 39)
            {
                p.Position = packet.DestinationSquare;
                playerPieces[GetPlayerPieceIndex(p.Id)].SetPosition(p.Position);
            }
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
            // TODO: Implement ui popup and whatnot
            Player p = Player.PlayerFromUUID(players, packet.PlayerId);
            if (packet.PlayerId.Equals(clientUUID))
            {
                CanRollDice = true;
                Debug.Log(string.Format("Your turn to roll the dice. ({0})",
                                        clientUUID));
            }
            else
            {
                Debug.Log(string.Format("Player {0} to roll the dice.",
                                        clientUUID));
            }
            LogMessage(string.Format(
                StringLocaliser.GetString("on_round_start"),
                    PlayerNameLoggable(p)));
        }

        public void OnRoundDiceResult(PacketRoundDiceResults packet)
        {
            // TODO: Implement ui popup, piece animation, etc.
            Player p = Player.PlayerFromUUID(players, packet.PlayerId);
            switch (packet.Reason)
            {
            case PacketRoundDiceResults.ResultEnum.JAIL_CARD_CHANCE:
                Debug.Log(string.Format(
                    "Player {0} used a chance jail card.", packet.PlayerId));
                break;
            case PacketRoundDiceResults.ResultEnum.JAIL_CARD_COMMUNITY:
                Debug.Log(string.Format(
                    "Player {0} used a community jail card.",
                    packet.PlayerId));
                break;
            case PacketRoundDiceResults.ResultEnum.JAIL_PAY:
                Debug.Log(string.Format(
                    "Player {0} paid to be released from jail.",
                    packet.PlayerId));
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
                LogMessage(msg);
                break;
            }
        }

        public void OnActionStart(PacketActionStart packet)
        {
            // TODO: Implement + update UI options
            if (packet.PlayerId.Equals(clientUUID))
                CanPerformAction = true;
            Debug.Log("Turn started.");
        }

        public void OnActionTimeout(PacketActionTimeout packet)
        {
            // TODO: Implement + update UI options
            CanPerformAction = false;
            Debug.Log("Turn ended.");
        }

    }

}
