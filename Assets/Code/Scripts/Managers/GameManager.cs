using UnityEngine;
using UnityEngine.UI;
using retrobarcelona.Utils.Singleton;

namespace retrobarcelona.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        private bool _inDialogue = false;
        private bool _inPause = false;
        private StateMachine.StateMachine _stateMachine;
        public Image fadeImage;

        public bool InDialogue() => _inDialogue;

        void Start()
        {
            GameEvents.current.onSetDialogue += OnSetDialogue;
            GameEvents.current.onSetPause += OnSetPause;
        }

        private void OnDestroy() {
            GameEvents.current.onSetDialogue -= OnSetDialogue;
            GameEvents.current.onSetPause -= OnSetPause;
        }

        void Update()
        {
            CheckState();
        }

        public void SetStateMachine(StateMachine.StateMachine sm) =>_stateMachine = sm;

        void CheckState()
        {
            if (_stateMachine == null)
                return;
                
            bool playing = !_inDialogue && !_inPause;
            if (playing)
            {
                _stateMachine.enabled = true;
            }
            else
            {
                _stateMachine.enabled = false;
                Rigidbody2D rb = _stateMachine.GetComponent<Rigidbody2D>();
                rb.velocity = Vector2.zero;
            }
        }

        void OnSetDialogue(bool inDialogue) => _inDialogue = inDialogue;
        void OnSetPause(bool inPause) => _inPause = inPause;
    }
}