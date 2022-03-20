using System;
using System.Collections.Generic;
using Monopoly.Classes;
using UnityEngine;
using UnityEngine.UI;
using NUnit.Framework;

namespace Monopoly.Classes
{
    public class TerminalTest
    {   
        [Test]
        public void TestPlayerCreation()
        {
            Player p = new Player("1","Bob",null);
            Assert.True(p.Id == "1");
            Assert.True(p.Money == 1500);
            Assert.True(p.Score == 0);
            Assert.True(p.Name == "Bob");
            Assert.True(p.Position == 0);
            Assert.True(p.Bankrupt == false);
            Assert.True(p.InJail == false);
            Assert.True(p.Bot == false);
            Assert.True(p.ChanceJailCard == false);
            Assert.True(p.CommunityJailCard == false);
        }
        [Test]
        public void TestPlayerGoToJail()
        {
            Player p = new Player("1","Bob",null);
            p.EnterPrison();
            Assert.True(p.Position == 10);
            Assert.True(p.InJail == true);
            Assert.True(p.Doubles == 0);
        }
        [Test]
        public void TestExitPrison()
        {
            Player p = new Player("1","Bob",null);
            p.EnterPrison();
            p.JailTurns ++;            
            p.ExitPrison();
            Assert.True(p.JailTurns == 0);
            Assert.True(p.InJail == false);
        }
        [Test]
        public void TestTransferMoney()
        {
            Player p1 = new Player("1","Bob",null);
            Player p2 = new Player("2","Jules",null);
            p1.TransferMoney(p2,0);
            Assert.True(p1.Money == 1500);
            Assert.True(p2.Money == 1500);            
            p1.TransferMoney(p2,1500);
            Assert.True(p1.Money == 0);
            Assert.True(p2.Money == 3000);
            p1.TransferMoney(p2,1500);
            Assert.True(p1.Money == 0);
            Assert.True(p2.Money == 3000);
            p1.TransferMoney(p2, -1500);
            Assert.True(p1.Money == 0);
            Assert.True(p2.Money == 3000);
        }
        
    }
}