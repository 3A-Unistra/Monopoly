/*
 * PacketEnterRoom.cs
 * 
 * Date created : 15/04/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

using Monopoly.Net;

namespace Monopoly.Net.Packets
{

    public class PacketStatusRoom : Packet
    {

        [JsonProperty("game_token")]
        public string LobbyToken { get; private set; }

        [JsonProperty("game_name")]
        public string GameName { get; private set; }

        [JsonProperty("nb_players")]
        public int NumberPlayers { get; private set; }

        [JsonProperty("max_nb_players")]
        public int MaxNumberPlayers { get; private set; }

        [JsonProperty("players")]
        public List<string> Players { get; private set; }

        [JsonProperty("option_auction")]
        public bool EnableAuctions { get; private set; }

        [JsonProperty("option_double_on_start")]
        public bool EnableDoubleOnGo { get; private set; }

        [JsonProperty("option_first_round_buy")]
        public bool EnableFirstTourBuy { get; private set; }

        [JsonProperty("option_max_rounds")]
        public bool MaxRounds { get; private set; }

        [JsonProperty("option_max_time")]
        public bool TurnTimeout { get; private set; }

        [JsonProperty("starting_balance")]
        public int StartingBalance { get; private set; }

        public PacketStatusRoom(string lobbyToken,
                                string gameName,
                                int numPlayers,
                                int maxPlayers,
                                List<string> players,
                                bool auctions,
                                bool doubleOnGo,
                                bool firstTourBuy,
                                bool maxRounds,
                                bool turnTimeout,
                                int startingBalance) : base("StatusRoom")
        {
            this.LobbyToken = lobbyToken;
            this.GameName = gameName;
            this.NumberPlayers = numPlayers;
            this.MaxNumberPlayers = maxPlayers;
            this.Players = players;
            this.EnableAuctions = auctions;
            this.EnableDoubleOnGo = doubleOnGo;
            this.EnableFirstTourBuy = firstTourBuy;
            this.MaxRounds = maxRounds;
            this.TurnTimeout = turnTimeout;
            this.StartingBalance = startingBalance;
        }

    }

}
