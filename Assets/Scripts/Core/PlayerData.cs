using System;
using ConfigData;
using UnityEngine.Serialization;

namespace Core
{
    [Serializable]
    public class PlayerData
    {
        [FormerlySerializedAs("Level")] public int CurrentLevel;
        public int Currency;
        public CarData CurrentCar;
    }

    [Serializable]
    public class CarData
    {
        public CarType CarType = CarType.Model1;
        public TurretType TurretType = TurretType.Standard;
    }
}