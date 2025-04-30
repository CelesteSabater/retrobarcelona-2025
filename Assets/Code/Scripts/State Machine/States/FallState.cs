using UnityEngine;
using retrobarcelona.StateMachine;
using retrobarcelona.Managers.ControlsManager;

namespace retrobarcelona.StateMachine.States
{
    public class FallState : BasicState
    {
        public override void EnterState(StateMachine state)
        {
            state.GetAnimator().SetBool("isWalking", false);
            //AnimationSystem.Instance.PlayAnimation("Fall",state.GetAnimator());
        }

        public override void UpdateState(StateMachine state)
        {
            if (StateMachine.isGrounded(state))
            {
                state.SwitchState(new IdleState());
            }
            state.Move(ControlsManager.Instance.GetMovementDirection());
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