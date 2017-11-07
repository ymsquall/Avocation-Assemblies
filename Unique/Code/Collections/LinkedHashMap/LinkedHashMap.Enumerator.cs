
/*********************************************************************
created:    2014-05-26
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
		public struct Enumerator
		{
            public Enumerator(LinkedHashMap<TKey, TValue> map)
			{
				_map    = map;
                _version= _map._version;
                _key    = default(TKey);
                _value  = default(TValue);

                _currentNode = map._linkedList.First;
			}
			
			public bool MoveNext()
			{
                if(_version != _map._version)
                {
                    throw new InvalidOperationException("Invalid map version");
                }

                if(null != _currentNode)
                {
                    var node = _currentNode as LinkedListNode<NodeData>;
                    var inner= node.Value;
                    _key = inner.Key;
                    _value = inner.Value;
                    _currentNode = node.Next;

                    return true;
                }

				_key    = default(TKey);
				_value  = default(TValue);
				return false;
			}

            public KeyValuePair<TKey, TValue> Current
			{
				get 
				{
					return new KeyValuePair<TKey, TValue>(_key, _value);
				}
			}

            private readonly LinkedHashMap<TKey, TValue> _map;
            // we can not use LinkedList<NodeData> directly, the command line compiler will report cs0584:
            // Internal compiler error: Unexpected error when loading type `Unique.Collections.LinkedHashMap<TKey,TValue>.Enumerator'
            // so we use object as its type.
            private object _currentNode;

			private TKey    _key;
			private TValue  _value;
            private int     _version;
		}
	}
}