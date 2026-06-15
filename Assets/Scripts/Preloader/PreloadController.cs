using System;
using System.Collections;
using Core;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Preloader
{
    public class PreloadController : MonoBehaviour
    {
        private PlayerProgressSaver _playerProgressSaver;
        private ConfigProvider _configProvider;

        [Inject]
        public void Construct(PlayerProgressSaver playerProgressSaver, ConfigProvider configProvider)
        {
            _configProvider = configProvider;
            _playerProgressSaver = playerProgressSaver;
        }

        private void Start()
        {
            StartTheGame();
        }

        private async void StartTheGame()
        {
            await LoadConfig();
            InitPlayerPrefs();
            GoToGame();
        }

        private async UniTask LoadConfig()
        {
            await _configProvider.InitializeConfigs();
        }

        private void InitPlayerPrefs()
        {
            _playerProgressSaver.Initialize();
        }

        private void GoToGame()
        {
            StartCoroutine(GoToGameCoroutine());
        }

        private IEnumerator GoToGameCoroutine()
        {
            yield return SceneManager.LoadSceneAsync(Constants.GameScene);
        }
    }
}