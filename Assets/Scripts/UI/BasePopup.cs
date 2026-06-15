using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class BasePopup : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup _canvasGroup;
        [SerializeField] protected float _duration = 0.5f;
        private Tween _tween;


        protected virtual void Show()
        {
            _canvasGroup.blocksRaycasts = true;
            _tween = _canvasGroup.DOFade(1f, _duration);
            _tween.Play();
        }

        protected virtual void Hide()
        {
            _tween = _canvasGroup.DOFade(0f, _duration);
            _tween.onComplete += () =>
            {
                _canvasGroup.blocksRaycasts = false;
            };
            _tween.Play();
        }

        private void OnDestroy()
        {
            _tween.Kill(false);
        }
    }
}