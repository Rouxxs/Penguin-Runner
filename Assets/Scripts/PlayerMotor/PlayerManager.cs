using System;
using Audio;
using GameFlow;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector] public Vector3 moveVector;
    [HideInInspector] public float verticalVelocity;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public int currentLane;
    
    public float distanceBetweenLanes = 3.0f;
    public float baseRunSpeed = 5.0f;
    public float baseSidewaySpeed = 10.0f;
    public float gravity = 14.0f;
    public float terminalVelocity = 20.0f;
    
    public CharacterController controller;
    public Animator animator;
    private BaseState _state;
    private bool _isPaused;
    public AudioClip deathSfx;

    #region States
    public RunningState RunningState { get; private set; }
    public DeathState DeathState { get; private set; }
    public FallingState FallingState { get; private set; }
    public RespawnState RespawnState { get; private set; }
    public SlidingState SlidingState { get; private set; }
    public JumpingState JumpingState { get; private set; }
    #endregion

    private void Awake()
    {
        RunningState = GetComponent<RunningState>();
        DeathState = GetComponent<DeathState>();
        FallingState = GetComponent<FallingState>();
        RespawnState = GetComponent<RespawnState>();
        SlidingState = GetComponent<SlidingState>();
        JumpingState = GetComponent<JumpingState>();
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        _state = GetComponent<RunningState>();
        animator = GetComponent<Animator>();
        _isPaused = true;
    }

    private void Update()
    {
        if (!_isPaused)
        {
            UpdateMotor();
        }
    }

    private void UpdateMotor()
    {
        isGrounded = controller.isGrounded;
        
        // Check state
        moveVector = _state.ProcessMotion();
        
        // Update animator values
        animator?.SetFloat("Speed", Mathf.Abs(moveVector.z));
        animator?.SetBool("IsGrounded", isGrounded);
        
        // Change state?
        _state.Transition();
        
        // Move player
        controller.Move(moveVector * Time.deltaTime);
    }

    public float SnapToLane()
    {
        float result = 0.0f;

        if (transform.position.x != currentLane * distanceBetweenLanes)
        {
            float delta = (currentLane * distanceBetweenLanes) - transform.position.x;
            result = (delta > 0) ? 1 : -1;
            result *= baseSidewaySpeed;
            
            float actualDistance = result * Time.deltaTime;
            if (Mathf.Abs(actualDistance) > Mathf.Abs(delta))
            {
                result = delta / Time.deltaTime;
            }
        }
        else
        {
            result = 0.0f;
        }

        return result;
    }

    public void ApplyPhysics()
    {
        verticalVelocity -= gravity * Time.deltaTime;
        if (verticalVelocity < -terminalVelocity)
        {
            verticalVelocity = -terminalVelocity;
        }
    }
    public void ChangeLane(int direction)
    {
        currentLane = Mathf.Clamp(currentLane + direction, -1, 1);
    }
    public void ChangeState(BaseState newState)
    {
        _state.Destruct();
        _state = newState;
        _state.Construct();
    }

    public void PausePlayer()
    {
        _isPaused = true;
    }
    
    public void ResumePlayer()
    {
        _isPaused = false;
    }

    
    public void RespawnPlayer()
    { 
        ChangeState(RespawnState);
        GameManager.Instance.ChangeCamera(GameCamera.Respawn);
    }

    public void ResetPlayer()
    {
        currentLane = 0;
        transform.position = Vector3.zero;
        animator?.SetTrigger("Idle");
        PausePlayer();
        ChangeState(RunningState);

    }

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        string hitLayerName = LayerMask.LayerToName(hit.gameObject.layer);

        if (hitLayerName == "Death")
        {
            ChangeState(DeathState);
            AudioManager.Instance.PlaySfx(deathSfx);
        }
    }
}
