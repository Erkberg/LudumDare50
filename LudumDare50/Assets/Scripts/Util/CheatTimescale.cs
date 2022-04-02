using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50
{
    public class CheatTimescale : MonoBehaviour
    {
        public float scale = 8f;

        private void Update()
        {
            if (Game.inst.input.GetCheat())
                Time.timeScale = scale;
            else
                Time.timeScale = 1f;
        }
    }
}
