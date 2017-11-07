//
///*********************************************************************
//created:    2015-02-22
//author:     lixianmin
//
//Copyright (C) - All Rights Reserved
//*********************************************************************/
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//namespace Unique.Collections
//{
//    public class AgeCostCache<T> where T : ISize
//	{
//        struct CacheItem
//        {
//            public string path;
//            public float accessTime;
//            public T data;
//
//            public override string ToString ()
//            {
//                return string.Format("[path={0}, accessTime={1}, cost={2}, node={3}]"
//                                     , path, accessTime, (Time.time- accessTime) * data.size, data);
//            }
//        }
//
//        public AgeCostCache (int capacity)
//		{
//			if ( capacity < 0 )
//			{
//				throw new ArgumentOutOfRangeException("capacity should be greater than 0.");
//			}
//
//            _capacity = capacity;
//            _cacheItems.ReserveEx(capacity);
//        }
//
//        public T AddEx (string path, T node)
//        {
//            Add(path, node);
//            return node;
//        }
//
//        public void Add (string path, T node)
//		{
//            if (null != node)
//            {
//                path = path ?? string.Empty;
//                var count = Count;
//                var current = Time.time;
//
//                var reusedIndex = -1;
//                
//                if (count >= _capacity)
//                {
//                    var reusedWeight = -1.0f;
//                    
//                    for (int index= 0; index< count; ++index)
//                    {
//                        var item = _cacheItems[index];
//
//                        if (item.path == path)
//                        {
//                            reusedIndex = index;
//                            break;
//                        }
//                        
//                        if (!_IsDisposable(item))
//                        {
//                            continue;
//                        }
//                        
//                        var weight = (current - item.accessTime) * item.data.size;
//                        
//                        if (weight > reusedWeight)
//                        {
//                            reusedWeight = weight;
//                            reusedIndex = index;
//                        }
//                    }
//                }
//
//                var cacheItem = new CacheItem { path = path, accessTime= current, data= node};
//
//                if (reusedIndex >= 0)
//                {
//                    var oldData = _cacheItems[reusedIndex].data as IDisposable;
//                    if (null != oldData)
//                    {
//                        oldData.Dispose();
//                    }
//
//                    _cacheItems[reusedIndex] = cacheItem;
//                }
//                else
//                {
//                    _cacheItems.Add(cacheItem);
//                }
//            }
//		}
//
//		public void Clear ()
//		{
//            if (Count > 0)
//            {
//                _cacheItems.Clear();
//            }
//		}
//
//		public bool Remove (string path)
//		{
//            path = path ?? string.Empty;
//            var count = _cacheItems.Count;
//
//            for (int i= 0; i< count; ++i)
//            {
//                var item = _cacheItems[i];
//                if (item.path == path)
//                {
//                    var idxLast = count - 1;
//                    _cacheItems[i] = _cacheItems[idxLast];
//                    _cacheItems.RemoveAt(idxLast);
//                    return true;
//                }
//            }
//
//            return false;
//		}
//
//        public T GetEx (string path)
//        {
//            T node;
//            TryGetValue(path, out node);
//            return node;
//        }
//
//        public bool TryGetValue (string path, out T node)
//		{
//            path = path ?? string.Empty;
//
//            var count = _cacheItems.Count;
//            for (int i= 0; i< count; ++i)
//            {
//                var item = _cacheItems[i];
//                if (item.path == path)
//                {
//                    node = item.data;
//                    _cacheItems[i] = new CacheItem{ path= path, accessTime= Time.time, data= node };
//                    return true;
//                }
//            }
//
//            node = default(T);
//            return false;
//		}
//
//        private bool _IsDisposable (CacheItem item)
//        {
//            var checkable = item.data as IIsDisposable;
//            return null == checkable || checkable.IsDisposable();
//        }
//
//        public override string ToString ()
//        {
//            return string.Format("[AgeCostCache: Count={0}, _cacheItems={1}]", Count, "\n\n".JoinEx(_cacheItems));
//        }
//
//        public int  Count       { get { return _cacheItems.Count; } }
//
//        private int _capacity;
//
//        private readonly List<CacheItem> _cacheItems = new List<CacheItem>();
//	}
//}