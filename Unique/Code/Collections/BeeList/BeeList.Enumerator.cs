//
///*********************************************************************
//created:    2014-10-24
//author:     lixianmin
//
//Copyright (C) - All Rights Reserved
//*********************************************************************/
//using System;
//using System.Collections;
//using System.Collections.Generic;
//
//namespace Unique.Collections
//{
//	partial class BeeList<T>
//	{
//		public struct Enumerator
//		{
//            public Enumerator(BeeList<T> list)
//			{
//				_list   = list;
//				_index  = 0;
//				_item   = default(T);
//                _version= _list._version;
//			}
//			
//			public bool MoveNext()
//			{
//                if(_version != _list._version)
//                {
//                    throw new InvalidOperationException("Invalid list version");
//                }
//
//				if(_index < _list._size)
//				{
//					_item   = _list._items[_index];
//					++_index;
//
//					return true;
//				}
//
//				_index  = _list._size + 1;
//				_item   = default(T);
//
//                return false;
//			}
//
//			public void Reset()
//			{
//                if(_version != _list._version)
//                {
//                    throw new InvalidOperationException("Invalid list version");
//                }
//
//				_index  = 0;
//				_item   = default(T);
//			}
//
//            public T Current
//			{
//				get 
//				{
//                    return _item;
//				}
//			}
//
//            private readonly BeeList<T> _list;
//			private int _index;
//			private T   _item;
//            private int _version;
//		}
//	}
//}