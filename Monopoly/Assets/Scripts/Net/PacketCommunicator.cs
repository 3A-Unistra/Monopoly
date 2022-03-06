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
using NativeWebSocket;

using Monopoly.Net.Packets;

namespace Monopoly.Net
{

    public class PacketCommunicator
    {

        public delegate void PacketDelegate<T>(T packet);

        // TODO: Send
        // - AppletReady
        // - GameStartDiceThrow
        // - ActionAuctionProperty
        // - ActionBuyHouse
        // - ActionSellHouse
        // - ActionBuyProperty
        // - ActionMortgageProperty
        // - ActionUnmortgateProperty
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
        public event PacketDelegate<PacketPlayerDisconnect>  Onisconnect;
        public event PacketDelegate<PacketPlayerUpdateBalance> OnBalanceUpdate;
        public event PacketDelegate<PacketPlayerUpdateProperty>
                                                         OnPropertyUpdate;
        public event PacketDelegate<PacketPlayerEnterPrison> OnEnterPrison;
        public event PacketDelegate<PacketPlayerExitPrison>  OnExitPrison;
        public event PacketDelegate<PacketPlayerDefeat>      OnDefeat;
        public event PacketDelegate<PacketActionTimeout>     OnActionTimeout;
        public event PacketDelegate<PacketAuctionEnd>        OnAuctionEnd;
        public event PacketDelegate<PacketAuctionRound>      OnAuctionRound;
        public event PacketDelegate<PacketActionStart>       OnActionStart;
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

        private WebSocket socket;

        public PacketCommunicator(WebSocket socket)
        {
            if (socket == null)
            {
                throw new System.NullReferenceException(
                    "Packet communicator instantiated with null socket!");
            }
            this.socket = socket;
            socket.OnMessage += (data) => ReceivePacket(data);
        }

        private async void SendPacket(Packet packet)
        {
            await socket.SendText(packet.Serialize());
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
            /*case PacketException packet:
                OnError(packet); break;
            case PacketAppletPrepare packet:
                OnGameLoad(packet); break;
            case PacketAppletReady packet:
                OnGameReady(packet); break;
            case PacketPlayerUpdateBalance packet:
                OnBalanceUpdate(packet); break;
            case PacketPlayerUpdateProperty packet:
                OnPropertyUpdate(packet); break;
            case PacketPlayerMove packet:
                OnPositionUpdate(packet); break;
            case PacketPlayerDisconnect packet:
                OnPlayerDisconnect(packet); break;
            case PacketPlayerReconnect packet:
                OnPlayerReconnect(packet); break;
            case PacketPlayerEnterPrison packet:
                OnEnterPrison(packet); break;
            case PacketPlayerExitPrison packet:
                OnExitPrison(packet); break;
            case PacketRoundDiceThrow packet:
                OnDiceThrow(packet); break;
            case PacketRoundDiceResults packet:
                OnDiceResult(packet); break;*/
            }
        }

    }

}
