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
        private InputAction _interact;
        private InputAction _lane1;
        private InputAction _lane2;
        private InputAction _lane3;
        private InputAction _lane4;
        private bool _gameControlsControlsAreActive = false;

        public void ActivateGameControls() => _gameControlsControlsAreActive = true;
        public void DisableGameControls() => _gameControlsControlsAreActive = false;

        protected override void Awake()
        {
            base.Awake(); 
            _playerControls = new PlayerInputActions();
        }

        private void OnEnable()
        {
            _move = _playerControls.GameControls.Move;
            _accelerate = _playerControls.GameControls.Accelerate;
            _jump = _playerControls.GameControls.Jump;
            _interact = _playerControls.GameControls.Interact;
            _lane1 = _playerControls.GameControls.Lane1;
            _lane2 = _playerControls.GameControls.Lane2;
            _lane3 = _playerControls.GameControls.Lane3;
            _lane4 = _playerControls.GameControls.Lane4;

            _move.Enable();
            _accelerate.Enable();
            _jump.Enable();
            _interact.Enable();
            _lane1.Enable();
            _lane2.Enable();
            _lane3.Enable();
            _lane4.Enable();
        }

        private void OnDisable()
        {
            _move.Disable();
            _accelerate.Disable();
            _jump.Disable();
            _interact.Disable();
            _lane1.Disable();
            _lane2.Disable();
            _lane3.Disable();
            _lane4.Disable();
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

       /* public float GetZoomDirection()
        {
            float zoomDirection = 0;

            if (!_gameControlsControlsAreActive)
                return zoomDirection;
            
            zoomDirection = _zoom.ReadValue<float>();

            return zoomDirection;
        }*/

        public bool GetIsJumping()
        {
            if (!_gameControlsControlsAreActive)
                return false;
            
            return _jump.ReadValue<float>() > 0.1f ;
        }

        public bool GetIsInteracting()
        {
            if (!_gameControlsControlsAreActive)
                return false;
            
            return _interact.ReadValue<float>() > 0.1f ;
        }

        public bool GetIsLane1()
        {
            if (!_gameControlsControlsAreActive)
                return false;
            return Input.GetKeyDown(KeyCode.Alpha1);
            //return _lane1.ReadValue<float>() > 0.1f ;
        }

        public bool GetIsLane2()
        {
            if (!_gameControlsControlsAreActive)
                return false;
            return Input.GetKeyDown(KeyCode.Alpha2);
            //return _lane2.ReadValue<float>() > 0.1f ;
        }

        public bool GetIsLane3()
        {
            if (!_gameControlsControlsAreActive)
                return false;
            return Input.GetKeyDown(KeyCode.Alpha3);
            //return _lane3.ReadValue<float>() > 0.1f ;
        }

        public bool GetIsLane4()
        {
            if (!_gameControlsControlsAreActive)
                return false;
            return Input.GetKeyDown(KeyCode.Alpha4);
            //return _lane4.ReadValue<float>() > 0.1f ;
        }
    }
}