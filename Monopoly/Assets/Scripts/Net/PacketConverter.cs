using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

using Monopoly.Net.Packets;

namespace Monopoly.Net
{

    /*
     * See: https://stackoverflow.com/a/30579193
     */
    public class PacketConverter : JsonConverter
    {

        private static readonly Dictionary<string, Type> packetTypes =
            new Dictionary<string, Type>()
        {
            { "AppletPrepare", typeof(PacketAppletPrepare) },
            { "AppletReady", typeof(PacketAppletReady) },
            { "GameStart", typeof(PacketGameStart) },
            { "PlayerReconnect", typeof(PacketPlayerReconnect) },
            { "PlayerDisconnect", typeof(PacketPlayerDisconnect) },
            { "GameStartDice", typeof(PacketGameStartDice) },
            { "GameStartDiceThrow", typeof(PacketGameStartDiceThrow) },
            { "GameStartDiceResults", typeof(PacketGameStartDiceResults) },
            { "RoundStart", typeof(PacketRoundStart) },
            { "RoundDiceThrow", typeof(PacketRoundDiceThrow) },
            { "RoundDiceChoice", typeof(PacketRoundDiceChoice) },
            { "RoundDiceResults", typeof(PacketRoundDiceResults) },
            { "RoundRandomCard", typeof(PacketRoundRandomCard) },
            { "RoundMove", typeof(PacketRoundMove) },
            { "PlayerEnterPrison", typeof(PacketPlayerEnterPrison) },
            { "PlayerExitPrion", typeof(PacketPlayerExitPrison) },
            { "ActionExchange", typeof(PacketActionExchange) },
            { "ActionExchangePlayerSelect",
                typeof(PacketActionExchangePlayerSelect) },
            { "ActionExchangeTradeSelect",
                typeof(PacketActionExchangeTradeSelect) },
            { "ActionExchangeSend", typeof(PacketActionExchangeSend) },
            { "ActionExchangeDecline", typeof(PacketActionExchangeDecline) },
            { "AcitonExchangeCounter", typeof(PacketActionExchangeCounter) },
            { "ActionExchangeAccept", typeof(PacketActionExchangeAccept) },
            { "ActionExchangeCancel", typeof(PacketActionExchangeCancel) },
            { "PlayerUpdateProperty", typeof(PacketPlayerUpdateProperty) },
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
            { "ActionMortgagePropertySucceed",
                typeof(PacketActionMortgagePropertySucceed) },
            { "ActionUnmortgageProperty",
                typeof(PacketActionUnmortgageProperty) },
            { "ActionUnmortgagePropertySucceed",
                typeof(PacketActionUnmortgagePropertySucceed) },
            { "ActionBuyHouse", typeof(PacketActionBuyHouse) },
            { "ActionBuyHouseSucceed", typeof(PacketActionBuyHouseSucceed) },
            { "ActionSellHouse", typeof(PacketActionSellHouse) },
            { "ActionSellHouseSucceed", typeof(PacketActionSellHouseSucceed) }
        };

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
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value,
                                       JsonSerializer serializer)
        {
            /* will not occur because CanWrite = false */
            throw new NotImplementedException();
        }

    }

}
