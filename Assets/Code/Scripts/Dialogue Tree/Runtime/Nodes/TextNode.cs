using System.Collections.Generic;
using UnityEngine;

namespace retrobarcelona.DialogueTree.Runtime
{
    public abstract class TextNode : DialogueNode
    {
        [HideInInspector] public DialogueNode _child;
        
        public override DialogueNode Clone()
        {
            TextNode node = Instantiate(this);
            node._child = _child.Clone();
            return node;
        }
    }
}
