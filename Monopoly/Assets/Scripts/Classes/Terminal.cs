/*
 * Fichier.cs
 * Fichier définissant la classe plateau et ses 
 * interaction avec les différentes cases
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
     *<summary>
     * class for initializing the game 
     * </summary>
     */
    public class TerminalInit
    {
    	Board board;
    	for(int i = 0; i < 16; i++)
    	{
    		board.ChanceDeck.add(new Card(i,"Chance","...");
    		board.CommunityDeck.add(new Card(i,"Community","...");
    	}
    	board.Boardbank=new Bank(); 
    	board.PrisonSquare = 10;
    	board.BoardMoney = 0;
    	board.Elements.add(new Square(Go,0,"square0",null));
    	board.Elements.add(new Square(Community,2,"square2",null));
    	board.Elements.add(new Square(Community,17,"square17",null));
    	board.Elements.add(new Square(Community,33,"square33",null));
    	board.Elements.add(new Square(Tax,4,"square4",null));
    	board.Elements.add(new Square(Tax,38,"square38",null));    	   	
    	board.Elements.add(new OwnableSquare(Station,5,"square5",null,200,50));
    	board.Elements.add(new OwnableSquare(Station,15,"square15",null,200,50));
    	board.Elements.add(new OwnableSquare(Station,25,"square25",null,200,50));
    	board.Elements.add(new OwnableSquare(Station,35,"square35",null,200,50));    	
    	board.Elements.add(new Square(Chance,7,"square7",null));
    	board.Elements.add(new Square(Chance,22,"square22",null));
    	board.Elements.add(new Square(Chance,36,"square"36,null));  		
    	board.Elements.add(new Square(Prison,10,"square10",null));
    	board.Elements.add(new Square(GoTojail,30,"square30",null));
    	board.Elements.add(new Square(Parking,20,"square20",null));
    	board.Elements.add(new OwnableSquare(Company,12,"square12",null),150,6);
    	board.Elements.add(new OwnableSquare(Company,28,"square28",null),150,6);
    	board.Elements.add(new PropertySquare(Field,1,"square1",null,
    	60,2,50,10,30,90,160,250,"#955436"));
    	
    	board.Elements.add(new PropertySquare(Field,3,"square3",null,
    	60,4,50,20,60,180,320,450,"#955436"));
    	
    	board.Elements.add(new PropertySquare(Field,6,"square6",null,
    	100,6,50,30,90,270,400,550,"#aae0fa"));
    	
    	board.Elements.add(new PropertySquare(Field,8,"square8",null,
    	100,6,50,30,90,270,400,550,"#aae0fa"));
    	
    	board.Elements.add(new PropertySquare(Field,9,"square9",null,
    	120,8,50,40,100,300,450,600,"#aae0fa"));
    	
    	board.Elements.add(new PropertySquare(Field,11,"square11",null,
    	140,10,100,50,150,450,625,750,"#d93a96"));
    	
    	board.Elements.add(new PropertySquare(Field,13,"square13",null,
    	140,10,100,50,150,450,625,750,"#d93a96"));
    	
    	board.Elements.add(new PropertySquare(Field,14,"square14",null,
    	160,12,100,60,180,500,700,900,"#d93a96"));
    	
  	board.Elements.add(new PropertySquare(Field,16,"square16",null,
    	180,14,100,70,200,550,700,900,"#f7941d"));
  	
    	board.Elements.add(new PropertySquare(Field,18,"square18",null,
    	180,14,100,70,200,550,700,950,"#f7941d"));
    	
    	board.Elements.add(new PropertySquare(Field,19,"square19",null,
    	200,16,100,90,220,600,800,1000,"#f7941d"));
    	
    	board.Elements.add(new PropertySquare(Field,21,"square21",null,
    	220,18,150,90,250,700,875,1050,"#ed1b24"));
    	
 	board.Elements.add(new PropertySquare(Field,23,"square23",null,
    	220,18,150,90,250,700,875,1050,"#ed1b24"));
 	
    	board.Elements.add(new PropertySquare(Field,24,"square24",null,
    	240,20,150,100,300,750,925,1100,"#ed1b24"));
    	
    	board.Elements.add(new PropertySquare(Field,26,"square26",null,
    	260,22,150,110,330,800,975,1150,"#fef200"));
    	
    	board.Elements.add(new PropertySquare(Field,27,"square27",null,
    	260,22,150,110,330,800,975,1150,"#fef200"));
    	
	board.Elements.add(new PropertySquare(Field,29,"square29",null,
    	280,24,150,120,360,850,1025,1200,"#fef200"));
	
    	board.Elements.add(new PropertySquare(Field,31,"square31",null,
    	300,26,200,130,390,900,1100,1275,"#1fb25a"));
    	
    	board.Elements.add(new PropertySquare(Field,32,"square32",null,
    	300,26,200,130,390,900,1100,1275,"#1fb25a"));
    	
    	board.Elements.add(new PropertySquare(Field,34,"square34",null,
    	320,28,200,150,450,1000,1200,1400,"#1fb25a"));
    	
    	board.Elements.add(new PropertySquare(Field,37,"square37",null,
    	350,35,200,175,500,1100,1300,1500,"#0072bb"));
    	
    	board.Elements.add(new PropertySquare(Field,39,"square39",null,
    	400,50,200,200,600,140,1700,2000,"#0072bb"));  	 
   	   	    	
    }
} 		
    		 
    	
