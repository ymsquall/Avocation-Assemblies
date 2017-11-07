
/********************************************************************
created:    2014-08-07
author:     lixianmin

Copyright (C) - All Rights Reserved
*********************************************************************/
using UnityEngine;

namespace Unique
{
	public abstract class ReferenceBase: Disposable, IIsDisposable
	{
		~ReferenceBase ()
		{
			AssertTools.IsTrue(false, "[ReferenceBase.dctr()] should never be called.");
		}

        internal void AddReference ()
        {
            ++_referCount;
        }
        
        internal void RemoveReference ()
        {
            --_referCount;
        }

        public int GetReference ()
        {
            return _referCount;
        }
        
        public bool IsDisposable ()
        {
            return 0 == _referCount;
        }

        private int _referCount;
	}
}