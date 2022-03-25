using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Monopoly.Classes
{
    public class PropertySquareTest
    {
        [Test]
        public void TestInitialisation()
        {
            PropertySquare st = new PropertySquare(SquareType.Field,3,"test",null, 200, 40, 20, 80, 120, 140 , 200 , 250 );
            Assert.True(st != null);
            Assert.AreEqual(SquareType.Field, st.Type);
            Assert.AreEqual(3, st.Id);
            Assert.AreEqual("test", st.Name);
            Assert.AreEqual(null, st.Image);
            Assert.AreEqual(200, st.Price);
            Assert.AreEqual(40, st.Rent);
            Assert.AreEqual(20, st.HouseCost);
            Assert.AreEqual(80, st.House1Rent);
            Assert.AreEqual(120, st.House2Rent);
            Assert.AreEqual(140, st.House3Rent);
            Assert.AreEqual(200, st.House4Rent);
            Assert.AreEqual(250, st.HotelRent);
        }

        [Test]
        public void TestFunctions()
        {
            PropertySquare st = new PropertySquare(SquareType.Field,3,"test",null, 200, 40, 20, 80, 120, 140 , 200 , 250 );
            PropertySquare st2 = new PropertySquare(SquareType.Field,1,"test1",null, 200, 40, 20, 80, 120, 140 , 200 , 250 );
            Player p = new Player("0", "test", null);
            Player p2 = new Player("1", "test1", null);
            st.Owner = p2;
            st.PayRent(p);
            Assert.AreEqual(1460, p.Money);
            Assert.AreEqual(1540, p2.Money);
            st2.PayRent(p2);
            Assert.AreEqual(1540, p2.Money);
            st.NbHouse = 1;
            st.PayRent(p);
            Assert.AreEqual(1380, p.Money);
            Assert.AreEqual(1620, p2.Money);
            st.NbHouse = 2;
            st.PayRent(p);
            Assert.AreEqual(1260, p.Money);
            Assert.AreEqual(1740, p2.Money);
            st.NbHouse = 3;
            st.PayRent(p);
            Assert.AreEqual(1120, p.Money);
            Assert.AreEqual(1880, p2.Money);
            st.NbHouse = 4;
            st.PayRent(p);
            Assert.AreEqual(920, p.Money);
            Assert.AreEqual(2080, p2.Money);
            st.NbHouse = 5;
            st.PayRent(p);
            Assert.AreEqual(670, p.Money);
            Assert.AreEqual(2330, p2.Money);
            Assert.AreEqual(40, st.GetRent(0));
            Assert.AreEqual(80, st.GetRent(1));
            Assert.AreEqual(120, st.GetRent(2));
            Assert.AreEqual(140, st.GetRent(3));
            Assert.AreEqual(200, st.GetRent(4));
            Assert.AreEqual(250, st.GetRent(5));
        }
    }
}

