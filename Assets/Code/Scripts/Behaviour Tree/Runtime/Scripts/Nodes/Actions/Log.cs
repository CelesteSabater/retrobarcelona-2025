using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.BehaviourTree.Runtime
{
    public class Log : ActionNode
    {
        [SerializeField] private string message;
        protected override void OnStart() { }

        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            Debug.Log($"Debug: {message}");
            return State.Success;
        }
    }
}
