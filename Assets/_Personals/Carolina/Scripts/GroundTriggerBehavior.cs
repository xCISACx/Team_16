using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTriggerBehavior : MonoBehaviour
{
    public DropManager DropManager;

    private void Awake()
    {
        DropManager = FindObjectOfType<DropManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Droplet")
        {
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            DropManager.inactiveDroplets.Add(other.gameObject);
            other.gameObject.SetActive(false);
        }
    }
}
