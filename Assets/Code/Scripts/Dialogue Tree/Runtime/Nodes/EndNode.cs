using System.Collections.Generic;
using UnityEngine;

namespace retrobarcelona.DialogueTree.Runtime
{
    public abstract class EndNode : DialogueNode
    {
        public override DialogueNode Clone()
        {
            EndNode node = Instantiate(this);
            return node;
        }
    }
}
