using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefuelTriggerBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (GameManager.Instance.GroundGenerator.activeFuelStations.Count > 0)
            {
                GameManager.Instance.GroundGenerator.activeFuelStations.Remove(GameManager.Instance.GroundGenerator.activeFuelStations[0]);
            }
        }
    }
}
