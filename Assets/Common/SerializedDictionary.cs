using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    [Serializable]
    public class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [Serializable]
        public struct Pair
        {
            public TKey key;
            public TValue value;

            public static implicit operator KeyValuePair<TKey, TValue>(Pair pair)
            {
                return new KeyValuePair<TKey, TValue>(pair.key, pair.value);
            }

            public static implicit operator Pair(KeyValuePair<TKey, TValue> pair)
            {
                return new Pair
                {
                    key = pair.Key, 
                    value = pair.Value
                };
            }
        }

        [SerializeField] private List<Pair> entries = new();

        public SerializedDictionary()
        {
        }

        public SerializedDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary)
        {
        }

        public void OnBeforeSerialize()
        {
            entries.Clear();
            foreach (var pair in this)
                entries.Add(pair);
        }

        public void OnAfterDeserialize()
        {
            Clear();
            foreach (var entry in entries)
                this[entry.key] = entry.value;
        }
    }
}