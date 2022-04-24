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

        [JsonProperty("dice_result")]
        public List<PacketGameStartDiceResultsInternal> DiceResult;

        public PacketGameStartDiceResults(
            List<PacketGameStartDiceResultsInternal> diceResult)
            : base("GameStartDiceResults")
        {
            this.DiceResult = diceResult;
        }

    }

}
