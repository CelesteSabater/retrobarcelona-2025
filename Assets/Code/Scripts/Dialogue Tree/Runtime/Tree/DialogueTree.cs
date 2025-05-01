using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;

namespace retrobarcelona.DialogueTree.Runtime
{
    [CreateAssetMenu(menuName = "Tree/DialogueTree/DialogueTree")]
    public class DialogueTree : ScriptableObject
    {
        [SerializeField] private DialogueNode _rootNode;
        [SerializeField] private List<DialogueNode> _nodes = new List<DialogueNode>();
        public Blackboard _blackboard = new Blackboard();

        public DialogueNode GetRootNode() => _rootNode;
        public DialogueNode SetRootNode(DialogueNode node) => _rootNode = node;
        [HideInInspector] public List<DialogueNode> GetNodes() => _nodes;
        public DialogueTree _nextDialogueTree;

        public DialogueNode CreateNode(System.Type type, Vector2 position)
        {
            if (type == typeof(RootNode))
            {
                if (_rootNode != null) throw new InvalidOperationException("There already exists a Root Node.");
            }

            DialogueNode node = ScriptableObject.CreateInstance(type) as DialogueNode;

            node.name = type.Name;
            #if UNITY_EDITOR
            node.SetGUID(GUID.Generate().ToString());
            node._position = position;

            Undo.RecordObject(node, "Dialogue Tree (CreateNode)");
            _nodes.Add(node);

            if (!Application.isPlaying)
                AssetDatabase.AddObjectToAsset(node, this);
            Undo.RegisterCreatedObjectUndo(node, "Dialogue Tree (CreateNode)");
            AssetDatabase.SaveAssets();
            #endif

            return node;
        }

        public void DeleteNode(DialogueNode node)
        {
            if (node.GetType() == typeof(RootNode))
            {
                throw new InvalidOperationException("Root Node cannot be deleted.");
            }
            #if UNITY_EDITOR
            Undo.RecordObject(this, "Dialogue Tree (DeleteNode)");
            _nodes.Remove(node);

            Undo.DestroyObjectImmediate(node);
            AssetDatabase.SaveAssets();
            #endif
        }

        public void AddChild(DialogueNode parent, DialogueNode child)
        {
            #if UNITY_EDITOR
            RootNode rootNode = parent as RootNode;
            if (rootNode != null)
            {
                Undo.RecordObject(rootNode, "Dialogue Tree (AddChild)");
                rootNode._child = child;
                EditorUtility.SetDirty(rootNode);
            }

            TextNode textNode = parent as TextNode;
            if (textNode != null)
            {
                Undo.RecordObject(textNode, "Dialogue Tree (AddChild)");
                textNode._child = child;
                EditorUtility.SetDirty(textNode);
            }

            ActionNode actionNode = parent as ActionNode;
            if (actionNode != null)
            {
                Undo.RecordObject(actionNode, "Dialogue Tree (AddChild)");
                actionNode._child = child;
                EditorUtility.SetDirty(actionNode);
            }

            StartNode start = parent as StartNode;
            if (start != null)
            {
                Undo.RecordObject(start, "Dialogue Tree(AddChild)");
                start._choices.Add(child);
                EditorUtility.SetDirty(start);
            }
            #endif
        }

        public void RemoveChild(DialogueNode parent, DialogueNode child)
        {
            #if UNITY_EDITOR
            RootNode rootNode = parent as RootNode;
            if (rootNode != null)
            {
                Undo.RecordObject(rootNode, "Dialogue Tree (RemoveChild)");
                rootNode._child = null;
                EditorUtility.SetDirty(rootNode);
            }

            TextNode textNode = parent as TextNode;
            if (textNode != null)
            {
                Undo.RecordObject(textNode, "Dialogue Tree (RemoveChild)");
                textNode._child = null;
                EditorUtility.SetDirty(textNode);
            }

            ActionNode actionNode = parent as ActionNode;
            if (actionNode != null)
            {
                Undo.RecordObject(actionNode, "Dialogue Tree (RemoveChild)");
                actionNode._child = null;
                EditorUtility.SetDirty(actionNode);
            }

            StartNode start = parent as StartNode;
            if (start != null)
            {
                Undo.RecordObject(start, "Dialogue Tree(RemoveChild)");
                start._choices.Remove(child);
                EditorUtility.SetDirty(start);
            }
            #endif
        }

        public static List<DialogueNode> GetChildren(DialogueNode parent)
        {
            List<DialogueNode> result = new List<DialogueNode>();

            RootNode rootNode = parent as RootNode;
            if (rootNode != null) result.Add(rootNode._child);

            TextNode textNode = parent as TextNode;
            if (textNode != null) result.Add(textNode._child);

            ActionNode actionNode = parent as ActionNode;
            if (actionNode != null) result.Add(actionNode._child);

            StartNode start = parent as StartNode;
            if (start != null) result = start._choices;

            return result;
        }

        public void CreateRoot()
        {
            if (_rootNode == null)
            {
                RootNode root = CreateNode(typeof(RootNode), Vector2.zero) as RootNode;
                _rootNode = root;
            }
        }

        public static List<DialogueNode> GetNodes(DialogueNode node,Type type)
        {
            List<DialogueNode> nodes = new List<DialogueNode>();
            Traverse(node, (n) =>
            {
                if (n.GetType() == type)
                    nodes.Add(n);
            });
            return nodes;
        }

        private static void Traverse(DialogueNode node, Action<DialogueNode> func)
        {
            if (node)
            { 
                List<DialogueNode> children = GetChildren(node);
                children.ForEach((n) => Traverse(n, func));
                func.Invoke(node);
            }
        }

        public DialogueTree Clone() 
        {
            DialogueTree tree = Instantiate(this);
            tree._rootNode = _rootNode.Clone();
            tree._nodes = new List<DialogueNode>();
            Traverse(tree._rootNode,(n) =>
            {
                tree._nodes.Add(n);
            });
            return tree;
        }

        public void Bind()
        {
            Traverse(_rootNode, (n) =>
            { 
                n._blackboard = _blackboard;
            });
        }

        public void Bind(NPCData npcData, Animator anim)
        {
            if (npcData != null) 
                _blackboard._npcData = npcData;

            if (anim != null) 
                _blackboard._animator = anim;

            Traverse(_rootNode, (n) =>
            { 
                n._blackboard = _blackboard;
            });
        }
    }
}