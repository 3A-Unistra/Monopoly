/*
 * PlayerPiece.cs
 * Graphical handler for the player pieces.
 * 
 * Date created : 06/04/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
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
        public int playerIndex;

        void Start()
        {
            if (playerUUID == null)
            {
                Debug.LogError("Tried to instantiate dead player piece.");
                Destroy(gameObject);
            }
            //playerIndex = ClientGameState.current.GetPlayerIndex(playerUUID);

            StartCoroutine(Move());
        }

        int idx = 0, i = 0;
        IEnumerator Move()
        {
            yield return new WaitForSeconds(1);
            while (i < 2)
            {
                SetPosition(idx);
                yield return new WaitForSeconds(1);
                ++idx;
                if (idx >= 40)
                {
                    idx = 0;
                    ++i;
                }
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
            //Debug.Log("xoff:" + xOff + ", zoff:" + zOff);
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
            transform.position = CalculateDesiredPosition(idx);
            transform.rotation = CalculateDesiredRotation(idx);
        }

        void Update()
        {
            
        }

    }

}
