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
//	public partial class BeeList<T>
//              : IEnumerable<T>
//              , ICollection<T>
//              , IList<T>
//              , IEnumerable
//              , ICollection
//              , IList
//	{
//		public BeeList()
//		{
//            _items  = _emptyItems;
//        }
//
//		public BeeList(int capacity)
//		{
//			if(capacity < 0)
//			{
//				throw new ArgumentOutOfRangeException("capacity should be greater than 0.");
//			}
//
//            _items  = new T[capacity];
//		}
//
//        public BeeList (IEnumerable<T> collection)
//        {
//            if (collection == null)
//            {
//                throw new ArgumentNullException ("collection");
//            }
//
//            var collection2 = collection as ICollection<T>;
//
//            if (collection2 == null)
//            {
//                _items = _emptyItems;
//                _AddEnumerable(collection);
//            }
//            else
//            {
//                _size = collection2.Count;
//                _items = new T[Math.Max (_size, 4)];
//                collection2.CopyTo(_items, 0);
//            }
//        }
// 
//		public void Add(T item)
//		{
//            Reserve(_size + 1);
//
//            _items [_size++] = item;
//            _version++;
//		}
//
//        public void AddRange (IEnumerable<T> collection)
//        {
//            if (collection == null)
//            {
//                throw new ArgumentNullException ("collection");
//            }
//
//            ICollection<T> collection2 = collection as ICollection<T>;
//
//            if (collection2 != null)
//            {
//                _AddCollection (collection2);
//            }
//            else
//            {
//                _AddEnumerable (collection);
//            }
//
//            _version++;
//        }
//
//        public void Clear()
//        {
//            Array.Clear(_items, 0, _size);
//            _size = 0;
//            ++_version;
//        }
//        
//        public bool Contains(T item)
//        {
//            return Array.IndexOf(_items, item, 0, _size) != -1;
//        }
//
//        public void CopyTo (T[] array)
//        {
//            Array.Copy (_items, 0, array, 0, _size);
//        }
//
//        public void CopyTo (T[] array, int arrayIndex)
//        {
//            Array.Copy (_items, 0, array, arrayIndex, _size);
//        }
//
//        public void CopyTo (int index, T[] array, int arrayIndex, int count)
//        {
//            _CheckRange (index, count);
//            Array.Copy (_items, index, array, arrayIndex, count);
//        }
//
//        public void ForEach (Action<T> action)
//        {
//            if (action == null)
//            {
//                throw new ArgumentNullException ("action");
//            }
//
//            for (int i = 0; i < _size; i++)
//            {
//                action (_items [i]);
//            }
//        }
//
//        public Enumerator GetEnumerator()
//        {
//            return new Enumerator(this);
//        }
//        
//        IEnumerator<T> IEnumerable<T>.GetEnumerator()
//        {
//            return _GetEnumerator();
//        }
//        
//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return _GetEnumerator();
//        }
//        
//        private IEnumerator<T> _GetEnumerator()
//        {
//            for(int i= 0; i< _size; ++i)
//            {
//                yield return _items[i];
//            }
//        }
//
//        public int IndexOf (T item)
//        {
//            return Array.IndexOf (_items, item, 0, _size);
//        }
//
//        public int IndexOf (T item, int index)
//        {
//            _CheckIndex(index);
//            return Array.IndexOf (_items, item, index, _size - index);
//        }
//
//        public void Insert (int index, T item)
//        {
//            _CheckIndex (index);
//            Reserve(_size + 1);
//
//            _Shift (index, 1);
//            _items [index] = item;
//            _version++;
//        }
//
//        public bool Remove(T item)
//        {
//            int index = IndexOf(item);
//            if(index >= 0)
//            {
//                RemoveAt(index);
//                return true;
//            }
//            
//            return false;
//        }
//
//        public void RemoveAt(int index)
//        {
//            _CheckIndex(index);
//            --_size;
//
//            if(index < _size)
//            {
//                Array.Copy(_items, index + 1, _items, index, _size - index);
//            }
//            
//            _items[_size] = default(T);
//            ++_version;
//        }
//
//        public void Reserve (int minCapacity)
//        {
//			var currentCapacity = _items.Length;
//			if (minCapacity > currentCapacity)
//            {
//				Capacity = ArrayTools.EnsureCapacity(currentCapacity, minCapacity);
//            }
//        }
//
//        public void Sort (IComparer<T> comparer)
//        {
//            Array.Sort (_items, 0, _size, comparer);
//            _version++;
//        }
//
//        public void Sort ()
//        {
//            Array.Sort (_items, 0, _size);
//            _version++;
//        }
//
//        public void Sort (int index, int count, IComparer<T> comparer)
//        {
//            _CheckRange (index, count);
//            Array.Sort (_items, index, count, comparer);
//            _version++;
//        }
//
//        public T[] ToArray()
//        {
//            var array = new T[_size];
//            Array.Copy(_items, array, _size);
//
//            return array;
//        }
//
//        public void TrimExcess ()
//        {
//            Capacity = _size;
//        }
//
//        public T this[int index]
//        {
//            get
//            {
//                // Fllowing trick can reduce the range check by one
//                if ((uint) index >= (uint) _size)
//                {
//                    throw new ArgumentOutOfRangeException ("index");
//                }
//
//                return _items[index];
//            }
//
//            set
//            {
//                // Fllowing trick can reduce the range check by one
//                if ((uint) index >= (uint) _size)
//                {
//                    throw new ArgumentOutOfRangeException ("index");
//                }
//
//                _items [index] = value;
//                _version++;
//            }
//        }
//
//        private void _AddCollection (ICollection<T> collection)
//        {
//            int count = collection.Count;
//            if (count == 0)
//            {
//                return;
//            }
//
//            Reserve(_size + count);
//            collection.CopyTo (_items, _size);
//            _size += count;
//        }
//
//        private void _AddEnumerable (IEnumerable<T> enumerable)
//        {
//            foreach (T current in enumerable)
//            {
//                Add (current);
//            }
//        }
//
//        private void _CheckIndex (int index)
//        {
//            if (index < 0 || index > _size)
//            {
//                ThrowHelper.ThrowArgumentOutOfRangeException("index is out of range, index={0}, _size ={1}"
//                                                             , index.ToString(), _size.ToString());
//            }
//        }
//
//        private void _CheckRange (int idx, int count)
//        {
//            if (idx < 0)
//            {
//                throw new ArgumentOutOfRangeException ("index");
//            }
//
//            if (count < 0)
//            {
//                throw new ArgumentOutOfRangeException ("count");
//            }
//
//            if (idx + count > _size)
//            {
//                throw new ArgumentException ("index and count exceed length of list");
//            }
//        }
//
//        private void _Shift (int start, int delta)
//        {
//            if (delta < 0)
//            {
//                start -= delta;
//            }
//
//            if (start < _size)
//            {
//                Array.Copy (_items, start, _items, start + delta, _size - start);
//            }
//
//            _size += delta;
//
//            if (delta < 0)
//            {
//                Array.Clear (_items, _size, -delta);
//            }
//        }
//
//        public int Capacity
//        {
//            get
//            {
//                return _items.Length;
//            }
//
//            set
//            {
//                if (value < this._size)
//                {
//                    throw new ArgumentOutOfRangeException ();
//                }
//
//                Array.Resize<T>(ref _items, value);
//            }
//        }
//
//        public override string ToString()
//        {
//            var sb = new System.Text.StringBuilder(32);
//            sb.Append("Count= ");
//            sb.Append(Count);
//            sb.Append(", Capacity= ");
//            sb.Append(Capacity);
//
//            sb.Append("\n items=[");
//            sb.Append(", ".JoinEx(this));
//            sb.Append("]");
//
//            var text = sb.ToString();
//            return text;
//        }
//
//		public int Count { get { return _size; } }
//
//		private T[] _items;
//		private int _size;
//		private int _version;
//
//		private static readonly T[] _emptyItems = new T[0];
//	}
//}