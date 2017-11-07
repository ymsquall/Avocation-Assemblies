
///*********************************************************************
//created:    2016-02-05
//author:     lixianmin

//Copyright (C) - All Rights Reserved
//*********************************************************************/
//using System;
//using System.Collections;

//namespace Unique.Collections
//{
//	public partial class Deque : IEnumerable
//	{
//		public Deque () : this (0)
//		{

//		}

//		public Deque (int capacity)
//		{
//			capacity  = Math.Max(0, capacity);
//			_capacity = capacity;

//			if (capacity > 0)
//			{
//				_items = new object[capacity];
//			}
//			else
//			{
//				_items = EmptyArray<object>.Instance;
//			}
//		}

//		public void PushBack (object item)
//		{
//			_version++;

//			if (_size == _capacity)
//			{
//				_capacity = ArrayTools.EnsureCapacity(_capacity, _size + 1);
//				var items = new object[_capacity];
//				CopyTo(items, 0);

//				_items = items;
//				_head  = 0;
//				_tail  = _size;
//			}

//			_items[_tail] = item;
//			_tail = (_tail + 1) % _capacity;
//			_size++;
//		}

//		public object PopFront ()
//		{
//			_version++;

//			if (_size < 1)
//			{
//				throw new InvalidOperationException ();
//			}

//			var result = _items [_head];
//			_items[_head] = null;
//			_head = (_head + 1) % _capacity;
//			_size--;

//			return result;
//		}

//		public object PopBack ()
//		{
//			_version++;
			
//			if (_size < 1)
//			{
//				throw new InvalidOperationException ();
//			}

//			_tail = (_tail - 1 + _capacity) % _capacity;
//			var result = _items [_tail];
//			_items[_tail] = null;
//			_size--;
			
//			return result;
//		}

//		public object Front ()
//		{
//			if (_size < 1)
//			{
//				throw new InvalidOperationException ();
//			}

//			var result = _items [_head];
//			return result;
//		}

//		public object Back ()
//		{
//			if (_size < 1)
//			{
//				throw new InvalidOperationException ();
//			}

//			var index = (_tail - 1 + _capacity) % _capacity;
//			var result = _items[index];
//			return result;
//		}

//		public void CopyTo (Array array, int index)
//		{
//			if (array == null)
//			{
//				throw new ArgumentNullException ("array");
//			}

//			if (index < 0)
//			{
//				throw new ArgumentOutOfRangeException ("index");
//			}

//			if (array.Rank > 1 || (index != 0 && index >= array.Length) || this._size > array.Length - index)
//			{
//				throw new ArgumentException ();
//			}

//			int tailCount  = _items.Length - _head;
//			Array.Copy (_items, _head, array, index, Math.Min (_size, tailCount));

//			if (_size > tailCount)
//			{
//				Array.Copy (_items, 0, array, index + tailCount, _size - tailCount);
//			}
//		}

//		public object this [int index]
//		{
//			get
//			{
//				if (index >= 0 && index < _size)
//				{
//					var realIndex = (_head + index) % _capacity;
//					var result = _items[realIndex];
//					return result;
//				}

//				return null;
//			}
//		}

//		public bool Contains (object item)
//		{
//			if (null != _items && _size > 0)
//			{
//				for (int i= 0; i< _size; ++i)
//				{
//					var index = (_head + i) % _capacity;
//					if (object.Equals(_items[index], item))
//					{
//						return true;
//					}
//				}
//			}

//			return false;
//		}

//		public void RemoveAt (int index)
//		{
//			if (index < 0 || index >= _size)
//			{
//				throw new ArgumentOutOfRangeException("index is out of range.");
//			}
			
//			int beginIndex = _head + index;
//			int endIndex = _head + _size;

//			int p = beginIndex % _capacity;
//			int q = 0;

//			for (int i= beginIndex; i < endIndex; ++i)
//			{
//				q = p;
//				p = (i + 1) % _capacity;
//				_items[q] = _items[p];
//			}

//			_version++;
//			_size--;
//			_tail = q;
//			_items[q] = null;
//		}
		
//		public void Clear ()
//		{
//			_version++;

//			if (_size > 0)
//			{
//				Array.Clear(_items, 0, _capacity);
//			}

//			_head = 0;
//			_size = 0;
//			_tail = 0;
//		}

//		public Enumerator GetEnumerator()
//		{
//			return new Enumerator(this);
//		}
		
//		IEnumerator IEnumerable.GetEnumerator()
//		{
//			var lastVersion = _version;

//			for (int i= 0; i< _size; ++i)
//			{
//				var index = (_head + i) % _capacity;
//				var result = _items[index];
//				yield return result;

//				if (lastVersion != _version)
//				{
//					throw new InvalidOperationException("Invalid Deque version");
//				}
//			}
//		}
		
//		public override string ToString ()
//		{
//			var sb = new System.Text.StringBuilder(32);
//			sb.AppendFormat("Count={0}, _capacity={1}, _head={2}, _tail={3}"
//			                , Count, _capacity, _head, _tail);
			
//			sb.Append("\n     items=[");
//			sb.Append(", ".JoinEx(this));
//			sb.Append("]");
			
//			var text = sb.ToString();
//			return text;
//		}

//		public int Count { get { return _size; } }

//		private int _head;
//		private int _tail;	
//		private int _size;
//		private int _capacity;
//		private int _version;
//		private object[] _items;
//	}
//}