using UnityEngine;

namespace StatePattern
{

    public class CrowWalk : ICrowState
    {
        private CrowController crowController;

        private Rigidbody2D rb;
        private Transform transform;
        private Animator animator;

        private float walkSpeed;
        private float jumpForce;

        private float gravityScale;

        private Vector2 posLastFrame;

        public CrowWalk(Transform _transform, Rigidbody2D _rb, Animator _animator, float _walkSpeed, float _jumpForce, float _gravityScale)
        {
            transform = _transform;
            rb = _rb;
            animator = _animator;

            walkSpeed = _walkSpeed;
            jumpForce = _jumpForce;

            gravityScale = _gravityScale;
        }

        public void EnterState(CrowController _crowController)
        {
            crowController = _crowController;

            animator.SetTrigger("hasLanded");

            //return scale to normal
            Vector3 localScale = transform.localScale;
            localScale.x = localScale.y;
            localScale.y = 1f;
            transform.localScale = localScale;

            //return rotation to normal
            transform.eulerAngles = new Vector3(0, 0, 0);

            //no wings gravity
            rb.gravityScale = gravityScale;
        }

        public void UpdateState()
        {
            //jump when space is pressed
            if (crowController.jumpAction.triggered && crowController.jumpAction.ReadValue<float>() > 0f && crowController.IsGrounded())
            {
                Jump();
            }
            
            Flip();
            
            //fly when space is pressed in the air
            if (crowController.jumpAction.triggered && crowController.jumpAction.ReadValue<float>() > 0f && !crowController.IsGrounded())
            {
                crowController.FlyStateEnter();
            }

            
        }

        public void FixedUpdateState()
        {
            rb.linearVelocityX = crowController.moveValue.x * walkSpeed;
            if (Mathf.Round(transform.position.x * 100) != Mathf.Round(posLastFrame.x * 100))
            {
                animator.SetBool("isWalking", true);
                
            }
            else
            {
                animator.SetBool("isWalking", false);
            }
            posLastFrame = transform.position;
        }

        public void Jump()
        {
            rb.AddForceY(jumpForce, ForceMode2D.Impulse);

            animator.SetTrigger("jump");
        }

        public void Flip()
        {
            Vector3 localScale = transform.localScale;
            if (crowController.moveValue.x < 0f)
            {
                localScale.x = -1f;
            }
            if (crowController.moveValue.x > 0f)
            {
                localScale.x = 1f;
            }
            transform.localScale = localScale;
        }


        public void Collision(Collision2D collision)
        {
        }
    }
}