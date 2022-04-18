/*
 * PacketBroadcastNewRoomToLobby.cs
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
    
    public class PacketBroadcastNewRoomToLobby : Packet
    {

        [JsonProperty("game_token")]
        public string LobbyToken { get; private set; }

        [JsonProperty("game_name")]
        public string GameName { get; private set; }

        [JsonProperty("nb_players")]
        public int NumberPlayers { get; private set; }

        [JsonProperty("max_nb_players")]
        public int MaxPlayers { get; private set; }

        [JsonProperty("is_private")]
        public bool Private { get; private set; }

        [JsonProperty("has_password")]
        public bool Password { get; private set; }

        public PacketBroadcastNewRoomToLobby(string lobbyToken, string gameName,
                                             int numPlayers, int maxPlayers,
                                             bool privateGame, bool password) 
            : base("BroadcastNewRoomToLobby")
        {
            this.LobbyToken = lobbyToken;
            this.GameName = gameName;
            this.NumberPlayers = numPlayers;
            this.MaxPlayers = maxPlayers;
            this.Private = privateGame;
            this.Password = password;
        }

    }

}
