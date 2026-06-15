using Core;
using Core.BusEvents;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class PausePopup : MonoBehaviour
    {
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _quitButton;
        private IEventBus _eventBus;
        
        [Inject]
        public void Construct(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        private void Start()
        {
            _resumeButton.onClick.AddListener(Resume);
            _restartButton.onClick.AddListener(Restart);
            _quitButton.onClick.AddListener(Exit);
        }

        private void OnDestroy()
        {
            _resumeButton.onClick.RemoveAllListeners();
            _restartButton.onClick.RemoveAllListeners();
            _quitButton.onClick.RemoveAllListeners();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            Pause();
        }

        private void Pause()
        {
            _eventBus.Publish<PauseEvent>(new PauseEvent(true));
        }

        private void Resume()
        {
            gameObject.SetActive(false);
            _eventBus.Publish<PauseEvent>(new PauseEvent(false));
        }
        
        private void Restart()
        {
            gameObject.SetActive(false);
            _eventBus.Publish<RestartEvent>(new RestartEvent());
        }

        private void Exit()
        {
            Application.Quit();
        }
    }
}
