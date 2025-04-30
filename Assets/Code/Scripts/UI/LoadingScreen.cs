using UnityEngine;
using UnityEngine.UI;

namespace retrobarcelona.UI
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingScreenPrefab;
        private GameObject _loadingScreen;
        private Slider _loadingSlider; 

        public void Show()
        {
            if (_loadingScreen != null)
                Destroy(_loadingScreen);

            _loadingScreen = Instantiate(_loadingScreenPrefab);
            _loadingSlider = _loadingScreen.GetComponentInChildren<Slider>();
            UpdateLoadingProgress(0);
            _loadingScreen.SetActive(true); 
        }

        public void Hide()
        {
            if (_loadingScreen == null)
                return;
            
            Destroy(_loadingScreen);
        }

        public void UpdateLoadingProgress(float progress)
        {
            if(_loadingSlider == null)
                return;

            _loadingSlider.value = progress; 
        }
    }
}