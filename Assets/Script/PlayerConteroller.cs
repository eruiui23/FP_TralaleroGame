using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection))] // misal kita add script ini, nanti lgsg ngadain component lain di inspectornya
public class PlayerConteroller : MonoBehaviour
{
    
    Rigidbody2D rb;
    Animator animator;
    Vector2 moveInput;
    TouchingDirection touchingDirection;
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpImpulse = 10f;
    public float currentSpeed
    {
        get
        {
            if (IsMoving)
            {
                if (IsRunning)
                {
                    return runSpeed;
                }
                return walkSpeed;
            }
            else
            { // idle
                return 0;
            }
        }
    }
    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationString.isMoving, value);
        }
    }
    [SerializeField]
    private bool _isRunning = false;
    public bool IsRunning
    {
        get => _isRunning;
        set
        {
            _isRunning = value;
            animator.SetBool(AnimationString.isRunning, value);
        }
    }
    public bool _isFacingRight = true;
    public bool IsFacingRight
    {
        get => _isFacingRight; private set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirection = GetComponent<TouchingDirection>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * currentSpeed, rb.linearVelocityY);

        animator.SetFloat(AnimationString.yVel, rb.linearVelocityY);
    }
    private void setFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true; //face right
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false; //face left
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        IsMoving = moveInput != Vector2.zero; // ngecheck kalo playernya gerak artinya di set ke true, kalo playernya ga gerak, di set ke false
        setFacingDirection(moveInput);
    }
    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirection.IsGrounded) //&& touchingDirection.IsGrounded
        {
            animator.SetTrigger(AnimationString.jump);
            rb.linearVelocity = new Vector2(rb.linearVelocityX + 100, jumpImpulse);

        }
    }
}
