using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Monopoly.Classes
{
    public class NewTestScript
    {
        [Test]
        public void TestInitialisation()
        {
            OwnableSquare st = new OwnableSquare(SquareType.Field,3,"test",null, 200, 40);
            Assert.True(st != null);
            Assert.AreEqual(SquareType.Field, st.Type);
            Assert.AreEqual(3, st.Id);
            Assert.AreEqual("test", st.Name);
            Assert.AreEqual(null, st.Image);
            Assert.AreEqual(200, st.Price);
            Assert.AreEqual(40, st.Rent);
        }

        [Test]
        public void TestFuncions()
        {
            OwnableSquare s = new OwnableSquare(SquareType.Company,12,"test",null, 200, 40);
            OwnableSquare s2 = new OwnableSquare(SquareType.Company,28,"test2",null, 200, 40);
            OwnableSquare s3 = new OwnableSquare(SquareType.Station,5,"test",null, 200, 40);
            Player p = new Player("0", "test", null);
            Player p2 = new Player("1", "test1", null);
            s.Owner = p2;
            s.PayRent(p);
            Assert.AreEqual(1460, p.Money);
            Assert.True(OwnableSquare.IsSameGroup(s,s2));
            Assert.False(OwnableSquare.IsSameGroup(s,s3));
            int[] ids = {1,3,5,6,8,9,11,12,13,14,15,16,18,19,21,23,
                24,25,26,27,28,29,31,32,34,35,37,39};
            List<int> validIdx = new List<int>(ids);
            foreach (var e in validIdx)
            {
                Assert.True(OwnableSquare.IsOwnableIndex(e));
            }
            Assert.False(OwnableSquare.IsOwnableIndex(2));
            s.MortgageProperty(p);
            Assert.False(s.Mortgaged);
            Assert.AreEqual(1460,p.Money);
            s.MortgageProperty(p2);
            Assert.True(s.Mortgaged);
            Assert.AreEqual(1640,p2.Money);
            s2.UnmortgageProperty(p);
            Assert.False(s2.Mortgaged);
            Assert.True(s.Mortgaged);
            Assert.AreEqual(1460,p.Money);
            s.UnmortgageProperty(p2);
            Assert.False(s.Mortgaged);
            Assert.AreEqual(1540,p2.Money);
        }
    }
}

