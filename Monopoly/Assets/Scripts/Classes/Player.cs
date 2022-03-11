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
    /**
    * <summary>
    * Class defining the player characteristics, position, name
    * score, in game money, Injail states if the player is in jail
    * and two boolean to check if the player has an out of jail card
    * a bot will replace the player if Bot is true
    * </summary>
    */
    public class Player
    {
        /**
        * <summary>
        * getter setter
        * player's id
        * </summary>
        */         
        public string Id
        {
            get;
            set;
        }
        /**
        * <summary>
        * getter setter
        * the player's name
        * </summary>
        */         
        public string Name
        {
            get;
            set;
        }
        /**
        * <summary>
        * getter setter
        * Player's pawn on the board
        * </summary>
        */         
        public Character Character
        {
            get;
            set;
        }
        /**
        * <summary>
        * getter setter
        * Player position on the board,
        * position begin at 0 and ends at 39  as there are 40 squares
        * on a monopoly board and the square list begins at 0
        * </summary>
        */            
        public int Position
        {
            get;
            set;             
        }
        /**
        * <summary>
        * getter setter
        * the score which will be shown at the end of the game
        * </summary>
        */        
        public int Score
        {
            get;
            set;
        }
        /**
        * <summary>
        * getter setter
        * the amount of money the player has
        * </summary>
        */         
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
        /**
        * <summary>
        * getter setter
        * the boolean Bankrupt states if the player has lost all his money 
        * and cannot pay his debt, if that's the case he loses the game.
        * </summary>
        */        
        public bool Bankrupt
        {
            get;
            set;
        }
        /**
        * <summary>
        * getter setter
        * boolean stating if the player possesses the out of jail
        * chance card
        * </summary>
        */         
        public bool ChanceJailCard
        {
            get;
            set;
        }
        /**
        * <summary>
        * getter setter
        * boolean stating if the player possesses the out of jail
        * community card
        * </summary>
        */         
        public bool CommunityJailCard
        {
            get;
            set;
        }
        /**
        * <summary>
        * getter setter
        * boolean stating if the player is controlled
        * by an AI
        * </summary>
        */         
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
        /**
        * <summary>
        * The player is sent to jail,
        * his position is set to 9(Jail Square) 
        * and InJail bool set to true
        * </summary>
        */ 
        public void EnterPrison()
        {
            Position = 10;
            InJail = true;
            Doubles = 0;
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
            JailTurns = 0;
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
                throw new InvalidOperationException("Unsufficient money");
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
                throw new InvalidOperationException("Unavailable card");
            }
        }
    }
}
