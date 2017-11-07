
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
        public struct ValueList: IEnumerable<TValue>, IEnumerable
        {
            public ValueList(LinkedHashMap<TKey, TValue> map)
            {
                _map = map;
            }
            
            public void ForEach(Action<TValue> action)
            {
                if(null != action)
                {
                    var iter = new ValueEnumerator(_map);
                    while(iter.MoveNext())
                    {
                        action(iter.Current);
                    }
                }
            }
            
            public ValueEnumerator GetEnumerator()
            {
                return new ValueEnumerator(_map);
            }
            
            IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
            {
                return _GetEnumerator();
            }
            
            IEnumerator IEnumerable.GetEnumerator()
            {
                return _GetEnumerator();
            }
            
            private IEnumerator<TValue> _GetEnumerator()
            {
                var iter = new ValueEnumerator(_map);
                while(iter.MoveNext())
                {
                    yield return iter.Current;
                }
            }
            
            private readonly LinkedHashMap<TKey, TValue> _map;
        }

		public struct ValueEnumerator
		{
            public ValueEnumerator(LinkedHashMap<TKey, TValue> map)
            {
                _map     = map;
                _value   = default(TValue);
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
                    _value   = node.Value.Value;
                    _currentNode = node.Next;

                    return true;
                }
                
                _value    = default(TValue);
                return false;
            }
            
            public TValue Current
            {
                get 
                {
                    return _value;
                }
            }
            
            private readonly LinkedHashMap<TKey, TValue> _map;
            // we can not use LinkedList<NodeData> directly, the command line compiler will report cs0584:
            // Internal compiler error: Unexpected error when loading type `Unique.Collections.LinkedHashMap<TKey,TValue>.ValueEnumerator'
            // so we use object as its type.
            private object _currentNode;
            
            private TValue  _value;
            private int     _version;
		}
	}
}