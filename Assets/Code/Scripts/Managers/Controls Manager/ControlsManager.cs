using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using retrobarcelona.Utils.Singleton;

namespace retrobarcelona.Managers.ControlsManager
{
    public class ControlsManager : Singleton<ControlsManager>
    {
        [Header("Camera Actions")]
        public PlayerInputActions _playerControls;
        private InputAction _move;
        private InputAction _accelerate;
        private InputAction _zoom;
        private InputAction _jump;
        private bool _gameControlsControlsAreActive = false;

        public void ActivateCameraControls() => _gameControlsControlsAreActive = true;
        public void DisableCameraControls() => _gameControlsControlsAreActive = false;

        protected override void Awake()
        {
            base.Awake(); 
            _playerControls = new PlayerInputActions();
        }

        private void OnEnable()
        {
            _move = _playerControls.GameControls.Move;
            _accelerate = _playerControls.GameControls.Accelerate;
            _zoom = _playerControls.GameControls.Zoom;

            _move.Enable();
            _accelerate.Enable();
            _zoom.Enable();
        }

        private void OnDisable()
        {
            _move.Disable();
            _accelerate.Disable();
            _zoom.Disable();
        }

        public Vector2 GetMovementDirection()
        {
            Vector2 moveDirection = Vector2.zero;

            if (!_gameControlsControlsAreActive)
                return moveDirection;
            
            moveDirection = _move.ReadValue<Vector2>();

            return moveDirection;
        }

        public bool GetIsAccelerating()
        {
            if (!_gameControlsControlsAreActive)
                return false;
            
            return _accelerate.ReadValue<float>() > 0.1f ;
        }

        public float GetZoomDirection()
        {
            float zoomDirection = 0;

            if (!_gameControlsControlsAreActive)
                return zoomDirection;
            
            zoomDirection = _zoom.ReadValue<float>();

            return zoomDirection;
        }

        public bool GetIsJumping()
        {
            if (!_gameControlsControlsAreActive)
                return false;
            
            return _jump.ReadValue<float>() > 0.1f ;
        }
    }
}