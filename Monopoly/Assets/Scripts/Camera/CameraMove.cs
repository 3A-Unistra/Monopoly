/*
 * CameraMove.cs
 * Camera movement script.
 * 
 * Date created : 27/02/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monopoly.Camera
{

    /**
     * <summary>
     *     Camera movement script.
     * </summary>
     */
    [RequireComponent(typeof(UnityEngine.Camera))]
    [RequireComponent(typeof(CameraLook))]
    public class CameraMove : MonoBehaviour
    {

        /**
         * <summary>
         *     The speed at which the camera should move.
         * </summary>
         */
        [Range(0.1f, 100.0f)]
        public float moveSpeed = 5.0f;
        /**
         * <summary>
         *     The pivot point from which all of the camera rotations and
         *     movements take place. This point should be flat on the surface of
         *     the game board.
         * </summary>
         */
        public GameObject pivotPoint;
        /**
         * <summary>
         *     The minimum X and Z that the camera can be moved to. Each
         *     X and Y value of this vector <b>must</b> be less than the
         *     corresponding values of the <see href="boundsMax" /> vector, or
         *     else it is undefined behaviour.
         * </summary>
         */
        public Vector2 boundsMin;
        /**
         * <summary>
         *     The maximum X and Z that the camera can be moved to. Each
         *     X and Y value of this vector <b>must</b> be greater than the
         *     corresponding values of the <see href="boundsMin" /> vector, or
         *     else it is undefined behaviour.
         * </summary>
         */
        public Vector2 boundsMax;

        private UnityEngine.Camera cam;
        private CameraLook look;

        void Start()
        {
            cam = GetComponent<UnityEngine.Camera>();
            look = GetComponent<CameraLook>();
            if (pivotPoint == null)
            {
                Debug.LogError("Camera has no active pivot point.");
                Destroy(this);
            }
        }

        /**
         * <summary>
         *     Clamp the position of the camera to a given boundary.
         * </summary>
         * <param name="t">
         *     The transform upon which the bounds are applied. This is
         *     typically the pivot point.
         * </param>
         */
        private void ClampPosition(Transform t)
        {
            Vector3 pos = t.localPosition;
            if (pos.x < boundsMin.x)
                pos.x = boundsMin.x;
            if (pos.z < boundsMin.y)
                pos.z = boundsMin.y;
            if (pos.x > boundsMax.x)
                pos.x = boundsMax.x;
            if (pos.z > boundsMax.y)
                pos.z = boundsMax.y;
            pos.y = 0f;
            t.localPosition = pos;
        }

        /**
         * <summary>
         *     Move the camera up, down, left or right.
         * </summary>
         * <param name="horizontal">
         *     The horizontal delta by which to move the camera.
         * </param>
         * <param name="vertical">
         *     The vertical delta by which to move the camera.
         * </param>
         */
        private void MoveCamera(float horizontal, float vertical)
        {
            if (horizontal == 0f && vertical == 0f)
                return;
            float d = moveSpeed * Time.deltaTime;
            if (look.LookMode == CameraLook.CameraLookMode.ISOMETRIC)
            {
                Vector3 flatForward =
                    cam.transform.forward;
                flatForward.y = 0;
                flatForward.Normalize();
                pivotPoint.transform.localPosition +=
                    flatForward * d * vertical;
            }
            else
            {
                pivotPoint.transform.localPosition +=
                    cam.transform.up * d * vertical;
            }
            pivotPoint.transform.localPosition +=
                cam.transform.right * d * horizontal;
            ClampPosition(pivotPoint.transform);
        }

        void Update()
        {
            if (!look.Animating)
            {
                // camera movement
                MoveCamera(Input.GetAxis("Horizontal"),
                           Input.GetAxis("Vertical"));
            }
        }

    }

}
