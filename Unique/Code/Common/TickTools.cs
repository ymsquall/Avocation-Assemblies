
/********************************************************************
created:    2015-01-04
author:     lixianmin

Copyright (C) - All Rights Reserved
*********************************************************************/
using UnityEngine;
using System.Collections;

namespace Unique
{
    public static class TickTools
    {
		internal static void Tick (float deltaTime)
        {
			_frameStartTime = Time.realtimeSinceStartup;

			var count = _tickItems.Count;
			for (int i= 0; i< count; ++i)
			{
				var tickable = _tickItems[i] as ITickable;
				tickable.Tick(deltaTime);
			}
        }

		public static bool IsTimeout (float timeout = 0.1f)
		{
			return Time.realtimeSinceStartup > _frameStartTime + timeout;
		}

		internal static void AttachTick (ITickable tickable)
		{
			if (null != tickable)
			{
				_tickItems.Add(tickable);
			}
		}

		internal static void DetachTick (ITickable tickable)
		{
			if (null != tickable)
			{
				_tickItems.Remove(tickable);
			}
		}

		// to ensure IsTimeout() is false when Tick() is not called (in editor).
		private static float _frameStartTime = float.MaxValue;
		private static ArrayList _tickItems = new ArrayList();
    }
}