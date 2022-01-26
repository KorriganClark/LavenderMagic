using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavender.FrameWork
{
    /// <summary>
    /// 引用池
    /// </summary>
    public static partial class ReferencePool
    {
        private static readonly Dictionary<Type, ReferenceSet> referenceSets = new Dictionary<Type, ReferenceSet>();

        public static int Count
        {
            get
            {
                return referenceSets.Count;
            }
        }

        public static void ClearAll()
        {
            lock (referenceSets)
            {
                foreach(var keyVaue in referenceSets)
                {
                    keyVaue.Value.Clear();
                }
                referenceSets.Clear();
            }
        }

        public static T Acquire<T>() where T : class, IReference, new()
        {
            return GetReferenceSet(typeof(T)).Acquire<T>();
        }

        public static IReference Acquire(Type referenceType)
        {
            return GetReferenceSet(referenceType).Acquire();
        }

        public static void Release(IReference reference)
        {
            if(reference == null)
            {
                throw new Exception("Reference is Null");
            }
            Type referenceType = reference.GetType();//接口指向的是一个具体的实例，获取了那个实例的类型
            GetReferenceSet(referenceType).Release(reference);
        }

        private static ReferenceSet GetReferenceSet(Type referenceType)
        {
            if (referenceType == null)
            {
                throw new Exception("RefereceType is null!");
            }
            ReferenceSet referenceSet = null;
            lock (referenceSets)
            {
                if (!referenceSets.TryGetValue(referenceType, out referenceSet))
                {
                    referenceSet = new ReferenceSet(referenceType);
                    referenceSets.Add(referenceType, referenceSet);
                }
            }
            
            return referenceSet;
        }

    }
}
