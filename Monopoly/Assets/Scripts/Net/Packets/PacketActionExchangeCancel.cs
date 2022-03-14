/*
 * PacketActionExchangeCancel.cs
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

    public class PacketActionExchangeCancel : Packet
    {
        [JsonProperty("reason")]
        public string ReasonCancel { get; private set; }

        public PacketActionExchangeCancel(string reasonCancel) : 
            base("ActionExchangeCancel")
        {
            this.ReasonCancel = reasonCancel;
        }

    }

}
