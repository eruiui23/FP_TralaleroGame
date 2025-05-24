using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerConteroller : MonoBehaviour
{
    
    Rigidbody2D rb;
    Animator animator;
    Vector2 moveInput;
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
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
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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

        // animator.SetFloat(AnimationString.yVelocity, rb.linearVelocityY);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        IsMoving = moveInput != Vector2.zero; // ngecheck kalo playernya gerak artinya di set ke true, kalo playernya ga gerak, di set ke false
        //setFacingDirection(moveInput);
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
}
