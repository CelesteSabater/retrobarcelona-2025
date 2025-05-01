using UnityEngine;
using Cinemachine;
using retrobarcelona.Managers.ControlsManager;
using retrobarcelona.Utils.Singleton;
using Cinemachine;

namespace retrobarcelona.StateMachine
{
    public class CameraController : MonoBehaviour
    {
        private CinemachineVirtualCamera camera;
        
        void Start()
        {       
            camera = GetComponentInChildren<CinemachineVirtualCamera>();
        }
        
        public void SetFollow(GameObject go)
        {
            if (camera == null)
                return;
            
            camera.Follow = go.transform;
        }
    }
}