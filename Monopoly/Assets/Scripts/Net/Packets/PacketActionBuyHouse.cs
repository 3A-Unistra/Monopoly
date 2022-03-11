/*
 * PacketActionBuyHouse.cs
 * 
 * Date created : 01/03/2022
 * Author       : Maxime MAIRE <maxime.maire2@etu.unistra.fr>
 *              : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Monopoly.Net;

namespace Monopoly.Net.Packets
{

    public class PacketActionBuyHouse : Packet
    {
        [JsonProperty("id_player")]
        public string PlayerId { get; private set; }
        
        [JsonProperty("id_house")]
        public int IdHouse { get; private set; }

        public PacketActionBuyHouse() : base("ActionActionBuyHouse")
        {

        }

    }

}
