/*
 * PlayerPiece.cs
 * Graphical handler for the player pieces.
 * 
 * Date created : 06/04/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 * Author       : Christophe PIERSON <chrsitophe.pierson@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Monopoly.Runtime;
using Monopoly.Graphics;

namespace Monopoly.Graphics
{

    public class PlayerPiece : MonoBehaviour
    {

        [HideInInspector]
        public string playerUUID;

        [HideInInspector]
        private float animateTime = 0.0f;

        [HideInInspector]
        private bool dirty = true;

        [HideInInspector]
        public bool step1 = false;
        [HideInInspector]
        public bool step2 = false;

        private Vector3 floor1;

        [Range(2.0f, 5.0f)]
        public float moveSpeed = 5.0f;

        [HideInInspector]
        public int playerIndex = 0;

        private int idx = 0;
        private int oldIdx = 0;

        void Start()
        {
            idx = 0;
            oldIdx = 0;
            //if (playerUUID == null)
            //{
            //    Debug.LogError("Tried to instantiate dead player piece.");
            //    Destroy(gameObject);
            //}
            transform.position = CalculateDesiredPosition(idx);
            transform.rotation = CalculateDesiredRotation(idx);
            //StartCoroutine(Move());
        }

        int tmpindex = 0, i = 0;
        IEnumerator Move()
        {
            yield return new WaitForSeconds(1);
            while (i < 2)
            {
                yield return new WaitForSeconds(2);
                ++tmpindex;
                if (tmpindex >= 40)
                {
                    tmpindex = 0;
                    ++i;
                }
                SetPosition(tmpindex);
            }
        }

        private Vector3 CalculateOffset(Vector3 center, int idx)
        {
            // this crap produces a grid-like offset that lets the 8 characters
            // occupy the same square without overlap
            // FIXME: need to implement special cases for prison as well as
            // rotations for corner pieces
            float offset = 0.3f;
            float xAmount = offset * playerIndex % 3;
            float zAmount = offset * (playerIndex / 3);
            Vector3 pos = center;

            float xOff =
                (idx >= 0 && idx < 10 ? 1 : idx >= 20 && idx <= 30 ? -1 : 0);
            float zOff =
                (idx >= 10 && idx < 20 ? 1 : idx >= 30 && idx <= 40 ? -1 : 0);
            
            Debug.Log("xoff:" + xOff + ", zoff:" + zOff);
            if ((idx >= 0 && idx < 10) || (idx >= 20 && idx < 30))
            {
                pos.x += offset; // base offset
                pos.x -= xOff * xAmount;
                pos.z += zOff * zAmount;
            }
            else
            {
                pos.x -= xOff * zAmount;
                pos.z -= offset; // base offset
                pos.z += zOff * xAmount;
            }
            return pos;
        }

        private Vector3 CalculateDesiredPosition(int idx)
        {
            // find the center position of the relevant SquareCollider
            SquareCollider square = SquareCollider.Colliders[idx];
            Vector3 squarePos = square.transform.position;
            // now calculate an offset from this position based on the players
            // index, or 'player number'. we do this to stop all of the
            // character pieces from colliding and intersecting on the same
            // space in the scene which is ugly and wrong
            squarePos = CalculateOffset(squarePos, idx);
            return squarePos;
        }

        private Vector3 CalculateDesiredScale(int idx)
        {
            return Vector3.one;
        }

        private Quaternion CalculateDesiredRotation(int idx)
        {
            SquareCollider square = SquareCollider.Colliders[idx];
            return square.transform.rotation;
        }

        public void SetPosition(int idx)
        {
            if (idx < 0 || idx >= 40)
            {
                Debug.LogWarning(string.Format(
                    "Can't set piece position to invalid location {0}!", idx));
                return;
            }

            this.idx = idx;
        }

        public void AnimationPawn(int idx)
        {
            animateTime += Time.deltaTime * moveSpeed;
            transform.rotation = CalculateDesiredRotation(idx);
            Vector3 pos = CalculateDesiredPosition(idx);
            if (dirty)
            {
                floor1 = transform.position;
                dirty = false;
            }
            Vector3 ceiling1 = floor1;
            ceiling1.y += 1.0f;

            Vector3 floor2 = pos;
            Vector3 ceiling2 = floor2;
            ceiling2.y += 1.0f;
            Debug.Log(floor2);
            if (!step1)
            {
                transform.position = Vector3.Lerp(floor1, ceiling1, animateTime);
                if (transform.position.y >= ceiling1.y)
                {
                    step1 = true;
                    transform.position = ceiling1;
                    animateTime = 0.0f;
                }
                return;
            }

            else if (!step2)
            {
                transform.position = Vector3.Lerp(ceiling1, ceiling2, animateTime);
                if (animateTime > 1.0f)
                {
                    step2 = true;
                    transform.position = ceiling2;
                    animateTime = 0.0f;
                }
                return;
            }

            else
            {
                transform.position = Vector3.Lerp(ceiling2, floor2, animateTime);
                if (animateTime > 1.0f)
                {
                    step1 = false;
                    step2 = false;
                    animateTime = 0.0f;
                    oldIdx = idx;
                    dirty = true;
                    transform.rotation = CalculateDesiredRotation(idx);
                }
                return;
            }
        }

        void Update()   
        {
            if (oldIdx != idx)
                AnimationPawn(idx);
        }
    }

}
