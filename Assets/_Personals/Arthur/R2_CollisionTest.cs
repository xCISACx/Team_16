using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R2_CollisionTest : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Movement>() != null)
        {
            Debug.Log("MHUAHAHAHAHA");
        }
    }
}
