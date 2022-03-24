/*
 * MathUtil.cs
 * Math utility file.
 * 
 * Date created : 15/03/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monopoly.Util
{

    public static class MathUtil
    {

        public static bool EpsilonEquals(float x, float y, float epsilon)
        {
            return Mathf.Abs(x - y) < Mathf.Abs(epsilon);
        }

        public static bool EpsilonLesser(float x, float y, float epsilon)
        {
            if (EpsilonEquals(x, y, epsilon))
                return false;
            return x < y;
        }

        public static bool EpsilonGreater(float x, float y, float epsilon)
        {
            if (EpsilonEquals(x, y, epsilon))
                return false;
            return x > y;
        }

        public static int CompareVector3(Vector3 a, Vector3 b)
        {
            float epsilon = 0.01f;
            if (EpsilonLesser(a.x, b.x, epsilon))
                return -1;
            if (EpsilonGreater(a.x, b.x, epsilon))
                return 1;
            if (EpsilonLesser(a.y, b.y, epsilon))
                return -1;
            if (EpsilonGreater(a.y, b.y, epsilon))
                return 1;
            if (EpsilonLesser(a.z, b.z, epsilon))
                return -1;
            if (EpsilonGreater(a.z, b.z, epsilon))
                return 1;
            return 0;
        }

    }

}
