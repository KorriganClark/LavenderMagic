using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavender
{
    /// <summary>
    /// 可以同时有多种Buff类型
    /// </summary>
    public enum BuffType
    {
        Damage,
        Element,
    }

    public abstract class LBuff
    {
        public LEntity Entity { get; set; }


        public virtual bool NeedUpdate { get { return false; } }
        public virtual bool ApplyOnAttach { get { return false; } }
        public virtual bool RemoveAfterApply { get { return false; } }


        public abstract List<BuffType> BuffTypes { get; }

        public virtual void OnAttachToEntity(LEntity entity)
        {
            this.Entity = entity;
            if(ApplyOnAttach)
            {
                var battleCom = entity.GetComponent<LBattleComponent>();
                if (battleCom != null)
                {
                    battleCom.ApplyBuff(this);
                }
            }
        }

        public virtual void OnLeaveFromEntity(LEntity entity)
        {
            
        }

        public virtual void OnUpdateAtEntity()
        {
            if(Entity == null||!NeedUpdate) return;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual int DamageVal()
        {
            if (!BuffTypes.Contains(BuffType.Damage))
            {
                throw new Exception("不是伤害类型buff");
            }
            return 0;
        }

    }
}
