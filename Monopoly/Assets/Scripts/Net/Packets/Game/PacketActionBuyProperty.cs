﻿/*
 * PacketActionBuyProperty.cs
 * 
 * Date created : 01/03/2022
 * Author       : Maxime MAIRE <maxime.maire2@etu.unistra.fr>
 *              : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Monopoly.Net;

namespace Monopoly.Net.Packets
{

    public class PacketActionBuyProperty : Packet
    {
        [JsonProperty("player_token")]
        public string PlayerId { get; private set; }

        [JsonProperty("property_id")]
        public int Property { get; private set; }


        public PacketActionBuyProperty(string playerId, int property) : 
            base("ActionBuyProperty")
        {
            this.PlayerId = playerId;
            this.Property = property;
        }

    }

}
