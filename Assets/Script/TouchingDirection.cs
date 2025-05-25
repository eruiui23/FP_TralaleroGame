using UnityEngine;

public class TouchingDirection : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    Rigidbody2D rb;
    Animator animator;
    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    CapsuleCollider2D touchingCol;
    [SerializeField]
    private bool _isGrounded ;
    public bool IsGrounded
    {
        get => _isGrounded;
        private set
        {
            _isGrounded = value;
            animator.SetBool(AnimationString.isGrounded, value);
        }
    }

    void Awake()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
    }
}
