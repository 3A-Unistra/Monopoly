/*
 * PacketEnterRoom.cs
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

    public class PacketEnterRoom : Packet
    {

        [JsonProperty("game_token")]
        public string LobbyToken { get; private set; }

        [JsonProperty("password")]
        public string Password { get; private set; }

        public PacketEnterRoom(string lobbyToken,
                               string password) : base("EnterRoom")
        {
            this.LobbyToken = lobbyToken;
            this.Password = password;
        }

    }

}
