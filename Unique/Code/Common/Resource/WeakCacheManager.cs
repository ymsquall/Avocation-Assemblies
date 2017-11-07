
///********************************************************************
//created:    2016-03-17
//author:     lixianmin

//Copyright (C) - All Rights Reserved
//*********************************************************************/

//using System;
//using System.Collections.Generic;
//using Unique.Collections;

//namespace Unique
//{
//	using CacheTable = Unique.Collections.SortedTable<string, object>;

//    internal class WeakCacheManager
//    {
//		private struct ExpireItem
//		{
//			public float expireTime;
//			public object target;

//			public override string ToString ()
//			{
//				return string.Format("expireTime={0}, target={1}"
//				                     , expireTime.ToString("f2")
//				                     , target);
//			}
//		}

//		public void AddItem (string key, object target)
//		{
//			if (string.IsNullOrEmpty(key) || null == target)
//			{
//				return;
//			}

//			object cachedWeak;
//			var index = _cacheTable.TryIndexValue(key, out cachedWeak);
//			var weak = cachedWeak as WeakReference;

//			if (null != weak)
//			{
//				weak.Target = target;
//			}
//			else if (index < 0)
//			{
//				weak = _CreateOneWeak(target);
//				_cacheTable.InsertByIndex(~index, key, weak);
//			}

//			_SetExpireTime(key, target);
//		}

//		public object GetItem (string key)
//		{
//			if (!string.IsNullOrEmpty(key))
//			{
//				object cachedWeak;
//				_cacheTable.TryGetValue(key, out cachedWeak);
//				var weak = cachedWeak as WeakReference;

//				if (null != weak)
//				{
//					var target = weak.Target;
//					if (null != target)
//					{
//						_SetExpireTime(key, target);
//						return target;
//					}
//				}
//			}

//			return null;
//		}

////		internal void Tick ()
////		{
////			if (os.time > _nextCheckExpireTime && _expireItems.Count > 0)
////			{
////				_nextCheckExpireTime = os.time + 1.0f;
////				_expireItems.RemoveAll((_, expire) => os.time > expire.expireTime );
////
//////				var removedCount = _expireItems.RemoveAll((_, expire) => os.time > expire.expireTime );
//////				if (removedCount > 0)
//////				{
//////					Console.Warning.WriteLine(this);
//////				}
////			}
////		}

//		internal void Clear ()
//		{
//			if (_cacheTable.Count > 0)
//			{
//				foreach (WeakReference weak in _cacheTable.Values)
//				{
//					if (null == weak.Target)
//					{
//						_weaks.PushBack(weak);
//					}
//				}

//				_cacheTable.RemoveAll((_, weak)=> null == (weak as WeakReference).Target);
//			}
//		}

//		private WeakReference _CreateOneWeak (object target)
//		{
//			if (_weaks.Count > 0)
//			{
//				var weak = _weaks.PopBack() as WeakReference;
//				weak.Target = target;

//				return weak;
//			}

//			var result = new WeakReference(target);
//			return result;
//		}

//		private void _SetExpireTime (string key, object target)
//		{
////			var expire = new ExpireItem
////			{
////				expireTime = os.time + _delayedExpireTime,
////				target = target
////			};
////
////			_expireItems[key] = expire;
//		}
////
////		public override string ToString ()
////		{
////			var sbText = new System.Text.StringBuilder("[WeakCacheManager]");
////			sbText.Append("_expireItems=[");
////			sbText.Append("Count=");
////			sbText.Append(_expireItems.Count.ToString());
////			sbText.AppendLine();
////			sbText.Append("\n\n".JoinEx(_expireItems.Values));
////			sbText.Append("]");
////
////			return sbText.ToString();
////		}

//		public static readonly WeakCacheManager Instance = new WeakCacheManager();

//		private readonly CacheTable _cacheTable = new CacheTable();
//		private readonly Deque _weaks = new Deque();

////		private float _nextCheckExpireTime; 
////		private const float _delayedExpireTime = 10.0f;
////		private readonly SortedTable<string, ExpireItem> _expireItems = new SortedTable<string, ExpireItem>(128);
//    }
//}