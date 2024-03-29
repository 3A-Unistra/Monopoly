﻿/*
 * PacketPlayerEnterPrison.cs
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

    public class PacketPlayerEnterPrison : Packet
    {
        [JsonProperty("player_token")]
        public string PlayerId { get; private set; }

        public PacketPlayerEnterPrison(string playerId) : 
            base("PlayerEnterPrison")
        {
            this.PlayerId = playerId;
        }

    }

}
