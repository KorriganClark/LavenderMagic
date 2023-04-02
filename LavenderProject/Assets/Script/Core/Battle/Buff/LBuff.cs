using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavender
{
    /// <summary>
    /// 
    /// </summary>
    public enum BuffType
    {
        Damage,
        Element,
    }

    public abstract class LBuff
    {
        public LEntity Entity { get; set; }
        public LBattleComponent BattleComponent { get; private set; }
        public virtual bool NeedUpdate { get { return false; } }
        public virtual bool ApplyOnAttach { get { return false; } }
        public virtual bool RemoveAfterApply { get { return false; } }
        public abstract BuffType TypeOfBuff { get; }

        public virtual void OnAttachToEntity(LEntity entity)
        {
            this.Entity = entity;
            BattleComponent = entity.GetComponent<LBattleComponent>();
        }

        public virtual void OnLeaveFromEntity(LEntity entity)
        {
            
        }

        public virtual void OnUpdateAtEntity()
        {
            if(Entity == null||!NeedUpdate) return;

        }

    }
}
