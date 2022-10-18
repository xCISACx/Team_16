using System;
using System.Collections;
using UnityEngine;

public class WheelRotate : MonoBehaviour
{
    //Decide in which rotation you want the wheel to be put inside the inspector. Higher values = faster rotation.
    [SerializeField] public Vector3 rotation;
    [SerializeField] private float right;
    [SerializeField] private float left;
    private void Start()
    {
        // collider = GetComponent<Collider>();
        // collider.isTrigger = false;
    }

    private void Update()
    {
        Movement(); //Has to be in Update
    }

    private void FixedUpdate()
    {
        RotateWheel(); 
    }

    private void LateUpdate()
    {
    }

    private void RotateWheel()
    {
        transform.Rotate(rotation * Time.deltaTime);
    }
    
    private void Movement()
    {
        Vector3 setRotation1 = new Vector3(right, 90, 0);
        Vector3 setRotation2 = new Vector3(left, 90, 0);
        Vector3 setRotation3 = new Vector3(0, 90, 0);

        if (Input.GetKeyDown("d")) transform.eulerAngles = setRotation1;
            else if (Input.GetKeyUp("d")) transform.eulerAngles = setRotation3;
        if (Input.GetKeyDown("a")) transform.eulerAngles = setRotation2;
            else if (Input.GetKeyUp("a")) transform.eulerAngles = setRotation3;
    }
    
    // public void OnTriggerEnter(Collider other)
    // {
    //     transform.Rotate(rotation * 0);
    //
    //     // //Debug.Log("hit");
    //     // if (other.isTrigger == this)
    //     // {
    //     //     StartCoroutine(WaitForFewSeconds());
    //     // }
    // }
    // public void OnTriggerExit(Collider other)
    // {
    //     if (other.isTrigger == this)
    //     {
    //         collider.isTrigger = false;
    //     }
    //     RotateWheel();
    // }
    // private IEnumerator WaitForFewSeconds()
    // {
    //     //Debug.Log("Hit");
    //     yield return new WaitForSeconds(5);
    //     transform.Rotate(rotation*0);
    // }

}


