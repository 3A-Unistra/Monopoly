/*
 * StringLocaliserTest.cs
 * 
 * Date created : 11/03/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Monopoly.Util
{

    public class StringLocaliserTest
    {

        [OneTimeSetUp]
        public void SetUp()
        {
            StringLocaliser.Reset();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            StringLocaliser.Reset();
        }

        [Test, Order(0)]
        public void TestGetStringUninitialised()
        {
            string s = StringLocaliser.GetString("test_string");
            Assert.True(s.Equals("null"));
        }

        [Test, Order(1)]
        public void TestLoadStrings()
        {
            bool x =
                StringLocaliser.LoadStrings("Locales/french", "french", "Français");
            Assert.True(x);
        }

        [Test, Order(2)]
        public void TestSetLanguage()
        {
            bool x = StringLocaliser.SetLanguage("french");
            Assert.True(x);
        }

        [Test, Order(3)]
        public void TestGetString()
        {
            string s = StringLocaliser.GetString("test_string");
            Assert.True(s.Equals("test string"));
        }

        [Test, Order(4)]
        public void TestGetLanguageName()
        {
            Assert.AreEqual("Français", StringLocaliser.GetLanguageName());
        }

        [Test, Order(5)]
        public void TestGetLanguageList()
        {
            string[] langs = StringLocaliser.GetLanguageList();
            Debug.Log(
                string.Format("There are {0} languages loaded.", langs.Length));
            Assert.True(langs.Length == 1);
            Assert.AreEqual("french", langs[0]);
        }

    }

}
