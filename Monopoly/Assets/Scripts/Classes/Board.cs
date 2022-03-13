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
using System.Linq;
using UnityEngine;
using Monopoly.Classes;

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
        public static List<Square> Elements { get; private set; }
        public static int PrisonSquare { get; set; }
        public int BoardMoney { get; set; }
        public static List<Card> ChanceDeck {get; private set;}
        public static List<Card> CommunityDeck {get; private set;}
        public static List<Player> players {get; set;}


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
		    Color brown;
            ColorUtility.TryParseHtmlString("#955436", out brown);
            Color lightBlue;
            ColorUtility.TryParseHtmlString("#aae0fa", out lightBlue);
            Color pink;
            ColorUtility.TryParseHtmlString("#d93a96", out pink);
		    Color orange;
            ColorUtility.TryParseHtmlString("#f7941d", out orange);
            Color red;
            ColorUtility.TryParseHtmlString("#ed1b24", out red);
            Color yellow;
            ColorUtility.TryParseHtmlString("#fef20", out yellow);
		    Color green;
            ColorUtility.TryParseHtmlString("#1fb25a", out green);
		    Color blue;
            ColorUtility.TryParseHtmlString("#0072bb", out blue);                                    
			Elements.Add(new Square(SquareType.Go,0,"square0",null));
			Elements.Add(new PropertySquare(SquareType.Field,1,"square1",null,
				60,2,50,10,30,90,160,250,brown));
	        Elements.Add(new Square(SquareType.Community,2,"square2",null));
	        Elements.Add(new PropertySquare(SquareType.Field,3,"square3",null,
				60,4,50,20,60,180,320,450,brown));
	        Elements.Add(new Square(SquareType.Tax,4,"square4",null));
	        Elements.Add(new OwnableSquare(SquareType.Station,5,"square5",null,200,50));
	        Elements.Add(new PropertySquare(SquareType.Field,6,"square6",null,
		        100,6,50,30,90,270,400,550,lightBlue));
	        Elements.Add(new Square(SquareType.Chance,7,"square7",null));
	        Elements.Add(new PropertySquare(SquareType.Field,8,"square8",null,
		        100,6,50,30,90,270,400,550,lightBlue));
	        Elements.Add(new PropertySquare(SquareType.Field,9,"square9",null,
		        120,8,50,40,100,300,450,600,lightBlue));
	        Elements.Add(new Square(SquareType.Prison,10,"square10",null));
	        Elements.Add(new PropertySquare(SquareType.Field,11,"square11",null,
		        140,10,100,50,150,450,625,750,pink));
	        Elements.Add(new OwnableSquare(SquareType.Company,12,"square12",null,150,6));
	        Elements.Add(new PropertySquare(SquareType.Field,13,"square13",null,
		        140,10,100,50,150,450,625,750, pink));
	        Elements.Add(new PropertySquare(SquareType.Field,14,"square14",null,
		        160,12,100,60,180,500,700,900,pink));
	        Elements.Add(new OwnableSquare(SquareType.Station,15,"square15",null,200,50));
	        Elements.Add(new PropertySquare(SquareType.Field,16,"square16",null,
		        180,14,100,70,200,550,700,900,orange));
	        Elements.Add(new Square(SquareType.Community,17,"square17",null));
	        Elements.Add(new PropertySquare(SquareType.Field,18,"square18",null,
		        180,14,100,70,200,550,700,950,orange));
	        Elements.Add(new PropertySquare(SquareType.Field,19,"square19",null,
		        200,16,100,90,220,600,800,1000,orange));
	        Elements.Add(new Square(SquareType.Parking,20,"square20",null));
	        Elements.Add(new PropertySquare(SquareType.Field,21,"square21",null,
		        220,18,150,90,250,700,875,1050,red));
	        Elements.Add(new Square(SquareType.Chance,22,"square22",null));
	        Elements.Add(new PropertySquare(SquareType.Field,23,"square23",null,
		        220,18,150,90,250,700,875,1050,red));
	        Elements.Add(new PropertySquare(SquareType.Field,24,"square24",null,
		        240,20,150,100,300,750,925,1100,red));
	        Elements.Add(new OwnableSquare(SquareType.Station,25,"square25",null,200,50));
	        Elements.Add(new PropertySquare(SquareType.Field,26,"square26",null,
		        260,22,150,110,330,800,975,1150,yellow));
	        Elements.Add(new PropertySquare(SquareType.Field,27,"square27",null,
		        260,22,150,110,330,800,975,1150,yellow));
	        Elements.Add(new OwnableSquare(SquareType.Company,28,"square28",null,150,6));
	        Elements.Add(new PropertySquare(SquareType.Field,29,"square29",null,
		        280,24,150,120,360,850,1025,1200,yellow));
	        Elements.Add(new GoToJailSquare(SquareType.GoToJail,30,"square30",null));
	        Elements.Add(new PropertySquare(SquareType.Field,31,"square31",null,
		        300,26,200,130,390,900,1100,1275,green));
	        Elements.Add(new PropertySquare(SquareType.Field,32,"square32",null,
		        300,26,200,130,390,900,1100,1275,green));
			Elements.Add(new Square(SquareType.Community,33,"square33",null));
			Elements.Add(new PropertySquare(SquareType.Field,34,"square34",null,
				320,28,200,150,450,1000,1200,1400,green));
			Elements.Add(new OwnableSquare(SquareType.Station,35,"square35",null,200,50));  
			Elements.Add(new Square(SquareType.Chance,36,"square36",null)); 			
			Elements.Add(new PropertySquare(SquareType.Field,37,"square37",null,
				350,35,200,175,500,1100,1300,1500,blue));
			Elements.Add(new Square(SquareType.Tax,38,"square38",null));    	
			Elements.Add(new PropertySquare(SquareType.Field,39,"square39",null,
			400,50,200,200,600,1400,1700,2000,blue)); 
			
			
			ChanceDeck = new List<Card>();
			ChanceDeck.Add(new Card("Chance",0,"..."));
			ChanceDeck.Add(new Card("Chance",1,"..."));
			ChanceDeck.Add(new Card("Chance",2,"..."));
			ChanceDeck.Add(new Card("Chance",3,"..."));
			ChanceDeck.Add(new Card("Chance",4,"..."));
			ChanceDeck.Add(new Card("Chance",5,"..."));
			ChanceDeck.Add(new Card("Chance",6,"..."));
			ChanceDeck.Add(new Card("Chance",7,"..."));
			ChanceDeck.Add(new Card("Chance",8,"..."));
			ChanceDeck.Add(new Card("Chance",9,"..."));
			ChanceDeck.Add(new Card("Chance",10,"..."));
			ChanceDeck.Add(new Card("Chance",11,"..."));
			ChanceDeck.Add(new Card("Chance",12,"..."));
			ChanceDeck.Add(new Card("Chance",13,"..."));
			ChanceDeck.Add(new Card("Chance",14,"..."));
			ChanceDeck.Add(new Card("Chance",15,"OutOfJail"));
			CommunityDeck = new List<Card>();
			CommunityDeck.Add(new Card("Community",0,"..."));
			CommunityDeck.Add(new Card("Community",1,"..."));
			CommunityDeck.Add(new Card("Community",2,"..."));	
			CommunityDeck.Add(new Card("Community",3,"..."));
			CommunityDeck.Add(new Card("Community",4,"..."));
			CommunityDeck.Add(new Card("Community",5,"..."));
			CommunityDeck.Add(new Card("Community",6,"..."));	
			CommunityDeck.Add(new Card("Community",7,"..."));	
			CommunityDeck.Add(new Card("Community",8,"..."));
			CommunityDeck.Add(new Card("Community",9,"..."));
			CommunityDeck.Add(new Card("Community",10,"..."));	
			CommunityDeck.Add(new Card("Community",11,"..."));
			CommunityDeck.Add(new Card("Community",12,"..."));
			CommunityDeck.Add(new Card("Community",13,"..."));
			CommunityDeck.Add(new Card("Community",14,"..."));	
			CommunityDeck.Add(new Card("Community",15,"OutofJail"));

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
         * <return>
         * propertySet the set of properties of color c
         * </return>        
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
            Color c = ps.Col; //getting the property's color to verify if
            //the player own a full set of this color
                              
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
            
            Color c = ps.Col; //getting the property's color to verify if
            //the player own a full set of this color
                              
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

            if (ps.NbHouse - minimumHouse > 0 || p.Money < ps.HouseCost)
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
            
            Color c = ps.Col;//getting the property's color to verify if
            //the player own a full set of this color
                            
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
            if (CanBuyHouse(p,ps))
            {
                p.Money -= ps.HouseCost; // Paying the cost of the house
                ps.NbHouse++; // adding a house to the property
            }else 
                return;
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
            if (CanSellHouse(p,ps))
            {
                p.Money += ps.HouseCost; // refund the house cost
                ps.NbHouse--; // reduce the number of houses by 1
            }else
                return;
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
        * returns a OutOfJail card to its deck after use
        * </summary>
        * <param name="type">
        * The given card type.
        * </param>
        */        
        public void ReturnCard(string type)
        {
	        if (type == "CHANCE")
	        {
		        ChanceDeck.Add(new Card("CHANCE",15,"OutOfJail"));
	        }
	        else if (type == "COMMUNITY")
	        {
		        CommunityDeck.Add(new Card("COMMUNITY",15,"OutOfJail"));
	        }
	        else
	        {
		        throw new InvalidOperationException("invalid parameter");
	        }
        }
        public static void Move(Player p, int r)
        {
            if (p.Position + r >= 40)
                p.Money += 200;
            p.Position = (p.Position + r)%40;
        }

    }
}

