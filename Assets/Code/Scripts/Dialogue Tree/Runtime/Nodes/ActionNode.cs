using System.Collections.Generic;
using UnityEngine;

namespace retrobarcelona.DialogueTree.Runtime
{
    public abstract class ActionNode : DialogueNode
    {
        [HideInInspector] public DialogueNode _child;
        protected abstract void Action();
        
        public override void EndDialogue()
        {
            base.EndDialogue();
            Action();
        }

        public override DialogueNode Clone()
        {
            ActionNode node = Instantiate(this);
            node._child = _child.Clone();
            return node;
        }
    }
}
