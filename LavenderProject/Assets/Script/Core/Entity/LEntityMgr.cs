using System.Collections.Generic;
using UnityEngine;

namespace Lavender
{
    public class LEntityMgr : LSingleton<LEntityMgr>
    {
        public LEntityMgr() { }

        // 存储实体的列表
        private List<LEntity> entityList = new List<LEntity>();

        // 创建实体的泛型方法
        public LEntity CreateEntity<T>(LEntityConfig config = null) where T : LEntity, new()
        {
            var entity = new T
            {
                Config = config
            };
            entityList.Add(entity);
            entity.Init();
            return entity;
        }

        // 销毁实体
        public void DestroyEntity(LEntity entity)
        {
            entityList.Remove(entity);
            entity.UnInit();
            if (MonsterCreater.Instance.TestMonster == entity)
            {
                MonsterCreater.Instance.TestMonster = null;
            }
        }

        // 更新实体
        public void Update(float delta)
        {
            int entityListCount = entityList.Count;
            for (int i = 0; i < entityListCount; i++)
            {
                entityList[i].Update(delta);
            }
        }

        // 根据位置和半径获取实体列表
        // 使用迭代器方法遍历符合条件的实体
        public IEnumerable<LEntity> GetEntitysByPos(Vector3 centerPosition, float radius)
        {
            int entityListCount = entityList.Count;
            for (int i = 0; i < entityListCount; i++)
            {
                var pos = entityList[i].Root.transform.position;
                if (Vector3.Distance(pos, centerPosition) <= radius)
                {
                    yield return entityList[i];
                }
            }
        }
        // 使用迭代器方法遍历符合条件的实体，同时排除指定的实体
        public IEnumerable<LEntity> GetEntitysByPos(Vector3 centerPosition, float radius, LEntity excludeEntity)
        {
            int entityListCount = entityList.Count;
            for (int i = 0; i < entityListCount; i++)
            {
                if (entityList[i] == excludeEntity)
                {
                    continue;
                }

                var pos = entityList[i].Root.transform.position;
                if (Vector3.Distance(pos, centerPosition) <= radius)
                {
                    yield return entityList[i];
                }
            }
        }

    }
}
