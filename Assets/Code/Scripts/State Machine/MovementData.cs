using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace retrobarcelona.StateMachine
{
    [CreateAssetMenu(menuName = "Data/Movement Data")] 
    public class MovementData : ScriptableObject
    {
        [Header("Run")]
        public float runMaxSpeed;
        public float runAcceleration; 
        public float runDecceleration; 
        public bool doConserveMomentum;

        [Header("Jump")]
        public float jumpForce; 

        private void OnValidate()
        {
            runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
            runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);
        }
    }
}
