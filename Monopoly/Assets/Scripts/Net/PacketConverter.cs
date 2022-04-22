/*
 * PacketConverter.cs
 * JSON string deserialiser class for communication packets.
 * 
 * Date created : 19/02/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using UnityEngine;

using Monopoly.Net.Packets;

namespace Monopoly.Net
{

    /*
     * See: https://stackoverflow.com/a/30579193
     */
    /**
     * <summary>
     *     JSON string deserialiser for communication packets.
     * </summary>
     */
    public class PacketConverter : JsonConverter
    {

        private static readonly Dictionary<string, Type> packetTypes =
            new Dictionary<string, Type>()
        {
                // TODO: ADD THE OTHERS HERE

            // LOBBY PACKETS
            { "AppletPrepare", typeof(PacketAppletPrepare) },
            { "BroadcastUpdateLobby", typeof(PacketBroadcastUpdateLobby) },
            { "BroadcastUpdateRoom", typeof(PacketBroadcastUpdateRoom) },
            { "BroadcastNewRoomToLobby", typeof(PacketBroadcastNewRoomToLobby) },
            { "CreateGame", typeof(PacketCreateGame) },
            { "CreateGameSucceed", typeof(PacketCreateGameSucceed) },
            { "EnterRoom", typeof(PacketEnterRoom) },
            { "EnterRoomSucceed", typeof(PacketEnterRoomSucceed) },
            { "LeaveRoom", typeof(PacketLeaveRoom) },
            { "LeaveRoomSucceed", typeof(PacketLeaveRoomSucceed) },
            { "DeleteRoom", typeof(PacketDeleteRoom) },
            { "DeleteRoomSucceed", typeof(PacketDeleteRoomSucceed) },
            { "LaunchGame", typeof(PacketLaunchGame) },
            { "NewHost", typeof(PacketNewHost) },
            { "AddBot", typeof(PacketAddBot) },

            // GAME PACKETS
            { "Exception", typeof(PacketException) },
            { "AppletReady", typeof(PacketAppletReady) },
            { "Ping", typeof(PacketPing) },
            { "Chat", typeof(PacketChat) },
            { "GameStart", typeof(PacketGameStart) },
            { "GameEnd", typeof(PacketGameEnd) },
            { "PlayerReconnect", typeof(PacketPlayerReconnect) },
            { "PlayerDisconnect", typeof(PacketPlayerDisconnect) },
            { "PlayerDefeat", typeof(PacketPlayerDefeat) },
            { "PlayerValid", typeof(PacketPlayerValid) },
            { "GameStartDice", typeof(PacketGameStartDice) },
            { "GameStartDiceThrow", typeof(PacketGameStartDiceThrow) },
            { "GameStartDiceResults", typeof(PacketGameStartDiceResults) },
            { "RoundStart", typeof(PacketRoundStart) },
            { "RoundDiceThrow", typeof(PacketRoundDiceThrow) },
            { "RoundDiceChoice", typeof(PacketRoundDiceChoice) },
            { "RoundDiceResults", typeof(PacketRoundDiceResults) },
            { "RoundRandomCard", typeof(PacketRoundRandomCard) },
            { "PlayerMove", typeof(PacketPlayerMove) },
            { "PlayerEnterPrison", typeof(PacketPlayerEnterPrison) },
            { "PlayerExitPrion", typeof(PacketPlayerExitPrison) },
            { "ActionStart", typeof(PacketActionStart) },
            { "ActionTimeout", typeof(PacketActionTimeout) },
            { "ActionExchange", typeof(PacketActionExchange) },
            { "ActionExchangePlayerSelect",
                typeof(PacketActionExchangePlayerSelect) },
            { "ActionExchangeTradeSelect",
                typeof(PacketActionExchangeTradeSelect) },
            { "ActionExchangeSend", typeof(PacketActionExchangeSend) },
            { "ActionExchangeDecline", typeof(PacketActionExchangeDecline) },
            { "ActionExchangeCounter", typeof(PacketActionExchangeCounter) },
            { "ActionExchangeAccept", typeof(PacketActionExchangeAccept) },
            { "ActionExchangeCancel", typeof(PacketActionExchangeCancel) },
            { "PlayerUpdateBalance", typeof(PacketPlayerUpdateBalance) },
            { "ActionAuctionProperty", typeof(PacketActionAuctionProperty) },
            { "AuctionRound", typeof(PacketAuctionRound) },
            { "AuctionBid", typeof(PacketAuctionBid) },
            { "AuctionConcede", typeof(PacketAuctionConcede) },
            { "AuctionEnd", typeof(PacketAuctionEnd) },
            { "ActionBuyProperty", typeof(PacketActionBuyProperty) },
            { "ActionBuyPropertySucceed",
                typeof(PacketActionBuyPropertySucceed) },
            { "ActionMortgageProperty", typeof(PacketActionMortgageProperty) },
            { "ActionMortgageSucceed",
                typeof(PacketActionMortgageSucceed) },
            { "ActionUnmortgageProperty",
                typeof(PacketActionUnmortgageProperty) },
            { "ActionUnmortgageSucceed",
                typeof(PacketActionUnmortgageSucceed) },
            { "ActionBuyHouse", typeof(PacketActionBuyHouse) },
            { "ActionBuyHouseSucceed", typeof(PacketActionBuyHouseSucceed) },
            { "ActionSellHouse", typeof(PacketActionSellHouse) },
            { "ActionSellHouseSucceed", typeof(PacketActionSellHouseSucceed) }
        };

        /**
         * <summary>
         *     Packet converter class to be used by Newtonsoft for
         *     deserialisation resolution.
         * </summary>
         */
        private class PacketClassConverter : DefaultContractResolver
        {
            protected override JsonConverter ResolveContractConverter
                (Type objectType)
            {
                if (typeof(Packet).IsAssignableFrom(objectType) &&
                    !objectType.IsAbstract)
                    /* pretend TableSortRuleConvert is not specified
                       (thus avoiding a stack overflow) */
                    return null;
                return base.ResolveContractConverter(objectType);
            }
        }

        private static JsonSerializerSettings SpecifiedSubclassConversion =
            new JsonSerializerSettings()
            {
                ContractResolver = new PacketClassConverter()
            };

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Packet));
        }

        public override bool CanRead => true;
        public override bool CanWrite => false;

        public override object ReadJson(JsonReader reader, Type objectType,
                                        object existingValue,
                                        JsonSerializer serializer)
        {
            try
            {
                JObject jo = JObject.Load(reader);
                string name = jo["name"].Value<string>();
                Type type = null;
                if (packetTypes.ContainsKey(name))
                    type = packetTypes[name];
                if (type != null)
                {
                    return (Packet)
                        JsonConvert.DeserializeObject(jo.ToString(),
                                                      type,
                                                      SpecifiedSubclassConversion);
                }
                return null;
            }
            catch (JsonReaderException e)
            {
                Debug.LogException(e);
                return null;

            }
            //throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value,
                                       JsonSerializer serializer)
        {
            /* will not occur because CanWrite = false */
            throw new NotImplementedException();
        }

    }

}
