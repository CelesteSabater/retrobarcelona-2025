using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.BehaviourTree.Runtime
{
    public class Repeat : DecoratorNode
    {
        [SerializeField] private int _numberOfRepeats = 2;
        private int _repeatCount;

        protected override void OnStart() => _repeatCount = 0;

        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            if (_numberOfRepeats != 0)
            {
                if (_repeatCount < _numberOfRepeats)
                {
                    _child.Update();
                    if (_child.GetState() == State.Success) _repeatCount++;
                    if (_child.GetState() == State.Failure) SetState(State.Failure);
                }
                else SetState(State.Success);
            } else _child.Update();

            return GetState();
        }
    }
}