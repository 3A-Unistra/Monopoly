/*
 * PacketRoundDiceChoice.cs
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

    public class PacketRoundDiceChoice : Packet
    {

        public enum DiceChoice
        {
            ROLL_DICE = 0,
            JAIL_PAY = 1,
            JAIL_CARD = 2
        }

        [JsonProperty("player_token")]
        public string PlayerId { get; private set; }

        [JsonProperty("choice")]
        public DiceChoice Choice { get; private set; }

        public PacketRoundDiceChoice(string playerId, DiceChoice choice)
            : base("RoundDiceChoice")
        {
            this.PlayerId = playerId;
            this.Choice = choice;
        }

    }

}
