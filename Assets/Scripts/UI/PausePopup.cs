using Core;
using Core.BusEvents;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class PausePopup : BasePopup
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

        public void TurnOn()
        {
            Show();
            Pause();
        }

        private void Pause()
        {
            _eventBus.Publish<PauseEvent>(new PauseEvent(true));
        }

        private void Resume()
        {
            Hide();
            _eventBus.Publish<PauseEvent>(new PauseEvent(false));
        }
        
        private void Restart()
        {
            Hide();
            
            _eventBus.Publish<StartGameClickedEvent>(new StartGameClickedEvent());
        }

        private void Exit()
        {
            Application.Quit();
        }
    }
}
