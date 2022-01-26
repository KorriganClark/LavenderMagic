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
        private sealed class ReferenceSet
        {
            private readonly Queue<IReference> references;
            private readonly Type referenceType;

            private int usingReferenceCount;

            public ReferenceSet(Type type)
            {
                references = new Queue<IReference>();
                referenceType = type;
                usingReferenceCount = 0;
            }

            public int UsingReferenceCount
            {
                get
                {
                    return usingReferenceCount;
                }
            }

            public Type ReferenceType
            {
                get
                {
                    return referenceType;
                }
            }

            public T Acquire<T>() where T: class, IReference, new()
            {
                if (typeof(T) != referenceType)
                {
                    throw new Exception("Type Not Match!");
                }
                usingReferenceCount++;
                if(references.Count > 0)
                {
                    return (T)references.Dequeue();
                }
                return new T();
            }

            public IReference Acquire()
            {
                usingReferenceCount++;
                if(references.Count > 0)
                {
                    return references.Dequeue();
                }
                return (IReference)Activator.CreateInstance(referenceType);
            }

            public void Release(IReference reference)
            {
                if(reference == null)
                {
                    throw new Exception("Reference is Null!");
                }
                reference.Clear();
                usingReferenceCount--;
                references.Enqueue(reference);
            }

            public void Clear()
            {
                lock (references)
                {
                    references.Clear();
                }
            }

        }
    }
}
