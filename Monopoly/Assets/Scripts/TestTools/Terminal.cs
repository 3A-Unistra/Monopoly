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
            String msg;
            Debug.Log("The game started. Enter your next move");
            while (!endGame)
            {
                for ( i = 0; i < sortedPlayersList.Count() ;)
                // The incrementation of i happens whenever
                // a player chooses to end his turn
                {
                    Player p = sortedPlayersList[i];
                    Debug.Log("It\'s " + p.Name + "\'s turn.");
                    if (p.InJail)
                    {
                        if (p.JailTurns < 3)
                            msg = "Possible actions : Roll Dice or Pay 50";
                        else
                            msg = "Possible actions : Pay 50";
                        if (p.ChanceJailCard)
                            msg += " or Use chance jail card";
                        if (p.CommunityJailCard)
                            msg += " or Use community jail card";
                    }
                    else
                    {
                        msg = "Possible actions : Roll dice";
                    }
                    Debug.Log(msg);
                    //BEFORE THE MOVEMENT
                    sendButton.onClick.AddListener(SendInput);
                    //AFTER THE MOVEMENT
                    Square currentSquare = Board.Elements[p.Position];
                    Debug.Log("You are now at " + currentSquare.Name);
                    ObligatoryActions(p,currentSquare);
                    msg = "Possible actions : End turn";
                    if (currentSquare.IsProperty())
                    {
                        OwnableSquare os = (OwnableSquare) currentSquare;
                        if(os.Owner == null)
                            msg += " or Buy property";
                        else if(os.Owner != p)
                            os.PayRent(p);
                        else
                        {
                            msg += "or Sell property";
                            if 
                                (GameBoard.OwnSameColorSet(p, (PropertySquare) os) 
                                 || GameBoard.CanBuyHouse(p, (PropertySquare) os))
                                msg += " or Buy house";
                            if 
                                (GameBoard.OwnSameColorSet(p, (PropertySquare) os) 
                                 || GameBoard.CanSellHouse(p, (PropertySquare) os))
                                msg += " or Sell house";
                        }
                    }
                        
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

        private void DrawChanceCard(Player p)
        {
            GameBoard.GetRandomChanceCard();
        }
        
        private void DrawCommunityCard(Player p)
        {
            GameBoard.GetRandomCommunityCard();
        }

        private void ReturnChanceJailCard(Player p)
        {
            p.ChanceJailCard = false;
            p.InJail = false;
            p.JailTurns = 0;
        }
        
        private void ReturnCommunityJailCard(Player p)
        {
            p.CommunityJailCard = false;
            p.InJail = false;
            p.JailTurns = 0;
        }
        
        private void PayTax(Player p, Square s)
        {
            TaxSquare ts = (TaxSquare) s;
            if (p.Money < ts.TaxPrice)
                p.Bankrupt = true;
            else
                p.Money -= ts.TaxPrice;
            GameBoard.BoardMoney += ts.TaxPrice;
        }

        private void PickUpBoardMoney(Player p)
        {
            p.Money += GameBoard.BoardMoney;
            GameBoard.BoardMoney = 0;
        }

        public void BuyHouse(Player p, Square s)
        {
            PropertySquare ps = (PropertySquare) s;
            GameBoard.BuyHouse(ps,p);
        }
        
        public void SellHouse(Player p, Square s)
        {
            PropertySquare ps = (PropertySquare) s;
            GameBoard.SellHouse(ps,p);
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

        public void Pay50(Player p)
        {
            if (p.Money < 50)
                p.Bankrupt = true;
            else
            {
                p.Money -= 50;
                p.InJail = false;
                p.JailTurns = 0;
            }
            
        }
        
        private void EndTurn()
        {
            i++;
        }
        
        private void EndGame()
        {
            endGame = true;
        }
        
        private void ObligatoryActions(Player p, Square s)
        {
            int position = p.Position;
            if(Board.Elements[position].IsChance())
                DrawChanceCard(p);
            else if(Board.Elements[position].IsCommunityChest())
                DrawCommunityCard(p);
            else if(Board.Elements[position].IsTax())
                PayTax(p,s);
            else if(Board.Elements[position].IsFreeParking())
                PickUpBoardMoney(p);
            else if (Board.Elements[position].IsGoToJail())
                p.EnterPrison();
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
                case "Buy house":
                    BuyHouse(sortedPlayersList[i], 
                        Board.Elements[sortedPlayersList[i].Position]);
                    break;
                case "Sell house":
                    SellHouse(sortedPlayersList[i], 
                        Board.Elements[sortedPlayersList[i].Position]);
                    break;
                case "Pay 50" :
                    Pay50(sortedPlayersList[i]);
                    break;
                case "Use chance jail card" :
                    ReturnChanceJailCard(sortedPlayersList[i]);
                    break;
                case "Use community jail card" :
                    ReturnCommunityJailCard(sortedPlayersList[i]);
                    break;
                case "End turn":
                    EndTurn();
                    break;
                case "End game" :
                    EndGame();
                    break;
            }
            
        }
        
    }
}