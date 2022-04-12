﻿/*
 * PacketAuctionConcede.cs
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

    public class PacketAuctionConcede : Packet
    {
        [JsonProperty("id_bidder")]
        public string BidderId { get; private set; }

        public PacketAuctionConcede(string bidderId) : base("AuctionConcede")
        {
            this.BidderId = bidderId;
        }

    }

}