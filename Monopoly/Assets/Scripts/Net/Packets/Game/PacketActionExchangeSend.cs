﻿/*
 * PacketActionExchangeSend.cs
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

    public class PacketActionExchangeSend : Packet
    {
        [JsonProperty("id_init_request")]
        public string PlayerIdInitRequest { get; private set; }

        [JsonProperty("id_of_requested")]
        public string PlayerIdRequested { get; private set; }

        [JsonProperty("content_trade")]
        public string ContentTrade { get; private set; }

        public PacketActionExchangeSend(string playerIdInitRequest, 
            string playerIdRequested, string contentTrade) : 
            base("ActionExchangeSend")
        {
            this.PlayerIdInitRequest = playerIdInitRequest;
            this.PlayerIdRequested = playerIdRequested;
            this.ContentTrade = contentTrade;
        }

    }

}