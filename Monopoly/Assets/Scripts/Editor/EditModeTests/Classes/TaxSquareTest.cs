/*
 * TaxSquareTest.cs
 * Unitary test of the class TaxSquare
 * 
 * Date created : 26/02/2022
 * Author       : Rayan MARMAR <rayan.marmar@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Monopoly.Classes
{
    public class TaxSquareTest
    {

        [Test]
        public void TestInitialisation()
        {
            TaxSquare s = new TaxSquare(38, 100);
            Assert.True(s != null);
            Assert.AreEqual(SquareType.Tax, s.Type);
            Assert.AreEqual(38, s.Id);
            Assert.AreEqual(100, s.TaxPrice);
        }

        [Test]
        public void TestTypeFunctions()
        {
            Assert.True(TaxSquare.IsTaxIndex(4));
            Assert.True(TaxSquare.IsTaxIndex(38));
            int[] ids = {1,2,3,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,
                21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,39};
            List<int> validIdx = new List<int>(ids);
            foreach (var e in validIdx)
            {
                Assert.False(TaxSquare.IsTaxIndex(e));
            }
           
        }
    }
}


