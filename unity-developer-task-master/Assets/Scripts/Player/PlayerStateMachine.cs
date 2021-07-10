using System;
using UnityEngine;



namespace Player
{
    public class PlayerStateMachine : MonoBehaviour
    {
        [Header("Movement settings")]
        [SerializeField] private float _speed;

        [Header("Player Model")] 
        [SerializeField] private GameObject _playerModel;
        [SerializeField] private GameObject _player;
        
        private Transform _cameraMain;
        private Rigidbody _rb;

        private enum State { IDLE, RUN, FINISHING };

        private State _state = State.IDLE;
        private State _lastState = State.IDLE;
        private Animator _animator;
        private object Vector3;

        private void Start()
        {
            _cameraMain = Camera.main.transform;
            
            _rb = GetComponent<Rigidbody>();
            
            _animator = _playerModel.GetComponent<Animator>();
        }
        private void FixedUpdate()
        {
 
            if(_state == State.RUN) MoveLogic();
        }

        private void Update()
        {
            switch (_state)
            {
                case State.IDLE:
                    _lastState = State.IDLE;
                    if (Input.GetButtonDown("Horizontal")||Input.GetButtonDown("Vertical"))
                    {
                        SetState(State.RUN);
                    }
                    break;
                case State.RUN :
                    if (_rb.velocity.x<=1 && _rb.velocity.x>=-1
                        && _rb.velocity.z <=1 && _rb.velocity.z >=-1
                                          && !Input.GetButton("Horizontal") && !Input.GetButton("Vertical"))
                    {
                        SetState(State.IDLE);
                    }
                    break;
                case State.FINISHING:
                    break;
            }

            SetAnimation();
        }

        private void MoveLogic()
        {
            _rb.AddForce(_movementVector * _speed, ForceMode.Impulse);
            RotationPlayer();
        }
    
        //Поворот модельки персонажа в сторону камеры
        private void RotationPlayer()
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                Vector3 flatForward = _cameraMain.transform.forward;
                flatForward.y = 0;
                if (flatForward != UnityEngine.Vector3.zero)
                {
                    flatForward.Normalize();
                    _playerModel.transform.rotation = Quaternion.LookRotation(flatForward);
                }
            }
        }
        //Высчитывается вектор для перемещения
        private Vector3 _movementVector
        {
            get
            {
                var horizontal = Input.GetAxis("Horizontal");
                var vertical = Input.GetAxis("Vertical");
                
                Vector3 camF = _cameraMain.forward;
                Vector3 camR = _cameraMain.right;
                camF.y = 0f;
                camR.y = 0f;
 
                Vector3 movingVector = horizontal * camR.normalized + vertical * camF.normalized;
                return movingVector;
            }
        }


        //Смена состояния
        private void SetState(State to)
        {
            _lastState = _state;
            _state = to;
        }
        
        //Установка анимации
        private void SetAnimation()
        {
            switch (_state)
            {
                case State.IDLE:
                    _animator.Play("Idle");
                    break;
                case State.RUN :
                    if (Input.GetKey(KeyCode.S))
                    {
                        _animator.Play("Back_Run_Rifle");
                    }
                    else if (Input.GetKey(KeyCode.W))
                    {
                        _animator.Play("Run_Rifle");
                    }else
                    if (Input.GetKey(KeyCode.A))
                    {
                        _animator.Play("Run_Left_Rifle");
                    } else if (Input.GetKey(KeyCode.D))
                    {
                        _animator.Play("Run_Right_Rifle");
                    }
                    
                    break;
                case State.FINISHING:
                    break;
            }
        }
        
    }
}