using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraL
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTarget;
        [SerializeField] private float _minAngle;
        [SerializeField] private float _maxAngle;
        [SerializeField] private float _mouseSensitivity;
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            float aimX = Input.GetAxis("Mouse X");
            float aimY = Input.GetAxis("Mouse Y");

            _cameraTarget.rotation *= Quaternion.AngleAxis(aimX * _mouseSensitivity,Vector3.up);
            _cameraTarget.rotation *= Quaternion.AngleAxis(-aimY * _mouseSensitivity, Vector3.right);
            
            var angleX = _cameraTarget.localEulerAngles.x;
            if(angleX > 180 && angleX < _maxAngle)
            {
                angleX = _maxAngle;
            }
            else if (angleX < 180 && angleX > _minAngle)
            {
                angleX = _minAngle;
            }
 
            _cameraTarget.localEulerAngles = new Vector3(angleX, _cameraTarget.localEulerAngles.y, 0);
        }
    }
}