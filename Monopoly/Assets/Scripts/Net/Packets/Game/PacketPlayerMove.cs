﻿/*
 * PacketPlayerMove.cs
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

    public class PacketPlayerMove : Packet
    {
        [JsonProperty("player_token")]
        public string MovingPlayerId { get; private set; }

        [JsonProperty("destination")]
        public int DestinationSquare { get; private set; }

        [JsonProperty("instant")]
        public bool Instant { get; private set; }

        public PacketPlayerMove(string movingPlayerId, 
            int destinationSquare, bool instant) : base("PlayerMove")
        {
            this.MovingPlayerId = movingPlayerId;
            this.DestinationSquare = destinationSquare;
            this.Instant = instant;
        }

    }

}
