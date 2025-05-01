using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.BehaviourTree.Runtime
{
    [System.Serializable]
    public class Blackboard 
    {
        public bool _startedTimer = false, _lowTime = false;
        public float _timer1, _timer2, _timerDuration1, _timerDuration2;
        public GameObject _gameObject;
    }
}
