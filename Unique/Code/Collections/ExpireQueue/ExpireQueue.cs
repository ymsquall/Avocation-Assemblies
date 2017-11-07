
///*********************************************************************
//created:    2016-11-15
//author:     lixianmin

//Copyright (C) - All Rights Reserved
//*********************************************************************/
//using System;
//using System.Collections;
//using UnityEngine;

//namespace Unique.Collections
//{
//	internal class ExpireQueue : ITickable, IDisposable
//	{
//		public ExpireQueue (float expireDelay)
//		{
//			if (expireDelay <= 0.0f)
//			{
//				throw new ArgumentOutOfRangeException("expireDelay should be a positive value.");	
//			}

//			_expireDelay = expireDelay;
//			TickTools.AttachTick(this);
//		}

//		public void Dispose ()
//		{
//			TickTools.DetachTick(this);
//		}

//		public void PushBack (IExpirable target)
//		{
//			if (null == target)
//			{
//				throw new ArgumentNullException();
//			}

//			var expireTime= Time.time + _expireDelay;
//			var item = ExpireItem.Spawn(expireTime, target);
//			_items.PushBack(item);
//		}

//		void ITickable.Tick (float deltaTime)
//		{
//			_CheckExpire();
//		}

//		private void _CheckExpire ()
//		{
//			var current = Time.time;
//			while (_items.Count > 0)
//			{
//				var head = _items.Front() as ExpireItem;
//				if (current < head.GetExpireTime())
//				{
//					break;
//				}

//				_items.PopFront();
//				var target = head.GetTarget() as IExpirable;
//				ExpireItem.Recycle(head);

//				if (null != target)
//				{
//					try
//					{
//						target.OnExpire();
//					}
//					catch (Exception ex)
//					{
//						Console.Error.WriteLine(ex.ToStringEx());
//					}
//				}
//			}
//		}

//		public void Clear ()
//		{
//			var count = _items.Count;
//			if (count > 0)
//			{
//				for (int i= 0; i< count; ++i)
//				{
//					var item = _items[i] as ExpireItem;
//					ExpireItem.Recycle(item);
//				}

//				_items.Clear();
//			}
//		}

//		private readonly Deque _items = new Deque();
//		private float _expireDelay;
//	}
//}