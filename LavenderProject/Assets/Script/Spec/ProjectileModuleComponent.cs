using Lavender.Framework;
using Lavender.Framework.ObjectPool;
using Lavender.UnityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;

namespace Assets.Script.Spec
{
    public class ProjectileModuleComponent: FrameworkComponent
    {
        [SerializeField]
        private int objectPoolCapacity = 16;

        private IObjectPool<ProjectileObject> objectPool = null;
        private List<GameObject> activeProjectile = null;
        
        private void Start()
        {
            objectPool = FrameworkComponentControl.GetComponent<ObjectPoolComponent>().CreateObjectPool<ProjectileObject>("Projectile");
            objectPool.Capacity = objectPoolCapacity;
            objectPool.AutoReleaseInterval = 3;
            objectPool.ExpireTime = 15;
            activeProjectile = new List<GameObject>();
        }

        public GameObject CreateProjectile(GameObject ori)
        {
            ProjectileObject projectileObject = objectPool.Spawn();
            if(projectileObject == null)
            {
                projectileObject = ProjectileObject.Create(ori);
                objectPool.Register(projectileObject, true);
            }
            return (GameObject)projectileObject.Target;
        }

        public void RecycleProjectile(GameObject target)
        {
            target.SetActive(false);
            objectPool.Unspawn(target);
        }
    }
    public class ProjectileObject : ObjectBase
    {

        public static ProjectileObject Create(GameObject ori)
        {
            GameObject gameObject = GameObject.Instantiate(ori);
            gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            gameObject.AddComponent<Rigidbody>();
            gameObject.AddComponent<ProjectileComponent>();
            ProjectileObject projectileObject = ReferencePool.Acquire<ProjectileObject>();
            projectileObject.Initialize(gameObject);
            return projectileObject;
        }

        protected override void Release(bool isShutdown)
        {
            GameObject target = (GameObject)this.Target;
            if(target == null)
            {
                return;
            }
            GameObject.Destroy(target);
        }

        protected override void OnSpawn()
        {
            GameObject target = (GameObject)this.Target;
            target.SetActive(true);
            var com = target.GetComponent<ProjectileComponent>();
            com.RecycleDelay();
        }

        protected override void OnUnspawn()
        {
            GameObject target = (GameObject)this.Target;
            target.SetActive(false);
        }
    }
}
