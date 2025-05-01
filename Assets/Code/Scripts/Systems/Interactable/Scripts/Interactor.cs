using retrobarcelona.Managers.ControlsManager;
using UnityEngine;

namespace retrobarcelona.Systems.Talkable
{
    public class Interactor : MonoBehaviour
    {
        [SerializeField] private Transform _interactionPoint;
        [SerializeField] private float _interactionPointRadius = 0.5f;
        [SerializeField] private LayerMask _interactableMask;

        private Collider2D _collider;
        private IInteractable _interactable;

        private void Update()
        {
            CheckInteractable();
            Interact();
        }

        private void CheckInteractable()
        {   
            _collider = Physics2D.OverlapCircle(_interactionPoint.position, _interactionPointRadius, _interactableMask);
            if (_collider != null)
            {
                if (_interactable != null && _interactable != _collider.GetComponent<IInteractable>())
                    _interactable.SetupPrompt(false);

                if (_collider.GetComponent<IInteractable>().IsActive())
                {
                    _interactable = _collider.GetComponent<IInteractable>();
                    _interactable.SetupPrompt(true);
                }
            } else
            {
                if (_interactable != null)
                {
                    _interactable.SetupPrompt(false);
                    _interactable = null;
                }
            }
        }

        private void Interact()
        {
            if (!ControlsManager.Instance.GetIsInteracting())
                return;

            if (_interactable != null)
                _interactable.Interact(this);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
        }
    }
}
