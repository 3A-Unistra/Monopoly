/*
 * PacketAuctionRound.cs
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

    public class PacketAuctionRound : Packet
    {
        [JsonProperty("property")]
        public string Property { get; private set; }

        [JsonProperty("id_seller")]
        public string SellerId { get; private set; }

        [JsonProperty("current_price")]
        public int CurrentPrice { get; private set; }

        public PacketAuctionRound() : base("AuctionRound")
        {

        }

    }

}
