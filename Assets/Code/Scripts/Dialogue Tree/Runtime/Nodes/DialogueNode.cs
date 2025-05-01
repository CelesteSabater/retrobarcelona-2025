using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using retrobarcelona.Systems.AudioSystem;

namespace retrobarcelona.DialogueTree.Runtime
{
    public abstract class DialogueNode : ScriptableObject
    {
        [Header("Displayed Dialogue")]
        [TextArea] public string _text;

        [HideInInspector] public string _guid;
        [HideInInspector] public Vector2 _position;
        [HideInInspector] public Blackboard _blackboard;

        [Header("Triggered Events")]
        public UnityEvent _startDialogueEvent = new UnityEvent();
        public UnityEvent _endDialogueEvent = new UnityEvent();

        [Header("Visual Settings")]
        public string _npcTheme;

        public void SetGUID(string guid) => _guid = guid;
        public string GetGUID() => _guid;

        public void SetPosition(Vector2 position)
        {
            #if UNITY_EDITOR
            Undo.RecordObject(this, "Dialogue Tree (Set Position)");
            _position = position;
            EditorUtility.SetDirty(this);
            #endif
        }

        public Vector2 GetPosition() => _position;

        public virtual DialogueNode Clone()
        { 
            return Instantiate(this);
        }

        public virtual void StartDialogue()
        {
            AudioSystem.Instance.PlayMusic(_npcTheme);
            _startDialogueEvent?.Invoke();
        }

        public virtual void EndDialogue()
        {
            _endDialogueEvent?.Invoke();
        }

    }
}