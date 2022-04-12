/*
 * PacketAuctionEnd.cs
 * 
 * Date created : 03/03/2022
 * Author       : Maxime MAIRE <maxime.maire2@etu.unistra.fr>
 *              : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;

using Monopoly.Net;

namespace Monopoly.Net.Packets
{

    public class PacketAuctionEnd : Packet
    {

        public PacketAuctionEnd() : base("AuctionEnd")
        {

        }

    }

}
