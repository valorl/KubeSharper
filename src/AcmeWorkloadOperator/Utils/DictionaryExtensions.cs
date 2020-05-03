using System;
using System.Collections.Generic;
using System.Text;

namespace AcmeWorkloadOperator.Utils
{
    public static class DictionaryExtensions
    {
        public static IDictionary<K,V> WithKeyValue<K,V>(this IDictionary<K,V> dict, K key, V value)
        {
            dict.TryAdd(key, value);
            return dict;
        }
    }
}
