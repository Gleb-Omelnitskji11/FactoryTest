using UnityEngine;

namespace GameServices
{
    public interface IInputProvider
    {
        bool IsPressed { get; }
        bool TryGetInputPosition(out Vector3 worldPosition);
    }
}