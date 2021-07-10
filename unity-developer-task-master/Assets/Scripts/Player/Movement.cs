using UnityEngine;


public class Movement : MonoBehaviour
{ 
    
    [Header("Movement settings")]
    [SerializeField] private float _speed;

    private Rigidbody _rb;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        MoveLogic();
    }
    
    private void MoveLogic()
    {
        _rb.AddForce(_movementVector * _speed, ForceMode.Impulse);
    }
    
    private Vector3 _movementVector
    {
        get
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            return new Vector3(horizontal, 0.0f, vertical);
        }
    }




}