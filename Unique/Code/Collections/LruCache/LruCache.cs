
/*********************************************************************
created:    2014-08-05
author:     lixianmin

   緩存類的正確使用方式：
1. CheckCache()		檢查緩存中是否存在需求數據，如果有，則按命中處理
2. LoadWebItem()	如果沒有，則需要加載資源
3. _InitCallback()	這個需要在回調方法與LoadWebItem()之下調用兩次，處
   理正常回調以及立馬回調兩種情況下的公共邏輯
4. 在加載的回調中要做如下幾件事情：
	1. 檢查cacheTable中是否已經有資源了，如果有使用cacheTable中的那一份，以保證大家都使用同一份；
	2. 如果沒有，將資源項加入到cacheTable中，因為這是資源項變為正確資源的第一時間；
	3. 同時外部函數可能也會由於再次加載資源而使用到cacheTable，因此必須在回調方法中第一時間加入；

Copyright (C) - All Rights Reserved
*********************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;

namespace Unique.Collections
{
	//[HeapDumpAttribute(HeapDumpFlags.DontDump)]
    public partial class LruCache<TKey, TValue> 
        : IDictionary<TKey, TValue>
        , ICollection<KeyValuePair<TKey, TValue>>
        , IEnumerable<KeyValuePair<TKey, TValue>>
        , IEnumerable
	{
        public LruCache (int capacity)
		{
			if ( capacity < 0 )
			{
				throw new ArgumentOutOfRangeException("capacity should be greater than 0.");
			}

            _dict       = new Hashtable(capacity);
            _cacheList  = CacheListBase<TKey, TValue>.Create();
            _capacity   = capacity;

            _lpfnOnItemRemoved = _OnItemRemoved;
        }
        
		public void Add (TKey key, TValue value)
		{
			if (null == key)
			{
				return;
			}

            var reuseEntry   = false;

            if (Count >= _capacity)
            {
                var entry    = _cacheList.GetEldestEntry();
                reuseEntry   = null != entry;
                if (reuseEntry)
                {
                    var item = entry.Value;

                    // remove old entry
                    {
                        var oldKey = item.Key;
                        _dict.Remove(oldKey);
                        _cacheList.RemoveEntry(entry);
                    }

                    // add new entry
                    {
                        item.Key    = key;
                        item.Value  = value;
                        
						_AddToDict(key, entry);
                        _cacheList.AddEntry(entry);
                    }
                }
            }
            
            if (!reuseEntry)
            {
                var entry = _cacheList.CreateEntry(key, value);
				_AddToDict(key, entry);
                _cacheList.AddEntry(entry);
            }

            ++_addCount;
		}

		private void _AddToDict (TKey key, LinkedListNode<CacheListBase<TKey, TValue>.CacheItem> value)
		{
			try 
			{
				_dict.Add(key, value);
			}
			catch (Exception ex)
			{
				var oldValue = _dict[key] as LinkedListNode<CacheListBase<TKey, TValue>.CacheItem>;
				Console.Error.WriteLine("[LruCache._AddToDict()] key={0}, value={1}, oldValue={2}, ex={3}"
				                        , key
				                        , value.Value.Value
				                        , oldValue.Value.Value
				                        , ex.ToStringEx());
				throw;
			}
		}

		public void Clear ()
		{
            if (Count > 0)
            {
				var iter = _dict.GetEnumerator();
				while (iter.MoveNext())
				{
					var v = (iter.Value as LinkedListNode<CacheListBase<TKey, TValue>.CacheItem>).Value.Value;

					var disposable = v as IDisposable;
					if (null == disposable)
					{
						break;
					}

					disposable.Dispose();
				}

                _dict.Clear();
                _cacheList.Clear();
            }
		}

		public bool Remove (TKey key)
		{
			if (null == key)
			{
				return false;
			}

            var objEntry = _dict[key];
			if (null == objEntry)
			{
				return false;
			}

			var entry = objEntry as LinkedListNode<CacheListBase<TKey, TValue>.CacheItem>;
			_cacheList.RemoveEntry(entry);
			_dict.Remove(key);
			
			return true;
		}

		public bool TryGetValue (TKey key, out TValue value)
		{
			CacheListBase<TKey, TValue>.CacheItem item;
			if (null != key && _TryGetCacheItem(key, out item))
			{
				value = item.Value;
				return true;
			}

            value = default(TValue);
            return false;
		}
        
        public bool ContainsKey (TKey key)
        {
			CacheListBase<TKey, TValue>.CacheItem item;
			var contains = null != key && _TryGetCacheItem(key, out item);
			return contains;
        }

        public void TrimExcess ()
        {
            _cacheList.TrimExcess(_lpfnOnItemRemoved);
        }

        private void _OnItemRemoved (CacheListBase<TKey, TValue>.CacheItem item)
        {
            _dict.Remove(item.Key);
        }

        public TValue this [TKey key]
        {
            get
            {
				CacheListBase<TKey, TValue>.CacheItem item;
				if (null != key && _TryGetCacheItem(key, out item))
				{
					return item.Value;
				}

				throw new KeyNotFoundException("key =" + key);
            }
            
            set 
            {
                if (null == key)
                {
                    throw new ArgumentNullException("key is null");
                }

				CacheListBase<TKey, TValue>.CacheItem item;
				if (_TryGetCacheItem(key, out item))
				{
					item.Value = value;
				}
				else 
				{
					Add(key, value);
				}
            }
        }

		private bool _TryGetCacheItem (TKey key, out CacheListBase<TKey, TValue>.CacheItem item)
        {
			if (null == key)
			{
				item = null;
				return false;
			}

			var entry = _dict[key] as LinkedListNode<CacheListBase<TKey, TValue>.CacheItem>;
			if (null == entry)
			{
				item = null;
				return false;
			}

			_cacheList.SetRecentEntry(entry);
			item = entry.Value;
			++_hitCount;
			
			return true;
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator ()
        {
            return _GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator ()
        {
            return _GetEnumerator();
        }

        private IEnumerator<KeyValuePair<TKey, TValue>> _GetEnumerator ()
        {
            var iter = _dict.GetEnumerator();
            while (iter.MoveNext())
            {
                var k = (TKey) iter.Key;
                var v = (iter.Value as LinkedListNode<CacheListBase<TKey, TValue>.CacheItem>).Value.Value;

                var pair = new KeyValuePair<TKey, TValue>(k, v);
                yield return pair;
            }
        }

        public override string ToString ()
        {
            return string.Format("[LruCache: Count={0}, _capacity={1}, HitRate ={2}, _cacheList={3}]"
                                 , Count, _capacity, HitRate , _cacheList );
        }

        public int      Count       { get { return _dict.Count; } }
        public float    HitRate     { get { return _hitCount > 0 ? (float) _hitCount / (_hitCount + _addCount) : 0.0f; } }

        private int _capacity;
        private int _hitCount;
        private int _addCount;

        private readonly Hashtable _dict;
        private readonly CacheListBase<TKey, TValue> _cacheList;
        private readonly Action<CacheListBase<TKey, TValue>.CacheItem> _lpfnOnItemRemoved;
	}
}
