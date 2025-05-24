using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerConteroller : MonoBehaviour
{
    
    Rigidbody2D rb;
    Vector2 moveInput;
    public float walkSpeed = 5f;

    // pubilcfloat currentSpeed;

    public bool IsMoving { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        rb.linearVelocity = new Vector2(moveInput.x * walkSpeed, rb.linearVelocityY);

        // animator.SetFloat(AnimationString.yVelocity, rb.linearVelocityY);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        IsMoving = moveInput != Vector2.zero; // ngecheck kalo playernya gerak artinya di set ke true, kalo playernya ga gerak, di set ke false
        //setFacingDirection(moveInput);
    }
}
