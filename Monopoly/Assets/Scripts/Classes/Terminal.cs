/*
 * Fichier.cs
 * Fichier définissant la classe TerminalInit 
 * initialise les paramètres de la partie au début du jeu
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
     *<summary>
     * class for initializing the game
	 * Adds all square elements one by one as they are all different
     * </summary>
     */
    public class TerminalInit
    {
		/**
     	* <summary>
     	* Converts hexadecimal color string into RGBA color,
		* needs to be used as the monopoly colors have been captured
		* and coded in hexadecimals
     	* </summary>
		* <param name="hexColor">
		* the color in hexadecimal
		* </param>
        * <returns>
        * the color in RGBA
        * </returns>
    	*/
		public Color HexToUnityColor(string hexColor)
		{
			Color unityColor;
			ColorUtility.TryParseHtmlString(hexColor, out unityColor);
			return unityColor;
		}
		/**
     	* <summary>
     	* Initializes the community and chance deck for the board
        * </summary>
    	*/		
		public void DeckInit()
		{
    		for(int i = 0; i < 16; i++)
    		{
    			Board.ChanceDeck.Add(new Card("Chance",i,"..."));
    			Board.CommunityDeck.Add(new Card("Community",i,"..."));
    		}
		}
		/**
     	* <summary>
     	* Initializes the Bank and the money on the board
        * </summary>
		* <param name="board">
		* the initialized board
    	* </param>
		*/
		public void BankInit(Board board)
		{
    		board.BoardBank=new Bank(); 
    		board.BoardMoney = 0;
		}
		/**
     	* <summary>
     	* Initializes the squares on the board,
		* colors need to be converted from hex to rgba for the field properties
        * <param name="board">
		* the initialized board
    	* </param>
    	*/
		public void SquaresInit(Board board)
		{			
			Color brown = HexToUnityColor("955436");
			Color lightBlue = HexToUnityColor("#aae0fa");
			Color pink = HexToUnityColor("#d93a96");
			Color orange = HexToUnityColor("#f7941d");
			Color red = HexToUnityColor("#ed1b24");
			Color yellow = HexToUnityColor("#fef200");
			Color green = HexToUnityColor("#1fb25a");
			Color blue = HexToUnityColor("#0072bb");
			board.PrisonSquare = 10;
			Board.Elements.Add(new Square(SquareType.Go,0,"square0",null));
			Board.Elements.Add(new Square(SquareType.Community,2,"square2",null));
			Board.Elements.Add(new Square(SquareType.Community,17,"square17",null));
			Board.Elements.Add(new Square(SquareType.Community,33,"square33",null));
			Board.Elements.Add(new Square(SquareType.Tax,4,"square4",null));
			Board.Elements.Add(new Square(SquareType.Tax,38,"square38",null));    	   	
			Board.Elements.Add(new OwnableSquare(SquareType.Station,5,"square5",null,200,50));
			Board.Elements.Add(new OwnableSquare(SquareType.Station,15,"square15",null,200,50));
			Board.Elements.Add(new OwnableSquare(SquareType.Station,25,"square25",null,200,50));
			Board.Elements.Add(new OwnableSquare(SquareType.Station,35,"square35",null,200,50));    	
			Board.Elements.Add(new Square(SquareType.Chance,7,"square7",null));
			Board.Elements.Add(new Square(SquareType.Chance,22,"square22",null));
			Board.Elements.Add(new Square(SquareType.Chance,36,"square36",null));  		
			Board.Elements.Add(new Square(SquareType.Prison,10,"square10",null));
			Board.Elements.Add(new GoToJailSquare(SquareType.GoToJail,30,"square30",null));
			Board.Elements.Add(new Square(SquareType.Parking,20,"square20",null));
			Board.Elements.Add(new OwnableSquare(SquareType.Company,12,"square12",null,150,6));
			Board.Elements.Add(new OwnableSquare(SquareType.Company,28,"square28",null,150,6));
			Board.Elements.Add(new PropertySquare(SquareType.Field,1,"square1",null,
			60,2,50,10,30,90,160,250,brown));
			
			Board.Elements.Add(new PropertySquare(SquareType.Field,3,"square3",null,
			60,4,50,20,60,180,320,450,brown));
			
			Board.Elements.Add(new PropertySquare(SquareType.Field,6,"square6",null,
			100,6,50,30,90,270,400,550,lightBlue));
			
			Board.Elements.Add(new PropertySquare(SquareType.Field,8,"square8",null,
			100,6,50,30,90,270,400,550,lightBlue));
			
			Board.Elements.Add(new PropertySquare(SquareType.Field,9,"square9",null,
			120,8,50,40,100,300,450,600,lightBlue));
			
			Board.Elements.Add(new PropertySquare(SquareType.Field,11,"square11",null,
			140,10,100,50,150,450,625,750,pink));
			
			Board.Elements.Add(new PropertySquare(SquareType.Field,13,"square13",null,
			140,10,100,50,150,450,625,750,pink));

			Board.Elements.Add(new PropertySquare(SquareType.Field,14,"square14",null,
			160,12,100,60,180,500,700,900,pink));
			
			Board.Elements.Add(new PropertySquare(SquareType.Field,16,"square16",null,
			180,14,100,70,200,550,700,900,orange));
		
			Board.Elements.Add(new PropertySquare(SquareType.Field,18,"square18",null,
			180,14,100,70,200,550,700,950,orange));
			
			Board.Elements.Add(new PropertySquare(SquareType.Field,19,"square19",null,
			200,16,100,90,220,600,800,1000,orange));
			
			Board.Elements.Add(new PropertySquare(SquareType.Field,21,"square21",null,
			220,18,150,90,250,700,875,1050,red));
			
			Board.Elements.Add(new PropertySquare(SquareType.Field,23,"square23",null,
			220,18,150,90,250,700,875,1050,red));
		
			Board.Elements.Add(new PropertySquare(SquareType.Field,24,"square24",null,
			240,20,150,100,300,750,925,1100,red));
			
			Board.Elements.Add(new PropertySquare(SquareType.Field,26,"square26",null,
			260,22,150,110,330,800,975,1150,yellow));
			
			Board.Elements.Add(new PropertySquare(SquareType.Field,27,"square27",null,
			260,22,150,110,330,800,975,1150,yellow));
			
			Board.Elements.Add(new PropertySquare(SquareType.Field,29,"square29",null,
			280,24,150,120,360,850,1025,1200,yellow));
		
			Board.Elements.Add(new PropertySquare(SquareType.Field,31,"square31",null,
			300,26,200,130,390,900,1100,1275,green));
			
			Board.Elements.Add(new PropertySquare(SquareType.Field,32,"square32",null,
			300,26,200,130,390,900,1100,1275,green));
			
			Board.Elements.Add(new PropertySquare(SquareType.Field,34,"square34",null,
			320,28,200,150,450,1000,1200,1400,green));
			
			Board.Elements.Add(new PropertySquare(SquareType.Field,37,"square37",null,
			350,35,200,175,500,1100,1300,1500,blue));
			
			Board.Elements.Add(new PropertySquare(SquareType.Field,39,"square39",null,
			400,50,200,200,600,1400,1700,2000,blue)); 
		} 	 
   	   	    	
    }
} 		
    		 
    	
