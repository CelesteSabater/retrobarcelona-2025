using System;
using UnityEngine;
using UnityEngine.Events;

namespace Project.BehaviourTree.Runtime
{
    public abstract class ConditionalNode : Node
    {
        [HideInInspector] public Node _child;
        private bool condition;

        protected abstract bool Question();

        protected override void OnStart() { }

        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            condition = Question();

            if (condition)
            {
                _child.Update();
                SetState(_child.GetState());
            }
            else SetState(State.Failure);

            return GetState();
        }

        public override Node Clone()
        {
            ConditionalNode node = Instantiate(this);
            node._child = _child.Clone();
            return node;
        }
    }
}