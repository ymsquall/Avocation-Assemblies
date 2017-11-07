//using System;
//using System.Collections.Generic;

//namespace Framework.Tools
//{
//    /// <summary>
//    /// http://www.cnblogs.com/mgen/archive/2011/09/19/2181212.html
//    /// </summary>
//    /// <typeparam name="TKey1"></typeparam>
//    /// <typeparam name="TKey2"></typeparam>
//    public class TwoKeyDictionary<TKey1, TKey2> : IEnumerable<KeyValuePair<TKey1, TKey2>>, ICloneable
//    {
//        #region 字段
//        Dictionary<TKey1, TKey2> dic1;
//        Dictionary<TKey2, TKey1> dic2;

//        #endregion

//        #region 公共属性

//        public IEqualityComparer<TKey1> Comparer1 { get; private set; }
//        public IEqualityComparer<TKey2> Comparer2 { get; private set; }

//        public int Count
//        { get { return dic1.Count; } }

//        public ICollection<TKey1> Keys1
//        {
//            get { return dic1.Keys; }
//        }

//        public ICollection<TKey2> Keys2
//        {
//            get { return dic2.Keys; }
//        }

//        #endregion

//        #region 构造函数
//        public TwoKeyDictionary()
//            : this(0)
//        { }
//        public TwoKeyDictionary(int capacity)
//            : this(0, null, null)
//        { }
//        public TwoKeyDictionary(IEqualityComparer<TKey1> comp1, IEqualityComparer<TKey2> comp2)
//            : this(0, comp1, comp2)
//        { }
//        public TwoKeyDictionary(int capacity, IEqualityComparer<TKey1> comp1, IEqualityComparer<TKey2> comp2)
//        {
//            Initialize(capacity, comp1, comp2);
//        }
//        public TwoKeyDictionary(TwoKeyDictionary<TKey1, TKey2> copy)
//        {
//            if (copy == null)
//                throw new ArgumentNullException("copy");
//            Initialize(copy.Count, copy.Comparer1, copy.Comparer2);
//            foreach (var pair in copy)
//            {
//                dic1.Add(pair.Key, pair.Value);
//                dic2.Add(pair.Value, pair.Key);
//            }
//        }

//        #endregion

//        #region 私有函数

//        void Initialize(int capacity, IEqualityComparer<TKey1> comp1, IEqualityComparer<TKey2> comp2)
//        {
//            Comparer1 = comp1 ?? EqualityComparer<TKey1>.Default;
//            Comparer2 = comp2 ?? EqualityComparer<TKey2>.Default;
//            dic1 = Allocate<TKey1, TKey2>(capacity, Comparer1);
//            dic2 = Allocate<TKey2, TKey1>(capacity, Comparer2);
//        }

//        void CheckKeys(TKey1 key1, TKey2 key2)
//        {
//            var res = ContainsPair(key1, key2);
//            if (res.Item1 || res.Item2)
//                throw new ArgumentException("相同的键已存在");
//        }

//        void CheckKey1(TKey1 key)
//        {
//            if (ContainsKey1(key))
//                throw new ArgumentException("相同的键1值已存在");
//        }

//        void CheckKey2(TKey2 key)
//        {
//            if (ContainsKey2(key))
//                throw new ArgumentException("相同的键2值已存在");
//        }

//        #endregion

//        #region 公共函数

//        public bool ContainsKey1(TKey1 key)
//        {
//            return dic1.ContainsKey(key);
//        }
//        public bool ContainsKey2(TKey2 key)
//        {
//            return dic2.ContainsKey(key);
//        }
//        public Tuple<bool, bool, bool> ContainsPair(TKey1 key1, TKey2 key2)
//        {
//            var f1 = ContainsKey1(key1);
//            var f2 = ContainsKey2(key2);
//            bool f3 = false;
//            if (f1 && f2 && dic2.Comparer.Equals(key2, dic1[key1]))
//                f3 = true;
//            return new Tuple<bool, bool, bool>(f1, f2, f3);
//        }

//        public void Add(TKey1 key1, TKey2 key2)
//        {
//            CheckKeys(key1, key2);
//            dic1.Add(key1, key2);
//            dic2.Add(key2, key1);
//        }

//        public void EditKey1(TKey1 key, TKey2 val)
//        {
//            CheckKey2(val);
//            TKey2 oldval = dic1[key];
//            dic1[key] = val;
//            ReplaceKey(dic2, oldval, val);
//        }

//        public void EditKey2(TKey2 key, TKey1 val)
//        {
//            CheckKey1(val);
//            TKey1 oldval = dic2[key];
//            dic2[key] = val;
//            ReplaceKey(dic1, oldval, val);
//        }

//        public TKey2 GetValueFromKey1(TKey1 key)
//        {
//            return dic1[key];
//        }

//        public TKey1 GetValueFromKey2(TKey2 key)
//        {
//            return dic2[key];
//        }

//        public KeyValuePair<TKey1, TKey2>? GetPairFromKey1(TKey1 key)
//        {
//            if (ContainsKey1(key))
//                return new KeyValuePair<TKey1, TKey2>(key, dic1[key]);
//            return null;
//        }

//        public KeyValuePair<TKey1, TKey2>? GetPairFromKey2(TKey2 key)
//        {
//            if (ContainsKey2(key))
//                return new KeyValuePair<TKey1, TKey2>(dic2[key], key);
//            return null;
//        }

//        public bool RemoveFromKey1(TKey1 key)
//        {
//            if (ContainsKey1(key))
//            {
//                dic2.Remove(dic1[key]);
//                dic1.Remove(key);
//                return true;
//            }
//            return false;
//        }

//        public bool RemoveFromKey2(TKey2 key)
//        {
//            if (ContainsKey2(key))
//            {
//                dic1.Remove(dic2[key]);
//                dic2.Remove(key);
//                return true;
//            }
//            return false;
//        }

//        public void Clear()
//        {
//            dic1.Clear();
//            dic2.Clear();
//        }

//        public object Clone()
//        {
//            return new TwoKeyDictionary<TKey1, TKey2>(this);
//        }

//        #endregion

//        #region 静态辅助函数
//        static Dictionary<K, V> Allocate<K, V>(int capacity, IEqualityComparer<K> comp)
//        {
//            return new Dictionary<K, V>(capacity, comp);
//        }

//        static void ReplaceKey<K, V>(IDictionary<K, V> dic, K oldkey, K newkey)
//        {
//            V val = dic[oldkey];
//            dic.Remove(oldkey);
//            dic.Add(newkey, val);
//        }

//        #endregion

//        #region IEnumerable 成员
//        public IEnumerator<KeyValuePair<TKey1, TKey2>> GetEnumerator()
//        {
//            return dic1.GetEnumerator();
//        }

//        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
//        {
//            return GetEnumerator();
//        }

//        #endregion

//    }
//}
