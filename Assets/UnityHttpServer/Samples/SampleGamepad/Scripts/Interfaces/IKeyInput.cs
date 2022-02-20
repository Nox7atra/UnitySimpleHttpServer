using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HTTPServer.Samples
{
    public interface IKeyInput
    {
        bool GetKey(KeyCode key);
        bool GetKeyDown(KeyCode key);
        bool GetKeyUp(KeyCode key);
        void ProcessState();
    }
}