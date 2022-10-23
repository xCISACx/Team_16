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
    [SerializeField] private Vector3 _groundCheckDistance;
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private LayerMask _groundLayerMask;
    private Coroutine _movementCooldownRoutine = null;
    public float StrafeDuration;
    [SerializeField] private float _fallMultiplier;
    [SerializeField] private GameObject _impact;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _maxJumpHeight;
    private Vector3 _velocity;
    [SerializeField] private float _airTime;
    [SerializeField] private float _maxAirTime;
    [SerializeField] private Controls _controls;
    [SerializeField] public GameObject TankWater;
    public List<GameObject> Wheels;
    public SkinnedMeshRenderer ModelMeshRenderer;

    public Animator Animator;

    public bool EnableInvincibility = false;
    private Coroutine _invincibilityCooldownRoutine;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        _controls = new Controls();
        
        _controls.Player.Move.canceled += OnMoveCanceled;
    }

    private void OnMoveCanceled(InputAction.CallbackContext obj)
    {
        Animator.SetInteger("XInput", 0);
    }

    private void OnEnable()
    {
        _controls = new Controls();
        _controls.Enable();
    }

    private void OnDisable()
    {
        GetComponent<PlayerInput>().actions = null;
        _controls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        Grounded = Physics.CheckSphere(transform.position - _groundCheckDistance, _groundCheckRadius, _groundLayerMask);
        
        if (!Grounded && _airTime >= _maxAirTime)
        {
            Debug.Log("force going down");
            
            Rb.AddForce(-Vector3.up * _fallMultiplier);
            
            Animator.SetTrigger("Fall");
        }
        
        if (!Grounded && transform.position.y >= _jumpHeight)
        {
            _airTime += Time.deltaTime;
        }
        
        Animator.SetBool("Grounded", Grounded);
    }

    private void FixedUpdate()
    {
        if (!Grounded && Rb.velocity.y <= 0) //we're falling
        {
            Rb.AddForce(-Vector3.up * _fallMultiplier);
        }

        var clampedPosY = Mathf.Clamp(transform.position.y, 1.01f, _jumpHeight);
        transform.position = new Vector3(transform.position.x, clampedPosY, transform.position.z);
    }

    public void Move(InputAction.CallbackContext context)
    {
        Animator.SetInteger("XInput", (int) context.ReadValue<Vector2>().x);
        
        if (!context.started) return;

        if (!CanStrafe) return;

        var sign = Mathf.Sign(context.ReadValue<Vector2>().x);

        GameManager.Instance.CurrentLaneIndex += (int) sign;
        
        GameManager.Instance.CurrentLaneIndex = Mathf.Clamp(GameManager.Instance.CurrentLaneIndex, 0, 2);

        if (sign != 0 && context.ReadValue<Vector2>().y == 0)
        {
            transform.DOMoveX(GameManager.Instance.LanePositions[GameManager.Instance.CurrentLaneIndex].x, StrafeDuration, false).SetEase(Ease.Linear);   
        }
        
        if (context.ReadValue<Vector2>().x > 0) transform.eulerAngles = new Vector3(0, 30, 0);
        
        else if (context.ReadValue<Vector2>().x < 0) transform.eulerAngles = new Vector3(0, -30, 0);
        
        StartCoroutine(RotateStraight());
    }

    public IEnumerator RotateStraight()
    {
        yield return new WaitForSeconds(0.2f);
        
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    public IEnumerator WaitForFewSeconds(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        
        EnableInvincibility = false;
        
        //GameManager.Instance.invincTemp.InvincibleOff();
    }
    
    public void Jump(InputAction.CallbackContext context)
    {
        //Debug.Log(context.ReadValue<float>());
        
        if (context.ReadValue<float>() == 0)
            return;
        
        if (Grounded && CanJump)
        {
            Animator.SetTrigger("Jump");
            
            //Debug.Log("Jumping");
            
            Rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            
            //Debug.Log("can't jump jumping started");
            
            Jumping = true;
        }
        else if (!Grounded && Jumping)
        {
            Animator.SetTrigger("Fall");
            
            //Debug.Log("Force falling");
            
            Rb.AddForce(-Vector3.up * JumpForce * 2, ForceMode.Impulse);
            
            Animator.ResetTrigger("Jump");
        }
    }

    IEnumerator ResumeMovementAfterSeconds(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        
        FindObjectOfType<GroundGenerator>().moving = true;
        
        //Debug.Log("resuming movement");
        
        CanStrafe = true;
        
        CanJump = true;
    }

    public void GainInvincibility()
    {
        EnableInvincibility = true;
        
        //GameManager.Instance.invincTemp.InvincibleOn();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ChaseTrigger") && !EnableInvincibility)
        {
            StopCoroutine(GameManager.Instance.Player._movementCooldownRoutine);

            GameManager.Instance.StartGameOver();
        }
        
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            OnHitObstacle(other, EnableInvincibility);
        }

        else if (other.gameObject.CompareTag("Refuel") && !EnableInvincibility)
        {
            OnEnterRefuel(Grounded);
        }
    }

    private void OnEnterRefuel(bool grounded)
    {
        if (grounded)
        {
            GameManager.Instance.GainFuel(GameManager.Instance.FuelGainAmount);

            GameManager.Instance.GroundGenerator.moving = false;

            StopCoroutine(GameManager.Instance.AddToMultiplier());

            CanJump = false;
            Debug.Log("can't jump refuel");
            CanStrafe = false;

            if (_movementCooldownRoutine != null)
            {
                StopCoroutine(_movementCooldownRoutine);
                //Debug.Log("coroutine already running, stopping and starting");
            }

            if (!GameManager.Instance.GameOver)
            {
                _movementCooldownRoutine = StartCoroutine(ResumeMovementAfterSeconds(1f));
            }
        }
        else
        {
            GameManager.Instance.GainFuel(GameManager.Instance.FuelGainAmount / 2);
        }

        if (GameManager.Instance.Fuel >= GameManager.Instance.MaxFuel)
        {
            GainInvincibility();
        }

        GameManager.Instance.musician.PlaySound(1);
    }

    private void OnHitObstacle(Collider other, bool invincible)
    {
        if (invincible)
        {
            Destroy(other.gameObject);
            
            if (_invincibilityCooldownRoutine != null)
            {
                StopCoroutine(_invincibilityCooldownRoutine);
                
                //Debug.Log("coroutine already running, stopping and starting");
            }
            
            _invincibilityCooldownRoutine = StartCoroutine(WaitForFewSeconds(5));
        }
        
        else
        {
            GameManager.Instance.LoseFuel(GameManager.Instance.FuelLossAmount);

            GameManager.Instance.LoseSpeed();

            GameManager.Instance.GroundGenerator.moving = false;

            StopCoroutine(GameManager.Instance.AddToMultiplier());

            CanJump = false;

            //Debug.Log("can't jump obstacle");

            CanStrafe = false;

            other.transform.gameObject.SetActive(false);

            if (_movementCooldownRoutine != null)
            {
                StopCoroutine(_movementCooldownRoutine);
                
                //Debug.Log("coroutine already running, stopping and starting");
            }

            if (!GameManager.Instance.GameOver)
            {
                _movementCooldownRoutine = StartCoroutine(ResumeMovementAfterSeconds(1f));
            }

            var origin = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            Instantiate(_impact, origin, Quaternion.identity);

            _impact.GetComponent<ParticleSystem>().Play();

            GameManager.Instance.musician.PlaySound(2);

            other.transform.GetComponent<ObstacleBehaviour>().roadParent.SetScoreTriggers(false);

            GameManager.Instance.ChaseManager.TimesHit++;

            StartCoroutine(GameManager.Instance.ChaseManager.GetCloser());

            GameManager.Instance.ChaseManager.NoHitTimer = 0;
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
            _jumpHeight = 0;
            _jumpHeight = transform.position.y + _maxJumpHeight;
            _airTime = 0;

            Jumping = false;
            Animator.ResetTrigger("Jump");
            Animator.ResetTrigger("Fall");
        }
    }

    public void Init()
    {
        CanStrafe = true;
        CanJump = true;
    }
    
    private void OnDrawGizmos()
    {
        var pos = transform.position - _groundCheckDistance;
        
        Gizmos.DrawWireSphere(pos, _groundCheckRadius);
    }
}
