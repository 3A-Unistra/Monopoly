/*
 * PacketRoundDiceResults.cs
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
    
    public class PacketRoundDiceResults : Packet
    {
        [JsonProperty("id_player")]
        public string PlayerId { get; private set; }

        [JsonProperty("dice2")]
        public int DiceResult1 { get; private set; }

        [JsonProperty("dice1")]
        public int DiceResult2 { get; private set; }

        public enum ResultEnum
        {
            ROLL_DICE = 0,
            JAIL_PAY = 1,
            JAIL_CARD_CHANCE = 2,
            JAIL_CARD_COMMUNITY = 3
        }

        public ResultEnum Reason { get; private set; }

        [JsonProperty("result")]
        private int Result { get; set; }

        public PacketRoundDiceResults(string playerId,
                              int diceResult1, int diceResult2,
                              int result) :
            this(playerId, diceResult1, diceResult2, (ResultEnum) result)
        {
        }

        public PacketRoundDiceResults(string playerId,
                                      int diceResult1, int diceResult2,
                                      ResultEnum reason) : 
            base("RoundDiceResults")
        {
            this.PlayerId = playerId;
            this.DiceResult1 = diceResult1;
            this.DiceResult2 = diceResult2;
            this.Reason = reason;
        }

    }

}
