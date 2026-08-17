using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 7f;

    [SerializeField]
    private float _jumpForce = 10f;

    [Header("GroundCheck")]
    [SerializeField]
    private Transform _groundCheck;

    [SerializeField]
    private float _groundCheckRadius = 0.2f;

    [SerializeField]
    private LayerMask _groundLayer;

    private Rigidbody2D _rb;

    private Vector2 _moveInput;

    private bool _jumpRequested;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed) {
            _jumpRequested = true;
                }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer) != null;
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = new Vector2(
            _moveInput.x * _moveSpeed,
            _rb.linearVelocity.y
        );

        if (_jumpRequested && IsGrounded())
        {
            _rb.linearVelocity = new Vector2(
                _rb.linearVelocity.x,
                _jumpForce
            );
        }

        _jumpRequested = false;
    }
}
