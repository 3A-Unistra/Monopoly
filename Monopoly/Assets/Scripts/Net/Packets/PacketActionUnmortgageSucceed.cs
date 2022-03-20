﻿/*
 * PacketActionUnmortgageSucceed.cs
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

    public class PacketActionUnmortgageSucceed : Packet
    {
        [JsonProperty("id_player")]
        public string PlayerId { get; private set; }

        [JsonProperty("property")]
        public string Property { get; private set; }


        public PacketActionUnmortgageSucceed(string playerId, 
            string property) : base("ActionUnmortgageSucceed")
        {
            this.PlayerId = playerId;
            this.Property = property;
        }

    }

}
