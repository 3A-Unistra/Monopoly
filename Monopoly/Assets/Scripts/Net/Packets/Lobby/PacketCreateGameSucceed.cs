﻿/*
 * PacketCreateGameSucceed.cs
 * 
 * Date created : 14/04/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

using Monopoly.Net;

namespace Monopoly.Net.Packets
{

    public class PacketCreateGameSucceed : Packet
    {

        [JsonProperty("game_token")]
        public string GameToken { get; private set; }

        [JsonProperty("player_token")]
        public string PlayerId { get; private set; }

        [JsonProperty("username")]
        public string Username { get; private set; }

        [JsonProperty("piece")]
        public int PieceId { get; private set; }

        public PacketCreateGameSucceed(string gameToken, string playerId,
                                       string username, int pieceId)
            : base("CreateGameSucceed")
        {
            this.GameToken = gameToken;
            this.PlayerId = playerId;
            this.Username = username;
            this.PieceId = pieceId-1;
        }

    }

}
