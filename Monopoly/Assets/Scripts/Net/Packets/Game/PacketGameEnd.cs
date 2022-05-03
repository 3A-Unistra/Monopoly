/*
 * PacketGameEnd.cs
 * 
 * Date created : 03/05/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;

using Monopoly.Net;

namespace Monopoly.Net.Packets
{

    public class PacketGameEnd : Packet
    {

        public PacketGameEnd() : base("GameEnd")
        {

        }

    }

}
