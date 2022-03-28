/*
 * TestBank.cs
 * Fichier de test unitaire pour Bank.cs
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
    public class TestBank
    {  
        [Test]
        public void TestBankCreation()
        {
            Bank b = new Bank();
            Assert.True(b.NbHotel == 12);
            Assert.True(b.NbHouse == 32);
        }
        [Test]
        public void TestBuyHouse()
        {
            Bank b = new Bank();
            Assert.True(b.BuyHouse());
            Assert.True(b.NbHouse == 31);
            b.NbHouse = 0;
            Assert.False(b.BuyHouse());
        }
        [Test]
        public void TestBuyHotel()
        {
            Bank b = new Bank();
            Assert.True(b.BuyHotel());
            Assert.True(b.NbHotel == 11);
            b.NbHotel = 0;
            Assert.False(b.BuyHotel());            
        }       
        [Test]
        public void TestSellHouse()
        {
            Bank b = new Bank();
            b.SellHouse();
            Assert.True(b.NbHouse == 33);
        }
        [Test]
        public void TestSellHotel()
        {
            Bank b = new Bank();
            b.SellHotel();
            Assert.True(b.NbHotel == 13);
        }        
        [Test]
        public void TestBuyProperty()
        {
            Bank b = new Bank();
            Player p = new Player("1","Bob",null);
            CompanySquare s = new CompanySquare(SquareType.Company, 12, "Company",
            null, 150, 10);
            p.Money = 0;
            b.BuyProperty(p,s);
            Assert.False(s.Owner == p);
            p.Money = 1500;
            b.BuyProperty(p,s);
            Assert.True(s.Owner == p);

        }
        [Test]        
        public void TestSellProperty()
        {
            Bank b = new Bank();
            Player p = new Player("1","Bob",null);
            CompanySquare s = new CompanySquare(SquareType.Company, 12, "Company",
            null, 150, 10); 
            b.BuyProperty(p,s);
            Assert.True(s.Owner == p);
            Assert.True(p.Money == 1350);
            b.SellProperty(p,s);   
            Assert.True(p.Money == 1425);
            Assert.True(s.Owner == null);
        }        
    }
}