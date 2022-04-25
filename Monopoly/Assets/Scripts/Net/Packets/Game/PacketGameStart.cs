/*
 * PacketGameStart.cs
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

    public class PacketGameStart : Packet
    {

        [JsonProperty("game_name")]
        public string GameName { get; private set; }

        [JsonProperty("players")]
        public List<PacketGameStateInternal> Players { get; private set; }

        [JsonProperty("timeouts")]
        public Dictionary<string, int> Timeouts { get; private set; }

        public PacketGameStart(string gameName,
                               List<PacketGameStateInternal> players,
                               Dictionary<string, int> timeouts)
            : base("GameStart")
        {
            this.GameName = gameName;
            this.Players = players;
            this.Timeouts = timeouts;
        }

    }

}
