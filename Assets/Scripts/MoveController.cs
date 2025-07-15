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
    private float currentMoveInput;
    private float currentRotationInput;
    private float lerpCoefficient = 25;
    private Vector3 previousPosition;

    public float CurrentSpeed { get; private set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
        previousPosition = rb.position; 
    }

    private void FixedUpdate()
    {
        Vector3 displacement = rb.position - previousPosition;
        CurrentSpeed = displacement.magnitude / Time.fixedDeltaTime;
        previousPosition = rb.position;

        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
        animator.SetBool(isGroundedHash, isGrounded);

        if (Mathf.Abs(currentRotationInput) > 0.1f)
        {
            transform.Rotate(0, currentRotationInput * rotationSpeed * Time.fixedDeltaTime, 0);
        }

        if (Mathf.Abs(currentMoveInput) > 0.1f)
        {
            Vector3 movement = transform.forward * currentMoveInput * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);
        }

        animator.SetFloat(moveSpeedHash, Mathf.Abs(CurrentSpeed));
    }

    public void SetMovementInput(float horizontal, float vertical)
    {
        currentRotationInput = horizontal;
        currentMoveInput = Mathf.Lerp(currentMoveInput, vertical, lerpCoefficient * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}