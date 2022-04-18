/*
 * PacketChat.cs
 * 
 * Date created : 08/04/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

using Monopoly.Net;

namespace Monopoly.Net.Packets
{

    public class PacketChat : Packet
    {

        [JsonProperty("player_token")]
        public string PlayerId { get; private set; }

        [JsonProperty("message")]
        public string Message { get; private set; }

        public PacketChat(string playerId, string message) : base("Chat")
        {
            this.PlayerId = playerId;
            this.Message = message;
        }

    }

}
