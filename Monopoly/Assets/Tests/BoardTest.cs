/*
 * BoardTest.cs
 * Fichier de test unitaire pour Board.cs
 * 
 * Date created : 21/03/2022
 * Author       : Christophe Pierson <christophe.pierson@etu.unistra.fr>
 */
using System;
using System.Collections;
using System.Collections.Generic;
using Monopoly.Classes;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using NUnit.Framework;

using Monopoly.Runtime;

namespace Monopoly.Classes
{

    public class BoardTest
    {   


        [Test]
        public void TestBoardCreation()
        {
            GameObject clientStateObj = new GameObject("ClientGameState");
            clientStateObj.AddComponent<ClientGameState>();
            Board b = new Board();
            Assert.True(b.BoardBank.NbHouse == 32);
            Assert.True(b.BoardBank.NbHotel == 12);
            Assert.True(b.PrisonSquare == 10);
            Assert.True(b.Elements[5].Type == SquareType.Station);
            Assert.True(b.Elements[15].Type == SquareType.Station);
            Assert.True(b.Elements[25].Type == SquareType.Station);
            Assert.True(b.Elements[35].Type == SquareType.Station);
            Assert.True(b.Elements[0].Type == SquareType.Go);
            Assert.True(b.Elements[10].Type == SquareType.Prison);
            Assert.True(b.Elements[20].Type == SquareType.Parking);
            Assert.True(b.Elements[30].Type == SquareType.GoToJail);
            Assert.True(b.Elements[2].Type == SquareType.Community);
            Assert.True(b.BoardMoney == 0);
            Assert.True(b.Elements.Count == 40);
            Assert.True(Board.ChanceDeck.Count == 16);
            Assert.True(Board.CommunityDeck.Count == 16);
            Assert.True(Board.ChanceDeck[15].desc == "OutOfJail");
            Assert.True(Board.CommunityDeck[15].desc == "OutOfJail");
            
        }

        [Test]
        public void TestMove()
        {

            Player p = new Player("1", "Bob", null);
            Board.Move(p, 12);
            Assert.True(p.Position == 12);
            Board.Move(p, 38);
            Assert.True(p.Money == 1700);
            Assert.True(p.Position == 10);
        }
        [Test]
        public void TestGetSquare()
        {
            Board b = new Board();
            Assert.True(b.GetSquare(10).Name == "Prison");
            Assert.True(b.GetSquare(20).Name == "Parc gratuit");
        }
        [Test]
        public void TestSquareOwned()
        {
            Player p = new Player("1", "Bob", null);
            Board b = new Board();
            ((OwnableSquare)b.Elements[1]).Owner = p;
            ((OwnableSquare)b.Elements[5]).Owner = p;
            ((OwnableSquare)b.Elements[12]).Owner = p;
            ((OwnableSquare)b.Elements[39]).Owner = p;
            List<OwnableSquare> os = b.SquareOwned(p);
            Assert.True(os.Count == 4);

        }
               
