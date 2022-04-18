/*
 * PacketActionStart.cs
 * 
 * Date created : 03/03/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

using Monopoly.Net;

namespace Monopoly.Net.Packets
{

    public class PacketActionStart : Packet
    {

        public PacketActionStart() : base("ActionStart")
        {
        }

    }

}
