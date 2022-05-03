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

using Monopoly.UI;
using Monopoly.Util;

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
         *     The speed at which the camera should move when dragged.
         * </summary>
         */
        [Range(0.1f, 5.0f)]
        public float dragSpeed = 2.0f;
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
        private Vector3 mouseDragOrigin, mouseDragLast;

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
         * <param name="exact">
         *     Whether or not the <paramref name="horizontal"/> and
         *     <paramref name="vertical"/> parameters are multiples of the
         *     movement speed or the exact displacement of the camera.
         * </param>
         */
        public void MoveCamera(float horizontal, float vertical, bool exact)
        {
            if (horizontal == 0f && vertical == 0f)
                return;
            float d = exact ? 1.0f : moveSpeed * Time.deltaTime;
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
         *     Attempts to move the camera based on drag-and-drop
         *     interactability of the mouse on the screen.
         * </summary>
         */
        private bool MoveCameraMouse()
        {
            if (Input.GetMouseButtonDown(0))
            {
                mouseDragOrigin = Input.mousePosition;
                return true;
            }
            else if (Input.GetMouseButton(0))
            {
                mouseDragLast = mouseDragOrigin;
                mouseDragOrigin = Input.mousePosition;
            }
            else
            {
                return false;
            }

            Vector3 pos = cam.ScreenToViewportPoint(
                mouseDragLast - mouseDragOrigin) * look.desiredZoom * dragSpeed;
            Vector2 move = new Vector2(pos.x, pos.y);
            MoveCamera(move.x, move.y, true);
            return move.x != 0 || move.y != 0;
        }

        void LateUpdate()
        {
            if (!look.Animating && !UIDirector.IsMenuOpen &&
                !UIDirector.IsGameMenuOpen)
            {
                if (!MoveCameraMouse() && !UIDirector.IsEditingInputField())
                {
                    MoveCamera(Input.GetAxis("Horizontal"),
                               Input.GetAxis("Vertical"),
                               false);
                }
            }
        }

    }

}
