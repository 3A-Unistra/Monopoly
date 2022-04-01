/*
 * ClientGameState.cs
 * Client game state handler.
 * 
 * Date created : 15/03/2022
 * Author       : Finn Rayment <rayment@etu.unistra.fr>
 *              : Rayan Marmar <rayan.marmar@etu.unistra.fr>
                : Christophe Pierson <christophe.pierson@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Monopoly.Classes;
using Monopoly.Graphics;
using Monopoly.Net;
using Monopoly.Net.Packets;

namespace Monopoly.Runtime
{

    public class ClientGameState : MonoBehaviour
    {

        public static ClientGameState current;
        public List<Dictionary<string, int>> squareData;

        public Board Board { get; private set; }
        private List<Player> players;
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

        void OnDestroy()
        {
            if (current == this)
            {
                current = null;
                SquareCollider.ResetColliders(); // reset graphical board
                Debug.Log("Successfully destroyed gamestate.");
            }
        }

        public Dictionary<string, int> GetSquareDataIndex(int idx)
        {
            return squareData.First<Dictionary<string, int>>(
                (x) => x.ContainsKey("id") && x["id"] == idx
            );
        }

        private void InitGame()
        {
            Board = new Board();
            players = new List<Player>();
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
            comm.OnRoundStart += OnRoundStart;
            comm.OnRoundDiceResult += OnRoundDiceResult;
            comm.OnActionStart += OnActionStart;
            comm.OnActionTimeout += OnActionTimeout;
        }

        public void ManuallyRegisterPlayer(Player p)
        {
            players.Add(p);
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
                    Board.BoardBank.BuyProperty(p, os);
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
                    os.MortgageProperty();
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
                    os.UnmortgageProperty();
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
                p.Position = packet.DestinationSquare;
        }

        public void OnRoundStart(PacketRoundStart packet)
        {
            // TODO: Implement ui popup and whatnot
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
        }

        public void OnRoundDiceResult(PacketRoundDiceResults packet)
        {
            // TODO: Implement ui popup, piece animation, etc.
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
                Debug.Log(string.Format(
                    "Player {0} rolled {1} and {2}.",
                    packet.PlayerId, packet.DiceResult1, packet.DiceResult2));
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
