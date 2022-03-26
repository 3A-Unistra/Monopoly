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
    public class CardTest
    {  
        [Test]
        public void TestCardCreation()
        {
            Card ch = new Card("Chance", 1, "123");
            Card co = new Card("Community", 2, "456");
            Assert.True(ch.type == "Chance");
            Assert.True(ch.id == 1);
            Assert.True(ch.desc == "123");
            Assert.True(co.type == "Community");
            Assert.True(co.id == 2);
            Assert.True(co.desc == "456");
        }
    }
}