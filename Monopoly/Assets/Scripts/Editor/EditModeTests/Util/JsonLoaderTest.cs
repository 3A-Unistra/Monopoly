/*
 * JsonLoaderTest.cs
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

    public class JsonLoaderTest
    {

        [Test]
        public void TestLoadJsonAsset()
        {
            Dictionary<string, string> dic =
                JsonLoader.LoadJsonAsset
                <Dictionary<string, string>>("Locales/french");
            Assert.True(dic.Count > 0);
            Assert.True(dic.ContainsKey("test_string"));
            Assert.AreEqual(dic["test_string"], "test string");
        }

    }

}
