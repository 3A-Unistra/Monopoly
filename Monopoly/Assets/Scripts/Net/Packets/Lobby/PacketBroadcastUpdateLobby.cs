/*
 * BroadcastNewRoomToLobby.cs
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
    
    public class PacketBroadcastUpdateLobby : Packet
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

        [JsonProperty("reason")]
        public UpdateReason Reason { get; private set; }

        public PacketBroadcastUpdateLobby(string lobbyToken, int reason) 
            : base("BroadcastUpdateLobby")
        {
            this.LobbyToken = lobbyToken;
            this.Reason = (UpdateReason) reason;
        }

    }

}
