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

        public PacketActionExchangeSend() : base("ActionExchangeSend")
        {
        }

    }

}
