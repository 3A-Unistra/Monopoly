/*
 * PacketStatusInternal.cs
 * 
 * Date created : 26/04/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Monopoly.Net.Packets
{

    public class PacketStatusInternal
    {

        [JsonProperty("player_token")]
        public string PlayerId { get; private set; }

        [JsonProperty("username")]
        public string Username { get; private set; }

        [JsonProperty("avatar_url")]
        public string AvatarURL { get; private set; }

        [JsonProperty("piece")]
        public int Piece { get; private set; }

        public PacketStatusInternal(string playerId, string username,
                                    string avatar, int piece)
        {
            this.PlayerId = playerId;
            this.Username = username;
            this.AvatarURL = avatar;
            this.Piece = piece;
        }

    }

}
