using UnityEngine;

public class CrowFly : StateMachineBehaviour
{
    private CrowController crowController;
    private float rotationDamp;
    private float minGlideSpeed, MaxGlideSpeed;
    private float dx;

    private Vector2 moveValue;
    private float jumpValue;
    private float glideSpeed;
    private Rigidbody2D rb;
    private Transform transform;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        crowController = animator.GetComponent<CrowController>();
        transform = animator.GetComponent<Transform>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Turn();
        Thrust();
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
        if (jumpValue != 0)
        {
            dx = MaxGlideSpeed;
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

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
