using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monopoly.Test
{

    public class Rotator : MonoBehaviour
    {

        [Range(0.0f, 1.0f)]
        public float x = 1.0f, y = 0.0f, z = 0.0f;

        public float speed = 1.0f;

        void Start()
        {

        }

        void Update()
        {
            if (x > 0.0f)
                transform.Rotate(Vector3.right, speed);
            if (y > 0.0f)
                transform.Rotate(Vector3.up, speed);
            if (z > 0.0f)
                transform.Rotate(Vector3.forward, speed);
        }
    }

}
