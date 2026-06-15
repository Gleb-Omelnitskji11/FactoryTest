using Core;
using Core.BusEvents;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class ResultPopup : BasePopup
    {
        [SerializeField] private TMP_Text _winText;
        [SerializeField] private TMP_Text _loseText;
        [SerializeField] private Button _repeatButton;
        
        private IEventBus _eventBus;

        [Inject]
        public void Construct(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }
    
        private void Start()
        {
            _repeatButton.onClick.AddListener(StartGame);
            _eventBus.Subscribe<GameResultEvent>(OnGameEnd);
        }
        
        private void OnDestroy()
        {
            _repeatButton.onClick.RemoveAllListeners();
            _eventBus.Unsubscribe<GameResultEvent>(OnGameEnd);
        }

        private void OnGameEnd(GameResultEvent gameResultEvent)
        {
            TurnOn(gameResultEvent.IsWin);
        }

        private void TurnOn(bool win)
        {
            Show();
            _loseText.gameObject.SetActive(!win);
            _winText.gameObject.SetActive(win);
        }

        private void StartGame()
        {
            Hide();
            _eventBus.Publish<StartGame>(new StartGame());
        }
    }
}
