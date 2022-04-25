/*
 * PacketAuctionBid.cs
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

    public class PacketAuctionBid : Packet
    {
        [JsonProperty("player_token")]
        public string BidderId { get; private set; }

        [JsonProperty("bid")]
        public int NewPrice { get; private set; }

        public PacketAuctionBid(string bidderId, int newPrice) : 
            base("AuctionBid")
        {
            this.BidderId = bidderId;
            this.NewPrice = newPrice;
        }

    }

}
