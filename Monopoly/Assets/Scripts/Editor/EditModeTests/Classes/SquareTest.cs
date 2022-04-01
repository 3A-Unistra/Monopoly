/*
 * SquareTest.cs
 * Unitary test of the class Square
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
    public class SquareTest
    {

        [Test]
        public void TestInitialisation()
        {
            Square s = new Square(SquareType.Go,0);
            Assert.True(s != null);
            Assert.AreEqual(SquareType.Go, s.Type);
            Assert.AreEqual(0, s.Id);
        }

        [Test]
        public void TestTypeFunctions()
        {
            Square go = new Square(SquareType.Go, 0);
            Square prop = new Square(SquareType.Station, 15);
            Square tax = new Square(SquareType.Tax, 38);
            Square park = new Square(SquareType.Parking, 20);
            Square goJail = new Square(SquareType.GoToJail, 30);
            Square community = new Square(SquareType.Community, 17);
            Square chance = new Square(SquareType.Chance, 7);

            Assert.False(go.IsOwnable());
            Assert.False(go.IsStation());
            Assert.False(go.IsCompany());
            Assert.True(go.IsGo());
            Assert.False(go.IsChance());
            Assert.False(go.IsCommunityChest());
            Assert.False(go.IsProperty());
            Assert.False(go.IsFreeParking());
            Assert.False(go.IsTax());
            Assert.False(go.IsGoToJail());

            Assert.False(prop.IsCompany());
            Assert.True(prop.IsOwnable());
            Assert.True(prop.IsStation());
            Assert.False(prop.IsProperty());
            Assert.False(prop.IsChance());
            Assert.False(prop.IsCommunityChest());
            Assert.False(prop.IsGo());
            Assert.False(prop.IsFreeParking());
            Assert.False(prop.IsTax());
            Assert.False(prop.IsGoToJail());

            Assert.False(park.IsCompany());
            Assert.False(park.IsOwnable());
            Assert.False(park.IsStation());
            Assert.True(park.IsFreeParking());
            Assert.False(park.IsChance());
            Assert.False(park.IsCommunityChest());
            Assert.False(park.IsProperty());
            Assert.False(park.IsGo());
            Assert.False(park.IsTax());
            Assert.False(park.IsGoToJail());

            Assert.False(tax.IsCompany());
            Assert.False(tax.IsOwnable());
            Assert.False(tax.IsStation());
            Assert.True(tax.IsTax());
            Assert.False(tax.IsChance());
            Assert.False(tax.IsCommunityChest());
            Assert.False(tax.IsProperty());
            Assert.False(tax.IsFreeParking());
            Assert.False(tax.IsGo());
            Assert.False(tax.IsGoToJail());

            Assert.False(goJail.IsCompany());
            Assert.False(goJail.IsOwnable());
            Assert.False(goJail.IsStation());
            Assert.True(goJail.IsGoToJail());
            Assert.False(goJail.IsChance());
            Assert.False(goJail.IsCommunityChest());
            Assert.False(goJail.IsProperty());
            Assert.False(goJail.IsFreeParking());
            Assert.False(goJail.IsTax());
            Assert.False(goJail.IsGo());

            Assert.False(community.IsCompany());
            Assert.False(community.IsOwnable());
            Assert.False(community.IsStation());
            Assert.True(community.IsCommunityChest());
            Assert.False(community.IsChance());
            Assert.False(community.IsGo());
            Assert.False(community.IsProperty());
            Assert.False(community.IsFreeParking());
            Assert.False(community.IsTax());
            Assert.False(community.IsGoToJail());

            Assert.False(chance.IsCompany());
            Assert.False(chance.IsOwnable());
            Assert.False(chance.IsStation());
            Assert.True(chance.IsChance());
            Assert.False(chance.IsGo());
            Assert.False(chance.IsCommunityChest());
            Assert.False(chance.IsProperty());
            Assert.False(chance.IsFreeParking());
            Assert.False(chance.IsTax());
            Assert.False(chance.IsGoToJail());
            
        }
    }
}

