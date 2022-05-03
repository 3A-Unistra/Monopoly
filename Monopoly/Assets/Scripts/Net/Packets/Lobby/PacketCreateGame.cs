/*
 * PacketCreateGame.cs
 * 
 * Date created : 14/04/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

using Monopoly.Net;

namespace Monopoly.Net.Packets
{

    public class PacketCreateGame : Packet
    {

        [JsonProperty("player_token")]
        public string PlayerId { get; private set; }

        [JsonProperty("username")]
        public string Username { get; private set; }

        [JsonProperty("max_nb_players")]
        public int MaxPlayers { get; private set; }

        [JsonProperty("password")]
        public string Password { get; private set; }

        [JsonProperty("game_name")]
        public string GameName { get; private set; }

        [JsonProperty("is_private")]
        public bool Private { get; private set; }

        [JsonProperty("starting_balance")]
        public int StartBalance { get; private set; }

        [JsonProperty("option_auction")]
        public bool EnableAuctions { get; private set; }

        [JsonProperty("option_double_on_start")]
        public bool EnableDoubleGo { get; private set; }

        [JsonProperty("option_max_time")]
        public int TurnTime { get; private set; }

        [JsonProperty("option_max_rounds")]
        public int MaxNumberRounds { get; private set; }

        [JsonProperty("option_first_round_buy")]
        public bool CanBuyFirstCircle { get; private set; }

        public PacketCreateGame(string playerId, string username,
                                int maxPlayers,
                                string password, string gameName,
                                bool privateGame, int startBalance,
                                bool auctions, bool doubleGo,
                                int turnTime, int maxRounds,
                                bool canBuyFirstCircle) : base("CreateGame")
        {
            this.PlayerId = playerId;
            this.Username = username;
            this.MaxPlayers = maxPlayers;
            this.Password = password;
            this.GameName = gameName;
            this.Private = privateGame;
            this.StartBalance = startBalance;
            this.EnableAuctions = auctions;
            this.EnableDoubleGo = doubleGo;
            this.TurnTime = turnTime;
            this.MaxNumberRounds = maxRounds;
            this.CanBuyFirstCircle = canBuyFirstCircle;
        }

    }

}
