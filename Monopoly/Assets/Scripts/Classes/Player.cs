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
/**
* <summary>
* Class defining the player characteristics
* </summary>
*/
namespace Monopoly.Classes
{
    public class Player
    {
        public string Id
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public Character Character
        {
            get;
            set;
        }
        public int Position
        {
            get;
            set;
        }
        public int Score
        {
            get;
            set;
        }
        public int Money
        {
            get;
            set;
        }
        public bool InJail
        {
            get;
            set;
        }
        public bool Bankrupt
        {
            get;
            set;
        }
        public bool ChanceJailCard
        {
            get;
            set;
        }
        public bool CommunityJailCard
        {
            get;
            set;
        }
        public bool Bot
        {
            get;
            set;
        } 

        /**
        * <summary>
        * sets the new player attributes at the beginning of the game
        * following the Monopoly rules
        * the player is not a bot by default
        * </summary>
        */    
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
        /**
        * <summary>
        * The player is sent to jail,
        * his position is set to 9(Jail Square) 
        * and InJail bool set to true
        * </summary>
        */ 
        public void EnterPrison()
        {
            Position = 9;
            InJail = true;
        }
        /**
        * <summary>
        * the player is free, he can now roll his dices 
        * and moves across the board 
        * </summary>
        */     
        public void ExitPrison()
        {
            InJail = false;
        }
        /**
        * <summary>
        * the player gives money to player to
        * if he do not have as much money than the anoumt specified
        * he goes bankrupt
        * <parameter>
        * Player to the player which the money is given
        * int amount the amount of given money 
        * </summary>
        */  
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
        /**
        * <summary>
        * the player gives a property p to another player 
        * for an exchange
        * <parameter>
        * Player to is the player which the property is given
        * Ownable square p is the given property
        * </summary>
        */          
        void TransferProperty(Player to, OwnableSquare p)
        {
            p.Owner = to;
        }
        /**
        * <summary>
        * the player gives an out of jail card to another player 
        * for an exchange
        * <parameter>
        * Player to is the player which the card is given
        * Card card is the given card
        * </summary>
        */ 
        void TransferCard(Player to, Card card)
        {
            if ((card.type == "CHANCE") && (ChanceJailCard == true))
            {
                ChanceJailCard = false;
                to.ChanceJailCard = true;
            }
            else if((card.type == "COMMUNITY") && (CommunityJailCard == true))
            {
                CommunityJailCard = false;
                to.CommunityJailCard = true;
            }
            else
            {
                throw new InvalidOperationException("Unavailable card");
            }
        }
    }
}
