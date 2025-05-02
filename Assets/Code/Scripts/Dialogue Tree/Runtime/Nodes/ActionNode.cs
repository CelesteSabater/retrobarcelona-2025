using System.Collections.Generic;
using UnityEngine;

namespace retrobarcelona.DialogueTree.Runtime
{
    public abstract class ActionNode : DialogueNode
    {
        [HideInInspector] public DialogueNode _child;
        protected abstract void StartAction();
        protected abstract void EndAction();
        
        public override void StartDialogue()
        {
            base.StartDialogue();
            StartAction();
        }

        public override void EndDialogue()
        {
            base.EndDialogue();
            EndAction();
        }

        public override DialogueNode Clone()
        {
            ActionNode node = Instantiate(this);
            node._child = _child.Clone();
            return node;
        }
    }
}
