/*
 * PacketPlayerDefeat.cs
 * 
 * Date created : 30/04/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

using Monopoly.Net;

namespace Monopoly.Net.Packets
{

    public class PacketPlayerDefeat : Packet
    {

        [JsonProperty("player_token")]
        public string PlayerId { get; private set; }

        public PacketPlayerDefeat(string playerId) : base("PlayerDefeat")
        {
            this.PlayerId = playerId;
        }

    }

}
