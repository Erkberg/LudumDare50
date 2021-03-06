using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50
{
    [System.Serializable]
    public class PlayerStateData
    {
        public PlayerState state;
        public float duration = 0.5f;
        public float durationTillPerfect = 0f;
        public float perfectDuration = 0.05f;
        public float durationTillCancelPossible = 0.7f;
    }
}
