/*
 * PacketPing.cs
 * 
 * Date created : 14/03/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;

using Monopoly.Net;

namespace Monopoly.Net.Packets
{

    public class PacketPing : Packet
    {

        public PacketPing() : base("Ping")
        {

        }

    }

}
