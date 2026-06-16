using System;
using UnityEngine;

namespace Gameplay.GameServices
{
    public class GameInstance : MonoBehaviour
    {
        public event Action OnUpdate;

        private void Update()
        {
            OnUpdate?.Invoke();
        }
    }
}
