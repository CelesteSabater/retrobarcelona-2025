using UnityEngine;

namespace Project.BehaviourTree.Runtime
{
    public class Root : Node
    {
        [SerializeField] private bool _repeater = true;
        [HideInInspector] public Node _child;

        protected override void OnStart() { }

        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            _child.Update();

            SetState(State.Running);

            State childState = _child.GetState();
            if (!_repeater)
                if (childState != State.Running) SetState(childState);

            return GetState();
        }

        public override Node Clone()
        {
            Root node = Instantiate(this);
            node._child = _child.Clone();
            return node;
        }
    }
}