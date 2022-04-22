/*
 * CameraLookTest.cs
 * 
 * Date created : 11/03/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using Monopoly.Util;

namespace Monopoly.Camera
{

    public class CameraLookTest
    {

        private CameraLook cl;
        private UnityEngine.Camera cam;
        private GameObject pivot;

        private const float X_ROT = 30.0f;
        private const float EPSILON = 0.001f;

        [OneTimeSetUp]
        public void SetUp()
        {
            pivot = new GameObject("Pivot");
            pivot.transform.localRotation = Quaternion.Euler(X_ROT, 45f, 0);

            GameObject gameobj = new GameObject("Camera");
            gameobj.transform.SetParent(pivot.transform);
            cl = gameobj.AddComponent<CameraLook>();
            cam = gameobj.GetComponent<UnityEngine.Camera>();
            cl.pivotPoint = pivot;
            cl.rotationSpeed = 0.1f;
            cl.modeSpeed = 0.1f;
            cl.zoomMin = 1.0f;
            cl.zoomMax = 10.0f;
            cl.zoomSpeed = 10.0f;
            cam.orthographicSize = 5.0f;
        }

        /*[Test]
        public void TestZoomIn()
        {
            float orthosize = cam.orthographicSize;
            cl.ZoomCamera(-1.5f);
            float newsize = cam.orthographicSize;
            Assert.True(MathUtil.EpsilonLesser(orthosize, newsize, EPSILON));
        }

        [Test]
        public void TestZoomOut()
        {
            float orthosize = cam.orthographicSize;
            cl.ZoomCamera(1.5f);
            float newsize = cam.orthographicSize;
            Assert.True(MathUtil.EpsilonGreater(orthosize, newsize, EPSILON));
        }*/

        [UnityTest]
        public IEnumerator TestRotateLeft()
        {
            Quaternion rot = pivot.transform.localRotation;
            cl.RotateCamera(-1);

            // wait until animation complete
            yield return new WaitUntil(() => !cl.Animating);

            Quaternion newrot = pivot.transform.localRotation;
            Assert.AreEqual(90.0f, Quaternion.Angle(rot, newrot), EPSILON);

            Quaternion expectrot = Quaternion.Euler(0, -90f, 0) * rot;
            Assert.AreEqual(0.0f, Quaternion.Angle(expectrot, newrot), EPSILON);
        }

        [UnityTest]
        public IEnumerator TestRotateRight()
        {
            Quaternion rot = pivot.transform.localRotation;
            cl.RotateCamera(1);

            // wait until animation complete
            yield return new WaitUntil(() => !cl.Animating);

            Quaternion newrot = pivot.transform.localRotation;
            Assert.AreEqual(90.0f, Quaternion.Angle(rot, newrot), EPSILON);

            Quaternion expectrot = Quaternion.Euler(0, 90f, 0) * rot;
            Assert.AreEqual(0.0f, Quaternion.Angle(expectrot, newrot), EPSILON);
        }

        [UnityTest]
        public IEnumerator TestToggleMode()
        {
            Quaternion rot, newrot;
            Vector3 euler;

            // start in isometric
            Assert.AreEqual(CameraLook.CameraLookMode.ISOMETRIC, cl.LookMode);

            rot = pivot.transform.localRotation;
            cl.ToggleCameraMode();

            // wait until animation complete
            yield return new WaitUntil(() => !cl.Animating);

            // swap to top-down
            newrot = pivot.transform.localRotation;
            euler = newrot.eulerAngles;
            Assert.AreEqual(90.0f, euler.x, EPSILON);
            Assert.True(MathUtil.EpsilonEquals(0.0f, euler.y, EPSILON) ||
                        MathUtil.EpsilonEquals(0.0f, euler.z, EPSILON));
            Assert.AreEqual(CameraLook.CameraLookMode.TOP_DOWN, cl.LookMode);

            rot = pivot.transform.localRotation;
            cl.ToggleCameraMode();

            // wait until animation complete
            yield return new WaitUntil(() => !cl.Animating);

            // switch back to isometric
            newrot = pivot.transform.localRotation;
            euler = newrot.eulerAngles;
            Assert.AreEqual(X_ROT, euler.x, EPSILON);
            Assert.AreEqual(CameraLook.CameraLookMode.ISOMETRIC, cl.LookMode);
        }

    }

}
