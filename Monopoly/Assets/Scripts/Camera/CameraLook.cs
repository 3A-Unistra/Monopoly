using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monopoly.Camera
{

    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraLook : MonoBehaviour
    {

        public enum CameraLookMode
        {
            ISOMETRIC, TOP_DOWN
        }

        [Range(-15.0f, 15.0f)]
        public float height = 6.0f;
        [Range(0.0f, 20.0f)]
        public float distance = 10.0f;
        [Range(1.0f, 10.0f)]
        public float zoomMin = 1.0f;
        [Range(1.0f, 10.0f)]
        public float zoomMax = 10.0f;
        public GameObject pivotPoint;
        [Range(2.0f, 0.01f)]
        public float rotationSpeed = 1.0f;
        public float zoomSpeed = 5.0f;
        public float moveSpeed = 5.0f;
        public CameraLookMode lookMode = CameraLookMode.ISOMETRIC;

        private UnityEngine.Camera cam;
        private bool pivoting = false;
        private float pivotTime = 0.0f;
        private Quaternion pivotFrom, pivotTo;

        void Start()
        {
            float axisDistance = Mathf.Sqrt(2f) / 2f;
            cam = GetComponent<UnityEngine.Camera>();
            cam.orthographic = true;
            cam.transform.localPosition = new Vector3(0,
                                                      height,
                                                      -axisDistance * distance);
            cam.transform.localRotation = Quaternion.Euler(30, 0, 0);
            if (pivotPoint == null)
            {
                Debug.LogError("Camera has no active pivot point.");
                Destroy(this);
            }
        }

        void Update()
        {
            if (lookMode == CameraLookMode.ISOMETRIC)
            {
                // isometric view code
                if (!pivoting)
                {
                    // camera movement
                    float horDir = Input.GetAxis("Horizontal");
                    pivotPoint.transform.localPosition +=
                        cam.transform.right *
                        horDir * moveSpeed * Time.deltaTime;
                    float verDir = Input.GetAxis("Vertical");
                    pivotPoint.transform.localPosition +=
                        cam.transform.up *
                        verDir * moveSpeed * Time.deltaTime;
                    // camera rotatation
                    int pivotDir = (Input.GetKeyDown(KeyCode.Q) ? 1 : 0) +
                                   (Input.GetKeyDown(KeyCode.E) ? -1 : 0);
                    if (pivotDir != 0)
                    {
                        pivotFrom = pivotPoint.transform.localRotation;
                        pivotTo =
                            pivotFrom * Quaternion.Euler(0, pivotDir * 90, 0);
                        pivotTime = 0.0f;
                        pivoting = true;
                    }
                }
                else if (pivoting)
                {
                    pivotTime += Time.deltaTime / rotationSpeed;
                    if (pivotTime < 1.0f)
                    {
                        pivotPoint.transform.localRotation =
                            Quaternion.Lerp(pivotFrom, pivotTo, pivotTime);
                    }
                    else
                    {
                        pivoting = false;
                        pivotPoint.transform.localRotation = pivotTo;
                    }
                }
                // mouse zoom
                if (Input.mouseScrollDelta.y != 0)
                {
                    float zoomDisplacement =
                        Input.mouseScrollDelta.y * Time.deltaTime * zoomSpeed;
                    Debug.Log(Input.mouseScrollDelta.y);
                    cam.orthographicSize =
                        Mathf.Clamp(cam.orthographicSize + zoomDisplacement,
                                    zoomMin, zoomMax);
                }
            }
            else
            {
                // top down view code

            }
        }

    }

}
