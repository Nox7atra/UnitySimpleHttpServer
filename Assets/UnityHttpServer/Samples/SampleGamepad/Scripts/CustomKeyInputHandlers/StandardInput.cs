using System.Collections;
using System.Collections.Generic;
using HTTPServer.Samples;
using UnityEngine;

public class StandardInput : IKeyInput
{
    public bool GetKey(KeyCode key)
    {
        return Input.GetKey(key);
    }

    public bool GetKeyDown(KeyCode key)
    {
        return Input.GetKeyDown(key);
    }

    public bool GetKeyUp(KeyCode key)
    {
        return Input.GetKeyUp(key);
    }

    public void ProcessState()
    {
        
    }
}
