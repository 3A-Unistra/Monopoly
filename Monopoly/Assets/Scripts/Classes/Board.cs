/*
 * Board.cs
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
using System.Linq;
using UnityEngine;
using Monopoly.Runtime;

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
        
        public Bank BoardBank { get; set; }
        public List<Square> Elements { get; private set; }
        public int PrisonSquare { get; set; }
        public int BoardMoney { get; set; }
        public List<Card> ChanceDeck {get; private set;}
        public List<Card> CommunityDeck {get; private set;}

        /**
         * <summary>
         * The constructor of the board class. It initializes all
         * the attributes and lists of this class.
         * </summary>
         */
        public Board()
        {
	        Elements = new List<Square>();
            BoardBank = new Bank(); 
            BoardMoney = 0;
            PrisonSquare = 10;
            List<Dictionary<string, int>> squares =
                ClientGameState.current.squareData;
            foreach (Dictionary<string, int> dic in squares)
            {
                int pos = dic["id"];
                switch (dic["type"])
                {
                    case 0: // go
                        Elements.Add(new GoSquare(pos));
                        break;
                    case 1: // tax
                        Elements.Add(new TaxSquare(pos, dic["value"]));
                        break;
                    case 2:  // regular property
                        Elements.Add(
                            new PropertySquare(pos,
                                                dic["buy_price"], dic["rent_base"],
                                                dic["house_price"],
                                                dic["rent_1"], dic["rent_2"],
                                                dic["rent_3"], dic["rent_4"],
                                                dic["rent_5"]));
                        break;
                    case 3: // station
                        Elements.Add(
                            new StationSquare(pos,
                                                dic["buy_price"], dic["rent_base"]));
                        break;
                    case 4: // company
                        Elements.Add(
                            new CompanySquare(pos, dic["buy_price"]));
                        break;
                    case 5: // prison
                        Elements.Add(new JailSquare(pos));
                        break;
                    case 6: // go to jail
                        Elements.Add(new GoToJailSquare(pos));
                        break;
                    case 7: //Free parking
                        Elements.Add(new FreeParkingSquare(pos));
                        break;
                    case 8: //community chest
                        Elements.Add(new CommunitySquare(pos));
                        break;
                    case 9: // chance 
                        Elements.Add(new ChanceSquare(pos));
                        break;
                }
            }

            ChanceDeck = new List<Card>();
            CommunityDeck = new List<Card>();
            for(int i = 0; i < 16; i++)
                ChanceDeck.Add(new Card(i, false));
            for (int i = 0; i < 16; i++)
                CommunityDeck.Add(new Card(i, true));
        }
        
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
                if (s is OwnableSquare)
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
         * <return>
         * propertySet the set of properties of color c
         * </return>        
         */

        public List<PropertySquare> GetPropertySet(Color c)
        {
            List<PropertySquare> propertySet = new List<PropertySquare>();
            foreach (Square s in Elements)
            {
                if (s is PropertySquare)
                {
                    PropertySquare sps = (PropertySquare) s;
                    if (PropertySquare.GetColorIndex(s.Id).Equals(c))
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
         * This function verifies if a player owns a set of properties
         * with the same color.
         * </summary>
         * <param name="p">
         * The player that owns the property.
         * </param>
         * <param name="ps">
         * The property at question.
         * </param>
         * <returns>
         * true if the player owns the set and false if not.
         * </returns>
         */
        public bool OwnSameColorSet(Player p, PropertySquare ps)
        {
            //getting the property's color to verify if
            //the player own a full set of this color
            Color c = PropertySquare.GetColorIndex(ps.Id);
                              
            //List of the properties that belong to this set of color
            List<PropertySquare> sameColorFields = GetPropertySet(c);
            foreach (var field in sameColorFields)
            {
                if (field.Owner != p)
                    return false;
            }
            return true;
        }

        /**
         * <summary>
         * This function verifies if a player can buy a house on the property.
         * </summary>
         * <param name="p">
         * The player that owns the property.
         * </param>
         * <param name="ps">
         * The property at question.
         * </param>
         * <returns>
         * true if the player can buy a house and false if not.
         * </returns>
         */
        public bool CanBuyHouse(Player p, PropertySquare ps)
        {
            if (!OwnSameColorSet(p, ps))
                return false;

            //getting the property's color to verify if
            //the player own a full set of this color
            Color c = PropertySquare.GetColorIndex(ps.Id);

            //List of the properties that belong to this set of color
            List<PropertySquare> sameColorFields = GetPropertySet(c);
            int minimumHouse = 100; // A random big number 
            //Searching the minimum number of houses owned on a property
            //of this set and verifying if the player owns every one of
            //these properties
            foreach (var field in sameColorFields)
            {
                if (field != ps) // Searching for the other
                    // properties of this set
                    minimumHouse = Math.Min(minimumHouse, field.NbHouse);
            }

            if (ps.NbHouse - minimumHouse > 0 || p.Money < ps.HouseCost || ps.NbHouse > 4)
                return false;

            return true;
        }
        
        /**
         * <summary>
         * This function verifies if a player can sell a house on the property.
         * </summary>
         * <param name="p">
         * The player that owns the property.
         * </param>
         * <param name="ps">
         * The property at question.
         * </param>
         * <returns>
         * true if the player can sell a house and false if not.
         * </returns>
         */
        public bool CanSellHouse(Player p, PropertySquare ps)
        {
            if (!OwnSameColorSet(p, ps))
                return false;
            if (ps.NbHouse < 1 || ps.NbHouse == 5 && BoardBank.NbHouse < 4)
                return false;

            //getting the property's color to verify if
            //the player own a full set of this color
            Color c = PropertySquare.GetColorIndex(ps.Id);

            //List of the properties that belong to this set of color
            List<PropertySquare> sameColorFields = GetPropertySet(c);
                                  
            int maximumHouses = -1; // random negative number
            
            //searching for the maximum number of houses owned
            //on this set properties
            foreach (var field in sameColorFields)
            {
                if (field != ps)
                    maximumHouses = Math.Max(maximumHouses, field.NbHouse);
            }

            if ( maximumHouses - ps.NbHouse > 0)
                return false;
            
            return true;
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
            if (CanBuyHouse(p, ps))
            {
                p.Money -= ps.HouseCost; // Paying the cost of the house
                ps.NbHouse++; // adding a house to the property
            }
            else
            {
                return;
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
            if (CanSellHouse(p, ps))
            {
                p.Money += ps.HouseCost; // refund the house cost
                ps.NbHouse--; // reduce the number of houses by 1
            }
            else
            {
                return;
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
            int randcard = rnd.Next(0, CommunityDeck.Count);
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
            int randcard = rnd.Next(0, ChanceDeck.Count);
            return ChanceDeck[randcard];
        }
        
        /**
        * <summary>
        * </summary>
        */      
        
        public void ReturnCard(bool community, Card c)
        {
            if (community)
                CommunityDeck.Add(c);
            else
                ChanceDeck.Add(c);

        }

        public static void Move(Player p, int r)
        {
            if (p.Position + r >= 40)
                p.Money += 200;
            p.Position = (p.Position + r)%40;
        }

    }
}

