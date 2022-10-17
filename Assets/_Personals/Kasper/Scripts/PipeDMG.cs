using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeDMG : MonoBehaviour
{
    [SerializeField] CharacterMovement characterMovement;

    [SerializeField] private Rigidbody PlayerBody;
 
       

    void Die()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            characterMovement.health = characterMovement.health - 40;
        }
    }
}
