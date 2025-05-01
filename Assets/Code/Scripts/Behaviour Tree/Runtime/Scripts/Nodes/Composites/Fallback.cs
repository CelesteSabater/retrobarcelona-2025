using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.BehaviourTree.Runtime
{
    public class Fallback : CompositeNode
    {
        private int _current;

        protected override void OnStart()
        {
            _children.RemoveAll(item => item == null);
            _current = 0;
        } 

        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            Node child = _children[_current];
            switch (child.Update())
            {
                case State.Running:
                    return State.Running;
                case State.Failure:
                    _current++;
                    break;
                case State.Success:
                    return State.Success;
            }

            return _current == _children.Count ? State.Failure : State.Running;
        }
    }
}