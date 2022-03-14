/*
 * PacketActionSellHouseSucceed.cs
 * 
 * Date created : 03/03/2022
 * Author       : Maxime MAIRE <maxime.maire2@etu.unistra.fr>
 *              : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Monopoly.Net;

namespace Monopoly.Net.Packets
{

    public class PacketActionSellHouseSucceed : Packet
    {
        [JsonProperty("id_player")]
        public string PlayerId { get; private set; }

        [JsonProperty("id_house")]
        public string HouseId { get; private set; }

        public PacketActionSellHouseSucceed(string playerId, string houseId) : 
            base("ActionActionSellHouseSucceed")
        {
            this.PlayerId = playerId;
            this.HouseId = houseId;
        }

    }

}
