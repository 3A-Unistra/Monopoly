/*
 * PacketActionExchange.cs
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

    public class PacketActionExchange : Packet
    {
        [JsonProperty("player_token")]
        public string PlayerId { get; private set; }

        public PacketActionExchange(string playerId) : base("ActionExchange")
        {
            this.PlayerId = playerId;
        }

    }

}
