/*
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

        [JsonProperty("player_token")]
        public string PlayerId { get; private set; }

        [JsonProperty("piece")]
        public int PieceId { get; private set; }

        public PacketCreateGameSucceed(string playerId, int pieceId)
            : base("CreateGameSucceed")
        {
            this.PlayerId = playerId;
            this.PieceId = pieceId;
        }

    }

}
