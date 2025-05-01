using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;
using Project.BehaviourTree.Runtime;

namespace Project.BehaviourTree.Editor
{
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        public Action<NodeView> OnNodeSelected;
        public Project.BehaviourTree.Runtime.Node _node;
        [SerializeField] private Port _input, _output;

        public NodeView(Project.BehaviourTree.Runtime.Node node) : base("Assets/Code/Scripts/Behaviour Tree/Editor/UIBuilder/NodeView.uxml")
        {
            _node = node;
            title = node.name;
            viewDataKey = node.GetGUID();

            Vector2 v = node.GetPosition();
            style.left = v.x;
            style.top = v.y;

            CreateInputPorts();
            CreateOutputPorts();
            SetupClasses();

            Label descriptionLabel = this.Q<Label>("description");
            descriptionLabel.bindingPath = "description";
            descriptionLabel.Bind(new SerializedObject(node));
        }

        private void SetupClasses()
        {
            switch (_node)
            {
                case Root _node:
                    AddToClassList("root");
                    break;
                case ActionNode _node:
                    AddToClassList("action");
                    break;
                case CompositeNode _node:
                    AddToClassList("composite");
                    break;
                case DecoratorNode _node:
                    AddToClassList("decorator");
                    break;
                case ConditionalNode _node:
                    AddToClassList("conditional");
                    break;
            }
        }

        private void CreateInputPorts()
        {
            switch (_node)
            {
                case Root:
                    break;
                case ActionNode:
                case DecoratorNode:
                case ConditionalNode:
                case CompositeNode:
                    _input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
                    break;
            }

            if (_input != null)
            {
                _input.portName = "";
                _input.style.flexDirection = FlexDirection.Column;
                inputContainer.Add(_input);
            }
        }

        private void CreateOutputPorts()
        {
            switch (_node)
            {
                case ActionNode:
                    break;
                case Root:
                case DecoratorNode: 
                case ConditionalNode:
                    _output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
                    break;
                case CompositeNode:
                    _output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
                    break;
            }

            if (_output != null)
            {
                _output.portName = "";
                _output.style.flexDirection = FlexDirection.ColumnReverse;
                outputContainer.Add(_output);
            }
        }

        public Project.BehaviourTree.Runtime.Node GetNode() => _node;

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            _node.SetPosition(new Vector2(newPos.xMin, newPos.yMin));
        }

        internal Port GetOutput() => _output;
        internal Port GetInput() => _input;

        public override void OnSelected()
        {
            base.OnSelected();
            if (OnNodeSelected != null) OnNodeSelected.Invoke(this);
        }

        public void SortChildren()
        {
            CompositeNode node = _node as CompositeNode;
            if (node)
            {
                node._children.RemoveAll(item => item == null);
                node._children.Sort((left, right) => left._position.x < right._position.x ? -1 : 1);
            }
        }

        public void UpdateState()
        {
            if (!Application.isPlaying)
                return;

            switch (_node.GetState())
            {
                case Project.BehaviourTree.Runtime.Node.State.Running:
                    if (_node.GetStarted())
                        AddToClassList("running");
                    break;
                case Project.BehaviourTree.Runtime.Node.State.Failure:
                    AddToClassList("failure");
                    break;
                case Project.BehaviourTree.Runtime.Node.State.Success:
                    AddToClassList("success");
                    break;
            }
        }

        public void RestartState()
        {
            RemoveFromClassList("running");
            RemoveFromClassList("failure");
            RemoveFromClassList("success");
        }
    }
}
#endif