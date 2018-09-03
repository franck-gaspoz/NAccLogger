using System.Collections.Generic;

namespace NAccLogger.Ext.Collection
{
    public static class CollectionExt
    {
        /// <summary>
        /// clone a dictionary
        /// </summary>
        /// <typeparam name="K">key type</typeparam>
        /// <typeparam name="V">value type</typeparam>
        /// <param name="o">dictionary object</param>
        /// <returns>shallow cloned dictionary</returns>
        public static Dictionary<K,V> Clone<K,V>(this Dictionary<K,V> o)
        {
            var r = new Dictionary<K,V>();
            foreach (var kv in o)
                r.Add(kv.Key, kv.Value);
            return r;
        }

        /// <summary>
        /// clone a linked list
        /// </summary>
        /// <typeparam name="T">value type</typeparam>
        /// <param name="o">linked list</param>
        /// <returns>shallow cloned dictionary</returns>
        public static LinkedList<T> Clone<T>(this LinkedList<T> o)
        {
            var r = new LinkedList<T>();
            foreach (var e in o)
                r.AddLast(e);
            return r;
        }
    }
}
