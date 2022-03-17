using Lavender.UnityFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Script.Spec
{
    public class BulletCreater: MonoBehaviour
    {
        [SerializeField]
        public float IntervalTime;

        [SerializeField]
        public float LastSpawnTime;

        public BulletInfo bulletInfo;

        public List<GameObject> bullets = new List<GameObject>();

        public System.Random rd = new System.Random();

        private void Awake()
        {
            GameObject projectileModule = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            projectileModule.transform.position = new Vector3(19999, 9, 9);
            projectileModule.AddComponent<ProjectileModuleComponent>();
            
        }

        private IEnumerator Start()
        {
            for(int i = 0; i < 200; i++)
            {
                var asset = FrameworkComponentControl.GetComponent<ResourceComponent>().GetAssetAsync($"Assets/Artres/Prefabs/Sph{i}.prefab");
                yield return asset;
                bullets.Add((asset as Asset).AssetGO);
                Debug.Log($"球体编号{i}加载成功!");
            }
        }

        private void Update()
        {
            LastSpawnTime += Time.deltaTime;
            if(LastSpawnTime >= IntervalTime)
            {
                LastSpawnTime = 0;
                SpawnBullet();
            }
        }

        private void SpawnBullet()
        {
            int indx = -1;
            if (bullets.Count < 200 && bullets.Count > 0)
            {
                indx = bullets.Count - 1;
            }
            else if (bullets.Count >= 200) 
            {
                indx = rd.Next(0, 200);
            }
            if(indx == -1)
            {
                return;
            }
            var bul= bullets[indx];
            GameObject bullet = FrameworkComponentControl.GetComponent<ProjectileModuleComponent>().CreateProjectile(bul);
            bullet.transform.position = gameObject.transform.position;

            Debug.Log($"球体编号{indx}发射成功!");

        }
    }

    [CreateAssetMenu]
    public class BulletInfo : ScriptableObject
    {
        public int size;
        public GameObject bullet;
    }
}
