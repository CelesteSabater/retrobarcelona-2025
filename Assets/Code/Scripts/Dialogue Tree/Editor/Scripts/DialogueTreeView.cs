using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using System;
using System.Linq;
using retrobarcelona.DialogueTree.Runtime;

namespace retrobarcelona.DialogueTree.Editor
{
    public class DialogueTreeView : GraphView
    {
        public Action<DialogueNodeView> OnNodeSelected;
        public new class UxmlFactory : UxmlFactory<DialogueTreeView, GraphView.UxmlTraits> { }

        private UnityEngine.Vector2 worldMousePosition = UnityEngine.Vector2.zero;

        retrobarcelona.DialogueTree.Runtime.DialogueTree _tree;
        public DialogueTreeView() 
        {
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Code/Scripts/Dialogue Tree/Editor/UIBuilder/DialogueTreeEditor.uss");
            styleSheets.Add(styleSheet);

            focusable = true;
            Undo.undoRedoPerformed += OnUndoRedo;
        }

        public void UpdateNodeStates()
        {
            if (_tree != null) 
                _tree.GetNodes().ForEach(n => 
                {
                    DialogueNodeView node = FindNodeView(n);
                });
        }

        private void OnUndoRedo()
        {
            PopulateView(_tree);
            AssetDatabase.SaveAssets();
        }

        private DialogueNodeView FindNodeView(DialogueNode node)
        {
            return GetNodeByGuid(node.GetGUID()) as DialogueNodeView;
        }

        internal void PopulateView(retrobarcelona.DialogueTree.Runtime.DialogueTree tree)
        {
            _tree = tree;

            if (_tree != null)
            {
                graphViewChanged -= OnGraphViewChanged;
                DeleteElements(graphElements);
                graphViewChanged += OnGraphViewChanged;

                if (_tree.GetRootNode() == null)
                {
                    _tree.CreateRoot();
                    EditorUtility.SetDirty(_tree);
                    AssetDatabase.SaveAssets();
                }

                foreach (DialogueNode node in _tree.GetNodes()) 
                    CreateNodeView(node);

                foreach (DialogueNode parent in _tree.GetNodes())
                {
                    List<DialogueNode> children = retrobarcelona.DialogueTree.Runtime.DialogueTree.GetChildren(parent);
                    foreach (DialogueNode child in children)
                    {
                        if (child != null)
                        {
                            DialogueNodeView parentView = FindNodeView(parent);
                            DialogueNodeView childView = FindNodeView(child);

                            Edge edge = parentView.GetOutput().ConnectTo(childView.GetInput());
                            AddElement(edge);
                        }
                    }
                }
            }
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                graphViewChange.elementsToRemove.ForEach(elem =>
                {
                    DialogueNodeView nodeView = elem as DialogueNodeView;
                    if (nodeView != null) _tree.DeleteNode(nodeView.GetNode());

                    Edge edge = elem as Edge;
                    if (edge != null)
                    {
                        DialogueNodeView parentView = edge.output.node as DialogueNodeView;
                        DialogueNodeView childrenView = edge.input.node as DialogueNodeView;
                        _tree.RemoveChild(parentView.GetNode(), childrenView.GetNode());
                    }
                });
            }

            if (graphViewChange.edgesToCreate != null)
            {
                graphViewChange.edgesToCreate.ForEach(edge =>
                {
                    DialogueNodeView parentView = edge.output.node as DialogueNodeView;
                    DialogueNodeView childrenView = edge.input.node as DialogueNodeView;
                    _tree.AddChild(parentView.GetNode(), childrenView.GetNode());
                });
            }
            
            return graphViewChange;
        }

        private void CreateNodeView(DialogueNode node)
        {
            DialogueNodeView nodeView = new DialogueNodeView(node);
            nodeView.OnNodeSelected = OnNodeSelected;
            AddElement(nodeView);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort => 
                endPort.direction != startPort.direction && 
                endPort.node != startPort.node
            ).ToList();
        }

        private void CreateNode(Type type)
        {
            DialogueNode node = _tree.CreateNode(type, worldMousePosition);
            CreateNodeView(node);
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            //base.BuildContextualMenu(evt);

            VisualElement contentViewContainer = ElementAt(1);
            UnityEngine.Vector3 screenMousePosition = evt.localMousePosition;
            worldMousePosition = screenMousePosition - contentViewContainer.transform.position;
            worldMousePosition *= 1 / contentViewContainer.transform.scale.x;
            
            var types = TypeCache.GetTypesDerivedFrom<TextNode>();
            foreach (var type in types) evt.menu.AppendAction($"[Text]/{type.Name}", (a) => CreateNode(type));

            types = TypeCache.GetTypesDerivedFrom<ActionNode>();
            foreach (var type in types) evt.menu.AppendAction($"[Action]/{type.Name}", (a) => CreateNode(type));

            types = TypeCache.GetTypesDerivedFrom<StartNode>();
            foreach (var type in types) evt.menu.AppendAction($"[Start]/{type.Name}", (a) => CreateNode(type));

            types = TypeCache.GetTypesDerivedFrom<EndNode>();
            foreach (var type in types) evt.menu.AppendAction($"[End]/{type.Name}", (a) => CreateNode(type));
        }
    }
}