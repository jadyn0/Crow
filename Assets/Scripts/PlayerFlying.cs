using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEditor.Media;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using UnityEditor.ShaderGraph.Internal;

public class PlayerFlying : MonoBehaviour
{
    InputAction moveAction;
    InputAction jumpAction;

    
    [SerializeField] private float rotationDamp = 8f;
    [SerializeField] private float jumpPower = 16f;

    private Vector2 moveValue;
    private float jumpValue;

    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    private Vector3 currentAngle;

    [SerializeField] private float glideSpeed = 8f;
    [SerializeField] private float minGlideSpeed, MaxGlideSpeed;

    [SerializeField] private float thrustSpeed;

    private bool isThrust;



    private float dx;


    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        currentAngle = transform.eulerAngles;
    }

    void Update()
    {
        moveValue = moveAction.ReadValue<Vector2>();

        jumpValue = jumpAction.ReadValue<float>();

        Flip();

    }
    void FixedUpdate()
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

        dx = Mathf.Clamp(dx, minGlideSpeed, MaxGlideSpeed);

        rb.AddRelativeForceX(dx);
    }


    private void Thrust()
    {
        rb.AddRelativeForceX(thrustSpeed * jumpValue);
    }

    private void Turn()
    {
        /*Vector3 dir = new Vector3(moveValue.x, moveValue.y, transform.position.z);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), 0.25f);*/

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
            localScale.y *= -1f;
            transform.localScale = localScale;
        }
    }
}