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

        public int playerIndex = 0;

        private int idx = 0;
        private int oldIdx = 0;

        public Vector2 bigSquarePosition;
        public Vector2 squarePosition;
        public Vector2 prisonPosition;
        public Vector2 inPrisonPosition;
        public Vector3 squareScale;

        void Start()
        {
            idx = 0;
            oldIdx = 0;
            if (playerUUID == null)
            {
                Debug.LogError("Tried to instantiate dead player piece.");
                Destroy(gameObject);
            }
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
                yield return new WaitForSeconds(1);
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
            Vector3 pos = center;
            Vector2 off;
            if (idx != 0 && idx != 10 && idx != 20 && idx != 30)
            {
                off = squarePosition;
            }
            else if (idx != 10)
            {
                off = bigSquarePosition;
            }
            else
            {
                if (ClientGameState.current.GetPlayer(playerUUID).InJail)
                    off = inPrisonPosition;
                else
                    off = prisonPosition;
            }

            if ((idx >= 0 && idx < 10))
            {
                pos.x -= off.x;
                pos.z += off.y;
            }
            else if (idx >= 20 && idx < 30)
            {
                pos.x -= off.x;
                pos.z -= off.y;
            }
            else if (idx > 10 && idx < 20)
            {
                pos.x += off.y;
                pos.z += off.x;
            }
            else if (idx >= 30 && idx < 40)
            {
                pos.x += off.y;
                pos.z += off.x;
            }
            else if (idx == 10)
            {
                pos.x += off.x;
                pos.z += off.y;
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
            //if (idx != 0 && idx != 10 && idx != 20 && idx != 30)
                return squareScale;
            //return Vector3.one;
        }

        private Quaternion CalculateDesiredRotation(int idx)
        {
            if (idx != 0 && idx != 10 && idx != 20 && idx != 30)
            {
                SquareCollider square = SquareCollider.Colliders[idx];
                return square.transform.rotation;
            }
            if (idx == 10)
            {
                return Quaternion.Euler(0.0f, 0.0f, 0.0f);
            }
            else if (idx == 20)
            {
                return Quaternion.Euler(0.0f, 90.0f, 0.0f);
            }
            else if (idx == 30)
            {
                return Quaternion.Euler(0.0f, 180.0f, 0.0f);
            }
            else
            {
                return Quaternion.Euler(0.0f, 0.0f, 0.0f);
            }
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
