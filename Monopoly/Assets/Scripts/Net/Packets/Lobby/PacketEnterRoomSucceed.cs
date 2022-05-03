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

        [JsonProperty("host_token")]
        public string HostId { get; private set; }

        [JsonProperty("username")]
        public string Username { get; private set; }

        public PacketEnterRoomSucceed(string lobbyToken, string playerId,
                                      string hostId, string username)
            : base("EnterRoomSucceed")
        {
            this.LobbyToken = lobbyToken;
            this.PlayerId = playerId;
            this.HostId = hostId;
            this.Username = username;
        }

    }

}
