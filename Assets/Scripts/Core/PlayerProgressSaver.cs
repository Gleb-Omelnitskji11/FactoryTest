using System;
using Core.BusEvents;
using UnityEngine;
using Zenject;

namespace Core
{
    public class PlayerProgressSaver : IInitializable, IDisposable
    {
        private PlayerData _playerData;
        private IEventBus _eventBus;
        private const string PlayerKey = "PlayerDataKey";

        public CarData CarData => _playerData.CurrentCar;
        public int CurrentLevel => _playerData.CurrentLevel;

        public PlayerProgressSaver(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Initialize()
        {
            LoadPlayerData();
            Subscribe();
        }

        public void Dispose()
        {
            _eventBus.Unsubscribe<GameResultEvent>(CompleteLevel);
        }

        private void Subscribe()
        {
            _eventBus.Subscribe<GameResultEvent>(CompleteLevel);
        }

        private void LoadPlayerData()
        {
            if (PlayerPrefs.HasKey(PlayerKey))
            {
                string dataRaw = PlayerPrefs.GetString(PlayerKey);
                try
                {
                    _playerData = JsonUtility.FromJson<PlayerData>(dataRaw);
                }
                catch
                {
                    SetNewPlayerData();
                }
            }
            else
            {
                SetNewPlayerData();
            }
        }

        private void SetNewPlayerData()
        {
            _playerData = new PlayerData()
            {
                CurrentLevel = 1,
                Currency = 0,
                CurrentCar = new CarData(),
                //PurchasedCars = new List<CarType>(),
                //PurchasedTurrets = new List<TurretType>()
            };
            SavePlayerData();
        }

        private void SavePlayerData()
        {
            string dataJson = JsonUtility.ToJson(_playerData);
            PlayerPrefs.SetString(PlayerKey, dataJson);
            PlayerPrefs.Save();
        }

        private void CompleteLevel(GameResultEvent resultEvent)
        {
            if (resultEvent.IsWin)
            {
                _playerData.CurrentLevel++;
                SavePlayerData();
            }
        }
    }
}