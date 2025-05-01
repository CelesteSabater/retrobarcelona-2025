using UnityEngine;

namespace retrobarcelona.Systems.Talkable
{
    public interface IInteractable
    {
        public GameObject PromptPrefab { get;}
        public Transform PromptLocation { get;}
        public bool IsActive();
        public bool SetActive(bool b);

        public bool Interact(Interactor interactor);
        public void SetupPrompt(bool show);
    }
}