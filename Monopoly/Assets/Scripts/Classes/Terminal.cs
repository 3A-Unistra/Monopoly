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
    	board.PrisonSquare = 9;
    	board.BoardMoney = 0;
    	board.Elements.add(new Square(Start,0,"Start",null);
    }
} 		
    		 
    	
