/*
 * PacketCommunicator.cs
 * Asynchronous packet communication handler.
 * 
 * Date created : 01/03/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Monopoly.Net.Packets;
using Monopoly.Runtime;

namespace Monopoly.Net
{

    public class PacketCommunicator
    {

        public delegate void PacketDelegate<T>(T packet);

        // Send list:
        // - AppletReady
        // - GameStartDiceThrow
        // - RoundDiceChoice
        // - ActionAuctionProperty
        // - ActionBuyHouse
        // - ActionSellHouse
        // - ActionBuyProperty
        // - ActionMortgageProperty
        // - ActionUnmortgateProperty
        // - ActionEnd
        // - AuctionBid                 (also received)
        // - AuctionConcede             (also received)
        // - ActionExchange             (also received)
        // - ActionExchangeAccept       (also received)
        // - ActionExchangeCounter      (also received)
        // - ActionExchangeDecline      (also received)
        // - ActionExchangePlayerSelect (also received)
        // - ActionExchangeTradeSelect  (also received)

        // Receive list:
        // - Exception
        // - AppletPrepare
        // - GameStart
        // - GameStartDice
        // - GameStartDiceResults
        // - GameEnd
        // - RoundStart
        // - RoundDiceResults
        // - RoundRandomCard
        // - PlayerMove
        // - PlayerReconnect
        // - PlayerDisconnect
        // - PlayerUpdateBalance
        // - PlayerUpdateProperty
        // - PlayerEnterPrison
        // - PlayerExitPrison
        // - PlayerDefeat
        // - ActionTimeout
        // - AuctionEnd
        // - AuctionRound
        // - AuctionBid                 (also sent)
        // - AuctionConcede             (also sent)
        // - ActionStart
        // - ActionBuyHouseSucceed
        // - ActionSellHouseSucceed
        // - ActionBuyPropertySucceed
        // - ActionMortgagePropertySucceed
        // - ActionUnmortgatePropertySucceed
        // - ActionExchange             (also sent)
        // - ActionExchangeAccept       (also sent)
        // - ActionExchangeCancel       (also sent)
        // - ActionExchangeCounter      (also sent)
        // - ActionExchangeDecline      (also sent)
        // - ActionExchangePlayerSelect (also sent)
        // - ActionExchangeTradeSelect  (also sent)

        public event PacketDelegate<PacketException>         OnError;
        public event PacketDelegate<PacketChat>              OnMessage;
        public event PacketDelegate<PacketAppletPrepare>     OnGameLoad;
        public event PacketDelegate<PacketGameStart>         OnGameStart;
        public event PacketDelegate<PacketGameStartDice>     OnGameStartDice;
        public event PacketDelegate<PacketGameStartDiceResults>
                                                         OnGameStartDiceResult;
        public event PacketDelegate<PacketGameEnd>           OnGameEnd;
        public event PacketDelegate<PacketRoundStart>        OnRoundStart;
        public event PacketDelegate<PacketRoundDiceResults>  OnRoundDiceResult;
        public event PacketDelegate<PacketRoundRandomCard>   OnRoundRandomCard;
        public event PacketDelegate<PacketPlayerMove>        OnMove;
        public event PacketDelegate<PacketPlayerReconnect>   OnReconnect;
        public event PacketDelegate<PacketPlayerDisconnect>  OnDisconnect;
        public event PacketDelegate<PacketPlayerUpdateBalance> OnBalanceUpdate;
        public event PacketDelegate<PacketPlayerEnterPrison> OnEnterPrison;
        public event PacketDelegate<PacketPlayerExitPrison>  OnExitPrison;
        public event PacketDelegate<PacketPlayerDefeat>      OnDefeat;
        public event PacketDelegate<PacketActionStart>       OnActionStart;
        public event PacketDelegate<PacketActionTimeout>     OnActionTimeout;
        public event PacketDelegate<PacketActionAuctionProperty>
                                                         OnAuctionStart;
        public event PacketDelegate<PacketAuctionEnd>        OnAuctionEnd;
        public event PacketDelegate<PacketAuctionBid>        OnAuctionBid;
        public event PacketDelegate<PacketActionBuyHouseSucceed>
                                                         OnBuyHouse;
        public event PacketDelegate<PacketActionSellHouseSucceed>
                                                         OnSellHouse;
        public event PacketDelegate<PacketActionBuyPropertySucceed>
                                                         OnBuyProperty;
        public event PacketDelegate<PacketActionMortgageSucceed>
                                                         OnMortgage;
        public event PacketDelegate<PacketActionUnmortgageSucceed>
                                                         OnUnmortgage;
        public event PacketDelegate<PacketActionExchange>    OnExchange;
        public event PacketDelegate<PacketActionExchangeSend>
                                                         OnExchangeSend;
        public event PacketDelegate<PacketActionExchangeAccept>
                                                         OnExchangeAccept;
        public event PacketDelegate<PacketActionExchangeCancel>
                                                         OnExchangeCancel;
        public event PacketDelegate<PacketActionExchangeCounter>
                                                         OnExchangeCounter;
        public event PacketDelegate<PacketActionExchangeDecline>
                                                         OnExchangeDecline;
        public event PacketDelegate<PacketActionExchangePlayerSelect>
                                                         OnExchangePlayerSelect;
        public event PacketDelegate<PacketActionExchangeTradeSelect>
                                                         OnExchangeTradeSelect;

        private PacketSocket socket;

        public PacketCommunicator(PacketSocket socket)
        {
            if (socket == null)
            {
                throw new System.NullReferenceException(
                    "Packet communicator instantiated with null socket!");
            }
            this.socket = socket;
            socket.Sock.OnMessage += (data) => ReceivePacket(data);
        }

        private void OnPing(PacketPing packet)
        {
            DoPing(); /* send back again */
        }

        private void OnPlayerValidated(Packet packet)
        {
            /* the dispatch queue of the socket will lock up until the game
               scene is loaded, so as soon as that happens, we will need to send
               the AppletReady packet regardless, so, we wait for this packet,
               and we send it immediately */
            DoAppletReady();
        }

        /* TODO: Params for all of the following DoX functions. */

        public void DoPing()
        {
            PacketPing packet = new PacketPing("");
            SendPacket(packet);
        }

        public void DoMessage(string uuid, string message)
        {
            PacketChat packet = new PacketChat(uuid, message);
            SendPacket(packet);
        }

        public void DoAppletReady()
        {
            PacketAppletReady packet = new PacketAppletReady();
            SendPacket(packet);
        }

        public void DoGameStartDiceThrow(string uuid)
        {
            PacketGameStartDiceThrow packet = new PacketGameStartDiceThrow(uuid);
            SendPacket(packet);
        }

        public void DoRoundDiceChoice(string uuid, PacketRoundDiceChoice.DiceChoice choice)
        {
            PacketRoundDiceChoice packet = new PacketRoundDiceChoice(uuid, choice);
            SendPacket(packet);
        }

        public void DoEndAction()
        {
            PacketActionEnd packet = new PacketActionEnd();
            SendPacket(packet);
        }

        public void DoBuyHouse(string uuid, int idx)
        {
            PacketActionBuyHouse packet =
                new PacketActionBuyHouse(uuid, idx);
            SendPacket(packet);
        }

        public void DoSellHouse(string uuid, int idx)
        {
            PacketActionSellHouse packet =
                new PacketActionSellHouse(uuid, idx);
            SendPacket(packet);
        }

        public void DoBuyProperty(string uuid, int idx)
        {
            PacketActionBuyProperty packet =
                new PacketActionBuyProperty(uuid, idx);
            SendPacket(packet);
        }

        public void DoMortgageProperty(string uuid, int idx)
        {
            PacketActionMortgageProperty packet
                = new PacketActionMortgageProperty(uuid, idx);
            SendPacket(packet);
        }

        public void DoUnmortgageProperty(string uuid, int idx)
        {
            PacketActionUnmortgageProperty packet
                = new PacketActionUnmortgageProperty(uuid, idx);
            SendPacket(packet);
        }

        public void DoAuctionProperty(string uuid, int idx, int minPrice)
        {
            PacketActionAuctionProperty packet
                = new PacketActionAuctionProperty(uuid, minPrice, idx);
            SendPacket(packet);
        }

        public void DoBidAuction(string uuid, int price)
        {
            PacketAuctionBid packet = new PacketAuctionBid(uuid, price);
            SendPacket(packet);
        }

        public void DoExchange(string uuid)
        {
            PacketActionExchange packet = new PacketActionExchange(uuid);
            SendPacket(packet);
        }

        public void DoSendExchange()
        {
            PacketActionExchangeSend packet = new PacketActionExchangeSend();
            SendPacket(packet);
        }

        public void DoAcceptExchange()
        {
            PacketActionExchangeAccept packet = new PacketActionExchangeAccept();
            SendPacket(packet);
        }

        public void DoCounterExchange()
        {
            PacketActionExchangeCounter packet = new PacketActionExchangeCounter();
            SendPacket(packet);
        }

        public void DoDeclineExchange()
        {
            PacketActionExchangeDecline packet = new PacketActionExchangeDecline();
            SendPacket(packet);
        }

        public void DoCancelExchange()
        {
            PacketActionExchangeCancel packet = new PacketActionExchangeCancel();
            SendPacket(packet);
        }

        public void DoExchangePlayerSelect(string uuid, string toUuid)
        {
            PacketActionExchangePlayerSelect packet =
                new PacketActionExchangePlayerSelect(uuid, toUuid);
            SendPacket(packet);
        }

        public void DoExchangeTradeSelect(
            string uuid, bool recipient,
            int value, PacketActionExchangeTradeSelect.SelectType type)
        {
            PacketActionExchangeTradeSelect packet =
                new PacketActionExchangeTradeSelect(
                    uuid, recipient, value, type);
            SendPacket(packet);
        }

        private async void SendPacket(Packet packet)
        {
#if UNITY_EDITOR
            //if (!packet.Name.Equals("Ping"))
            Debug.Log("WebSocket send: " + packet.Serialize());
#endif
            try {
                await socket.Sock.SendText(packet.Serialize());
            }
            catch (System.Exception)
            {
                //Debug.LogException(e);
                Debug.LogWarning("WebSocket died. Will now quit the game...");
                ClientGameState.current.Crash();
            }
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
            {
                Debug.LogWarning(
                    string.Format("Received unserialisable packet {0}!",
                                  stringData));
                return;
            }
            switch (p)
            {
            case PacketException packet:
                OnError(packet); break;
            case PacketPing packet:
                OnPing(packet); break;
            case PacketChat packet:
                OnMessage(packet); break;
            case PacketAppletPrepare packet:
                OnGameLoad(packet); break;
            case PacketGameStart packet:
                OnGameStart(packet); break;
            case PacketGameStartDice packet:
                OnGameStartDice(packet); break;
            case PacketGameStartDiceResults packet:
                OnGameStartDiceResult(packet); break;
            case PacketGameEnd packet:
                OnGameEnd(packet); break;
            case PacketRoundStart packet:
                OnRoundStart(packet); break;
            case PacketRoundDiceResults packet:
                OnRoundDiceResult(packet); break;
            case PacketRoundRandomCard packet:
                OnRoundRandomCard(packet); break;
            case PacketPlayerMove packet:
                OnMove(packet); break;
            case PacketPlayerReconnect packet:
                OnReconnect(packet); break;
            case PacketPlayerDisconnect packet:
                OnDisconnect(packet); break;
            case PacketPlayerValid packet:
                OnPlayerValidated(packet); break;
            case PacketPlayerUpdateBalance packet:
                OnBalanceUpdate(packet); break;
            case PacketPlayerEnterPrison packet:
                OnEnterPrison(packet); break;
            case PacketPlayerExitPrison packet:
                OnExitPrison(packet); break;
            case PacketPlayerDefeat packet:
                OnDefeat(packet); break;
            case PacketActionTimeout packet:
                OnActionTimeout(packet); break;
            case PacketActionAuctionProperty packet:
                OnAuctionStart(packet); break;
            case PacketAuctionBid packet:
                OnAuctionBid(packet); break;
            case PacketAuctionEnd packet:
                OnAuctionEnd(packet); break;
            case PacketActionStart packet:
                OnActionStart(packet); break;
            case PacketActionBuyHouseSucceed packet:
                OnBuyHouse(packet); break;
            case PacketActionSellHouseSucceed packet:
                OnSellHouse(packet); break;
            case PacketActionBuyPropertySucceed packet:
                OnBuyProperty(packet); break;
            case PacketActionMortgageSucceed packet:
                OnMortgage(packet); break;
            case PacketActionUnmortgageSucceed packet:
                OnUnmortgage(packet); break;
            case PacketActionExchange packet:
                OnExchange(packet); break;
            case PacketActionExchangeSend packet:
                OnExchangeSend(packet); break;
            case PacketActionExchangeAccept packet:
                OnExchangeAccept(packet); break;
            case PacketActionExchangeCancel packet:
                OnExchangeCancel(packet); break;
            case PacketActionExchangeCounter packet:
                OnExchangeCounter(packet); break;
            case PacketActionExchangeDecline packet:
                OnExchangeDecline(packet); break;
            case PacketActionExchangePlayerSelect packet:
                OnExchangePlayerSelect(packet); break;
            case PacketActionExchangeTradeSelect packet:
                OnExchangeTradeSelect(packet); break;
            }
        }

    }

}
