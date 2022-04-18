/*
 * PacketLaunchGame.cs
 * 
 * Date created : 12/04/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

using Monopoly.Net;

namespace Monopoly.Net.Packets
{

    public class PacketLaunchGame : Packet
    {

        public PacketLaunchGame() : base("LaunchGame")
        {
        }

    }

}
