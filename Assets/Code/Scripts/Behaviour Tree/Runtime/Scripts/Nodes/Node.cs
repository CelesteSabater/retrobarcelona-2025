using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Project.BehaviourTree.Runtime
{
    public abstract class Node : ScriptableObject
    {
        public enum State
        {
            Running,
            Failure,
            Success
        }

        private State _state = State.Running;
        private bool _started = false;
        [HideInInspector] public string _guid;
        [HideInInspector] public Vector2 _position;
        [HideInInspector] public Blackboard _blackboard;
        [TextArea] public string description;

        public State Update()
        { 
            if (!_started)
            { 
                OnStart();
                _started = true;
            }

            _state = OnUpdate();

            if (_state == State.Failure || _state == State.Success)
            {
                OnStop();
                _started = false;
            }

            return _state;
        }

        public void SetState(State state) => this._state = state;
        public State GetState() => _state;
        public bool GetStarted() => _started;

        public void SetGUID(string guid) => this._guid = guid;
        public string GetGUID() => _guid;

        public void SetPosition(Vector2 position)
        {
            #if UNITY_EDITOR
            Undo.RecordObject(this, "Behaviour Tree (Set Position)");
            _position = position;
            EditorUtility.SetDirty(this);
            #endif
        }
        public Vector2 GetPosition() => _position;

        protected abstract void OnStart();
        protected abstract State OnUpdate();
        protected abstract void OnStop();
        public virtual void RestartNode()
        {
            this._state = State.Running;
            this._started = false;
        }

        public virtual Node Clone()
        { 
            return Instantiate(this);
        }
    }
}