using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using Unity.VisualScripting;

public class InputTest : MonoBehaviour
{
    public float StrafeDistance;
    public Rigidbody Rb;
    public float JumpForce;
    public bool CanStrafe;
    public bool CanJump;
    public bool Grounded;
    public bool Jumping;
    [SerializeField] private Vector3 GroundCheckDistance;
    [SerializeField] private float GroundCheckRadius;
    [SerializeField] private LayerMask GroundLayerMask;
    public Coroutine movementCooldownRoutine = null;
    public float StrafeDuration;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private GameObject Impact;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float maxJumpHeight;
    private Vector3 velocity;
    [SerializeField] private float _airTime;
    [SerializeField] private float _maxAirTime;
    [SerializeField] private Controls Controls;
    [SerializeField] public GameObject TankWater;
    public List<GameObject> Wheels;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        Controls = new Controls();
        Controls.Enable();
    }

    private void OnDisable()
    {
        GetComponent<PlayerInput>().actions = null;
        Controls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        Grounded = Physics.CheckSphere(transform.position - GroundCheckDistance, GroundCheckRadius, GroundLayerMask);
        //Grounded = Physics.CheckBox(transform.position - GroundCheckDistance, (transform.position - GroundCheckDistance) + GroundCheckRadius, GroundLayerMask);
        /*CanJump = Grounded;
        CanStrafe = Grounded;*/
    }

    private void FixedUpdate()
    {
        if (!Grounded && Rb.velocity.y <= 0) //we're falling
        {
            //Rb.velocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.deltaTime;
            Rb.AddForce(-Vector3.up * fallMultiplier);
        }

        if (!Grounded && transform.position.y >= jumpHeight)
        {
            _airTime += Time.fixedDeltaTime;
        }

        if (!Grounded && _airTime > _maxAirTime)
        {
            Rb.AddForce(-Vector3.up * fallMultiplier);
        }

            /*if (Rb.velocity.y > 0)
            {
                Rb.velocity = Rb.velocity += Vector3.up * Physics.gravity.y * Time.deltaTime;
            }*/
        
        /*if (!Grounded)
        {
            velocity += (velocity.y < 0 ? Physics.gravity * 1.1f : Physics.gravity * 0.2f) * Time.deltaTime;
            Rb.velocity = velocity;
        }*/
        
        //Rigidbody.MovePosition(new Vector3(0, 0, transform.position.z + 1));
        
        var clampedPosY = Mathf.Clamp(transform.position.y, 1.01f, jumpHeight);
        transform.position = new Vector3(transform.position.x, clampedPosY, transform.position.z);
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if (!CanStrafe) return;

        var sign = Mathf.Sign(context.ReadValue<Vector2>().x);
        
        GameManager.Instance.CurrentLaneIndex += (int) sign;
        
        GameManager.Instance.CurrentLaneIndex = Mathf.Clamp(GameManager.Instance.CurrentLaneIndex, 0, 2);

        if (sign != 0 && context.ReadValue<Vector2>().y == 0)
        {
            transform.DOMoveX(GameManager.Instance.LanePositions[GameManager.Instance.CurrentLaneIndex].x, StrafeDuration, false).SetEase(Ease.Linear);   
        }

        /*if (context.ReadValue<Vector2>().x > 0)
        {
            if (transform.position.x < StrafeDistance && CanStrafe)
            {
                //Debug.Log("moving left");
                
                var initialPos = transform.position;
                
                var newPos = initialPos - new Vector3(StrafeDistance, 0, 0);
                
                //transform.position += new Vector3(-StrafeDistance, 0, 0);

                transform.DOMoveX(GameManager.Instance.LanePositions[GameManager.Instance.CurrentLaneIndex].x, StrafeDuration, false).SetEase(Ease.Linear);
                
                CanStrafe = false;
                
                StartCoroutine(EnableStrafeAfterSeconds(StrafeDuration));
            }
        }
        else if (context.ReadValue<Vector2>().x < 0)
        {
            if (transform.position.x > -StrafeDistance && CanStrafe)
            {
                //Debug.Log("moving left");
                
                var initialPos = transform.position;
                
                var newPos = initialPos - new Vector3(StrafeDistance, 0, 0);
                
                //transform.position += new Vector3(-StrafeDistance, 0, 0);

                transform.DOMoveX(GameManager.Instance.LanePositions[GameManager.Instance.CurrentLaneIndex].x, StrafeDuration, false).SetEase(Ease.Linear);
                
                CanStrafe = false;
                StartCoroutine(EnableStrafeAfterSeconds(StrafeDuration));

                /*while (transform.position != newPos)
                {
                    transform.DOMoveX(transform.position.x - StrafeDistance, 0.5f, false).SetEase(Ease.Linear);
                    CanStrafe = false;
                }#1#
            }
        }*/
    }

    public void Jump(InputAction.CallbackContext context)
    {
        /*svar sign = Mathf.Sign(context.ReadValue<Vector2>().y);
        Debug.Log(context.ReadValue<Vector2>());

        if (context.ReadValue<Vector2>() == Vector2.zero)
            return;*/
        
        Debug.Log(context.ReadValue<float>());
        if (context.ReadValue<float>() == 0)
            return;
        
        if (Grounded && CanJump)
        {
            Debug.Log("Jumping");
            Rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            Debug.Log("can't jump jumping started");
            Jumping = true;
            //Rb.velocity += Vector3.up * JumpForce;
        }
        else if (!Grounded && Jumping)
        {
            Debug.Log("Force falling");
            Rb.AddForce(-Vector3.up * JumpForce * 2, ForceMode.Impulse);
            //Rb.velocity -= Vector3.up * JumpForce * 2;
        }

        /*if (sign > 0)
        {
            if (Grounded && CanJump)
            {
                CanJump = false;
                Debug.Log("Jumping");
                Rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                //Rb.velocity += Vector3.up * JumpForce;
            }
        }
        
        else if (sign < 0)
        {
            if (!Grounded && !CanJump)
            {
                Debug.Log("Force falling");
                Rb.AddForce(-Vector3.up * JumpForce * 2, ForceMode.Impulse);
                //Rb.velocity -= Vector3.up * JumpForce * 2;
            }
        }*/
        
        ////Debug.Log("Jump pressed");
        
        /*if (Grounded && CanJump)
        {
            CanJump = false;
            CanStrafe = false;
            //Debug.Log("Jumping");

            Rb.velocity += Vector3.up * JumpForce;
            
            //Rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            Falling = true;
        }*/
    }

    IEnumerator ResumeMovementAfterSeconds(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        FindObjectOfType<GroundGenerator>().moving = true;
        CanStrafe = true;
        CanJump = true;
    }
    
    IEnumerator EnableStrafeAfterSeconds(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        CanStrafe = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            GameManager.Instance.LoseFuel(GameManager.Instance.FuelLossAmount);
            GameManager.Instance.LoseSpeed();

            GameManager.Instance.GroundGenerator.moving = false;
            
            StopCoroutine(GameManager.Instance.AddToMultiplier());
            
            CanJump = false;
            Debug.Log("can't jump obstacle");
            CanStrafe = false;
            
            other.transform.gameObject.SetActive(false);

            if (movementCooldownRoutine != null)
            {
                StopCoroutine(movementCooldownRoutine);
                ////Debug.Log("coroutine already running, stopping and starting");
            }

            if (!GameManager.Instance.GameOver)
            {
                movementCooldownRoutine = StartCoroutine(ResumeMovementAfterSeconds(1f));
            }
            
            var origin = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            
            Instantiate(Impact, origin, Quaternion.identity);
            Impact.GetComponent<ParticleSystem>().Play();

            GameManager.Instance.musician.PlaySound(2);
            
            other.transform.GetComponent<ObstacleBehaviour>().roadParent.SetScoreTriggers(false);
        }

        if (other.gameObject.CompareTag("Refuel"))
        {
            if (Grounded)
            {
                GameManager.Instance.GainFuel(GameManager.Instance.FuelGainAmount);
                
                GameManager.Instance.GroundGenerator.moving = false;
            
                StopCoroutine(GameManager.Instance.AddToMultiplier());
            
                CanJump = false;
                Debug.Log("can't jump refuel");
                CanStrafe = false;
            
                if (movementCooldownRoutine != null)
                {
                    StopCoroutine(movementCooldownRoutine);
                    ////Debug.Log("coroutine already running, stopping and starting");
                }
            
                if (!GameManager.Instance.GameOver)
                {
                    movementCooldownRoutine = StartCoroutine(ResumeMovementAfterSeconds(1f));
                }
            }
            else
            {
                GameManager.Instance.GainFuel(GameManager.Instance.FuelGainAmount / 2);
                
                //GameManager.Instance.GroundGenerator.moving = false;
            
                //StopCoroutine(GameManager.Instance.AddToMultiplier());
            
                //CanJump = false;
                //Debug.Log("can't jump refuel");
                //CanStrafe = false;
            
                /*if (movementCooldownRoutine != null)
                {
                    StopCoroutine(movementCooldownRoutine);
                    ////Debug.Log("coroutine already running, stopping and starting");
                }
            
                if (!GameManager.Instance.GameOver)
                {
                    movementCooldownRoutine = StartCoroutine(ResumeMovementAfterSeconds(1f));
                }*/
            }
            GameManager.Instance.musician.PlaySound(1);
            // Add coroutine that changes speed multiplier to 1 temporarily and then resets it to what it was before

            

            //         Destroy(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("ScoreTrigger"))
        {
            GameManager.Instance.AddScore(50);
            GameManager.Instance.UpdateScoreUI();
            other.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Grounded = true;
            CanJump = true;
            CanStrafe = true;
            jumpHeight = 0;
            jumpHeight = transform.position.y + maxJumpHeight;
            _airTime = 0;

            Jumping = false;

            /*if (!Grounded)
            {
                Grounded = true;
                CanJump = true;
                CanStrafe = true;
                Falling = false;
            }*/
        }
    }

    public void Init()
    {
        CanStrafe = true;
        CanJump = true;
    }
    
    private void OnDrawGizmos()
    {
        var pos = transform.position - GroundCheckDistance;
        //Gizmos.DrawWireCube(pos, new Vector3(GroundCheckRadius, GroundCheckRadius * 2, GroundCheckRadius));
        Gizmos.DrawWireSphere(pos, GroundCheckRadius);
        //Gizmos.DrawWireSphere(pos, GroundCheckRadius);
        //Gizmos.DrawWireSphere(transform.position + GroundCheckDistance, GroundCheckRadius);
    }
}