        [Test]
        public void TestGetPropertySet()
        {
            Board b = new Board();
            Color c = new Color(40f / 255f, 78f / 255f, 161f / 255f, 1f);
            List<PropertySquare> ps = b.GetPropertySet(c);
            Assert.True(ps.Count == 2);
        }
        [Test]
        public void TestFreeParking()
        {
            Player p = new Player("1", "Bob", null);
            Board b = new Board();
            b.BoardMoney = 500;
            b.FreeParking(p);
            Assert.True(p.Money == 2000);
            Assert.True(b.BoardMoney == 0);
        }
        [Test]
        public void TestAddMoney()
        {
            Player p = new Player("1", "Bob", null);
            Board b = new Board();
            b.AddMoney(p, 100);
            Assert.True(p.Money == 1600);
        }
        [Test]
        public void TestOwnSameColorSet()
        {
            Player p = new Player("1", "Bob", null);
            Board b = new Board();
            ((PropertySquare)b.Elements[1]).Owner = p;
            Assert.False(b.OwnSameColorSet(p, (PropertySquare)b.Elements[1]));
            ((PropertySquare)b.Elements[3]).Owner = p;
            Assert.True(b.OwnSameColorSet(p, (PropertySquare)b.Elements[1]));
        }
        [Test]
        public void TestCanBuyHouse()
        {
            Player p = new Player("1", "Bob", null);
            Board b = new Board();
            Assert.False(b.CanBuyHouse(p, (PropertySquare)b.Elements[6]));
            ((PropertySquare)b.Elements[6]).Owner = p;
            Assert.False(b.CanBuyHouse(p, (PropertySquare)b.Elements[6]));
            ((PropertySquare)b.Elements[8]).Owner = p;
            Assert.False(b.CanBuyHouse(p, (PropertySquare)b.Elements[6]));
            ((PropertySquare)b.Elements[9]).Owner = p;
            Assert.True(b.CanBuyHouse(p, (PropertySquare)b.Elements[6]));
            ((PropertySquare)b.Elements[6]).NbHouse = 1;
            Assert.False(b.CanBuyHouse(p, (PropertySquare)b.Elements[6]));
            ((PropertySquare)b.Elements[6]).NbHouse = 5;
            ((PropertySquare)b.Elements[8]).NbHouse = 5;
            ((PropertySquare)b.Elements[9]).NbHouse = 5;
            Assert.False(b.CanBuyHouse(p, (PropertySquare)b.Elements[6]));
        }
        //TODO
        [Test]
        public void TestCanSellHouse()
        {
            Player p = new Player("1", "Bob", null);
            Board b = new Board();
            Assert.False(b.CanSellHouse(p, (PropertySquare)b.Elements[6]));
            ((PropertySquare)b.Elements[6]).Owner = p;
            Assert.False(b.CanSellHouse(p, (PropertySquare)b.Elements[6]));
            ((PropertySquare)b.Elements[8]).Owner = p;
            Assert.False(b.CanSellHouse(p, (PropertySquare)b.Elements[6]));
            ((PropertySquare)b.Elements[9]).Owner = p;
            Assert.False(b.CanSellHouse(p, (PropertySquare)b.Elements[6]));
            ((PropertySquare)b.Elements[6]).NbHouse = 3;
            ((PropertySquare)b.Elements[8]).NbHouse = 4;
            ((PropertySquare)b.Elements[9]).NbHouse = 4;
            Assert.False(b.CanSellHouse(p, (PropertySquare)b.Elements[6]));
            ((PropertySquare)b.Elements[6]).NbHouse = 4;
            Assert.True(b.CanSellHouse(p, (PropertySquare)b.Elements[6]));
        }
        [Test]
        public void TestBuyHouse()
        {
            Player p = new Player("1", "Bob", null);
            Board b = new Board();
            ((PropertySquare)b.Elements[1]).Owner = p;
            ((PropertySquare)b.Elements[3]).Owner = p;
            b.BuyHouse((PropertySquare)b.Elements[1], p);
            Assert.True(p.Money == 1450);
            Assert.True(((PropertySquare)b.Elements[1]).NbHouse == 1);

        }
       [Test]
        public void TestSellHouse()
        {

            Player p = new Player("1", "Bob", null);
            Board b = new Board();
            ((PropertySquare)b.Elements[1]).Owner = p;
            ((PropertySquare)b.Elements[3]).Owner = p;
            b.BuyHouse((PropertySquare)b.Elements[1], p);
            b.BuyHouse((PropertySquare)b.Elements[3], p);
            b.BuyHouse((PropertySquare)b.Elements[1], p);
            b.SellHouse((PropertySquare)b.Elements[1], p);
            Debug.Log(p.Money);
            Assert.True(p.Money == 1400);
            Assert.True(((PropertySquare)b.Elements[1]).NbHouse == 1);


        }
        [Test]
        public void TestGetRandomCommunityCard()
        {
            Board b = new Board();
            Card c = b.GetRandomCommunityCard();
            Assert.True(c.type == "Community");
            Assert.True(c.id < 16);
        }
        [Test]
        public void TestGetRandomChanceCard()
        {
            Board b = new Board();
            Card c = b.GetRandomChanceCard();
            Assert.True(c.type == "Chance");
            Assert.True(c.id < 16);
        }

        [Test]
        public void TestReturnCard()
        {
            Board b = new Board();
            b.ReturnCard("Chance");
            Assert.True(Board.ChanceDeck.Count == 17);
            Assert.True(Board.ChanceDeck[16].desc == "OutOfJail");
            b.ReturnCard("Community");
            Assert.True(Board.CommunityDeck.Count == 17);
            Assert.True(Board.CommunityDeck[16].desc == "OutOfJail");
        }
    }
}