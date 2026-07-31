using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEditor.Media;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using UnityEditor.ShaderGraph.Internal;

public class CrowController2 : MonoBehaviour
{
    InputAction moveAction;
    InputAction jumpAction;

    public Vector2 moveValue;
    public float jumpValue;

    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;

    public float glideSpeed = 8f;
    public float minGlideSpeed, maxGlideSpeed;

    public float rotationDamp = 8f;

    private float dx;


    [SerializeField] private float rayCastLength;
    [SerializeField] private LayerMask groundMask;

    [SerializeField] private float speed;

    private Animator animator;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");

        animator = GetComponent<Animator>();
        animator.updateMode = AnimatorUpdateMode.Fixed;
    }

    void Update()
    {
        moveValue = moveAction.ReadValue<Vector2>();
        jumpValue = jumpAction.ReadValue<float>();

        Flip();

    }
    void FixedUpdate()
    {
        if (!IsGrounded())
        {
            //Fly();
        }
        else
        {
            Walk();
        }


    }

    private void Walk()
    {
        rb.linearVelocityX = moveValue.x * speed;

        if (moveValue.x != 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    private void Fly()
    {
        Thrust();

        Turn();

        GlidingMovement();
    }

    private void GlidingMovement()
    {
        float angle;
        angle = Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad);
        dx = dx + glideSpeed * -angle;

        dx = Mathf.Clamp(dx, minGlideSpeed,maxGlideSpeed);

        rb.AddRelativeForceX(dx);
    }


    private void Thrust()
    {
        //rb.AddRelativeForceX(thrustSpeed * jumpValue);
        if (jumpValue != 0)
        {
            dx = maxGlideSpeed;
        }
    }

    private void Turn()
    {

        if (moveValue != new Vector2(0f, 0f))
        {
            Vector3 dir = new Vector3(moveValue.x, moveValue.y, transform.position.z);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rb.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), rotationDamp));
        }
    }

    private void Flip()
    {
        if (isFacingRight && moveValue.x < 0f || !isFacingRight && moveValue.x > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;

            if (animator.GetBool("isOnGround"))
            {
                localScale.x *= -1f;
            }
            else
            {
                localScale.y *= -1f;
            }
            transform.localScale = localScale;
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, rayCastLength, groundMask);

        // If it hits something...
        if (hit)
        {
            animator.SetBool("isOnGround", true);
            return true;
        }

        else
        {
            animator.SetBool("isOnGround", false);
            return false;
        }
    }
}