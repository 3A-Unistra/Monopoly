/*
 * Fichier.cs
 * Fichier définissant la classe joueur et ses interaction
 * 
 * Date created : 22/02/2022
 * Author       : Christophe Pierson <christophe.pierson@etu.unistra.fr>
 */


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monopoly.Classes
{
    public class Player
    {
        public string Id;
        {
            get;
            set;
        }
        public string Name;
        {
            get;
            set;
        }
        public Character Character;
        {
            get;
            set;
        }
        public int Position;
        {
            get;
            set;
        }
        public int Score;
        {
            get;
            set;
        }
        public int Money;
        {
            get;
            set;
        }
        public bool InJail;
        {
            get;
            set;
        }
        public bool Bankrupt;
        {
            get;
            set;
        }
        public bool ChanceJailCard;
        {
            get;
            set;
        }
        public bool CommunityJailCard;
        {
            get;
            set;
        }
        public bool Bot;
        {
            get;
            set;
        } 
        public Player(string id, string name, Character character)
        {
            this.Id = id;
            this.Character = character;
            this.Name = name;
            this.Money = 1500;
            this.Score = 0;
            this.Position = 0;
            this.InJail = false;
            this.Bankrupt = false;
            this.ChanceJailCard = false;
            this.CommunityJailCard = false;
            this.Bot = false;
        }
        public void EnterPrison()
        {
            Position = 9;
            InJail = true;
        }
        public void ExitPrison()
        {
            InJail = false;
        }

        public void TransferMoney(Player to, int amount)
        {
            if( Money > amount)
            {
                Money -= amount;
                to.Money += amount;
            }
            else
            {
                to.Money += Money;
                Money = 0;
                Bankrupt = true;
            }
        }
        void TransferProperty(Player to, OwnableSquare p)
        {
            p.setOwner(to);
        }

        void TransferCard(Player to, Card card)
        {
            if ((card.type == "CHANCE") && (ChanceJailCard == true))
            {
                ChanceJailCaMonopoly.Playerrd = false;
                to.ChanceJailCard = true;
            }
            else if((card.type == "COMMUNITY")&& (CommunityJailCard == true))
            {
                CommunityJailCard = false;
                to.CommunityJailCard = true;
            }
            else
            {
                throw new InvalidOperationException("Carte indisponible");
            }
        }
    }
}
