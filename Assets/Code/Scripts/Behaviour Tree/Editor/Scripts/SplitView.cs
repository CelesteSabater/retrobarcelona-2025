using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine.UIElements;

namespace Project.BehaviourTree.Editor
{
    public class SplitView : TwoPaneSplitView
    {
        public new class UxmlFactory : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits> { }
    }
}
#endif