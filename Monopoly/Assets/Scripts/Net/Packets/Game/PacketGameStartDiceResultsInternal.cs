/*
 * PacketGameStartDiceResultInternal.cs
 * 
 * Date created : 23/04/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Monopoly.Net;

namespace Monopoly.Net.Packets
{

    public class PacketGameStartDiceResultsInternal
    {

        [JsonProperty("player_token")]
        public string PlayerId { get; private set; }

        [JsonProperty("dice1")]
        public int Dice1 { get; private set; }

        [JsonProperty("dice2")]
        public int Dice2 { get; private set; }

        [JsonProperty("win")]
        public bool Win { get; private set; }

        public PacketGameStartDiceResultsInternal(string playerId,
                                                  int dice1, int dice2,
                                                  bool win)
        {
            this.PlayerId = playerId;
            this.Dice1 = dice1;
            this.Dice2 = dice2;
            this.Win = win;
        }

    }

}
