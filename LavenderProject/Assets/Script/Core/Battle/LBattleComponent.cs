using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace Lavender
{
    public class LBattleComponent : LComponent
    {

        public BuffProcessor Processor { get { return BuffProcessor.Instance; } }

        public List<LSkill> skills = new List<LSkill>();

        public List<LBuff> buffs = new List<LBuff>();

        public void RequestAddBuffToOther(LBuff buff, LEntity target)
        {
            LBattleMgr.Instance.ProcessAddBuffRequest(Entity, target, buff);
        }

        public void AddBuff(LBuff buff)
        {
            buffs.Add(buff);
            buff.OnAttachToEntity(Entity);
        }

        public void RemoveBuff(LBuff buff)
        {
            buffs.Remove(buff);
            buff.OnLeaveFromEntity(Entity);
        }
        
        public void ApplyBuff(LBuff buff)
        {
            Processor.ProcessBuff(buff);
            if (buff.RemoveAfterApply)
            {
                RemoveBuff(buff);
            }
        }
    }
}
