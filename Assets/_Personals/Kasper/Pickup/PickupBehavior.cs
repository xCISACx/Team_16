using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBehavior : MonoBehaviour
{
    public float speed;

    private void Update()
    {
        transform.position -= transform.forward * Time.deltaTime * speed;

        if (transform.position.z < -10)
        {
            Destroy(gameObject);
        }
    }
}
