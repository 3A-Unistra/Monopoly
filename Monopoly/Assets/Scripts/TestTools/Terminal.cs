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
    //TO DO ADD EXCHANGING INTERACTION
    //TO DO MODIFY MORTGAGE INTERACTION
    //TO DO ADD LOGIC OF PLAYER LOSING
    //TO DO ADD LOGIC OF A PLAYER WINNING
    //TO DO MODIFY LOGIC OF SAME PLAYER HAVING MULTIPLE TURNS
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
            if (GameBoard.SquareOwned(p).Count() > 0)
            {
                msg.Append(" or Exchange property or Mortgage property");
                for(int j=0 ; j<GameBoard.SquareOwned(p).Count(); j++)
                {
                    OwnableSquare s = GameBoard.SquareOwned(p)[j];
                    if (s.Mortgaged==true)
                    {
                        msg.Append(" or Unmortgage property");
                        j = GameBoard.SquareOwned(p).Count();
                    }
                }
            }
                
            Debug.Log(msg);
        }
        
        private void RollDice(Player p)
        {
            Random rnd = new Random();
            int dice1 = rnd.Next(1, 7);
            int dice2 = rnd.Next(1, 7);
            int dices = dice1 + dice2;
            int position = p.Position + dices;
            if ((dice1 != dice2 || p.Doubles != 3))
            {
                if (position > 39)
                {
                    p.Money += 200;
                    Debug.Log(p.Name + " just received 200 by passing go");
                }
                p.Position = (p.Position + dices) % 40;
                Debug.Log("Your moved forward " + dices + " steps.");
                Square currentSquare = Board.Elements[p.Position];
                Debug.Log("You are now at " + currentSquare.Name);
            }
            if (dice1 != dice2)
            {
                p.Doubles = 0;
            }
            else if (dice1 == dice2 && ++p.Doubles < 3)
            {
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
                    [15]);
                p.ChanceJailCard = true;
            }
                
        }
        
        private void DrawCommunityCard(Player p)
        {
            Card c = GameBoard.GetRandomCommunityCard();
            if (c.id == 15)
            {                
                Board.CommunityDeck.Remove(Board.CommunityDeck
                    [15]);
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
        
        private void MortgageProperty(Player p, Square s)
        {
            OwnableSquare os = (OwnableSquare) s;
            os.MortgageProperty(p);
            if(os.Owner == null)
                Debug.Log(os.Name + " is now mortgaged ");
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
        
        private void PerformExchange(List<OwnableSquare> fromSquares,
            List<OwnableSquare> toSquares, int fromMoney, int toMoney,
            List<Card> fromCards, List<Card> toCards, Player from, Player to)
        {
            if (fromSquares.Count() > 0)
            {
                foreach (var s in fromSquares)
                    from.TransferProperty(to,s);
            }
            if (toSquares.Count() > 0)
            {
                foreach (var s in toSquares)
                    to.TransferProperty(from,s);
            }
            
            if (fromCards.Count() > 0)
            {
                foreach (var c in fromCards)
                    from.TransferCard(to,c.type);
            }
            if (toCards.Count() > 0)
            {
                foreach (var c in toCards)
                    to.TransferCard(from,c.type);
            }

            if (toMoney > 0)
                to.TransferMoney(from,toMoney);

            if(fromMoney > 0)
                from.TransferMoney(to,fromMoney);
            
        }
        
        private void SendInput()
        {
            string txt = input.text;
            input.text = "";
            Debug.Log(txt);
            input.Select();
            switch (txt)
            {
                case "Roll dice" : 
                    RollDice(sortedPlayersList[i]);
                    break;
                case "Buy property":
                    BuyProperty(sortedPlayersList[i], 
                        Board.Elements[sortedPlayersList[i].Position]);
                    break;
                case "Mortgage property":
                    MortgageProperty(sortedPlayersList[i], 
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
           msg.Append("Possible actions : ");
           if(p.Doubles > 0)
               msg.Append("Roll dice");
           else
               msg.Append("End turn");
           if (currentSquare.IsProperty())
           {
               OwnableSquare os = (OwnableSquare) currentSquare;
               if(os.Owner == null && os.Mortgaged == false)
                   msg.Append(" or Buy property");
               else if(os.Owner != sortedPlayersList[i])
                   os.PayRent(p);
               else
               {
                   PropertySquare ps = (PropertySquare) os;
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
