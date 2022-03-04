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

        /**
         * <summary>
         *     The number of pixels on the X or Y axis from the edge of the
         *     screen that the mouse must be for the camera to begin moving in
         *     that direction. If these numbers are not more than zero, it is
         *     undefined behaviour.
         * </summary>
         */
        public Vector2 boundsMouse = new Vector2(48, 48);
        /**
         * <summary>
         *     Whether or not to allow the mouse to move the camera.
         * </summary>
         */
        public bool moveCameraByMouse = true;

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
                /*
                 * When we are in the isometric view, the forward vector
                 * of the camera is not parallel to the board, which causes
                 * problems, because the camera will want to move away from
                 * or into the plane of the board, which will cause clipping
                 * and visual artifacts.
                 * 
                 * To counteract this, we take the forward vector, reduce the
                 * pitch rotation (Y) to zero, normalize it again to get the
                 * directional vector of the camera now parallel to the board,
                 * and apply the translation to this vector rather than the
                 * normal forwards or upwards vectors.
                 */
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

        /**
         * <summary>
         *     Attempts to move the camera based on the position of the mouse
         *     on the screen.
         * </summary>
         * <returns>
         *     <c>true</c> if the mouse was on an edge and moved the camera.
         * </returns>
         */
        private bool MoveCameraMouse()
        {
            /* TODO: Disable mouse movement when mouse is over UI. */
            if (!moveCameraByMouse)
                return false;
            Vector3 mp = Input.mousePosition;
            int sx = Screen.width, sy = Screen.height;
            int dx = 0, dy = 0;
            if (mp.x <= boundsMouse.x)
                dx = -1;
            else if (mp.x >= sx - boundsMouse.x)
                dx = 1;
            if (mp.y <= boundsMouse.y)
                dy = -1;
            else if (mp.y >= sy - boundsMouse.y)
                dy = 1;
            MoveCamera(dx, dy);
            return dx != 0 || dy != 0;
        }

        void Update()
        {
            if (!look.Animating)
            {
                // camera movement
                if (!MoveCameraMouse())
                {
                    MoveCamera(Input.GetAxis("Horizontal"),
                               Input.GetAxis("Vertical"));
                }
            }
        }

    }

}
