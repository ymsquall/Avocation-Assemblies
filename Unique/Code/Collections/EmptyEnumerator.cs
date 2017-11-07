
/********************************************************************
created:    2014-02-24
author:     lixianmin

Copyright (C) - All Rights Reserved
*********************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;

namespace Unique.Collections
{
    public struct EmptyEnumerator<T> : IEnumerator<T>, IEnumerator, IDisposable
    {
        public bool MoveNext()
        {
            return false;
        }
        
        public void Reset()
        {

        }
        
        void IDisposable.Dispose()
        {
            
        }
        
        public T Current
        {
            get
            { 
                return default(T); 
            }
        }
        
        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }
    }
}