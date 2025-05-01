using UnityEngine.UIElements;

namespace retrobarcelona.DialogueTree.Editor
{
    public class DialogueInspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<DialogueInspectorView, VisualElement.UxmlTraits> { }

        UnityEditor.Editor editor;

        public DialogueInspectorView() { }

        public void UpdateSelection(DialogueNodeView nodeView) 
        {
            Clear();

            UnityEngine.Object.DestroyImmediate(editor);

            editor = UnityEditor.Editor.CreateEditor(nodeView.GetNode());
            IMGUIContainer container = new IMGUIContainer(() => { 
                if (editor.target) editor.OnInspectorGUI(); 
            });
            Add(container);
        }
        
    }
}