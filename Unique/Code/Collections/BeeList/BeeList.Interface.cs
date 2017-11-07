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
//        int IList.Add (object item)
//        {
//            try
//            {
//                Add ((T)((object)item));
//                return _size - 1;
//            }
//            catch (NullReferenceException)
//            {
//            }
//            catch (InvalidCastException)
//            {
//            }
//
//            throw new ArgumentException ("item");
//        }
//
//        bool IList.Contains (object item)
//        {
//            try
//            {
//                return Contains ((T)((object)item));
//            }
//            catch (NullReferenceException)
//            {
//            }
//            catch (InvalidCastException)
//            {
//            }
//            return false;
//        }
//
//        void ICollection.CopyTo (Array array, int arrayIndex)
//        {
//            if (array == null)
//            {
//                throw new ArgumentNullException ("array");
//            }
//
//            if (array.Rank > 1 || array.GetLowerBound (0) != 0)
//            {
//                throw new ArgumentException ("Array must be zero based and single dimentional", "array");
//            }
//
//            Array.Copy (_items, 0, array, arrayIndex, _size);
//        }
//
//        int IList.IndexOf (object item)
//        {
//            try
//            {
//                return IndexOf ((T)((object)item));
//            }
//            catch (NullReferenceException)
//            {
//            }
//            catch (InvalidCastException)
//            {
//            }
//
//            return -1;
//        }
//
//        void IList.Insert (int index, object item)
//        {
//            _CheckIndex (index);
//
//            try
//            {
//                Insert (index, (T)((object)item));
//                return;
//            }
//            catch (NullReferenceException)
//            {
//            }
//            catch (InvalidCastException)
//            {
//            }
//
//            throw new ArgumentException ("item");
//        }
//
//        void IList.Remove (object item)
//        {
//            try
//            {
//                Remove ((T)((object)item));
//            }
//            catch (NullReferenceException)
//            {
//            }
//            catch (InvalidCastException)
//            {
//            }
//        }
//
//        object IList.this [int index]
//        {
//            get
//            {
//                return this [index];
//            }
//            set
//            {
//                try
//                {
//                    this [index] = (T)((object)value);
//                    return;
//                }
//                catch (NullReferenceException)
//                {
//                }
//                catch (InvalidCastException)
//                {
//                }
//
//                throw new ArgumentException ("value");
//            }
//        }
//
//
//        bool IList.IsFixedSize
//        {
//            get
//            {
//                return false;
//            }
//        }
//
//        bool IList.IsReadOnly
//        {
//            get
//            {
//                return false;
//            }
//        }
//
//        bool ICollection<T>.IsReadOnly
//        {
//            get
//            {
//                return false;
//            }
//        }
//
//        bool ICollection.IsSynchronized
//        {
//            get
//            {
//                return false;
//            }
//        }
//
//        object ICollection.SyncRoot
//        {
//            get
//            {
//                return this;
//            }
//        }
//
//
////        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> pair)
////        {
////            Add(pair.Key, pair.Value);
////        }
////        
////        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> pair)
////        {
////            int index = IndexOfKey(pair.Key);
////            return index >= 0 && EqualityComparer<TValue>.Default.Equals(_values[index], pair.Value);
////        }
////        
////        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> pair)
////        {
////            int index = IndexOfKey(pair.Key);
////            if(index >= 0 && EqualityComparer<TValue>.Default.Equals(_values[index], pair.Value))
////            {
////                RemoveAt(index);
////                return true;
////            }
////            
////            return false;
////        }
////        
////        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
////        {
////            if(null == array)
////            {
////                throw new ArgumentNullException("array is null");
////            }
////            
////            if(arrayIndex < 0 || arrayIndex > array.Length)
////            {
////                var text = string.Format("ArrayIndex = {0}, array.Length={1}", arrayIndex, array.Length);
////                throw new ArgumentOutOfRangeException(text);
////            }
////            
////            if(array.Length - arrayIndex < Count)
////            {
////                var text = string.Format("ArrayIndex = {0}, array.Length={1}", arrayIndex, array.Length);
////                throw new ArgumentException(text);
////            }
////            
////            for(int i= 0; i < Count; ++i)
////            {
////                var pair = new KeyValuePair<TKey, TValue>(_keys[i], _values[i]);
////                array[arrayIndex + i] = pair;
////            }
////        }
////
////        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
////        { 
////            get 
////            { 
////                return false;
////            }
////        }
//
//	}
//}