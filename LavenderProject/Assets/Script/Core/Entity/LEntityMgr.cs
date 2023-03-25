using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Lavender
{
    public class LEntityMgr : LSingleton<LEntityMgr>
    {
        public LEntityMgr() { }

        public List<LEntity> entityList = new List<LEntity>();

        public LEntity CreateEntity<T>(LEntityConfig config = null) where T : LEntity, new()
        {
            var entity = new T();
            entity.Config = config;
            entityList.Add(entity);
            entity.Init();
            return entity;
        }

        public void DestroyEntity(LEntity entity)
        {
            entityList.Remove(entity);
            entity.UnInit();
            //临时
            if(MonsterCreater.Instance.TestMonster == entity)
            {
                MonsterCreater.Instance.TestMonster = null;
            }
        }

        public void Update(float delta)
        {
            for(int i = 0;i < entityList.Count;i++)
            {
                entityList[i].Update(delta);
            }
        }

        public List<LEntity> GetEntitysByPos(Vector3 centerPosition, float radius)
        {
            List<LEntity> res = new List<LEntity>();
            foreach(var entity in entityList)
            {
                var pos = entity.Root.transform.position;
                if (Vector3.Distance(pos, centerPosition) <= radius)
                {
                    res.Add(entity);
                }
            }
            return res;
        }
    }
}
