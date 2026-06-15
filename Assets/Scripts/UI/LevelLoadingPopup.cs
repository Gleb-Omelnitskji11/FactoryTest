using System;
using System.Threading;
using Core;
using Core.BusEvents;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class LevelLoadingPopup : BasePopup
    {
        [SerializeField] private TMP_Text _mainText;
        [SerializeField] private TMP_Text _levelText;
        
        [SerializeField] private Button _button;
        [SerializeField] private int _seconds;

        private float _timer;
        private const string LevelFormat = "{0} Level";
        
        private Tween _timerTween;
        private CancellationTokenSource _cts;
        private IEventBus _eventBus;
        private PlayerProgressSaver _playerProgress;
        
        private const string Description = "Press to start the game";

        [Inject]
        public void Construct(IEventBus eventBus, PlayerProgressSaver playerProgress)
        {
            _playerProgress = playerProgress;
            _eventBus = eventBus;
        }
        
        private void Start()
        {
            _cts = new CancellationTokenSource();
            _button.onClick.AddListener(StartCountdown);
            _eventBus.Subscribe<StartGameEvent>(TurnOn);
            TurnOn(null);
        }

        private void TurnOn(StartGameEvent signal)
        {
            Show();
            _levelText.text = string.Format(LevelFormat, _playerProgress.CurrentLevel);
            _mainText.text = Description;
        }

        private void StartCountdown()
        {
            Countdown().Forget();
        }

        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }

        private async UniTask Countdown()
        {
            for (int i = _seconds; i > 0; i--)
            {
                _mainText.text = i.ToString();
                await WaitOneSecond();
            }
            
            _mainText.text = "0";
            StartGame();
        }

        private async UniTask WaitOneSecond()
        {
            await UniTask.Delay(1000, cancellationToken: _cts.Token);
        }

        private void StartGame()
        {
            if (_cts.Token.IsCancellationRequested)
                return;
            
            Hide();
            _eventBus.Publish<RestartEvent>(new RestartEvent(true));
        }
    }
}