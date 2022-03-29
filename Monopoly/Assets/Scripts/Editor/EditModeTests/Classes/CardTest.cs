/* 
 * CardTest
 * Fichier de test unitaire pour Card.cs
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
            Card ch = new Card(1, false);
            Card co = new Card(2, true);
            Assert.True(ch.id == 1);
            Assert.True(co.id == 2);
        }
    }
}