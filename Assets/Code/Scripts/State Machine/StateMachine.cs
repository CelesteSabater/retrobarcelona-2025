using UnityEngine;
using retrobarcelona.StateMachine.States;

namespace retrobarcelona.StateMachine
{
    public class StateMachine : MonoBehaviour
    {
        public BasicState currentState;

        private Rigidbody2D rb;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundCheckPointRadius;

        [SerializeField] private MovementData PlayerMovementData;
        private bool isFacingRight = true;

        private float LastOnGroundTime;
        private Animator animator;

        public Animator GetAnimator() => animator;
        public Vector2 GetVelocity() => rb.velocity;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponentInChildren<Animator>();

            currentState = new IdleState();
            currentState.EnterState(this);
        }

        void Update()
        {
            Timers();
            currentState.UpdateState(this);
        }

        public void SwitchState(BasicState state) {
            currentState = state;
            currentState.EnterState(this);
        }

        public static bool isGrounded(StateMachine state)
        {
            bool isGroundedVar = false;

            if (Physics2D.OverlapCircle(state.groundCheck.position, state.groundCheckPointRadius, state.groundLayer))
            {
                isGroundedVar = true;
            }

            return isGroundedVar;
        }

        private void Timers()
        {
            LastOnGroundTime -= Time.deltaTime;
            if (isGrounded(this))
                LastOnGroundTime = 0.1f;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckPointRadius);
        }

        public void Move(Vector2 direction)
        {
            if (direction.x != 0)
                CheckDirectionToFace(direction.x > 0);

            float targetSpeed = direction.x * PlayerMovementData.runMaxSpeed;
            float accelRate;

            accelRate = PlayerMovementData.runAcceleration;
            //accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? PlayerMovementData.runAcceleration : PlayerMovementData.runDecceleration;

            if(PlayerMovementData.doConserveMomentum && 
                Mathf.Abs(rb.velocity.x) > Mathf.Abs(targetSpeed) && 
                Mathf.Sign(rb.velocity.x) == Mathf.Sign(targetSpeed) && 
                Mathf.Abs(targetSpeed) > 0.01f && 
                LastOnGroundTime < 0)
            {
                accelRate = 0; 
            }

            float speedDif = targetSpeed - rb.velocity.x;
            float movement = speedDif * accelRate;

            rb.AddForce(movement * Vector2.right, ForceMode2D.Force);      
        }

        public void Jump()
        {
            if (!isGrounded(this))
                return;
            rb.AddForce(PlayerMovementData.jumpForce * Vector2.up, ForceMode2D.Impulse);
        }

        private void Turn()
        {
            Vector3 scale = transform.localScale; 
            scale.x *= -1;
            transform.localScale = scale;

            isFacingRight = !isFacingRight;
        }

        private void CheckDirectionToFace(bool isMovingRight)
        {
            if (isMovingRight != isFacingRight)
                Turn();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            currentState.OnTriggerEnter(this, other);
        }
        private void OnTriggerStay2D(Collider2D other)
        {
            currentState.OnTriggerStay(this, other);
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            currentState.OnTriggerExit(this, other);
        }

        public void GenericTriggerEnterCode(Collider2D collider)
        {

        }
    }
}