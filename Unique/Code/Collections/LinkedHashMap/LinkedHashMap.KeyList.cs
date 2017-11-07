
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
        public struct KeyList: IEnumerable<TKey>, IEnumerable
        {
            public KeyList(LinkedHashMap<TKey, TValue> map)
            {
                _map = map;
            }

			public void ForEach(Action<TKey> action)
			{
				if(null != action)
				{
                    var iter = new KeyEnumerator(_map);
                    while(iter.MoveNext())
                    {
                        action(iter.Current);
                    }
				}
			}

            public KeyEnumerator GetEnumerator()
            {
                return new KeyEnumerator(_map);
            }

            IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
            {
				return _GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
				return _GetEnumerator();
            }

			private IEnumerator<TKey> _GetEnumerator()
			{
                var iter = new KeyEnumerator(_map);
                while(iter.MoveNext())
                {
                    yield return iter.Current;
                }
			}

            private readonly LinkedHashMap<TKey, TValue> _map;
        }

		public struct KeyEnumerator
		{
			public KeyEnumerator(LinkedHashMap<TKey, TValue> map)
			{
				_map = map;
				_key = default(TKey);
				_version = _map._version;

                _currentNode = map._linkedList.First;
            }
			
            public bool MoveNext()
            {
                if(_version != _map._version)
                {
                    throw new InvalidOperationException("map modified");
                }

                if(null != _currentNode)
                {
                    var node = _currentNode as LinkedListNode<NodeData>;
                    _key = node.Value.Key;
                    _currentNode = node.Next;

                    return true;
                }
                
                _key    = default(TKey);
                return false;
            }
			
			public TKey Current
			{
				get 
				{
					return _key;
				}
			}
			
            private readonly LinkedHashMap<TKey, TValue> _map;
            // we can not use LinkedList<NodeData> directly, the command line compiler will report cs0584:
            // Internal compiler error: Unexpected error when loading type `Unique.Collections.LinkedHashMap<TKey,TValue>.KeyEnumerator'
            // so we use object as its type.
            private object _currentNode;
            
            private TKey    _key;
            private int     _version;
		}

	}
}