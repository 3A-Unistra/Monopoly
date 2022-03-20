using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

using Monopoly.Net;

namespace Monopoly.Net.Packets
{

    public class PacketException : Packet
    {

        [JsonProperty("error")]
        public int Code { get; private set; }

        public PacketException() : base("Exception")
        {

        }

    }

}
