/*
 * PacketGameWin.cs
 * 
 * Date created : 03/05/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

using Monopoly.Net;

namespace Monopoly.Net.Packets
{

    public class PacketGameWin : Packet
    {

        [JsonProperty("player_token")]
        public string PlayerId { get; private set; }

        public PacketGameWin(string playerId) : base("GameWin")
        {
            this.PlayerId = playerId;
        }

    }

}
