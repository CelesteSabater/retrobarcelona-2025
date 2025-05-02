using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using retrobarcelona.Managers.ControlsManager;
using retrobarcelona.UI;
using retrobarcelona.Disposable;
using retrobarcelona.Systems.AudioSystem;
using retrobarcelona.DialogueTree.Runtime;
using retrobarcelona.MusicSystem;

namespace retrobarcelona.Other
{
    public class GameInitiator : MonoBehaviour
    {
        [Header("Binder")]
        [SerializeField] private LoadingScreen _loadingScreen;
        [SerializeField] private GameObject _mainCamera;
        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private Light _directionalLight;

        [Header("Creation")]
        [SerializeField] private AudioSystem _audioSystem;
        [SerializeField] private ControlsManager _controlsManager;
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private DialogueSystem _dialogueSystem;
        [SerializeField] private GameObject _enviorement;
        [SerializeField] private SongLoader _songLoader;
        [SerializeField] private NoteSpawner _noteSpawner;

        private int _currentStep, _maxSteps;

        private async void Start() {
            _currentStep = 0;
            _maxSteps = 4;
            BindObjects();

            using (LoadingScreenDisposable loadingScreenDisposable = new LoadingScreenDisposable(_loadingScreen))
            {
                await InitializeObjects();
                LoadingScreenStep(loadingScreenDisposable);

                await CreateObjects();
                LoadingScreenStep(loadingScreenDisposable);

                await PrepareGame();
                LoadingScreenStep(loadingScreenDisposable);

                await BeginGame();
                LoadingScreenStep(loadingScreenDisposable);
            }

            Destroy(gameObject);
        }

        private void BindObjects()
        {
            _eventSystem = Instantiate(_eventSystem);
            _loadingScreen = Instantiate(_loadingScreen);
            _mainCamera = Instantiate(_mainCamera);
            _directionalLight = Instantiate(_directionalLight);
        }

        private async UniTask InitializeObjects()
        {
            await UniTask.Yield();
        }

        private async UniTask CreateObjects()
        {
            _enviorement = Instantiate(_enviorement);
            _audioSystem = Instantiate(_audioSystem);
            _controlsManager = Instantiate(_controlsManager);
            _uiManager = Instantiate(_uiManager);
            _dialogueSystem = Instantiate(_dialogueSystem);
            _songLoader = Instantiate(_songLoader);
            _noteSpawner = Instantiate(_noteSpawner);

            await UniTask.Yield();
        }

        private async UniTask PrepareGame()
        {
            await UniTask.Yield();
        }

        private async UniTask BeginGame()
        {
            _audioSystem.StartMusic();
            _controlsManager.ActivateGameControls();
            _songLoader.StartTest();

            await UniTask.Yield();
        }

        private void LoadingScreenStep(LoadingScreenDisposable loadingScreenDisposable)
        {
            _currentStep++;
            loadingScreenDisposable.UpdateLoadingProgress((float)_currentStep/_maxSteps);
        }
    } 
}