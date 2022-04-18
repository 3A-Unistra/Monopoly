/*
 * PacketGameStartDiceResult.cs
 * 
 * Date created : 03/03/2022
 * Author       : Maxime MAIRE <maxime.maire2@etu.unistra.fr>
 *              : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Monopoly.Net;

namespace Monopoly.Net.Packets
{

    public class PacketGameStartDiceResults : Packet
    {
        [JsonProperty("player_token")]
        public string PlayerId { get; private set; }

        [JsonProperty("dice_results")]
        public Dictionary<string, int> DiceResult;

        /*[JsonProperty("dice1")]
        public int Dice1 { get; private set; }

        [JsonProperty("dice2")]
        public int Dice2 { get; private set; }

        [JsonProperty("win")]
        public bool Win { get; private set; }*/

        public PacketGameStartDiceResults(string playerId,
                                          Dictionary<string, int> diceResult)
            : base("GameStartDiceResults")
        {
            this.PlayerId = playerId;
            this.DiceResult = diceResult;
        }

    }

}
