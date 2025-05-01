using System.Collections.Generic;
using UnityEngine;

namespace retrobarcelona.DialogueTree.Runtime
{
    public class RootNode : DialogueNode
    {
        [HideInInspector] public DialogueNode _child;
        
        public override DialogueNode Clone()
        {
            RootNode node = Instantiate(this);
            node._child = _child.Clone();
            return node;
        }
    }
}
