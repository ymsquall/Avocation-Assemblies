
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
    internal class CacheListCheckDisposable<TKey, TValue>: CacheListBase<TKey, TValue>
	{
        public override LinkedListNode<CacheItem> GetEldestEntry ()
        {
            var first   = _linkedList.First;

            if (null != first)
            {
                var current = first;

                while (!_IsDisposableItem(current.Value))
                {
                    _linkedList.Remove(current);
                    _linkedList.AddLast(current);

                    current = _linkedList.First;

                    if (current == first)
                    {
                        return null;
                    }
                }
                
                return current;
            }

            return null;
        }

        public override void RemoveEntry (LinkedListNode<CacheItem> entry)
        {
            _linkedList.Remove(entry);
            
            var item   = entry.Value;
            _CheckDisposeItem(item);
        }

        public override void Clear ()
        {
            if ( _linkedList.Count > 0)
            {
                _linkedList.ForEachEx(item=> _CheckDisposeItem(item));
                _linkedList.Clear();
            }
        }

        public override void TrimExcess (Action<CacheItem> OnItemRemoved)
        {
            var entry  = _linkedList.First;
			var list = new ArrayList();
            
            while (null != entry)
            {
                var next = entry.Next;
                var item = entry.Value;
                
				if (_IsDisposableItem(item))
				{
					_linkedList.Remove(entry);
					list.Add(item);
				}
                
                entry = next;
            }

			var count = list.Count;
			for (int i= 0; i< count; ++i)
			{
				var item = list[i] as CacheItem;
				var checkable = item.Value as IDisposable;
				checkable.Dispose();

				if (null != OnItemRemoved)
				{
					OnItemRemoved(item);
				}
			}
        }

        private static bool _CheckDisposeItem (CacheItem item)
        {
            var checkable = item.Value as IIsDisposable;
            
            if (checkable.IsDisposable())
            {
                (checkable as IDisposable).Dispose();
                return true;
            }

            return false;
        }

        private static bool _IsDisposableItem (CacheItem item)
        {
            var checkable = item.Value as IIsDisposable;

            return checkable.IsDisposable();
        }

        public override string ToString ()
        {
            return "\n\n ".JoinEx(_linkedList);
        }
	}
}