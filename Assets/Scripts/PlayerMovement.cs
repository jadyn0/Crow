using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    InputAction moveAction;
    InputAction jumpAction;

    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpPower = 16f;
    private Vector2 moveValue;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    void Update()
    {
        moveValue = moveAction.ReadValue<Vector2>();
        //Debug.Log(moveValue);

        if (jumpAction.triggered && jumpAction.ReadValue<float>() > 0f && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
        }
        if (jumpAction.triggered && jumpAction.ReadValue<float>() == 0f && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

    }
    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveValue.x * speed, rb.linearVelocity.y);
        //Debug.Log(moveValue.x);
    }
    
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && moveValue.x < 0f || !isFacingRight && moveValue.x > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}