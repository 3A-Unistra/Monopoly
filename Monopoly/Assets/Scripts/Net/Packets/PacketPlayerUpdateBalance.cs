﻿using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

using Monopoly.Net;

namespace Monopoly.Net.Packets
{

    public class PacketPlayerUpdateBalance : Packet
    {

        [JsonProperty("id_player")]
        public string PlayerId
        {
            get;
            private set;
        }

        [JsonProperty("old_balance")]
        public int OldBalance
        {
            get;
            private set;
        }

        [JsonProperty("new_balance")]
        public int NewBalance
        {
            get;
            private set;
        }

        public PacketPlayerUpdateBalance() : base("PlayerUpdateBalance")
        {

        }

    }

}
