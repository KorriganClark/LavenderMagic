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

        public static T Acquire<T>() where T : class, IReference, new()
        {
            
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
                    referenceSet = new ReferenceSet();
                    referenceSets.Add(referenceType, referenceSet);
                }
            }
            
            return referenceSet;
        }

    }
}
