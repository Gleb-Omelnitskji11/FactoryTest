using UnityEngine;

namespace GameServices
{
    public class PlayerInputProvider : MonoBehaviour, IInputProvider
    {
        public bool IsPressed
        {
            get
            {
#if UNITY_EDITOR || UNITY_STANDALONE
                return Input.GetMouseButton(0);
#else
            return Input.touchCount > 0;
#endif
            }
        }

        private Camera _camera;
        private Plane _groundPlane;

        private void Awake()
        {
            _camera = Camera.main;
            _groundPlane = new Plane(Vector3.up, Vector3.zero);
        }

        public bool TryGetInputPosition(out Vector3 worldPosition)
        {
            worldPosition = Vector3.zero;

            Vector3 screenPosition;

#if UNITY_EDITOR || UNITY_STANDALONE
            if (!Input.GetMouseButton(0))
                return false;

            screenPosition = Input.mousePosition;
#else
        if (Input.touchCount == 0)
            return false;

        screenPosition = Input.GetTouch(0).position;
#endif

            Ray ray = _camera.ScreenPointToRay(screenPosition);

            if (_groundPlane.Raycast(ray, out float enter))
            {
                worldPosition = ray.GetPoint(enter);
                return true;
            }

            return false;
        }
    }
}