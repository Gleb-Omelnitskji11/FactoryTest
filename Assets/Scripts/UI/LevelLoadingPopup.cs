using System.Threading;
using Core;
using Core.BusEvents;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI
{
    public class LevelLoadingPopup : MonoBehaviour
    {
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private int _seconds;

        private float _timer;
        private const string LevelFormat = "{0} Level";
        
        private Tween _timerTween;
        private CancellationTokenSource _cts;
        private IEventBus _eventBus;
        private PlayerProgressSaver _playerProgress;

        [Inject]
        public void Construct(IEventBus eventBus, PlayerProgressSaver playerProgress)
        {
            _playerProgress = playerProgress;
            _eventBus = eventBus;
        }
        
        private void Start()
        {
            _cts = new CancellationTokenSource();
            StartCountdown().Forget();
            _levelText.text = string.Format(LevelFormat, _playerProgress.CurrentLevel);
        }

        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }

        private async UniTask StartCountdown()
        {
            for (int i = _seconds; i > 0; i--)
            {
                _timerText.text = i.ToString();
                await WaitOneSecond();
            }
            
            _timerText.text = "0";
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
            
            gameObject.SetActive(false);
            _eventBus.Publish<RestartEvent>(new RestartEvent(true));
        }
    }
}