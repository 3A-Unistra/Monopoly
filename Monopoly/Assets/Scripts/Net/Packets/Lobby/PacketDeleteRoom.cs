/*
 * PacketDeleteRoom.cs
 * 
 * Date created : 15/04/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

using Monopoly.Net;

namespace Monopoly.Net.Packets
{

    public class PacketDeleteRoom : Packet
    {

        [JsonProperty("player_token")]
        public string PlayerId { get; private set; }

        public PacketDeleteRoom(string playerId)
            : base("DeleteRoom")
        {
            this.PlayerId = playerId;
        }

    }

}
