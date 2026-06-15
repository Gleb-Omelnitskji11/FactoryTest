using ConfigData;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core
{
    public class ConfigProvider : MonoBehaviour
    {
        [FormerlySerializedAs("_unitConfig")] [SerializeField] private GameConfig _gameConfig;

        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        public GameConfig GameConfig => _gameConfig;
    }
}