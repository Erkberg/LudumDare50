using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50
{
    public class GameInput : MonoBehaviour
    {
        private Controls controls;

        private void Awake()
        {
            controls = new Controls();
            controls.Enable();
        }

        public bool GetAttack()
        {
            return controls.Player.Attack.IsPressed();
        }

        public bool GetAttackDown()
        {
            return controls.Player.Attack.WasPressedThisFrame();
        }

        public bool GetDefend()
        {
            return controls.Player.Defend.IsPressed();
        }

        public bool GetDefendDown()
        {
            return controls.Player.Defend.WasPressedThisFrame();
        }

        public bool GetJump()
        {
            return controls.Player.Jump.IsPressed();
        }

        public bool GetJumpDown()
        {
            return controls.Player.Jump.WasPressedThisFrame();
        }

        public bool GetDuck()
        {
            return controls.Player.Duck.IsPressed();
        }

        public bool GetDuckDown()
        {
            return controls.Player.Duck.WasPressedThisFrame();
        }

        public bool GetCheat()
        {
            return controls.Player.Cheat.IsPressed();
        }

        public bool GetMenuDown()
        {
            return controls.Player.Menu.WasPressedThisFrame();
        }
    }
}
