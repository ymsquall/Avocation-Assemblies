
/*********************************************************************
created:    2014-08-16
author:     lixianmin

Copyright (C) - All Rights Reserved
*********************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;

namespace Unique.Collections
{
    internal abstract class CacheListBase<TKey, TValue>
	{
        public class CacheItem
        {
            public TKey     Key;
            public TValue   Value;
            
            public override string ToString()
            {
                return string.Format("[{0}, {1}]", Key, Value);
            }
        }

        public static CacheListBase<TKey, TValue> Create ()
        {
            var type = typeof(TValue);

            if (typeof(IIsDisposable).IsAssignableFrom(type))
            {
                return new CacheListCheckDisposable<TKey, TValue>();
            }

            return new CacheListNormal<TKey, TValue>();
        }

        public LinkedListNode<CacheItem> CreateEntry (TKey key, TValue value)
        {
            var item    = new CacheItem { Key = key, Value = value };
            var entry   = new LinkedListNode<CacheItem>(item);

            return entry;
        }

        public void AddEntry (LinkedListNode<CacheItem> entry)
        {
            _linkedList.AddLast(entry);
        }

        public void SetRecentEntry (LinkedListNode<CacheItem> entry)
        {
            if (entry != _linkedList.Last)
            {
                _linkedList.Remove(entry);
                _linkedList.AddLast(entry);
            }
        }

        public abstract LinkedListNode<CacheItem> GetEldestEntry ();
        public abstract void RemoveEntry (LinkedListNode<CacheItem> entry);
        public abstract void Clear ();
        public abstract void TrimExcess (Action<CacheItem> OnItemRemoved);

        protected readonly LinkedList<CacheItem> _linkedList = new LinkedList<CacheItem>();
	}
}