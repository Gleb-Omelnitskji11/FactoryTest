using ConfigData;
using UnityEngine;

namespace Core
{
    public class ConfigProvider : MonoBehaviour
    {
        private GameConfig _gameConfig;
        private const string GameConfigFileName = "GameConfig";

        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        public async Awaitable InitializeConfigs()
        {
            ResourceRequest operation  = Resources.LoadAsync<GameConfig>(GameConfigFileName);
            await operation;
            if (operation.isDone)
                _gameConfig = operation.asset as GameConfig;
        }

        public GameConfig GameConfig => _gameConfig;
    }
}