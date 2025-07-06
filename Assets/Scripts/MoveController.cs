using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class MoveController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    private readonly int moveSpeedHash = Animator.StringToHash("MoveSpeed");
    private readonly int isGroundedHash = Animator.StringToHash("IsGrounded");

    private Rigidbody rb;
    private Animator animator;
    private bool isGrounded;
    private float currentMoveSpeed;
    private float lerpKoeficient = 15;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
        animator.SetBool(isGroundedHash, isGrounded);

        float rotationInput = Input.GetAxis("Horizontal");
        if (Mathf.Abs(rotationInput) > 0.1f)
        {
            transform.Rotate(0, rotationInput * rotationSpeed * Time.fixedDeltaTime, 0);
        }

        float moveInput = Input.GetAxis("Vertical");
        currentMoveSpeed = Mathf.Lerp(currentMoveSpeed, moveInput, lerpKoeficient * Time.fixedDeltaTime);

        if (Mathf.Abs(moveInput) > 0.1f)
        {
            Vector3 movement = transform.forward * currentMoveSpeed * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);
        }

        animator.SetFloat(moveSpeedHash, Mathf.Abs(currentMoveSpeed));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}