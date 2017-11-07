
/********************************************************************
created:    2014-03-20
author:     lixianmin

Copyright (C) - All Rights Reserved
*********************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;

namespace Unique.Collections
{
    public struct EmptyEnumerable<T> : IEnumerable<T>
    {
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new EmptyEnumerator<T>();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new EmptyEnumerator<T>();
        }
    }
}