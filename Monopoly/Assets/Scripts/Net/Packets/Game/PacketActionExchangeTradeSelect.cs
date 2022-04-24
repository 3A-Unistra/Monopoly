/*
 * PacketActionExchangeTradeSelect.cs
 * 
 * Date created : 03/03/2022
 * Author       : Maxime MAIRE <maxime.maire2@etu.unistra.fr>
 *              : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Monopoly.Net.Packets
{

    public class PacketActionExchangeTradeSelect : Packet
    {

        public enum SelectType
        {
            PROPERTY = 0,
            MONEY = 1,
            LEAVE_JAIL_COMMUNITY_CARD = 2,
            LEAVE_JAIL_CHANCE_CARD = 3
        }

        [JsonProperty("player_token")]
        public string PlayerId { get; private set; }

        [JsonProperty("update_affects_recipient")]
        public bool AffectsRecipient { get; private set; }

        [JsonProperty("value")]
        public int Value { get; private set; }

        [JsonProperty("exchange_type")]
        public int ExchangeTypeInt { get; private set; }

        public SelectType ExchangeType { get; private set; }

        public PacketActionExchangeTradeSelect(string playerId, 
            bool affectsRecipient, int value, int exchangeType)
            : base("ActionExchangeTradeSelect")
        {
            this.PlayerId = playerId;
            this.AffectsRecipient = affectsRecipient;
            this.Value = value;
            this.ExchangeTypeInt = exchangeType;
            this.ExchangeType = (SelectType) exchangeType;
        }

        public PacketActionExchangeTradeSelect(string playerId,
            bool affectsRecipient, int value, SelectType exchangeType)
            : base("ActionExchangeTradeSelect")
        {
            this.PlayerId = playerId;
            this.AffectsRecipient = affectsRecipient;
            this.Value = value;
            this.ExchangeTypeInt = (int) exchangeType;
            this.ExchangeType = exchangeType;
        }

    }

}
