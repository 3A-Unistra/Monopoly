﻿/*
 * PacketActionSellHouseSucceed.cs
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

    public class PacketActionSellHouseSucceed : Packet
    {
        [JsonProperty("player_token")]
        public string PlayerId { get; private set; }

        [JsonProperty("property_id")]
        public int HouseId { get; private set; }

        public PacketActionSellHouseSucceed(string playerId, int houseId) : 
            base("ActionSellHouseSucceed")
        {
            this.PlayerId = playerId;
            this.HouseId = houseId;
        }

    }

}
