/*
 * PacketEnterRoomSucceed.cs
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

    public class PacketEnterRoomSucceed : Packet
    {

        [JsonProperty("game_token")]
        public string LobbyToken { get; private set; }

        [JsonProperty("player_token")]
        public string PlayerId { get; private set; }

        public PacketEnterRoomSucceed(string lobbyToken, string playerId)
            : base("EnterRoomSucceed")
        {
            this.LobbyToken = lobbyToken;
            this.PlayerId = playerId;
        }

    }

}
