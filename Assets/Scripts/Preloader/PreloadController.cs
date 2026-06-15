using System.Collections;
using Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Preloader
{
    public class PreloadController : MonoBehaviour
    {
        private PlayerProgressSaver _playerProgressSaver;

        [Inject]
        public void Construct(PlayerProgressSaver playerProgressSaver)
        {
            _playerProgressSaver = playerProgressSaver;
        }

        private void Start()
        {
            InitPlayerPrefs();
            GoToGame();
        }

        private void GoToGame()
        {
            InitPlayerPrefs();
            StartCoroutine(GoToGameCoroutine());
        }

        private IEnumerator GoToGameCoroutine()
        {
            yield return SceneManager.LoadSceneAsync(Constants.GameScene);
        }

        private void InitPlayerPrefs()
        {
            _playerProgressSaver.Initialize();
        }
    }
}