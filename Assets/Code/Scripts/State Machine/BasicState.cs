using Unity.VisualScripting;
using UnityEngine;


namespace retrobarcelona.StateMachine
{
    public abstract class BasicState
    {
        public abstract void EnterState(StateMachine state);
        public abstract void UpdateState(StateMachine state);
        public abstract void OnTriggerEnter(StateMachine state, Collider2D collider);
        public abstract void OnTriggerStay(StateMachine state, Collider2D collider);
        public abstract void OnTriggerExit(StateMachine state, Collider2D collider);
    }
}