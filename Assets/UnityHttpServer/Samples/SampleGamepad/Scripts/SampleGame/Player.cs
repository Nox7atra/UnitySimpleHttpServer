using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HTTPServer.Samples
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _MoveSpeed = 1;
        private void Update()
        {
            if (CustomKeyInput.GetKey(KeyCode.A))
            {
                transform.Translate(Time.deltaTime* _MoveSpeed * Vector3.left);
            }
            if (CustomKeyInput.GetKey(KeyCode.D))
            {
                transform.Translate(Time.deltaTime* _MoveSpeed * Vector3.right);
            }
        }
    }
}