using System;
using UnityEngine;

using UnityEditor.Experimental.GraphView;

using UnityEngine.UIElements;
using retrobarcelona.DialogueTree.Runtime;

using UnityEditor.UIElements;

using UnityEditor;

namespace retrobarcelona.DialogueTree.Editor
{
    public class DialogueNodeView : UnityEditor.Experimental.GraphView.Node
    {
        public Action<DialogueNodeView> OnNodeSelected;
        public DialogueNode _node;
        [SerializeField] private Port _input, _output;

        public DialogueNodeView(DialogueNode node) : base("Assets/Code/Scripts/Dialogue Tree/Editor/UIBuilder/DialogueNodeView.uxml")
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
            descriptionLabel.bindingPath = "_text";
            descriptionLabel.Bind(new SerializedObject(node));
        }

        private void SetupClasses()
        {
            switch (_node)
            {
                case RootNode _node:
                    AddToClassList("root");
                    break;
                case EndNode _node:
                    AddToClassList("end");
                    break;
                case StartNode _node:
                    AddToClassList("start");
                    break;
                case TextNode _node:
                    AddToClassList("text");
                    break;
                case ActionNode _node:
                    AddToClassList("action");
                    break;
            }
        }

        private void CreateInputPorts()
        {
            switch (_node)
            {
                case RootNode:
                    break;
                case EndNode:
                case StartNode:
                case TextNode:
                case ActionNode:
                    _input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(bool));
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
                case EndNode:
                    break;
                case RootNode:
                case TextNode:
                case ActionNode:
                    _output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
                    break;
                case StartNode:
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

        public DialogueNode GetNode() => _node;

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
    }
    
}