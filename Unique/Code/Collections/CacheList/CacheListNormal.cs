
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
    internal class CacheListNormal<TKey, TValue>: CacheListBase<TKey, TValue>
	{
        public override LinkedListNode<CacheItem> GetEldestEntry ()
        {
            return _linkedList.First;
        }

        public override void RemoveEntry (LinkedListNode<CacheItem> entry)
        {
            _linkedList.Remove(entry);
        }

        public override void Clear ()
        {
            if ( _linkedList.Count > 0)
            {
                _linkedList.Clear();
            }
        }

        public override void TrimExcess (Action<CacheItem> OnItemRemoved)
        {
            
        }
        
        public override string ToString ()
        {
            return "\n\n ".JoinEx(_linkedList);
        }
	}
}