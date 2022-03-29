/*
 * PropertySquareTest.cs
 * Unitary test of the class PropertySquare
 * 
 * Date created : 24/02/2022
 * Author       : Rayan MARMAR <rayan.marmar@etu.unistra.fr>
 */

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
            PropertySquare st = new PropertySquare(3, 200, 40, 20, 80, 120, 140 , 200 , 250 );
            Assert.True(st != null);
            Assert.AreEqual(3, st.Id);
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
            PropertySquare st = new PropertySquare(3, 200, 40, 20, 80, 120, 140 , 200 , 250 );
            PropertySquare st2 = new PropertySquare(1, 200, 40, 20, 80, 120, 140 , 200 , 250 );
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
            int[] ids = {1,3,6,8,9,11,13,14,16,18,19,21,23,
                24,26,27,29,31,32,34,37,39};
            List<int> validIdx = new List<int>(ids);
            for (int i = 0; i < 40; i++)
            {
                if(validIdx.Contains(i))
                    Assert.True(PropertySquare.IsPropertyIndex(i));
                else
                    Assert.False(PropertySquare.IsPropertyIndex(i));
            }
        }
    }
}

