using System;
using UnityEngine;

namespace HTTPServer.Samples
{
    public class GamepadHandler : HTTPServerHandler
    {
        protected override string _route => "/gamepad/";
        private bool IsSuccess = true;
        private string ErrorMessage;
        public override void ProcessParams(string url)
        {
            base.ProcessParams(url);
            if(Enum.TryParse<KeyCode>(_params[1], true, out var key))
            {
                if (_params[0] == "down")
                {
                    WebInput.SetKeyDown(key);
                }
                else
                {
                    
                    WebInput.SetKeyUp(key);
                }
            }
            else
            {
                ErrorMessage = $"Invalid keycode in param 1. There are no keycode {_params[0]}";
            }
        }

        public override NetworkAnswer GetAnswerData()
        {
            return new NetworkAnswer()
            {
                status = IsSuccess ? 200 : 500,
                errorMessage = IsSuccess ? null : ErrorMessage
            };
        }
    }

}
