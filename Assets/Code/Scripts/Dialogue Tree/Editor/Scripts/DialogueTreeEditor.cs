using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;


namespace retrobarcelona.DialogueTree.Editor
{
    public class DialogueTreeEditor : EditorWindow
    {
        DialogueTreeView _treeView;
        DialogueInspectorView _inspectorView;
        IMGUIContainer _blackboardView;

        SerializedObject _treeObject;
        SerializedProperty _blackboardProperty;

        [MenuItem("Tools/Celeste/Dialogue Tree Editor")]
        public static void OpenWindow()
        {
            DialogueTreeEditor wnd = GetWindow<DialogueTreeEditor>();
            wnd.titleContent = new GUIContent("DialogueTreeEditor");
        }

        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            if (Selection.activeObject is retrobarcelona.DialogueTree.Runtime.DialogueTree)
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
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Code/Scripts/Dialogue Tree/Editor/UIBuilder/DialogueTreeEditor.uxml");
            visualTree.CloneTree(root);

            // StyleSheet
            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Code/Scripts/Dialogue Tree/Editor/UIBuilder/DialogueTreeEditor.uss");
            root.styleSheets.Add(styleSheet);

            _treeView = root.Q<DialogueTreeView>();
            _inspectorView = root.Q<DialogueInspectorView>();

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
            retrobarcelona.DialogueTree.Runtime.DialogueTree tree = Selection.activeObject as retrobarcelona.DialogueTree.Runtime.DialogueTree;

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

        private void OnNodeSelectionChanged(DialogueNodeView nodeView)
        {
            _inspectorView.UpdateSelection(nodeView);
        }
    }
    
}