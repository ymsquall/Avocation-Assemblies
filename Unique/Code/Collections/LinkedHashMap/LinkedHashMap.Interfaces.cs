
/*********************************************************************
created:    2014-08-11
author:     lixianmin

Copyright (C) - All Rights Reserved
*********************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;

namespace Unique.Collections
{
    partial class LinkedHashMap<TKey, TValue>
	{
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> pair)
        {
            Add(pair.Key, pair.Value);
        }
        
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> pair)
        {
            return ContainsKey(pair.Key);
        }
        
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> pair)
        {
            return Remove(pair.Key);
        }
        
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotSupportedException("CopyTo()");
        }
        
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        { 
            get 
            { 
                return false;
            }
        }
        
        ICollection<TKey> IDictionary<TKey, TValue>.Keys
        {
            get 
            {
                throw new NotSupportedException("Keys");
            }
        }
        
        ICollection<TValue> IDictionary<TKey, TValue>.Values
        {
            get 
            {
                throw new NotSupportedException("Values");
            }
        }
	}
}