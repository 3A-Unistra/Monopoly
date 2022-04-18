/*
 * PacketRoundRandomCard.cs
 * 
 * Date created : 03/03/2022
 * Author       : Maxime MAIRE <maxime.maire2@etu.unistra.fr>
 *              : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Monopoly.Net;

namespace Monopoly.Net.Packets
{

    public class PacketRoundRandomCard : Packet
    {
        [JsonProperty("player_token")]
        public string PlayerId { get; private set; }

        [JsonProperty("is_community")]
        public bool IsCommunity { get; private set; }

        [JsonProperty("card_id")]
        public int CardId { get; private set; }

        public PacketRoundRandomCard(string playerId, bool isCommunity, 
            int cardId) : base("RoundRandomCard")
        {
            this.PlayerId = playerId;
            this.IsCommunity = isCommunity;
            this.CardId = cardId;
        }

    }

}
