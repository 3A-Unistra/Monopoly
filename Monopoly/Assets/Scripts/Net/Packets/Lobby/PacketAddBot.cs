/*
 * PacketAddBot.cs
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

    public class PacketAddBot : Packet
    {

        [JsonProperty("game_token")]
        public string LobbyToken { get; private set; }

        [JsonProperty("bot_difficulty")]
        public int Difficulty { get; private set; }

        public PacketAddBot(string lobbyToken,
                            int difficulty) : base("AddBot")
        {
            this.LobbyToken = lobbyToken;
            this.Difficulty = difficulty;
        }

    }

}
