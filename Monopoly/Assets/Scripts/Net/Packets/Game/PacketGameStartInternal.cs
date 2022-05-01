/*
 * PacketGameStartInternal.cs
 * 
 * Date created : 01/05/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Monopoly.Net.Packets
{

    public class PacketGameStartInternal
    {

        [JsonProperty("auction_enabled")]
        public bool EnableAuctions { get; private set; }

        [JsonProperty("go_case_double_money")]
        public bool EnableDoubleOnGo { get; private set; }

        [JsonProperty("first_round_buy")]
        public bool EnableFirstTourBuy { get; private set; }

        [JsonProperty("max_rounds")]
        public int MaxRounds { get; private set; } /* unused */

        [JsonProperty("max_time")]
        public int TurnTimeout { get; private set; } /* unused */

        [JsonProperty("is_private")]
        public bool IsPrivate { get; private set; } /* unused */

        public PacketGameStartInternal(bool auctions, bool doubleOnGo,
                                       bool firstTourBuy, int maxRounds,
                                       int timeout, bool isPrivate)
        {
            this.EnableAuctions = auctions;
            this.EnableDoubleOnGo = doubleOnGo;
            this.EnableFirstTourBuy = firstTourBuy;
            this.MaxRounds = maxRounds;
            this.TurnTimeout = timeout;
            this.IsPrivate = isPrivate;
        }

    }

}
