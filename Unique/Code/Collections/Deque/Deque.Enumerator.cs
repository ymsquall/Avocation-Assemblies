
///*********************************************************************
//created:    2016-02-05
//author:     lixianmin

//Copyright (C) - All Rights Reserved
//*********************************************************************/
//using System;
//using System.Collections;

//namespace Unique.Collections
//{
//	partial class Deque
//	{
//		public struct Enumerator
//		{
//			public Enumerator (Deque queue)
//			{
//				_queue  	= queue;
//				_version	= queue._version;
//				_current 	= -1;
//			}
			
//			public bool MoveNext ()
//			{
//				if (_version != _queue._version)
//				{
//					throw new InvalidOperationException("Invalid queue version");
//				}

//				if (_current >= _queue._size - 1)
//				{
//					_current = -1;
//					return false;
//				}

//				_current++;
//				return true;
//			}
			
//			public void Reset ()
//			{
//				if (_version != _queue._version)
//				{
//					throw new InvalidOperationException ();
//				}

//				_current = -1;
//			}
			
//			public object Current
//			{
//				get 
//				{
//					if (_version != _queue._version || _current < 0 || _current >= _queue._size)
//					{
//						throw new InvalidOperationException ();
//					}

//					return _queue._items [(_queue._head + _current) % _queue._capacity];
//				}
//			}
			
//			private readonly Deque _queue;
//			private int		_current;
//			private int 	_version;
//		}
//	}
//}