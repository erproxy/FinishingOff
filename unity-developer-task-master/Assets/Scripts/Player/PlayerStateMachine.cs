using System;
using Enemy;
using UnityEngine;
using Player.Dto;
using UnityEngine.Events;



namespace Player
{
    public class PlayerStateMachine : MonoBehaviour
    {
        [Header("Player Settings")]
        [SerializeField] private float _speed;
        [SerializeField] private float _drag;

        [Header("Player Model")] 
        [SerializeField] private GameObject _playerModel;
        [SerializeField] private GameObject _player;

        [Header("Weapons")] 
        [SerializeField] private GameObject _w_Automatic;
        [SerializeField] private GameObject _w_Sword3;
        
        private Transform _cameraMain;
        private Rigidbody _rb;

        public UnityEvent NotEnemySearched;
        public UnityEvent EnemySearched;
        //enums
        private enum State { IDLE, RUN, FINISHING, KILLING};
        private enum Finishing{KILLING,DEFOLT}

        private Finishing _finishing = Finishing.DEFOLT;
        private State _state = State.IDLE;
        
        //Animator
        private Animator _animator;
        
        //GameObject который будет добиваться
        private GameObject _enemy;
        //Противник кешируется на случай, если добивание уже будет происходить а появится новый противник
        private GameObject _enemyCash;
        
        
        private void Start()
        {
            _cameraMain = Camera.main.transform;
            
            _rb = GetComponent<Rigidbody>();
            
            _animator = _playerModel.GetComponent<Animator>();
        }
        
        //Подпись на Ивент, когда найдется близстоячий противник
        public void StateKilling(EnemyDto dto)
        {
            _enemy = dto.EnemyGO;
            _finishing = Finishing.KILLING;
        }
        
        //Подпись на ивент, когда произойдет момент в анимации, пронзания оружием
        public void EnemyKill()
        {
            if (_enemyCash!=null)
            {
                _enemyCash.GetComponent<EnemyDie>().TakeDmg();
                
            }
            else
            {
                SetState(State.IDLE);
                _finishing = Finishing.DEFOLT;
            }
        }
        //Подпись на ивент, когда анимация убийства закончена
        public void EnemyKilled()
        {
            if (_enemyCash!=null)
            {
                _w_Automatic.SetActive(true);
                _w_Sword3.SetActive(false);
            }
            SetState(State.IDLE);
            _finishing = Finishing.DEFOLT;
        }


        private void FixedUpdate()
        {
 
            if(_state == State.RUN) MoveLogic();
        }

        private void Update()
        {
            if (_enemy!=null && _state!=State.FINISHING&& _state!=State.KILLING)
            {
                EnemySearched?.Invoke();
            }else
            {
                NotEnemySearched?.Invoke();
            }
            
            switch (_finishing)
            {
                case Finishing.DEFOLT:
                    break;
                case Finishing.KILLING: //SetState(State.FINISHING);
                    break;
            }
            
            switch (_state)
            {
                case State.IDLE:
                    _rb.drag = _drag;
                    if (Input.GetKey(KeyCode.Space)&&_finishing==Finishing.KILLING)
                    {
                        SetState(State.FINISHING);
                        _enemyCash = _enemy;
                    }else
                    if (Input.GetButtonDown("Horizontal")||Input.GetButtonDown("Vertical"))
                    {
                        SetState(State.RUN);
                        _enemyCash = _enemy;
                    }
                    break;
                case State.RUN :
                    if (Input.GetKey(KeyCode.Space)&&_finishing==Finishing.KILLING)
                    {
                        SetState(State.FINISHING);
                    }else
                    if (_rb.velocity.x<=1 && _rb.velocity.x>=-1
                                          && _rb.velocity.z <=1 && _rb.velocity.z >=-1
                                          && !Input.GetButton("Horizontal") && !Input.GetButton("Vertical"))
                    {
                        SetState(State.IDLE);
                    }
                    break;
                case State.FINISHING:
                    if (_enemyCash != null)
                    {
                        //Проверяется, достигнул ли игрок минимального расстояния для убийства
                        if (Vector3.Distance(_enemyCash.transform.position, _player.transform.position)<1.5f)
                        {
                            _w_Automatic.SetActive(false);
                            _w_Sword3.SetActive(true);
                            SetState(State.KILLING);
                            //обнуление скорости
                            var temp = _rb.velocity;
                            temp.y = 0.0f;
                            _rb.velocity = temp;
                            _rb.drag = 120;
                        }
                        else
                        {
                            MoveToEnemy();
                        }
                        
                    } else SetState(State.IDLE);
                    break;
                case State.KILLING:
                    break;
            }
            
            SetAnimation();
            _enemy = null;
        }

        //Логика передвежения Игрока для убийства цели
        //Поворачиваем модельку и двигаемся вперед без остановки, пока вызывается метод
        private void MoveToEnemy()
        {
            _playerModel.transform.LookAt(_enemyCash.transform);
            _rb.AddForce(_playerModel.transform.forward*_speed, ForceMode.Impulse);
        }
        
        //Логика свободного перемещения
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
                if (flatForward != Vector3.zero)
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
                    _animator.Play("Run_Rifle");
                    break;
                case State.KILLING:
                    _animator.Play("Finishing");
                    break;
            }
        }
        
    }
}