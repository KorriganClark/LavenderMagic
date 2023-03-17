using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavender
{
    public class LBattleMgr : LSingleton<LBattleMgr>
    {
        public void ProcessAddBuffRequest(LEntity requester, LEntity target, LBuff buff)
        {
            target.GetComponent<LBattleComponent>().AddBuff(buff);
        }
    }
}
