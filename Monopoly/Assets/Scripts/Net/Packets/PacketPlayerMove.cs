/*
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
        [JsonProperty("id_moving_player")]
        public string MovingPlayerId { get; private set; }

        [JsonProperty("destination_case")]
        public int DestinationSquare { get; private set; }

        public PacketPlayerMove(string movingPlayerId, 
            int destinationSquare) : base("PlayerMove")
        {
            this.MovingPlayerId = movingPlayerId;
            this.DestinationSquare = destinationSquare;
        }

    }

}
