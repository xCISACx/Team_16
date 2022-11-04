using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefuelStationBehaviour : MonoBehaviour
{
    private void OnDisable()
    {
        GameManager.Instance.GroundGenerator.activeFuelStations.Remove(this.gameObject);
    }
}
