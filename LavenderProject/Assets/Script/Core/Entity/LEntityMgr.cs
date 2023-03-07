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

        public LEntity CreateEntity<T>(ScriptableObject config = null) where T : LEntity, new()
        {
           var entity = new T();
            entity.config = config;
            entityList.Add(entity);
            entity.Init();
            return entity;
        }

        public void Update(float delta)
        {
            for(int i = 0;i < entityList.Count;i++)
            {
                entityList[i].Update(delta);
            }
        }
    }
}
