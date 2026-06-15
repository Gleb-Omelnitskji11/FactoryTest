using ConfigData;
using DG.Tweening;
using UI;
using UnityEngine;

namespace GameUnits
{
    public abstract class Unit : MonoBehaviour
    {
        [SerializeField] protected HpBar _hpBar;
        [SerializeField] protected Renderer _renderer;
        [SerializeField] protected Material _materialCache;
        protected static Material _damagedMaterial;
    
        protected int CurrentHp;
        protected UnitModel UnitModel;
        //protected Tween _resetDmgAnimation;
        protected const float _durationTakenDmgAnim = 0.3f;

        private void Start()
        {
            //_resetDmgAnimation = DOVirtual.DelayedCall(_durationTakenDmgAnim, ResetMaterial).SetAutoKill(false).Pause();
            
        }

        public void InitUnit(UnitModel model)
        {
            UnitModel = model;
            _materialCache = _renderer.material;
            Reset();
        }

        public static void SetDamagedMaterial(Material material)
        {
            _damagedMaterial = material;
        }

        public virtual void Reset()
        {
            ResetMaterial();
            CurrentHp = UnitModel.MaxHp;
            _hpBar.Init(UnitModel.MaxHp);
        }

        protected int GetCollisionDamage() => UnitModel.CollisionDamage;

        public virtual void TakeDamage(int damage)
        {
            CurrentHp = Mathf.Max(0, CurrentHp - damage);
            _hpBar.UpdateState(CurrentHp);
            if (CurrentHp <= 0)
            {
                Died();
            }
            else
            {
                AnimateTakeDamage();
            }
        }

        protected virtual void AnimateTakeDamage()
        {
            _renderer.material = _damagedMaterial;

            CancelInvoke(nameof(ResetMaterial));
            Invoke(nameof(ResetMaterial), _durationTakenDmgAnim);
        }

        private void ResetMaterial()
        {
            _renderer.material = _materialCache;
        }

        protected void TakeLethalDamage()
        {
            TakeDamage(CurrentHp);
        }

        protected abstract void Died();
    }
}