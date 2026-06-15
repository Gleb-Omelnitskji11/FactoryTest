using System;
using System.Collections.Generic;
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

        //public List<TurretType> PurchasedTurrets;
        //public List<CarType> PurchasedCars;
    }

    [Serializable]
    public class CarData
    {
        public CarType CarType = CarType.Model1;
        public TurretType TurretType = TurretType.Standard;
    }
}