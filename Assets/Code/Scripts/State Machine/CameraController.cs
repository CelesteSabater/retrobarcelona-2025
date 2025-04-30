using UnityEngine;
using Cinemachine;
using retrobarcelona.Managers.ControlsManager;
using retrobarcelona.Utils.Singleton;


namespace retrobarcelona.StateMachine
{
    public class CameraController : MonoBehaviour
    {
        [Header("Target")]
        public GameObject _target;
        public Vector2 _panLimit;
        public Vector2 _zoomLimit;

        [Header("Movement Settings")]
        public float _moveSpeed = 20f;
        public float _edgeScrollMargin = 0.1f; 
        public float _movementAcceleration = 2f;
        
        [Header("Zoom Settings")]
        public float _zoomSpeed = 20f;
        public float _zoomAcceleration = 2f;
        
        private float _xMargin, _yMargin;
        private Vector3 _moveDirection;
        
        void Start()
        {       
            CalculateMargins();
        }
        
        void Update()
        {
            CalculateMargins();
            HandleMovement();
            HandleZoom();
        }
        
        void HandleMovement()
        {
            _moveDirection = Vector3.zero;
            
            _moveDirection.x = ControlsManager.Instance.GetMovementDirection().x;
            _moveDirection.z = ControlsManager.Instance.GetMovementDirection().y;

            EdgeScrolling();
            _moveDirection.Normalize();

            float _currentMoveSpeed = ControlsManager.Instance.GetIsAccelerating() ? _moveSpeed * _movementAcceleration : _moveSpeed;
            Vector3 newPos = new Vector3(_moveDirection.x, 0, _moveDirection.z);
            newPos = _currentMoveSpeed * Time.deltaTime * newPos + _target.transform.position;

            newPos.x = Mathf.Clamp(newPos.x, -_panLimit.x, _panLimit.x);
            newPos.z = Mathf.Clamp(newPos.z, -_panLimit.y, _panLimit.y);

            _target.transform.position = newPos;
        }
        
        void HandleZoom()
        {
            float scroll = ControlsManager.Instance.GetZoomDirection();
            
            if (scroll != 0)
            {
                float _currentZoomSpeed = ControlsManager.Instance.GetIsAccelerating() ? _zoomSpeed * _zoomAcceleration : _zoomSpeed;
                Vector3 newPos = new Vector3(0, -scroll, 0);
                newPos = _currentZoomSpeed * Time.deltaTime * newPos + _target.transform.position;
                newPos.y = Mathf.Clamp(newPos.y, _zoomLimit.x, _zoomLimit.y);

            _target.transform.position = newPos;
            }
        }

        private void EdgeScrolling()
        {
            if (Input.mousePosition.x <= _xMargin)
            {
                _moveDirection.x = -1f;
            }
            else if (Input.mousePosition.x >= Screen.width - _xMargin)
            {
                _moveDirection.x = 1f;
            }
            
            if (Input.mousePosition.y <= _yMargin)
            {
                _moveDirection.z = -1f;
            }
            else if (Input.mousePosition.y >= Screen.height - _yMargin)
            {
                _moveDirection.z = 1f;
            }
        }

        private void CalculateMargins()
        {
            _xMargin = Screen.width * _edgeScrollMargin;
            _yMargin = Screen.height * _edgeScrollMargin;
        }
    }
}