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
using System.IO;
using System.Linq;
using System.Text;
using Monopoly.Classes;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = System.Random;

namespace Monopoly.TestTools
{
    public class Terminal : MonoBehaviour
    {
        private int i = 0;
        public TMP_InputField input;
        public Button sendButton;
        public Board GameBoard = new Board();

        private List<Player> sortedPlayersList = new List<Player>()
        {
            new Player("1", "Tony", null),
            new Player("2", "Laura", null),
            new Player("3", "Mike", null),
            new Player("4", "Sara", null),
            new Player("5", "Helene", null),
            new Player("6", "Anthony", null),
        };
        

        void Start()
        {
            Debug.Log("The game started. Enter your next move");
            YourTurn();
            sendButton.onClick.AddListener(SendInput);
        }


        private void YourTurn()
        {
            Player p = sortedPlayersList[i];
            StringBuilder msg = new StringBuilder();
            Debug.Log("It\'s " + p.Name + "\'s turn.");
            if (p.InJail)
            {
                if (p.JailTurns < 3)
                    msg.Append("Possible actions : Roll Dice or Pay 50");
                else
                    msg.Append("Possible actions : Pay 50");
                if (p.ChanceJailCard)
                    msg.Append(" or Use chance jail card");
                if (p.CommunityJailCard)
                    msg.Append(" or Use community jail card");
            }
            else
            {
                msg.Append("Possible actions : Roll dice");
            }
            if (p.ChanceJailCard)
                msg.Append(" or Exchange chance jail card");
            if (p.CommunityJailCard)
                msg.Append(" or Exchange community jail card");
            if (GameBoard.SquareOwned(p).Any())
                msg.Append(" or Exchange property");
            Debug.Log(msg);
        }
        
        private void RollDice(Player p)
        {
            Random rnd = new Random();
            int dice1 = rnd.Next(1, 7);
            int dice2 = rnd.Next(1, 7);
            int dices = dice1 + dice2;

            if (dice1 != dice2)
            {
                p.Position = (p.Position + dices) % 40;
                p.Doubles = 0;
                Debug.Log("Your moved forward " + dices + " steps.");
                Square currentSquare = Board.Elements[p.Position];
                Debug.Log("You are now at " + currentSquare.Name);
            }
            else if (dice1 == dice2 && ++p.Doubles < 3)
            {
                Debug.Log("Your moved forward " + dices + " steps.");
                Square currentSquare = Board.Elements[p.Position];
                Debug.Log("You are now at " + currentSquare.Name);
                p.Position = (p.Position + dices) % 40;
                Debug.Log("Your double counter is at " + p.Doubles);
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
            Card c = GameBoard.GetRandomChanceCard();
            if (c.id == 15)
            {
                Board.ChanceDeck.Remove(Board.ChanceDeck
                    [Board.ChanceDeck.Count() - 1]);
                p.ChanceJailCard = true;
            }
                
        }
        
        private void DrawCommunityCard(Player p)
        {
            Card c = GameBoard.GetRandomCommunityCard();
            if (c.id == 15)
            {                
                Board.CommunityDeck.Remove(Board.CommunityDeck
                    [Board.CommunityDeck.Count() - 1]);
                p.CommunityJailCard = true;
            }

        }

        private void ReturnChanceJailCard(Player p)
        {
            GameBoard.ReturnCard("CHANCE");
            p.ChanceJailCard = false;
            p.InJail = false;
            p.JailTurns = 0;
        }
        
        private void ReturnCommunityJailCard(Player p)
        {
            GameBoard.ReturnCard("COMMUNITY");
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
            i = (i+1) % sortedPlayersList.Count();
            YourTurn();
        }
        
        private void EndGame()
        {
            Debug.Log("The game has ended!");
            Application.Quit();
        }
        
        private void ObligatoryActions(Player p, Square s)
        {
            if (s.IsChance())
            {
                DrawChanceCard(p);
                Debug.Log(p.Name + " drew a chance card");
            }
            else if (s.IsCommunityChest())
            {
                DrawCommunityCard(p);
                Debug.Log(p.Name + " drew a chance card");
            }

            else if (s.IsTax())
            {
                TaxSquare ts = (TaxSquare) s;
                PayTax(p,s);
                Debug.Log(p.Name + " payed a tax of " + ts.TaxPrice);
            }

            else if (s.IsFreeParking())
            {
                Debug.Log(p.Name + " gained " + GameBoard.BoardMoney +
                          " from the free parking");
                PickUpBoardMoney(p);
            }
            else if (s.IsGoToJail())
            {
                p.EnterPrison();
                Debug.Log(p.Name + " entered prison.");
            }

        }

        private enum Exchangeable
        {
            Card,
            Property
        }
        private void ExchangePropertyToMoney(Player from, Player to, Square s,
            int price)
        {
            OwnableSquare os = (OwnableSquare) s;
            from.TransferProperty(to,os);
            to.TransferMoney(from,price);
        }
        
        private void ExchangePropertyToProperty(Player from, Player to, 
            Square sFrom,Square sTo)
        {
            OwnableSquare osFrom = (OwnableSquare) sFrom;
            OwnableSquare osTo = (OwnableSquare) sTo;
            from.TransferProperty(to,osFrom);
            to.TransferProperty(from,osTo);
        }
        
        private void ExchangePropertyToCard(Player from, Player to, Square s, 
            string c)
        {
            OwnableSquare os = (OwnableSquare) s;
            from.TransferProperty(to,os);
            to.TransferCard(from,c);
        }
        private void ExchangeCardToMoney(Player from, Player to, string c,
            int price)
        {
            from.TransferCard(to,c);
            to.TransferMoney(from,price);
        }
        
        private void ExchangeCardToProperty(Player from, Player to, string c,
            Square s)
        {
            OwnableSquare os = (OwnableSquare) s;
            from.TransferCard(to,c);
            to.TransferProperty(from,os);
        }
        
        private void ExchangeCardToCard(Player from, Player to, string cFrom,
            string cTo)
        {
            from.TransferCard(to,cFrom);
            to.TransferCard(from,cTo);
        }

        private void RequestExchange(Exchangeable fromType, 
            Exchangeable toType)
        {
            
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
                case "Exchange chance jail card":
                    break;
                case "Exchange community jail card":
                    break;
                case "Exchange property" :
                    break;
                case "End turn":
                    EndTurn();
                    return;
                case "End game" :
                    EndGame();
                    return;
            }

            //AFTER THE MOVEMENT
           Player p = sortedPlayersList[i];
           StringBuilder msg = new StringBuilder();
           Square currentSquare = Board.Elements[p.Position];
           ObligatoryActions(sortedPlayersList[i],currentSquare);
           msg.Append("Possible actions : End turn");
           if (currentSquare.IsProperty())
           {
               OwnableSquare os = (OwnableSquare) currentSquare;
               if(os.Owner == null)
                   msg.Append(" or Buy property");
               else if(os.Owner != sortedPlayersList[i])
                   os.PayRent(p);
               else
               {
                   PropertySquare ps = (PropertySquare) os;
                   msg.Append(" or Sell property");
                   if 
                       (GameBoard.OwnSameColorSet(sortedPlayersList[i], ps) 
                           || GameBoard.CanBuyHouse(sortedPlayersList[i], ps))
                       msg.Append(" or Buy house");
                   if 
                       (GameBoard.OwnSameColorSet(sortedPlayersList[i], ps) 
                           || GameBoard.CanSellHouse(sortedPlayersList[i], ps))
                       msg.Append(" or Sell house");
               }
           }
           Debug.Log(msg);
        }
    }
}