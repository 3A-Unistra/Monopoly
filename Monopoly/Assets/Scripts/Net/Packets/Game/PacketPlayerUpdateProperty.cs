/*
 * PacketPlayerUpdateProperty.cs
 * 
 * Date created : 03/05/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Monopoly.Net.Packets
{
    
    public class PacketPlayerUpdateProperty : Packet
    {

        [JsonProperty("player_token")]
        public string PlayerId { get; private set; }

        [JsonProperty("property_id")]
        public int PropertyId { get; private set; }

        public PacketPlayerUpdateProperty(string playerId, int propertyId)
            : base("PlayerUpdateProperty")
        {
            this.PlayerId = playerId;
            this.PropertyId = propertyId;
        }

    }

}
