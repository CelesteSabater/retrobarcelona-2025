using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace retrobarcelona.DialogueTree.Runtime
{
    public class DialogueEvents : MonoBehaviour
    {
        public static DialogueEvents current;

        private void Awake()
        {
            current = this;
        }

        public event Action<DialogueTree,NPCData> onSetDialogueNPC;
        public void SetDialogueNPC(DialogueTree tree, NPCData npc)
        {
            if(onSetDialogueNPC != null)
                onSetDialogueNPC(tree, npc);
        }

        public event Action onReadBook;
        public void ReadBook()
        {
            if(onReadBook != null)
                onReadBook();
        }
    }
}
