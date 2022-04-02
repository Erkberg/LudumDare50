using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ErksUnityLibrary;

namespace LudumDare50
{
    public class Cam : MonoBehaviour
    {
        public Camera cam;
        public Screenshake screenshake;

        private float moveSpeed = 0f;

        private void LateUpdate()
        {
            if (!moveSpeed.IsApproxEqual(0f))
            {
                transform.AddPositionX(moveSpeed * Time.deltaTime);
            }
        }

        public void SetMoveSpeed(float speed)
        {
            moveSpeed = speed;
        }
    }
}
