/*
 * Terminal.cs
 * This file contains the methods that test the implementation
 * of the game classes.
 * 
 * Date created : 04/03/2022
 * Author       : Rayan Marmar <rayan.marmar@etu.unistra.fr>
 *              
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Monopoly.Classes;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = System.Random;

namespace Monopoly.TestTools
{
    public class Terminal : MonoBehaviour
    {

        public int i;
        public TMP_InputField input;
        public Button sendButton;
        public Board GameBoard;
        private List<Player> sortedPlayersList = new List<Player>(6);
        private bool endGame = false;
        void Start()
        {
            Debug.Log("The game started. Enter your next move");
            while (!endGame)
            {
                for ( i = 0; i < sortedPlayersList.Count() ; i++)
                {
                    Player p = sortedPlayersList[i];
                        Debug.Log("It\'s " + p.Name + "\'s turn.");
                    sendButton.onClick.AddListener(SendInput);
                }
            }
        }
        
        
        private void RollDice(Player p)
        {
            Random rnd = new Random();
            int dice1 = rnd.Next(1, 7);
            int dice2 = rnd.Next(1, 7);
            int dices = dice1 + dice2;

            if (dice1 != dice2)
            {
                p.Position = (p.Position + 1) % 40;
                p.Doubles = 0;
                Debug.Log("Your moved forward " + dices + " steps.");
            }
            else if (dice1 == dice2 && ++p.Doubles < 3)
            {
                p.Position = (p.Position + 1) % 40;
                Debug.Log("Your double counter is at " + p.Doubles);
                i--;
            }
            else if (dice1 == dice2 && p.Doubles == 3)
            {
                p.EnterPrison();
                if(p.InJail)
                    Debug.Log("You went to jail in position " + p.Position);
                else
                    Debug.LogError("AN ERROR OCCURED THE PLAYER SHOULD " +
                                   "BE IN PRISON");
            }
        }

        private void BuyProperty(Player p, Square s)
        {
            OwnableSquare os = (OwnableSquare) s;
            GameBoard.BoardBank.BuyProperty(p, os);
            if(os.Owner == p)
                Debug.Log(p.Name + " now owns " + os.Name);
            else
                Debug.LogError("AN ERROR OCCURED");
        }
        
        private void SellProperty(Player p, Square s)
        {
            OwnableSquare os = (OwnableSquare) s;
            GameBoard.BoardBank.SellProperty(p,os);
            if(os.Owner == null)
                Debug.Log(os.Name + " is now sold ");
            else
                Debug.LogError("AN ERROR OCCURED");
        }
        
        private void EndGame()
        {
            endGame = true;
        }
        
        private void SendInput()
        {
            string txt = input.text;
            input.text = "";
            Debug.Log(txt);
            
            switch (txt)
            {
                case "Roll dice" : 
                    RollDice(sortedPlayersList[i]);
                    break;
                case "Buy property":
                    BuyProperty(sortedPlayersList[i], 
                        Board.Elements[sortedPlayersList[i].Position]);
                    break;
                case "Sell property":
                    SellProperty(sortedPlayersList[i], 
                        Board.Elements[sortedPlayersList[i].Position]);
                    break;
                case "End game" :
                    EndGame();
                    break;
            }
            
        }
        
    }
}