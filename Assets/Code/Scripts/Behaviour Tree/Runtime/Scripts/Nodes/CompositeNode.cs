using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Experimental.GraphView;
#endif

namespace Project.BehaviourTree.Runtime
{
    public abstract class CompositeNode : Node
    {
        public List<Node> _children = new List<Node>();

        public override Node Clone()
        {
            _children.RemoveAll(item => item == null);
            CompositeNode node = Instantiate(this);
            node._children = _children.ConvertAll(child => child.Clone());  
            return node;
        }
    }
}
