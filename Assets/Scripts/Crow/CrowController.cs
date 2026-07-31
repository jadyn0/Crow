using UnityEngine;
using UnityEngine.InputSystem;

namespace StatePattern
{
    public class CrowController : MonoBehaviour
    {
        [HideInInspector] public CrowFly crowFly;
        [HideInInspector] public CrowWalk crowWalk;

        private ICrowState currentState;

        [HideInInspector] public Animator animator;
        [HideInInspector] public Rigidbody2D rb;

        [HideInInspector] public InputAction moveAction;
        [HideInInspector] public InputAction jumpAction;
        [HideInInspector] public InputAction sprintAction;
        [HideInInspector] public Vector2 moveValue;
        [HideInInspector] public float jumpValue;
        [HideInInspector] public float sprintValue;

        [SerializeField] private float glideSpeed = 1f;
        [SerializeField] private float crawlFlySpeed = 10f;
        [SerializeField] private float minGlideSpeed, maxGlideSpeed;

        [SerializeField] private float glideDamp = 0.15f;
        [SerializeField] private float rotationDamp = 0.15f;


        [SerializeField] private float walkSpeed = 10f;
        [SerializeField] private float jumpForce = 10f;

        [SerializeField] private float flyingGravityScale = 1f;
        [SerializeField] private float walkingGravityScale = 5f;


        [SerializeField] private float rayCastLength;
        [SerializeField] private LayerMask groundMask;

        private void InitializeStates()
        {
            crowFly = new CrowFly(transform, rb, animator, glideSpeed, crawlFlySpeed, maxGlideSpeed, minGlideSpeed, glideDamp, rotationDamp, flyingGravityScale);
            crowWalk = new CrowWalk(transform, rb, animator, walkSpeed, jumpForce, walkingGravityScale);

            SetState(crowWalk);
        }

        public void Start()
        {
            moveAction = InputSystem.actions.FindAction("Move");
            jumpAction = InputSystem.actions.FindAction("Jump");
            sprintAction = InputSystem.actions.FindAction("Sprint");

            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();

            InitializeStates();
        }

        private void Update()
        {
            moveValue = moveAction.ReadValue<Vector2>();
            jumpValue = jumpAction.ReadValue<float>();
            sprintValue = sprintAction.ReadValue<float>();
            currentState.UpdateState();
        }

        private void FixedUpdate()
        {
            currentState.FixedUpdateState();
        }

        public void SetState(ICrowState iCrowState)
        {
            currentState = iCrowState;
            iCrowState.EnterState(this);
        }

        public void FlyStateEnter()
        {
            SetState(crowFly);
        }

        public void WalkStateEnter()
        {
            SetState(crowWalk);
        }

        public bool IsGrounded()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, rayCastLength, groundMask);

            // If it hits something...
            if (hit)
            {
                return true;
            }

            else
            {
                return false;
            }
        }
    }
}