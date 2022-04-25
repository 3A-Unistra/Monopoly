/*
 * PacketAuctionEnd.cs
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

    public class PacketAuctionEnd : Packet
    {

        [JsonProperty("player_token")]
        public string PlayerId { get; private set; }

        [JsonProperty("bid")]
        public int Bid { get; private set; }

        [JsonProperty("remaining_time")]
        public int RemainingTurnTime { get; private set; }

        public PacketAuctionEnd(string playerId, int bid, int remainingTime)
            : base("AuctionEnd")
        {
            this.PlayerId = playerId;
            this.Bid = bid;
            this.RemainingTurnTime = remainingTime;
        }

    }

}
