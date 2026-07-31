using UnityEngine;

namespace StatePattern
{

    public class CrowFly : ICrowState
    {
        private CrowController crowController;

        private float dx;

        private Rigidbody2D rb;
        private Transform transform;
        private Animator animator;

        private float glideSpeed;
        private float crawlFlySpeed;
        private float minGlideSpeed, maxGlideSpeed;

        private float glideDamp;
        private float rotationDamp;

        private float gravityScale;

        public CrowFly(Transform _transform, Rigidbody2D _rb, Animator _animator, float _glideSpeed,float _crawlFlySpeed, float _maxGlideSpeed, float _minGlideSpeed, float _glideDamp, float _rotationDamp, float _gravityScale)
        {
            transform = _transform;
            rb = _rb;
            animator = _animator;

            glideSpeed = _glideSpeed;
            crawlFlySpeed = _crawlFlySpeed;
            maxGlideSpeed = _maxGlideSpeed;
            minGlideSpeed = _minGlideSpeed;

            glideDamp = _glideDamp;
            rotationDamp = _rotationDamp;
            gravityScale = _gravityScale;
        }

        public void EnterState(CrowController _crowController)
        {
            crowController = _crowController;

            animator.SetTrigger("takeOff");

            //return scale to normal
            Vector3 localScale = transform.localScale;
            if (localScale.x == -1)
            {
                transform.eulerAngles = new Vector3(0, 0, 180f);
            }
            localScale.y = localScale.x;
            localScale.x = 1f;
            transform.localScale = localScale;

            //wings gravity
            rb.gravityScale = gravityScale;

            //resets dx
            dx = maxGlideSpeed;

            
        }

        public void UpdateState()
        {
            if (crowController.IsGrounded())
            {
                crowController.WalkStateEnter();
            }
            Flip();
        }

        public void FixedUpdateState()
        {
            Turn();
            Thrust();
            GlidingMovement();
        }


        private void GlidingMovement()
        {
            float angle;
            angle = Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad) - 0.2f;
            dx = dx + glideSpeed * -angle;

            dx = Mathf.Clamp(dx, minGlideSpeed, maxGlideSpeed);

            rb.AddRelativeForceX(dx);
        }

        private void Thrust()
        {
            if (crowController.jumpValue != 0)
            {
                animator.SetBool("isHovering", false);
                animator.SetBool("isAccelerating", true);
                dx = Mathf.Lerp(dx, crawlFlySpeed, glideDamp);
            }

            else if (crowController.sprintValue != 0)
            {
                animator.SetBool("isHovering", true);
                animator.SetBool("isAccelerating", false);
                dx = Mathf.Lerp(dx, maxGlideSpeed, glideDamp);
            }

            else
            {
                
            }
        }

        private void Turn()
        {

            if (crowController.moveValue != new Vector2(0f, 0f))
            {
                Vector3 dir = new Vector3(crowController.moveValue.x, crowController.moveValue.y, transform.position.z);
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                rb.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), rotationDamp));
            }
        }

        private void Flip()
        {
            Vector3 localScale = transform.localScale;
            if (transform.eulerAngles.z % 360 > 90 && transform.eulerAngles.z % 360 < 270)
            {
                localScale.y = -1f;
            }
            else
            {
                localScale.y = 1f;
            }
            transform.localScale = localScale;
        }
    }
}