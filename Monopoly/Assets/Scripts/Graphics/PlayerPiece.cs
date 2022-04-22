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

        private float animateTime = 0.0f;

        private bool dirty = true;
        private bool animating = false;

        [HideInInspector]
        public bool step1 = false;

        private Vector3 floor1;
        private Vector3 startScale;
        private Quaternion startRotation;
        private Coroutine moveEnumerator = null;

        [Range(2.0f, 10.0f)]
        public float moveSpeed = 8.0f;

        public int playerIndex = 0;

        private int idx = 0;
        private int oldIdx = 0;

        [Range(0.1f, 2.0f)]
        public float height = 0.8f;

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
                yield return new WaitUntil(() => !animating);
                ++tmpindex;
                if (tmpindex >= 40)
                {
                    tmpindex = 0;
                    ++i;
                }
                MoveToPosition(tmpindex, true);
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
                //if (ClientGameState.current.GetPlayer(playerUUID).InJail)
                //    off = inPrisonPosition;
                //else
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
            if (idx == 0)
                return Quaternion.Euler(0.0f, 270.0f, 0.0f);
            else if (idx == 10)
                return Quaternion.Euler(0.0f, 0.0f, 0.0f);
            else if (idx == 20)
                return Quaternion.Euler(0.0f, 90.0f, 0.0f);
            else if (idx == 30)
                return Quaternion.Euler(0.0f, 180.0f, 0.0f);
            else
                return Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }

        public void MoveToPosition(int idx, bool instant)
        {
            if (idx < 0 || idx >= 40)
            {
                Debug.LogWarning(string.Format(
                    "Can't set piece position to invalid location {0}!", idx));
                return;
            }
            if (moveEnumerator != null)
                StopCoroutine(moveEnumerator);
            moveEnumerator = StartCoroutine(MoveEnumerator(idx, instant));
        }

        private IEnumerator MoveEnumerator(int idx, bool instant)
        {
            int startidx = this.idx;
            int currentidx = instant ? idx : startidx;
            do
            {
                animating = true;
                this.idx = ++currentidx;
                currentidx %= 40;
                yield return new WaitUntil(() => !animating);
                if (instant)
                    break;
            }
            while (currentidx != idx);
            moveEnumerator = null;
        }

        public void AnimationPawn(int idx)
        {
            animateTime += Time.deltaTime * moveSpeed;
            Quaternion targetRotation = CalculateDesiredRotation(idx);
            Vector3 pos = CalculateDesiredPosition(idx);
            Vector3 targetScale = CalculateDesiredScale(idx);
            if (dirty)
            {
                floor1 = transform.position;
                startScale = transform.localScale;
                startRotation = transform.rotation;
                dirty = false;
            }
            Vector3 ceiling = (floor1 + pos) / 2;
            ceiling.y += height;

            Vector3 floor2 = pos;

            if (!step1)
            {
                transform.position = Vector3.Lerp(floor1, ceiling, animateTime);
                if (transform.position.y >= ceiling.y)
                {
                    step1 = true;
                    transform.position = ceiling;
                    animateTime = 0.0f;
                }
            }
            else
            {
                transform.rotation = Quaternion.Lerp(startRotation, targetRotation, animateTime);
                transform.localScale = Vector3.Lerp(startScale, targetScale, animateTime);
                transform.position = Vector3.Lerp(ceiling, floor2, animateTime);
                if (animateTime > 1.0f)
                {
                    step1 = false;
                    animateTime = 0.0f;
                    transform.position = floor2;
                    oldIdx = idx;
                    dirty = true;
                    animating = false;
                }
            }
        }


        void Update()   
        {
            if (oldIdx != idx)
                AnimationPawn(idx);
        }
    }

}
