using Lavender.UnityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Spec
{
    public class ProjectileComponent: MonoBehaviour
    {
        public void RecycleDelay()
        {
            Invoke("Recycle", 10f);
        }

        private void Recycle()
        {
            FrameworkComponentControl.GetComponent<ProjectileModuleComponent>().RecycleProjectile(gameObject);
        }
    }
}
