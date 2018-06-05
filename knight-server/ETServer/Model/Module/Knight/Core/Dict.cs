﻿//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Knight.Core
{
    public interface IDict : IEnumerable
    {
        void AddObject(object key, object value);
        int Count { get; }
        Dictionary<object, object> OriginCollection { get; }
    }
    
    public class CKeyValuePair<TKey, TValue>
    {
        private TKey    mKey;
        private TValue  mValue;

        public CKeyValuePair(TKey key, TValue value)
        {
            this.mKey = key;
            this.mValue = value;
        }

        public TKey     Key     { get { return mKey;    }   set { mKey = value;     } }
        public TValue   Value   { get { return mValue;  }   set { mValue = value;   } }
        
        public override string ToString()
        {
            return $"CKeyValuePair<{typeof(TKey)},{typeof(TValue)}>({mKey}, {mValue})";
        }
    }

    public static class DictExpand
    {
        public static bool TryGetValue<TKey, TValue>(this Dict<TKey, TValue> rDict, TKey key, out TValue value)
        {
            object result = null;
            if (rDict.Collection.TryGetValue((object)key, out result))
            {
                value = (TValue)result;
                return true;
            }
            value = default(TValue);
            return false;
        }

        public static bool ContainsKey<TKey, TValue>(this Dict<TKey, TValue> rDict, TKey key)
        {
            return rDict.Collection.ContainsKey((object)key);
        }

        public static bool ContainValue<TKey, TValue>(this Dict<TKey, TValue> rDict, TValue value)
        {
            return rDict.Collection.ContainsValue((object)value);
        }

        public static TKey LastKey<TKey, TValue>(this Dict<TKey, TValue> rDict)
        {
            return (TKey)rDict.Collection.Keys.Last();
        }

        public static TValue LastValue<TKey, TValue>(this Dict<TKey, TValue> rDict)
        {
            return (TValue)rDict.Collection.Values.Last();
        }

        public static TKey FirstKey<TKey, TValue>(this Dict<TKey, TValue> rDict)
        {
            return (TKey)rDict.Collection.Keys.First();
        }

        public static TValue FirstValue<TKey, TValue>(this Dict<TKey, TValue> rDict)
        {
            return (TValue)rDict.Collection.Values.First();
        }
        
        public static CKeyValuePair<TKey, TValue> First<TKey, TValue>(this Dict<TKey, TValue> rDict)
        {
            if (rDict.Count == 0) return null;
            return new CKeyValuePair<TKey, TValue>(rDict.FirstKey(), rDict.FirstValue());
        }

        public static CKeyValuePair<TKey, TValue> Last<TKey, TValue>(this Dict<TKey, TValue> rDict)
        {
            if (rDict.Count == 0) return null;

            return new CKeyValuePair<TKey, TValue>(rDict.LastKey(), rDict.LastValue());
        }

        public static void Clear<TKey, TValue>(this Dict<TKey, TValue> rDict)
        {
            rDict.Collection.Clear();
        }

        public static bool Remove<TKey, TValue>(this Dict<TKey, TValue> rDict, TKey key)
        {
            return rDict.Collection.Remove((object)key);
        }

        public static void RemoveLast<TKey, TValue>(this Dict<TKey, TValue> rDict)
        {
            rDict.Collection.Remove(rDict.LastKey());
        }

        public static void RemoveFirst<TKey, TValue>(this Dict<TKey, TValue> rDict)
        {
            rDict.Collection.Remove(rDict.FirstKey());
        }

        public static Dict<TKey, TValue> Clone<TKey, TValue>(this Dict<TKey, TValue> rDict)
        {
            var newDict = new Dict<TKey, TValue>();
            foreach (var item in rDict)
            {
                newDict.Add((TKey)item.Key, (TValue)item.Value);
            }
            return newDict;
        }

        public static Dict<TKey, TValue> Sort<TKey, TValue>(this Dict<TKey, TValue> rDict, Comparison<CKeyValuePair<TKey, TValue>> cmpAlgo)
        {
            var list = new List<CKeyValuePair<TKey, TValue>>();
            foreach (var item in rDict)
            {
                list.Add(item);
            }

            list.Sort(cmpAlgo);

            rDict.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                rDict.Add(list[i].Key, list[i].Value);
            }

            return rDict;
        }
    }
    
    public class Dict<TKey, TValue> : IEnumerable<CKeyValuePair<TKey, TValue>>, IDict
    {
        public class Enumerator : IEnumerator<CKeyValuePair<TKey, TValue>>
        {
            private Dictionary<object, object>.Enumerator   mCurrent;

            public Enumerator(Dictionary<object, object>.Enumerator rEnumerator)
            {
                mCurrent = rEnumerator;
            }

            public CKeyValuePair<TKey, TValue> Current
            {

                get { return new CKeyValuePair<TKey, TValue>((TKey)mCurrent.Current.Key, (TValue)mCurrent.Current.Value); }
            }

            object IEnumerator.Current
            {
                get { return mCurrent; }
            }

            public bool MoveNext()
            {
                return mCurrent.MoveNext();
            }

            public void Reset()
            {
                throw new System.NotImplementedException();
            }

            public void Dispose()
            {
                mCurrent.Dispose();
            }
        }

        public Dictionary<object, object> Collection = new Dictionary<object, object>();

        public IEnumerator<CKeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new Enumerator(Collection.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void AddObject(object key, object value)
        {
            Collection.Add(key, value);
        }

        public void Add(TKey key, TValue value)
        {
            Collection.Add((object)key, (object)value);
        }

        public int Count
        {
            get { return Collection.Count; }
        }

        public TValue this[TKey key]
        {
            get
            {
                if (Collection.ContainsKey((object)key))
                    return (TValue)Collection[(object)key];
                return default(TValue);
            }
            set { Collection[(object)key] = (object)value; }
        }

        public Dictionary<object, object> OriginCollection
        {
            get { return Collection; }
        }
    }
}