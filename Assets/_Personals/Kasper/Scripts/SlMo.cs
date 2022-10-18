using System.Collections;
using UnityEngine;

public class SlMo : MonoBehaviour
{

    public float slowdownFactor = 0.2f;
    public float slowdownLength = 3f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            this.GetComponent<MeshRenderer>().enabled = false;
            this.GetComponent<SphereCollider>().enabled = false;
            Invoke("RestoreTime", slowdownLength);
            Time.timeScale = slowdownFactor;
            //Time.fixedDeltaTime = 0.02F * Time.timeScale;
            GameManager.Instance.CurrentTimeScale = slowdownFactor;
        }
    }
    private void RestoreTime()
    {
        Time.timeScale = 1f;
        //Time.fixedDeltaTime = 0.02F * Time.timeScale;
        GameManager.Instance.CurrentTimeScale = 1f;
        
        Destroy(gameObject);
    }
}