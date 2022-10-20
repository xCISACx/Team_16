using System.Collections;
using UnityEngine;

public class SlMo : MonoBehaviour
{

    public float slowdownFactor = 0.2f;
    public float slowdownLength = 1f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            this.GetComponent<MeshRenderer>().enabled = false;
            this.GetComponent<SphereCollider>().enabled = false;
            Time.timeScale = slowdownFactor;
            GameManager.Instance.CurrentTimeScale = slowdownFactor;
            //Time.fixedDeltaTime = 0.02F * Time.timeScale;
            Invoke("RestoreTime", slowdownLength);
        }
    }
    private void RestoreTime()
    {
        GameManager.Instance.CurrentTimeScale = 1f;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;


        Destroy(gameObject);
    }
}