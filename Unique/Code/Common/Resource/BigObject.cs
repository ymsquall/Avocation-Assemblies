
/********************************************************************
created:    2015-07-25
author:     lixianmin

Copyright (C) - All Rights Reserved
*********************************************************************/

using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Unique
{
    public class BigObject
    {
		private class Proxy: Disposable
		{
			public Proxy (UnityEngine.Object target)
			{
				_target = target;
			}

			public UnityEngine.Object GetTarget ()
			{
				return _target;
			}

			protected override void _DoDispose (bool isManualDisposing)
			{
				var target = _target as UnityEngine.Object;
				UnityEngine.Object.Destroy(target);
				_target = null;
			}

			private UnityEngine.Object _target;
		}

		public BigObject (Func<UnityEngine.Object> creator)
		{
			if (null == creator)
			{
				throw new ArgumentNullException("creator should not be null.");
			}

			_creator = creator;
		}

		~BigObject ()
		{
			if (_gcHandle.IsAllocated)
			{
				_gcHandle.Free();
			}

			_proxy = null;
		}

		public UnityEngine.Object Target 
		{
			get
			{
				if (null == _proxy && _gcHandle.IsAllocated)
				{
					try
					{
						_proxy = _gcHandle.Target as Proxy;
					}
					catch (InvalidOperationException)
					{
						
					}
				}

				UnityEngine.Object target = null;
				if (null != _proxy)
				{
					target = _proxy.GetTarget();
				}
				else
				{
					try
					{
						target = _creator();

						if (null != target)
						{
							_proxy = new Proxy(target);
							_gcHandle = GCHandle.Alloc(_proxy, GCHandleType.Weak);
						}
					}
					catch (Exception ex)
					{
						Console.Error.WriteLine(ex.ToStringEx());
					}
				}
				
				return target;
			}
		}

		public void Free ()
		{
			_proxy = null;
		}

		private Func<UnityEngine.Object> _creator;
		private GCHandle _gcHandle;
		private Proxy _proxy;
    }
}