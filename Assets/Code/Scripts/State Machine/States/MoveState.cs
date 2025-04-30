using UnityEngine;
using retrobarcelona.StateMachine;
using retrobarcelona.Managers.ControlsManager;

namespace retrobarcelona.StateMachine.States
{
    public class MoveState : BasicState
    {

        public override void EnterState(StateMachine state)
        {
            state.GetAnimator().SetBool("isWalking", true);
            //AnimationSystem.Instance.PlayAnimation("Movement",state.GetAnimator());
        }
        public override void UpdateState(StateMachine state)
        {
            if (state.GetVelocity() == Vector2.zero) state.SwitchState(new IdleState());

            if (ControlsManager.Instance.GetIsJumping())
            {
                state.Jump();
                return;
            }

            state.Move(ControlsManager.Instance.GetMovementDirection());

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