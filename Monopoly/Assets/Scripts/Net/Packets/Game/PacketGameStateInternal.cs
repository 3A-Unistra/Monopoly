/*
 * PacketGameStartInternal.cs
 * 
 * Date created : 18/04/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

using Monopoly.Net;

namespace Monopoly.Net.Packets
{

    public class PacketGameStateInternal
    {

        [JsonProperty("player_token")]
        public string PlayerId { get; private set; }

        [JsonProperty("name")]
        public string PlayerName { get; private set; }

        [JsonProperty("bot")]
        public bool IsBot { get; private set; }

        [JsonProperty("money")]
        public int Money { get; private set; }

        [JsonProperty("position")]
        public int Position { get; private set; }

        [JsonProperty("jail_turns")]
        public int JailTurns { get; private set; }

        [JsonProperty("jail_cards")]
        public Dictionary<string, bool> JailCards { get; private set; }

        [JsonProperty("in_jail")]
        public bool IsInJail { get; private set; }

        [JsonProperty("bankrupt")]
        public bool IsBankrupt { get; private set; }

        [JsonProperty("piece")]
        public int Piece { get; private set; }

        public PacketGameStateInternal(string playerId, string name, bool bot,
                                       int money, int position, int jailTurns,
                                       Dictionary<string, bool> jailCards,
                                       bool inJail, bool bankrupt, int piece)
        {
            this.PlayerId = playerId;
            this.PlayerName = name;
            this.IsBot = bot;
            this.Money = money;
            this.Position = position;
            this.JailTurns = jailTurns;
            this.JailCards = jailCards;
            this.IsInJail = inJail;
            this.IsBankrupt = bankrupt;
            this.Piece = piece-1;
        }

    }

}
