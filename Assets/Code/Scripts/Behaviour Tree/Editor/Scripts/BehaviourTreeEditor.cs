using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor.Callbacks;
using UnityEditor;
#endif
using Project.BehaviourTree.Runtime;
using System;
#if UNITY_EDITOR
namespace Project.BehaviourTree.Editor
{
    public class BehaviourTreeEditor : EditorWindow
    {
        BehaviourTreeView _treeView;
        InspectorView _inspectorView;
        IMGUIContainer _blackboardView;

        SerializedObject _treeObject;
        SerializedProperty _blackboardProperty;

        [MenuItem("Tools/Celeste/Behaviour Tree Editor")]
        public static void OpenWindow()
        {
            BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
            wnd.titleContent = new GUIContent("BehaviourTreeEditor");
        }

        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            if (Selection.activeObject is Project.BehaviourTree.Runtime.BehaviourTree)
            {
                OpenWindow();
                return true;
            }
            return false;
        }

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private void OnDisable() 
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void OnInspectorUpdate()
        {
            if (_treeView != null)
                _treeView?.UpdateNodeStates();
        }

        private void OnPlayModeStateChanged(PlayModeStateChange obj)
        {
            switch (obj)
            {
                case PlayModeStateChange.EnteredEditMode:
                    OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    break;
            }
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Code/Scripts/Behaviour Tree/Editor/UIBuilder/BehaviourTreeEditor.uxml");
            visualTree.CloneTree(root);

            // StyleSheet
            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Code/Scripts/Behaviour Tree/Editor/UIBuilder/BehaviourTreeEditor.uss");
            root.styleSheets.Add(styleSheet);

            _treeView = root.Q<BehaviourTreeView>();
            _inspectorView = root.Q<InspectorView>();

            _treeView.OnNodeSelected = OnNodeSelectionChanged;
            OnSelectionChange();

            _blackboardView = root.Q<IMGUIContainer>();
            _blackboardView.onGUIHandler = () =>
            {
                if (_treeObject == null)
                    return;

                _treeObject.Update();
                EditorGUILayout.PropertyField(_blackboardProperty);
                _treeObject.ApplyModifiedProperties();
            };
        }

        private void OnSelectionChange()
        {
            Project.BehaviourTree.Runtime.BehaviourTree tree = Selection.activeObject as Project.BehaviourTree.Runtime.BehaviourTree;

            if (!tree)
            {
                if (Selection.activeGameObject)
                {
                    BehaviourTreeRunner runner = Selection.activeGameObject.GetComponent<BehaviourTreeRunner>();
                    if (runner) tree = runner.GetTree();
                }
            }

            if (tree && _treeView != null)
            {
                if (Application.isPlaying)
                {
                    if (_treeView != null && tree)
                        _treeView.PopulateView(tree);
                }
                else if (AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
                    _treeView.PopulateView(tree);
            }

            if (tree)
            { 
                _treeObject = new SerializedObject(tree);
                _blackboardProperty = _treeObject.FindProperty("_blackboard");
            }
        }

        private void OnNodeSelectionChanged(NodeView nodeView)
        {
            _inspectorView.UpdateSelection(nodeView);
        }
    }
}
#endif