using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Vector3 PlayerMovementInput;
    //private Vector2 PlayerMouseInput;

    public int health = 100;

    [SerializeField] private Rigidbody PlayerBody;
    [SerializeField] private float Speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpY;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float jumpTimer;
    [SerializeField] private float resetJumpTimer;
    [SerializeField] private float jumpCooldown;
    [SerializeField] public ParticleSystem Impact;
    [SerializeField] private Transform waterEffectSpawn;
        

    private void Start()
    {
        isGrounded = true;
       
    }

    public bool canMove;

    private void Update()
    {
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        MovePlayer();
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && canMove)
        {
            PlayerBody.AddForce(new Vector3(0, jumpY, 0) * jumpForce);
            isGrounded = false;

            //Debug.Log("Is jumping");
        }
        if (health <= 0f)
        {
            Die();
        }

        if(isGrounded == false)
        {
            jumpTimer += Time.deltaTime;
        }

        if(jumpTimer >= jumpCooldown)
        {
            jumpTimer = resetJumpTimer;
            isGrounded = true;
        }
    }

     
    void Die()
    {
        Destroy(gameObject);
    }

    public void MovePlayer()
    {
        if (canMove)
        {
            //Calculate movement dir
            Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput) * Speed;
            PlayerBody.velocity = new Vector3(MoveVector.x, PlayerBody.velocity.y, MoveVector.z);
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Pipe1")
        {
            Instantiate(Impact, waterEffectSpawn.position, waterEffectSpawn.rotation);           

        }
        if (collision.gameObject.name == "Obstacle")
        {
            Impact.Play();
        }
    }

    
   
 
}
