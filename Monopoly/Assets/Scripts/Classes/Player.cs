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

        /**
         * <summary>
         * The integer Doubles is used to keep count of the number of
         * consecutive doubles that the player got.
         * </summary>
         */
        public int Doubles
        {
            get;
            set;
        }
        
        /**
        * <summary>
        * getter setter
        * the boolean InJail states if the player serves en sentence in jail
        * </summary>
        */
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
         * Counter of the number of turns passed in jail.
         * </summary>
         */
        public int JailTurns
        {
            get;
            set;
        }
        
        /**
        * <summary>
        * Player constructor
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
        public void EnterPrison()
        {
            Position = 9;
            InJail = true;
        }
        public void ExitPrison()
        {
            InJail = false;
        }
        /**
        * <summary>
        * the player gives money to player to
        * if he do not have as much money than the anoumt specified
        * he goes bankrupt
        * </summary>
        * <param name="to">
        * Player to the player which the money is give
        * </param>       
        * <param name="amount">        
        * int amount the amount of given money 
        * </param>        
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
        * </summary>
        * <param name="to">
        * Player to is the player which the property is given
        * </param>
        * <param name="p">                
        * Ownable square p is the given property 
        * </param>               
        */          
        public void TransferProperty(Player to, OwnableSquare p)
        {
            p.Owner = to;
        }
        /**
        * <summary>
        * the player gives an out of jail card to another player 
        * for an exchange, if the player doesn't have the card
        * it throws an exeption error
        * </summary>
        * <param name="to">
        * Player to is the player which the card is given
        * </param>
        * <param name="cardType">        
        * string cardType is the given card type
        * </param>               
        */ 
        public void TransferCard(Player to, string cardType)
        {
            if ((cardType == "CHANCE") && (ChanceJailCard == true))
            {
                ChanceJailCard = false;
                to.ChanceJailCard = true;
            }
            else if((cardType == "COMMUNITY") && (CommunityJailCard == true))
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
