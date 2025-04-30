using System;
using UnityEngine;

namespace retrobarcelona.Managers
{
    public class GameEvents : MonoBehaviour
    {
        public static GameEvents current;

        private void Awake() => current = this;

        #region DIALOGUE
        public event Action<bool> onSetDialogue;
        public void SetDialogue(bool value) => onSetDialogue?.Invoke(value);
        #endregion

        #region GENERAL
        public event Action<bool> onSetPause;
        public void SetPause(bool value) => onSetPause?.Invoke(value);
        #endregion
    }
}