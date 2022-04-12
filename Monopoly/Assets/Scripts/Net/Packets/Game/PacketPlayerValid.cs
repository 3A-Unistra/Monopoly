/*
 * PacketPlayerValid.cs
 * 
 * Date created : 14/03/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Monopoly.Net;

namespace Monopoly.Net.Packets
{
    
    public class PacketPlayerValid : Packet
    {

        public PacketPlayerValid() : base("PlayerValid")
        {

        }

    }

}
