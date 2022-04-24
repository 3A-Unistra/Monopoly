/*
 * PacketPing.cs
 * 
 * Date created : 14/03/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

using Monopoly.Net;

namespace Monopoly.Net.Packets
{

    public class PacketPing : Packet
    {

        [JsonProperty("player_token")]
        public string PlayerId { get; private set; }

        public PacketPing(string playerId) : base("Ping")
        {
            this.PlayerId = playerId;
        } 

    }

}
