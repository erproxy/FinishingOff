using System;
using UnityEngine;


public class Movement : MonoBehaviour
{
    public float speed =10f;

    private Rigidbody rb;
    public GameObject Player;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.drag = 0;
            gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * speed);
           // Debug.Log("asdf");
        }
        else
        {
            rb.drag = 20;
        }
    }
}