/*
 * Board.cs
 * Fichier définissant la classe plateau et ses 
 * interaction avec les différentes cases
 * 
 * Date created : 21/03/2022
 * Author       : Christophe Pierson <christophe.pierson@etu.unistra.fr>
 */
using System;
using System.Collections.Generic;
using Monopoly.Classes;
using UnityEngine;
using UnityEngine.UI;
using NUnit.Framework;

namespace Monopoly.Classes
{
    public class TestBoard
    {  
        [Test]
        public void TestBoardCreation()
        {
            Board b = new Board();
        }
        [Test]
        public void TestMove()
        {
            
            Player p = new Player("1","Bob",null);
            Board.Move(p,12);
            Assert.True(p.Position == 12);
            Board.Move(p,38);
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
            Player p = new Player("1","Bob",null);
            Board b = new Board();
           ((OwnableSquare) b.Elements[1]).Owner = p;
           ((OwnableSquare) b.Elements[5]).Owner = p;
           ((OwnableSquare) b.Elements[12]).Owner = p;
           ((OwnableSquare) b.Elements[39]).Owner = p;
            List<OwnableSquare> os = b.SquareOwned(p);
            Debug.Log(b.Elements[1]);
            Debug.Log(b.Elements[5]);
            Debug.Log(b.Elements[12]);
            Debug.Log(((OwnableSquare)b.Elements[39]));
            Debug.Log(os.Count);
            Debug.Log(os);
            Assert.True(os.Count == 4);

        }
    }
}