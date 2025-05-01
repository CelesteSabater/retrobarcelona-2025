using System.Collections.Generic;
using UnityEngine;

namespace retrobarcelona.DialogueTree.Runtime
{
    public abstract class StartNode : DialogueNode
    {
        [HideInInspector] public List<DialogueNode> _choices = new List<DialogueNode>();
        public bool _returnOnEndBranch;
        
        public override DialogueNode Clone()
        {
            StartNode node = Instantiate(this);
            node._choices = _choices.ConvertAll(choice => choice.Clone()); 
            return node;
        }
    }
}
