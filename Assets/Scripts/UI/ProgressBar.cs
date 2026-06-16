using ConfigData;
using Core;
using Core.BusEvents;
using Gameplay.GameServices;
using TMPro;
using UnityEngine;
using Zenject;
using Slider = UnityEngine.UI.Slider;

namespace UI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Slider _progressBar;
        [SerializeField] private TMP_Text _timerText;

        private LevelModel _level;
        private float _startZ;
        private int _progress;
        private Transform _carTransform;
        private bool _paused = true;

        private IEventBus _eventBus;
        private PlayerProvider _playerProvider;

        [Inject]
        public void Construct(IEventBus eventBus, PlayerProvider playerProvider)
        {
            _playerProvider = playerProvider;
            _eventBus = eventBus;
        }

        public void Setup(LevelModel level, float startZ)
        {
            _startZ = startZ;
            _level = level;
            _carTransform = _playerProvider.PlayerCar.transform;
            _paused = false;
        }

        private void Update()
        {
            if (_paused) return;
            float progress = ((_carTransform.position.z - _startZ) / _level.Distance) * 100;
            _progressBar.value = progress;
            if (_progress != (int)progress)
            {
                _progress = (int)progress;
                _timerText.text = _progress + "%";
            }

            if (_progress >= 100)
            {
                _paused = true;
                GameResultEvent gameResultEvent = new GameResultEvent(true);
                _eventBus.Publish<GameResultEvent>(gameResultEvent);
            }
        }
    }
}