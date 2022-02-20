using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HTTPServer.Samples
{
    public class CustomKeyInput : MonoBehaviour
    {
        public static CustomKeyInput Instance;
        private List<IKeyInput> _Inputs;

        private void Awake()
        {
            Instance = this;
            _Inputs = new List<IKeyInput>()
            {
                new WebInput(),
                new StandardInput()
            };
        }

        private void LateUpdate()
        {
            foreach (var keyInput in _Inputs)
            {
                keyInput.ProcessState();
            }
        }

        public static bool GetKey(KeyCode keyCode)
        {
            var inputs = Instance._Inputs;
            foreach (var keyInput in inputs)
            {
                if (keyInput.GetKey(keyCode))
                {
                    return true;
                }
            }

            return false;
        }
        public static bool GetKeyDown(KeyCode keyCode)
        {
            var inputs = Instance._Inputs;
            foreach (var keyInput in inputs)
            {
                if (keyInput.GetKeyDown(keyCode))
                {
                    return true;
                }
            }

            return false;
        }
        public static bool GetKeyUp(KeyCode keyCode)
        {
            var inputs = Instance._Inputs;
            foreach (var keyInput in inputs)
            {
                if (keyInput.GetKeyUp(keyCode))
                {
                    return true;
                }
            }

            return false;
        }
    }
}