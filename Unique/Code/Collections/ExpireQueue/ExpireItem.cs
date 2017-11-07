
///*********************************************************************
//created:    2016-11-15
//author:     lixianmin

//Copyright (C) - All Rights Reserved
//*********************************************************************/
//using System;

//namespace Unique.Collections
//{
//	internal class ExpireItem
//	{
//		public static ExpireItem Spawn (float expireTime, object target)
//		{
//			var item = _cache.Count > 0 ? (_cache.PopBack() as ExpireItem) : new ExpireItem();
//			item._expireTime= expireTime;
//			item._target = target;

//			return item;
//		}

//		public static void Recycle (ExpireItem item)
//		{
//			if (null != item)
//			{
//				item._target = null;
//				_cache.PushBack(item);
//			}
//		}

//		public float GetExpireTime ()
//		{
//			return _expireTime;
//		}

//		public object GetTarget ()
//		{
//			return _target;
//		}

//		public override string ToString ()
//		{
//			return string.Format("expireTime={0}, target={1}"
//				, _expireTime.ToString("f2")
//				, _target);
//		}

//		private float _expireTime;
//		private object _target;

//		private static readonly Deque _cache = new Deque();
//	}
//}