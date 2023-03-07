using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavender
{
    public abstract class LSingleton<T> where T : class
    {
        class Nested
        {
            internal static readonly T instance = Activator.CreateInstance(typeof(T), true) as T;
        }
        private static T instance = null;
        public static T Instance { get { return Nested.instance; } }
    }
}
