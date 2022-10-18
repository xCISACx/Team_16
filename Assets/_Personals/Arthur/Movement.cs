using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]

public class Movement : MonoBehaviour
{
    [SerializeField] float Move;
    [SerializeField] Rigidbody Rb;
    void Start()
    {
        if (Rb == null)
        {
            gameObject.GetComponent<Rigidbody>();
        }
    }

    void FixedUpdate()
    {
        Rb.MovePosition(transform.position + Vector3.left * Move * Time.fixedDeltaTime);
    }
}
