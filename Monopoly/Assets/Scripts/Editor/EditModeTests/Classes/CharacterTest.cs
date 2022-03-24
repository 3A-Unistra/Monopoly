using System;
using System.Collections.Generic;
using Monopoly.Classes;
using UnityEngine;
using UnityEngine.UI;
using NUnit.Framework;

namespace Monopoly.Classes
{
    public class TestCharacter
    {  
        [Test]
        public void TestCharacterCreation()
        {
            Character c=new Character(1);
            Assert.True(c.Id == 1);
        }
    }
}