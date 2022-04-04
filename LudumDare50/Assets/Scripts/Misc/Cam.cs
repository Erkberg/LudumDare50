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
            transform.position = new Vector3(Game.inst.player.transform.position.x + 2f, 1f, -11f);

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
