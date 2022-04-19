/*
 * CameraLook.cs
 * Camera rotation and zoom script.
 * 
 * Date created : 26/02/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Monopoly.UI;

namespace Monopoly.Camera
{

    /**
     * <summary>
     *     Camera rotation and zoom script.
     *     Ensure the camera's pivot point is setup in an isometric starting
     *     position to ensure the script works correctly.
     * </summary>
     */
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraLook : MonoBehaviour
    {

        /**
         *  <summary>
         *      Camera viewing mode enum.
         *  </summary>
         */
        public enum CameraLookMode
        {
            ISOMETRIC, TOP_DOWN
        }

        /**
         * <summary>
         * The Euclidian distance from the camera to the pivot point.
         * This needs to be large enough to avoid plane-clipping between
         * the near-plane of the camera and the board, and the far-plane with
         * the distant side.
         * </summary>
         */
        [Range(0.0f, 20.0f)]
        public float distance = 10.0f;

        private UnityEngine.Camera cam;
        private Quaternion pivotFrom, pivotTo;
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
         *     The current viewing mode of the camera.
         * </summary>
         */
        [HideInInspector]
        public CameraLookMode LookMode
        {
            get;
            private set;
        } = CameraLookMode.ISOMETRIC;

        /**
         * <summary>
         *     The number of seconds that a rotation should take to complete.
         * </summary>
         */
        [Range(0.01f, 2.0f)]
        public float rotationSpeed = 0.25f;
        /**
         * <summary>
         *     The minimum zoom (closest the board) possible for the camera.
         * </summary>
         */
        [Range(1.0f, 10.0f)]
        public float zoomMin = 1.0f;
        /**
         * <summary>
         *     The maximum zoom (furthest from the board) possible for the camera.
         * </summary>
         */
        [Range(1.0f, 10.0f)]
        public float zoomMax = 10.0f;
        /**
         * <summary>
         *     The speed at which zoom should occur for the camera.
         * </summary>
         */
        [Range(0.01f, 20.0f)]
        public float zoomSpeed = 5.0f;
        /**
         * <summary>
         *     The number of seconds that a switch between isometric and top-down
         *     viewing angles should take to occur.
         * </summary>
         */
        [Range(0.01f, 2.0f)]
        public float modeSpeed = 0.25f;

        private bool modeToggle = false, rotateToggle = false;
        /**
         * <summary>
         *     Is the camera currently locked in an animation?
         * </summary>
         */
        [HideInInspector]
        public bool Animating
        {
            get;
            private set;
        }
        private float animateTime = 0.0f;
        private int rotationSide = 0;

        /**
         * <summary>
         *     The button that rotates the camera left.
         * </summary>
         */
        public Button rotateLeftButton;
        /**
         * <summary>
         *     The button that rotates the camera right.
         * </summary>
         */
        public Button rotateRightButton;
        /**
         * <summary>
         *     The button that changes the camera perspective.
         * </summary>
         */
        public Button perspectiveButton;

        /**
         * <summary>
         *     The sprite to use for the isometrtic view change.
         * </summary>
         */
        public Sprite isometricSprite;

        /**
         * <summary>
         *     The sprite to use for the top-down view change.
         * </summary>
         */
        public Sprite topdownSprite;

        void Start()
        {
            float axisDistance = Mathf.Sqrt(2f) / 2f;
            cam = GetComponent<UnityEngine.Camera>();
            cam.orthographic = true;
            cam.transform.localPosition = new Vector3(0,
                                                      0,
                                                      -axisDistance * distance);
            cam.transform.localRotation = Quaternion.identity;
            if (pivotPoint == null)
            {
                Debug.LogError("Camera has no active pivot point.");
                Destroy(this);
            }
            if (rotateLeftButton != null)
            {
                rotateLeftButton.onClick.AddListener(
                    delegate {
                        RotateCamera(1);
                    }
                );
            }
            if (rotateRightButton != null)
            {
                rotateRightButton.onClick.AddListener(
                    delegate {
                        RotateCamera(-1);
                    }
                );
            }
            if (perspectiveButton != null)
            {
                perspectiveButton.onClick.AddListener(ToggleCameraMode);
                SetPerspectiveSprite();
            }
        }

        /**
         * <summary>
         *     Toggle the camera viewing mode between isometric and top-down.
         * </summary>
         */
        public void ToggleCameraMode()
        {
            if (!Animating)
            {
                Animating = true;
                modeToggle = true;
                pivotFrom = pivotPoint.transform.localRotation;
                if (LookMode == CameraLookMode.TOP_DOWN)
                {
                    LookMode = CameraLookMode.ISOMETRIC;
                    pivotTo = Quaternion.Euler(30, 45+(90*rotationSide), 0);
                }
                else
                {
                    LookMode = CameraLookMode.TOP_DOWN;
                    pivotTo = Quaternion.Euler(90, 90+(90*rotationSide), 0);
                }
                animateTime = 0.0f;
                SetPerspectiveSprite();
            }
        }

        /**
         * <summary>
         *     Rotate the camera a full 90 degrees left or right.
         * </summary>
         * <param name="dir">
         *     The direction in which the rotation should take place.
         *     This value is clamped between -1 and 1. If the value given is
         *     equal to 0, then no rotation shall take place.
         * </param>
         */
        public void RotateCamera(int dir)
        {
            if (!Animating && dir != 0)
            {
                Animating = true;
                rotateToggle = true;
                pivotFrom = pivotPoint.transform.localRotation;
                dir = dir < 0 ? -1 : 1; // -1,1 clamp
                rotationSide += dir;
                if (rotationSide >= 4)
                    rotationSide -= 4;
                else if (rotationSide <= -4)
                    rotationSide += 4;
                pivotTo = Quaternion.Euler(0, dir * 90, 0) * pivotFrom;
                animateTime = 0.0f;
            }
        }

        /**
         * <summary>
         *     Zoom the camera in or out by a given delta.
         * </summary>
         * <param name="delta">
         *     The delta to zoom by. This is typically the delta motion of the
         *     mouse wheel.
         * </param>
         */
        public void ZoomCamera(float delta)
        {
            float zoomDisplacement = delta * Time.deltaTime * zoomSpeed;
            cam.orthographicSize =
                Mathf.Clamp(cam.orthographicSize + zoomDisplacement,
                            zoomMin, zoomMax);
        }

        /**
         * <summary>
         *     Animate a rotation or mode switch on the camera.
         *     This locks out any other rotation request until the animation
         *     is complete.
         * </summary>
         */
        private void AnimateCamera()
        {
            if (modeToggle || rotateToggle)
            {
                // animate camera mode toggle
                if (modeToggle)
                    animateTime += Time.deltaTime / modeSpeed;
                else
                    animateTime += Time.deltaTime / rotationSpeed;
                if (animateTime >= 1.0f)
                {
                    // end of animation, set to destination quaternion
                    Animating = false;
                    modeToggle = false;
                    rotateToggle = false;
                    pivotPoint.transform.localRotation = pivotTo;
                }
                else
                {
                    // animating, add to rotation
                    pivotPoint.transform.localRotation =
                        Quaternion.Lerp(pivotFrom, pivotTo, animateTime);
                }
            }
        }

        private void SetPerspectiveSprite()
        {
            if (perspectiveButton != null)
            {
                if (LookMode == CameraLookMode.ISOMETRIC)
                    perspectiveButton.image.sprite = topdownSprite;
                else
                    perspectiveButton.image.sprite = isometricSprite;
            }
        }

        void Update()
        {
            if (!UIDirector.IsEditingInputField())
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    ToggleCameraMode();
                }
                else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
                {
                    int pivotDir = (Input.GetKeyDown(KeyCode.Q) ? 1 : 0) +
                                   (Input.GetKeyDown(KeyCode.E) ? -1 : 0);
                    RotateCamera(pivotDir);
                }
            }
            if (Input.mouseScrollDelta.y != 0)
            {
                ZoomCamera(Input.mouseScrollDelta.y);
            }
            if (Animating)
                AnimateCamera();
        }

    }

}
