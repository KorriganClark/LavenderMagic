using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Lavender
{
    public class LGameObject
    {
        protected GameObject root;


        public int Id;
        public string Name;
        public Dictionary<Type, LComponent> components = new Dictionary<Type, LComponent>();
        public List<Type> ComponentTypes;
        public GameObject Root
        {
            get
            {
                if (root == null)
                {
                    root = new GameObject(Name);
                }
                return root;
            }
        }

        public void Init()
        {
            InitParams();
            InitComponents();
            OnInit();
        }
        /// <summary>
        /// 设置静态参数
        /// </summary>
        public virtual void InitParams()
        {
            
        }

        public virtual void OnInit()
        {

        }

        public void InitComponents()
        {
            if (ComponentTypes != null) 
            {
                for (int i = 0; i < ComponentTypes.Count; i++) 
                {
                    var component = Activator.CreateInstance(ComponentTypes[i]) as LComponent;
                    component.OnAttach(this);
                    components.Add(ComponentTypes[i], component);
                }
            }
        }

        public T GetComponent<T>(bool add = false) where T : LComponent, new()
        {
            LComponent component;
            if(!components.TryGetValue(typeof(T),out component))
            {
                if(!add)
                {
                    throw new Exception($"该GameObject上没有组件{typeof(T)}");
                }
                component = new T();
                component.OnAttach(this);
                components.Add(typeof(T), component);

            }
            return component as T;
        }

        public T AddComponent<T>() where T : LComponent, new()
        {
            LComponent component;
            if (!components.TryGetValue(typeof(T), out component))
            {
                component = new T();
                component.OnAttach(this);
                components.Add(typeof(T), component);
            }
            return component as T;
        }

        public virtual void Update(float delta)
        {
            foreach(var pair in components)
            {
                pair.Value.Update(delta);
            }
        }

    }
}
