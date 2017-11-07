
/********************************************************************
created:    2014-08-06
author:     lixianmin

1. ICheckDisposable的實現為不需要實現析構方法，該接口是為LruCache類
準備的，LruCache會主動調用它的Dispose()方法

Copyright (C) - All Rights Reserved
*********************************************************************/

namespace Unique
{
    interface IIsDisposable
    {
        bool IsDisposable();
    }
}
