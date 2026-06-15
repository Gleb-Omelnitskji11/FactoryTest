using System;
using GameUnits;
using UnityEngine;

namespace ConfigData
{
    [Serializable]
    public class PlayerUnitModel
    {
        [SerializeField] private PlayerCar _carPrefab;
        [SerializeField] private CarType _carType;
        [SerializeField] private UnitModel _unitModel;

        public PlayerCar CarPrefab => _carPrefab;
        public CarType CarType => _carType;
        public UnitModel UnitModel => _unitModel;
    }

    public enum CarType
    {
        Model1,
        Model2,
        Model3
    }
}