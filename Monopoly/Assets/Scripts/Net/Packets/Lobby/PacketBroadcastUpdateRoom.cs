/*
 * PacketBroadcastUpdateRoom.cs
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
    
    public class PacketBroadcastUpdateRoom : Packet
    {

        public enum UpdateReason
        {
            NEW_CONNECTION = 0,
            NEW_PLAYER     = 1,
            PLAYER_LEFT    = 2,
            ROOM_DELETED   = 3,
            ROOM_CREATED   = 4,
            HOST_LEFT      = 5,
            LAUNCHING_GAME = 6,
            NEW_BOT        = 7
        }

        [JsonProperty("game_token")]
        public string LobbyToken { get; private set; }

        [JsonProperty("nb_players")]
        public int NumberPlayers { get; private set; }

        [JsonProperty("player")]
        public string Player { get; private set; }

        [JsonProperty("reason")]
        public UpdateReason Reason { get; private set; }

        public PacketBroadcastUpdateRoom(string lobbyToken, int nbPlayers,
                                         string player, int reason) 
            : base("BroadcastUpdateRoom")
        {
            this.LobbyToken = lobbyToken;
            this.NumberPlayers = nbPlayers;
            this.Player = player;
            this.Reason = (UpdateReason) reason;
        }

    }

}
