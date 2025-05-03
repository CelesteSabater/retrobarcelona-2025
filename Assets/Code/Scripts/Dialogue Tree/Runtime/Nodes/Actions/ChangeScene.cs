using retrobarcelona.DialogueTree.Runtime;
using retrobarcelona.UI;
using UnityEngine.SceneManagement;

namespace DialogueTree.Runtime
{
    public class ChangeScene : ActionNode
    {
        protected override void StartAction() { }

        protected override void EndAction() 
        { 
            if (SistemaDePuntos.Instance.GetKarma() > 0) 
                SceneManager.LoadScene("FinHonor");
            else
                SceneManager.LoadScene("FinOdio");
        }
    }
}