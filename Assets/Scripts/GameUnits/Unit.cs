using ConfigData;
using UI;
using UnityEngine;

namespace GameUnits
{
    public abstract class Unit : MonoBehaviour
    {
        [SerializeField] protected HpBar _hpBar;
    
        protected int CurrentHp;
        protected UnitModel UnitModel;

        public void InitUnit(UnitModel model)
        {
            UnitModel = model;
            Reset();
        }

        public virtual void Reset()
        {
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
        }

        protected void TakeLethalDamage()
        {
            TakeDamage(CurrentHp);
        }

        protected abstract void Died();
    }
}