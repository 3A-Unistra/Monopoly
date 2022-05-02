/*
 * PacketActionExchangeTransfer.cs
 * 
 * Date created : 03/05/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Monopoly.Net.Packets
{
    
    public class PacketActionExchangeTransfer : Packet
    {

        public enum TransferType
        {
            PROPERTY = 0,
            CARD = 1
        }

        [JsonProperty("player_token")]
        public string FromId { get; private set; }

        [JsonProperty("player_to")]
        public string ToId { get; private set; }

        [JsonProperty("transfer_type")]
        public TransferType Type { get; private set; }

        [JsonProperty("value")]
        public int Value { get; private set; }

        public PacketActionExchangeTransfer(string fromId, string toId,
                                            TransferType type, int value)
            : base("ActionExchangeTransfer")
        {
            this.FromId = fromId;
            this.ToId = toId;
            this.Type = type;
            this.Value = value;
        }

    }

}
