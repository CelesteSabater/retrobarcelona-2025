using UnityEngine;
using retrobarcelona.StateMachine;
using retrobarcelona.Managers.ControlsManager;

namespace retrobarcelona.StateMachine.States
{
    public class IdleState : BasicState
    {
        public override void EnterState(StateMachine state)
        {
            state.GetAnimator().SetBool("isWalking", false);
            //AnimationSystem.Instance.PlayAnimation("Idle",state.GetAnimator());
        }
        public override void UpdateState(StateMachine state) 
        {

            if (ControlsManager.Instance.GetIsJumping())
            {
                state.Jump();
                return;
            }
            
            if (ControlsManager.Instance.GetMovementDirection() != Vector2.zero)
            {
                state.SwitchState(new MoveState());
            }
            else{
                state.Move(Vector2.zero);
            }

            if (!StateMachine.isGrounded(state))
            {
                state.SwitchState(new FallState());
            }
        }
        public override void OnTriggerEnter(StateMachine state, Collider2D collider)
        {
            state.GenericTriggerEnterCode(collider);
        }
        public override void OnTriggerStay(StateMachine state, Collider2D collider)
        {

        }
        public override void OnTriggerExit(StateMachine state, Collider2D collider)
        {

        }
    }
}