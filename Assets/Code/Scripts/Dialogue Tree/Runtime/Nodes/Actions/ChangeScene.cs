using retrobarcelona.DialogueTree.Runtime;
using retrobarcelona.UI;
using UnityEngine.SceneManagement;

namespace DialogueTree.Runtime
{
    public class ChangeScene : ActionNode
    {
        protected override void StartAction() 
        { 
            if (SistemaDePuntos.Instance.GetKarma() < 0) 
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);

        }

        protected override void EndAction() { }
    }
}