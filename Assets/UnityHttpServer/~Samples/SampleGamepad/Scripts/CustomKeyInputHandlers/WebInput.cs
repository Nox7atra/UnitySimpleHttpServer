using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HTTPServer.Samples
{
    public class WebInput : IKeyInput
    {
        private static Dictionary<KeyCode, KeyState> _Keys = new Dictionary<KeyCode, KeyState>();
        public static void SetKeyDown(KeyCode key)
        {
            _Keys[key] = KeyState.Down;
        }
        public static void SetKeyUp(KeyCode key)
        {
            _Keys[key] = KeyState.Up;
        }

        public bool GetKeyDown(KeyCode key)
        {
            if (!_Keys.ContainsKey(key)) return false;
            return _Keys[key] == KeyState.Down;
        }
        public bool GetKeyUp(KeyCode key)
        {
            if (!_Keys.ContainsKey(key)) return false;
            return _Keys[key] == KeyState.Up;
        }

        public bool GetKey(KeyCode key)
        {
            if (!_Keys.ContainsKey(key)) return false;
            return _Keys[key] == KeyState.Pressed;
        }
        
        public void ProcessState()
        {
            foreach (var key in _Keys.Keys.ToList())
            {
                switch (_Keys[key])
                {
                    case KeyState.Down:
                        _Keys[key] = KeyState.Pressed;
                        break;
                    case KeyState.Up:
                        _Keys[key] = KeyState.None;
                        break;
                }
            }
        }


        private enum KeyState
        {
            None = 0,
            Up = 1,
            Down = 2,
            Pressed = 4
        }
    }
}