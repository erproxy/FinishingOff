using System;
using UnityEngine;

namespace Player
{
    public class BodyRegular : MonoBehaviour
    {
        
        [SerializeField] private float _AngleX;
        [SerializeField] private Transform _targetBone;
        [SerializeField] private Transform _playerModel;
        [SerializeField] private Transform _camerTarger;
        
        
        private void LateUpdate()
        {
            var cameraRotX = _camerTarger.localEulerAngles.x;
            var cameraRotY = _camerTarger.localEulerAngles.y;
            var playerRotY = _playerModel.localEulerAngles.y;
            
            //Высчитывает разницу между углом игрока и углом камеры, которая подставляется для угла торса
              var _differenceRotationCamPlayer = playerRotY - cameraRotY;

              //если угол поворота персонажа меньше 180
              if (playerRotY<180)
              {
                  if (cameraRotY<=(playerRotY+180) && cameraRotY>=playerRotY)
                  {
                      if (cameraRotY >= playerRotY + _AngleX)
                      {
                          _targetBone.localEulerAngles = new Vector3(-_AngleX, 0.0f,-cameraRotX-15);
                      }else if (cameraRotY >= playerRotY && cameraRotY <= playerRotY + _AngleX)
                      {
                          _targetBone.localEulerAngles = new Vector3(_differenceRotationCamPlayer, 0.0f,-cameraRotX-15); 
                      }
                  }
                  else if (cameraRotY <= 360 && cameraRotY>=(playerRotY+180))
                  {
                      var cashCameraRotY = cameraRotY - 360;
                      RotationBefore180(cashCameraRotY, playerRotY, cameraRotX, _differenceRotationCamPlayer);
                  }
                  else
                  {
                      RotationBefore180(cameraRotY, playerRotY, cameraRotX , _differenceRotationCamPlayer);
                  }
              } else 
                  //если угол персонажа больше 180
              if (playerRotY > 180) 
              {
                  if (cameraRotY>=(playerRotY-180) && cameraRotY<=playerRotY)
                  {
                      if (cameraRotY <= playerRotY - _AngleX)
                      {
                          _targetBone.localEulerAngles = new Vector3(_AngleX, 0.0f,-cameraRotX-15);
                      }else if (cameraRotY <= playerRotY && cameraRotY >= playerRotY - _AngleX)
                      {
                          _targetBone.localEulerAngles = new Vector3(_differenceRotationCamPlayer, 0.0f,-cameraRotX-15); 
                      }
                  } else if (cameraRotY >=0 && cameraRotY<=(playerRotY-180))
                  {
                      var cashPlayerRotY = playerRotY - 360;
                      RotationAfter180(cameraRotY, cashPlayerRotY, cameraRotX, _differenceRotationCamPlayer);
                  }
                  else
                  {
                      RotationAfter180(cameraRotY, playerRotY, cameraRotX, _differenceRotationCamPlayer);
                  }
              }
        }

        //Данный алгоритм нужен, когда угол поворота персонажа после 180
        private void RotationAfter180(float cameraRotY, float playerRotY, float cameraRotX, float _differenceRotationCamPlayer)
        {
            if (cameraRotY>=playerRotY+_AngleX)
            {
                _targetBone.localEulerAngles = new Vector3(-_AngleX, 0.0f,-cameraRotX-15); 
            }else if (cameraRotY >= playerRotY && cameraRotY <= playerRotY + _AngleX)
            {
                _targetBone.localEulerAngles = new Vector3(_differenceRotationCamPlayer, 0.0f,-cameraRotX-15); 
            }
        }
        
        //Данный алгоритм нужен, когда угол поворота персонажа до 180
        private void RotationBefore180(float cameraRotY, float playerRotY, float cameraRotX, float _differenceRotationCamPlayer)
        {
            if (cameraRotY<=playerRotY-_AngleX)
            { 
                _targetBone.localEulerAngles = new Vector3(_AngleX, 0.0f,-cameraRotX-15); 
            }else if (cameraRotY <= playerRotY && cameraRotY >= playerRotY - _AngleX)
            {
               _targetBone.localEulerAngles = new Vector3(_differenceRotationCamPlayer, 0.0f,-cameraRotX-15); 
            }
        }
    }
}