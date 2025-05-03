using System.Collections.Generic;
using UnityEngine;

namespace retrobarcelona.DialogueTree.Runtime
{
    public abstract class EndNode : DialogueNode
    {
        protected abstract void EndAction();

        public override void EndDialogue()
        {
            base.EndDialogue();
            EndAction();
        }

        public override DialogueNode Clone()
        {
            EndNode node = Instantiate(this);
            return node;
        }
    }
}
