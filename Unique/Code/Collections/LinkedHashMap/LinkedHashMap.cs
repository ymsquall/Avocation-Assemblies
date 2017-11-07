
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
    public partial class LinkedHashMap<TKey, TValue> 
        : IDictionary<TKey, TValue>
        , ICollection<KeyValuePair<TKey, TValue>>
        , IEnumerable<KeyValuePair<TKey, TValue>>
        , IEnumerable
	{
        private class NodeData
        {
            public TKey     Key;
            public TValue   Value;

            public override string ToString()
            {
                return string.Format("[{0}, {1}]", Key, Value);
            }
        }

        public LinkedHashMap(): this(0)
        {

        }

        public LinkedHashMap(int capacity)
		{
			if(capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity should be greater than 0.");
			}

            _dict       = new Dictionary<TKey, object>(capacity);
            _linkedList = new LinkedList<NodeData>();
        }
        
		public void Add (TKey key, TValue value)
		{
            var data    = new NodeData { Key = key, Value = value };
            var node    = new LinkedListNode<NodeData>(data);

            _dict.Add(key, node);
            _linkedList.AddLast(node);

            ++_version;
		}

		public void Clear()
		{
            if(Count > 0)
            {
                _dict.Clear();
                _linkedList.Clear();

                ++_version;
            }
		}

		public bool Remove(TKey key)
		{
            object objNode;
            if(_dict.TryGetValue(key, out objNode))
            {
                var node   = objNode as LinkedListNode<NodeData>;
                _linkedList.Remove(node);
                _dict.Remove(key);

                ++_version;
                return true;
            }

            return false;
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
            var data  = _GetNodeData(key);
            if(null != data)
            {
                value = data.Value;
                return true;
            }

            value = default(TValue);
            return false;
		}
        
        public bool ContainsKey(TKey key)
        {
            var data    = _GetNodeData(key);
            var contains= null != data;
            return contains;
        }

        public TValue this[TKey key]
        {
            get 
            {
                var data = _GetNodeData(key);
                if(null != data)
                {
                    return data.Value;
                }
                
                throw new KeyNotFoundException("key =" + key);
            }
            
            set 
            {
                if(null == key)
                {
                    throw new ArgumentNullException("key is null");
                }

                var data = _GetNodeData(key);
                if(null != data)
                {
                    data.Value  = value;
                    ++_version;
                }
                else 
                {
                    Add(key, value);
                }
            }
        }

        private NodeData _GetNodeData(TKey key)
        {
            object objNode;
            if(null != key && _dict.TryGetValue(key, out objNode))
            {
                var node = objNode as LinkedListNode<NodeData>;
                var item = node.Value;
                return item;
            }

            return null;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return _GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _GetEnumerator();
        }

        private IEnumerator<KeyValuePair<TKey, TValue>> _GetEnumerator()
        {
            var iter = _linkedList.GetEnumerator();
            while(iter.MoveNext())
            {
                var data = iter.Current;
                yield return new KeyValuePair<TKey, TValue>(data.Key, data.Value);
            }
        }

        public override string ToString()
        {
            return string.Format("[LinkedHashMap: _dict.Count= {0}, _linkedList.Count= {1}, items= {2}]"
                                 , _dict.Count, _linkedList.Count, "\n\n ".JoinEx(_linkedList));
        }

        public int              Count       { get { return _linkedList.Count; } }
        public KeyList          Keys        { get { return new KeyList(this); } }
        public ValueList        Values      { get { return new ValueList(this); } }

        private int _version;

        private readonly Dictionary<TKey, object>   _dict;
        private readonly LinkedList<NodeData>       _linkedList;
	}
}