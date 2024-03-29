﻿/*
 * PacketPlayerUpdateBalance.cs
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

    public class PacketPlayerUpdateBalance : Packet
    {
        [JsonProperty("player_token")]
        public string PlayerId { get; private set; }

        [JsonProperty("old_balance")]
        public int OldBalance { get; private set; }

        [JsonProperty("new_balance")]
        public int NewBalance { get; private set; }

        [JsonProperty("reason")]
        public string Reason { get; private set; }

        public PacketPlayerUpdateBalance(string playerId, int oldBalance, 
            int newBalance, string reason) : base("PlayerUpdateBalance")
        {
            this.PlayerId = playerId;
            this.OldBalance = oldBalance;
            this.NewBalance = newBalance;
            this.Reason = reason;
        }

    }

}
