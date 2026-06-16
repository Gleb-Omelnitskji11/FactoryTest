using System;
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
        protected Material MaterialCache;
        
        protected static Material DamagedMaterial;
    
        protected int CurrentHp;
        protected UnitModel UnitModel;
        protected const float DurationTakenDmgAnim = 0.2f;

        public void InitUnit(UnitModel model)
        {
            UnitModel = model;
            MaterialCache = _renderer.material;
            Reset();
        }

        public static void SetDamagedMaterial(Material material)
        {
            DamagedMaterial = material;
        }

        public virtual void Reset()
        {
            ResetMaterial();
            CurrentHp = UnitModel.MaxHp;
            _hpBar.Init(UnitModel.MaxHp);
        }

        protected void OnDestroy()
        {
            CancelInvoke(nameof(ResetMaterial));
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
            _renderer.material = DamagedMaterial;

            CancelInvoke(nameof(ResetMaterial));
            Invoke(nameof(ResetMaterial), DurationTakenDmgAnim);
        }

        private void ResetMaterial()
        {
            _renderer.material = MaterialCache;
        }

        protected void TakeLethalDamage()
        {
            TakeDamage(CurrentHp);
        }

        protected abstract void Died();
    }
}