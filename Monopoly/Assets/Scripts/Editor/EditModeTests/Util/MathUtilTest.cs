/*
 * MathUtilTest.cs
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

    public class MathUtilTest
    {

        private const float EPSILON = 0.0001f;

        [Test]
        public void TestEpsilonEquals()
        {
            Assert.False(MathUtil.EpsilonEquals(
                100.0f, 10.0f, EPSILON));
            Assert.False(MathUtil.EpsilonEquals(
                10.0f, 100.0f, EPSILON));
            Assert.False(MathUtil.EpsilonEquals(
                3.141f, Mathf.PI, EPSILON));

            Assert.True(MathUtil.EpsilonEquals(
                0.0f, 0.0f, EPSILON));
            Assert.True(MathUtil.EpsilonEquals(
                -0.0f, 0.0f, EPSILON));
            Assert.True(MathUtil.EpsilonEquals(
                100.0f, 100.0f, EPSILON));
            Assert.True(MathUtil.EpsilonEquals(
                0.70710678f, Mathf.Sqrt(2.0f) / 2.0f, EPSILON));
            Assert.True(MathUtil.EpsilonEquals(
                0.33333f, 1.0f/3.0f, EPSILON));
        }

        [Test]
        public void TestEpsilonLesser()
        {
            Assert.False(MathUtil.EpsilonLesser(
                100.0f, 10.0f, EPSILON));
            Assert.False(MathUtil.EpsilonLesser(
                3.1416f, Mathf.PI, EPSILON));
            Assert.False(MathUtil.EpsilonLesser(
                -0.0f, 0.0f, EPSILON));
            Assert.False(MathUtil.EpsilonLesser(
                0.0f, -0.0f, EPSILON));
            Assert.False(MathUtil.EpsilonLesser(
                0.0f, -1.0f, EPSILON));

            Assert.True(MathUtil.EpsilonLesser(
                3.1414f, Mathf.PI, EPSILON));
            Assert.True(MathUtil.EpsilonLesser(
                0.0f, 1.0f, EPSILON));
            Assert.True(MathUtil.EpsilonLesser(
                -1.0f, 0.0f, EPSILON));
            Assert.True(MathUtil.EpsilonLesser(
                -1.0f, 1.0f, EPSILON));
        }

        [Test]
        public void TestEpsilonGreater()
        {
            Assert.False(MathUtil.EpsilonGreater(
                -0.0f, 0.0f, EPSILON));
            Assert.False(MathUtil.EpsilonGreater(
                0.0f, -0.0f, EPSILON));
            Assert.False(MathUtil.EpsilonGreater(
                0.0f, 1.0f, EPSILON));
            Assert.False(MathUtil.EpsilonGreater(
                3.1414f, Mathf.PI, EPSILON));
            Assert.False(MathUtil.EpsilonGreater(
                -1.0f, 0.0f, EPSILON));
            Assert.False(MathUtil.EpsilonGreater(
                -1.0f, 1.0f, EPSILON));

            Assert.True(MathUtil.EpsilonGreater(
                100.0f, 10.0f, EPSILON));
            Assert.True(MathUtil.EpsilonGreater(
                3.1417f, Mathf.PI, EPSILON));
            Assert.True(MathUtil.EpsilonGreater(
                0.0f, -1.0f, EPSILON));
        }

        [Test]
        public void TestCompareVector3()
        {
            Assert.AreEqual(0, MathUtil.CompareVector3(
                new Vector3(0, 0, 0), new Vector3(0, 0, 0)
            ));
            Assert.AreEqual(0, MathUtil.CompareVector3(
                new Vector3(0, 0, 0), new Vector3(-0, -0, -0)
            ));
            Assert.AreEqual(0, MathUtil.CompareVector3(
                new Vector3(1, 2, 3), new Vector3(1, 2, 3)
            ));
            Assert.AreEqual(1, MathUtil.CompareVector3(
                new Vector3(1, 2, 3), new Vector3(-1, -2, -3)
            ));
            Assert.AreEqual(-1, MathUtil.CompareVector3(
                new Vector3(-1, -2, -3), new Vector3(1, 2, 3)
            ));
            Assert.AreEqual(-1, MathUtil.CompareVector3(
                new Vector3(-1, 0, 0), new Vector3(0, 0, 0)
            ));
            Assert.AreEqual(-1, MathUtil.CompareVector3(
                new Vector3(0, -1, 0), new Vector3(0, 0, 0)
            ));
            Assert.AreEqual(-1, MathUtil.CompareVector3(
                new Vector3(0, 0, -1), new Vector3(0, 0, 0)
            ));
            Assert.AreEqual(1, MathUtil.CompareVector3(
                new Vector3(1, 0, 0), new Vector3(0, 0, 0)
            ));
            Assert.AreEqual(1, MathUtil.CompareVector3(
                new Vector3(0, 1, 0), new Vector3(0, 0, 0)
            ));
            Assert.AreEqual(1, MathUtil.CompareVector3(
                new Vector3(0, 0, 1), new Vector3(0, 0, 0)
            ));
            Assert.AreEqual(-1, MathUtil.CompareVector3(
                new Vector3(0, 0, 1), new Vector3(1, 0, 0)
            ));
            Assert.AreEqual(1, MathUtil.CompareVector3(
                new Vector3(1, 0, 1), new Vector3(1, 0, 0)
            ));
            Assert.AreEqual(-1, MathUtil.CompareVector3(
                new Vector3(1, 0, 1), new Vector3(1, 1, 0)
            ));
            Assert.AreEqual(1, MathUtil.CompareVector3(
                new Vector3(1, 1, 1), new Vector3(1, 1, 0)
            ));
        }

    }

}
