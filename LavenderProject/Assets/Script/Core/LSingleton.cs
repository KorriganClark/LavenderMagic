using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavender
{
    public abstract class LSingleton<T> where T : class
    {
        private static T instance = null;
        class Nested
        {
            internal static readonly T instance = Activator.CreateInstance(typeof(T), true) as T;
        }
        public static T Instance { get { return Nested.instance; } }
    }
}
