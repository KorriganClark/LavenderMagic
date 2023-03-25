using UnityEngine;

namespace Lavender
{
    public class MonsterCreater : LSingleton<MonsterCreater>
    {
        public LMonster TestMonster { get; set; }
        public LEntityConfig Config { get; set; }
        public Vector3 defaultPos { get; set; }
        public void Update(float delta)
        {
            if(TestMonster == null && Config != null)
            {
                TestMonster = (LMonster)LEntityMgr.Instance.CreateEntity<LMonster>(Config);
                TestMonster.Root.transform.position = defaultPos;
            }
        }
    }
}
