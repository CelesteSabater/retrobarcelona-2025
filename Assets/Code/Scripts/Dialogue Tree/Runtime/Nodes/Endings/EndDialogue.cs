using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace retrobarcelona.DialogueTree.Runtime
{
    public class EndDialogue : EndNode
    {
        protected override void EndAction() 
        { 
            SceneManager.LoadScene("Menu");
        }
    }
}