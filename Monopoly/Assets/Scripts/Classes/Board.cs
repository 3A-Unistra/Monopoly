/*
 * Fichier.cs
 * Fichier définissant la classe plateau et ses 
 * interaction avec les différentes cases
 * 
 * Date created : 22/02/2022
 * Author       : Christophe Pierson <christophe.pierson@etu.unistra.fr>
 *              : Rayan Marmar <rayan.marmar@etu.unistra.fr>
 */


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Monopoly.Classes
{
    /**
    * <summary>
    * Class Board listing the squares of the board
    * depending on their type and their set
    * Also store money for the player to get at free parking
    * </summary>
    */
    public class Board
    {
        
        public static Bank BoardBank { get; set; }
        public static List<Square> Elements { get; set; }
        public static int PrisonSquare { get; set; }
        public static int BoardMoney { get; set; }
        public static List<Card> ChanceDeck {get; set;}
        public static List<Card> CommunityDeck {get; set;}  
        public static List<Player> players {get; set;}      

        /**
        * <summary>
        * get the square at position pos from the list of elements of the board
        * </summary>
        * <param name="pos">
        * int pos the position of the square we want to get
        * </param>         
        * <returns>
        * the square at position pos
        * </returns>               
        */
        public Square GetSquare(int pos)
        {
            return Elements[pos];
        }

        public List<OwnableSquare> SquareOwned(Player p)
        {
            List<OwnableSquare> tempList = new List<OwnableSquare>();
            foreach (Square s in Elements)
            {
                if (s.GetType() == typeof(OwnableSquare))
                {
                    OwnableSquare sos = (OwnableSquare) s;
                    if (sos.Owner == p)
                        tempList.Add(sos);
                }
            }

            return tempList;
        }

        /**
         * <summary>
         * returns the set of properties of color c>
         * </summary>
         * <param name="c">
         * The color of the set.
	     * </param>
         * <returns>
         * propertySet the set of properties of color c
         * </returns>        
         */

        public static List<PropertySquare> GetPropertySet(Color c)
        {
            List<PropertySquare> propertySet = new List<PropertySquare>();
            foreach (Square s in Elements)
            {
                if (s.GetType() == typeof(PropertySquare))
                {
                    PropertySquare sps = (PropertySquare) s;
                    if (sps.Col.Equals(c))
                        propertySet.Add(sps);
                }
            }

            return propertySet;
        }

        /**
        * <summary>
        * player p gets the money from free parking
        * resets free parking money
        * </summary>
        * <param name="p">
        * player p the player who landed on free parking
        * </param>
        */
        public void FreeParking(Player p)
        {
            p.Money += BoardMoney;
            BoardMoney = 0;
        }

        /**
        * <summary>
        * method to add i amount of money to player p money
        * </summary>
        * <param name="p">
        * player p the player who recieves money
        * </param>         
        * <param name="i">        
        * int i the amount of money
        * </param>        
        */
        public void AddMoney(Player p, int i)
        {
            p.Money += i;
        }

        /**
         * <summary>
         * This function is responsible about the house buying action in the
         * game.
         * </summary>
         * <param name="ps">
         * The property that the player wishes to build a house on.
         * </param>
         * <param name="p">
         * The player who wants to buy the house.
         * </param>
         */
        public void BuyHouse(PropertySquare ps, Player p)
        {
            Color c = ps.Col; //getting the property's color to verify if
                              //the player own a full set of this color
                              
            //List of the properties that belong to this set of color
            List<PropertySquare> sameColorFields = GetPropertySet(c);
            
            bool playerOwnSet = true; // if true => the player owns
                                      // the full set
                                      
            bool enoughMoney = true; // if true => the player have enough
                                     // money to buy the desired house
                                     
            bool balanced = true;   // if true => the number of houses on
                                    // the set properties is balanced
                                    
            int minimumHouse = 100; // A random big number 
            
            //Searching the minimum number of houses owned on a property
            //of this set and verifying if the player owns every one of
            //these properties
            foreach (var field in sameColorFields)
            {
                if (field != ps) // Searching for the other
                                 // properties of this set
                    minimumHouse = Math.Min(minimumHouse, field.NbHouse);
                if (field.Owner != p)
                {
                    playerOwnSet = false;
                    //SHOULD THROW AN EXCEPTION
                }
            }

            if (p.Money < ps.HouseCost)
                //SHOULD THROW AN EXCEPTION
                enoughMoney = false;

            if (ps.NbHouse - minimumHouse > 0)
                //SHOULD THROW AN EXCEPTION
                balanced = false;


            if (balanced && playerOwnSet && enoughMoney
                && BoardBank.BuyHouse())
            {
                p.Money -= ps.HouseCost; // Paying the cost of the house
                ps.NbHouse++; // adding a house to the property
            }
        }

        /**
         * <summary>
         * This function is responsible about the house selling action in the
         * game.
         * </summary>
         * <param name="ps">
         * The property that the player wishes to sell a house from.
         * </param>
         * <param name="p">
         * The player who wants to buy the house.
         * </param>
         */
        public void SellHouse(PropertySquare ps, Player p)
        {
            Color c = ps.Col;//getting the property's color to verify if
                            //the player own a full set of this color
                            
            //List of the properties that belong to this set of color
            List<PropertySquare> sameColorFields = GetPropertySet(c);
            
            bool minimumHousesOwned = true; // if true => the player already
                                            // owns a house on this property
                                            
            bool enoughHouses = true; // if true => the bank has enough houses
                                      // to do the exchange of 4 houses to a 
                                      // hotel in case the player was selling
                                      // a hotel
                                      
            bool balanced = true; // if true => the number of houses owned on
                                  // the set properties is balanced
                                  
            int maximumHouses = -1; // random negative number
            if (ps.NbHouse < 0)
                minimumHousesOwned = false;
            if (ps.NbHouse == 5 && BoardBank.NbHouse < 4)
                enoughHouses = false;
            
            //searching for the maximum number of houses owned
            //on this set properties
            foreach (var field in sameColorFields)
            {
                if (field != ps)
                    maximumHouses = Math.Max(maximumHouses, field.NbHouse);
            }

            if ( maximumHouses- ps.NbHouse > 0)
                //SHOULD THROWN AN EXCEPTION
                balanced = false;

            if (balanced && minimumHousesOwned && enoughHouses)
            {
                p.Money += ps.HouseCost; // refund the house cost
                ps.NbHouse--; // reduce the number of houses by 1
            }
        }
        /**
        * <summary>
        * gets a random card from the list of community cards
        * </summary>
        * <returns>
        * random community Card
        * </returns>
        */
        public Card GetRandomCommunityCard()
        {
            System.Random rnd = new System.Random();
            int randcard = rnd.Next(0, CommunityDeck.Count-1);
            return CommunityDeck[randcard];
        }
        /**
        * <summary>
        * gets a random card from the list of chance cards
        * </summary>
        * <returns>
        * random chance Card
        * </returns>
        */
        public Card GetRandomChanceCard()
        {
            System.Random rnd = new System.Random();
            int randcard = rnd.Next(0, ChanceDeck.Count-1);
            return ChanceDeck[randcard];
        }
        /**
        * <summary>
        * returns a OutOfJail card to its deck after use
        * </summary>
        * <param name="type">
        * </param>
        */        
        public void ReturnCard(string type)
        {
            if (type == "Chance")
            {
                ChanceDeck.Add(new card(new Card("Chance",15,"OutOfJail")));
            }
            else if (type == "Community")
            {
                CommunityDeck.Add(new card(new Card("Community",15,"OutOfJail")));
            }
            else
            {
                throw new InvalidOperationException("invalid parameter");
            }

        }
    }
}

