using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opportunity.MathExpression.Internal
{
    internal class IdDictionary<TValue> : IDictionary<string, TValue>
    {
        private Dictionary<string, TValue> valueDic = new Dictionary<string, TValue>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, string> nameDic = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public ICollection<string> Keys => valueDic.Keys;

        public ICollection<TValue> Values => valueDic.Values;

        public int Count => valueDic.Count;

        public bool IsReadOnly => false;

        public TValue this[string key]
        {
            get
            {
                return valueDic[key];
            }
            set
            {
                valueDic[key] = value;
                nameDic[key] = key;
            }
        }

        public string GetKey(string key)
        {
            return nameDic[key];
        }

        public void Add(string key, TValue value)
        {
            valueDic.Add(key, value);
            nameDic.Add(key, key);
        }

        public bool ContainsKey(string key) => valueDic.ContainsKey(key);

        public bool Remove(string key)
        {
            valueDic.Remove(key);
            return nameDic.Remove(key);
        }

        public bool TryGetValue(string key, out TValue value) => valueDic.TryGetValue(key, out value);

        void ICollection<KeyValuePair<string, TValue>>.Add(KeyValuePair<string, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            valueDic.Clear();
            nameDic.Clear();
        }

        bool ICollection<KeyValuePair<string, TValue>>.Contains(KeyValuePair<string, TValue> item)
            => ((ICollection<KeyValuePair<string, TValue>>)valueDic).Contains(item);

        void ICollection<KeyValuePair<string, TValue>>.CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
            => ((ICollection<KeyValuePair<string, TValue>>)valueDic).CopyTo(array, arrayIndex);

        bool ICollection<KeyValuePair<string, TValue>>.Remove(KeyValuePair<string, TValue> item)
        {
            var r = ((ICollection<KeyValuePair<string, TValue>>)valueDic).Remove(item);
            if (r)
                nameDic.Remove(item.Key);
            return r;
        }

        public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator() => valueDic.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => valueDic.GetEnumerator();
    }
}
