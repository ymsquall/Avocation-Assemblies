
/********************************************************************
created:    2013-12-14
author:     lixianmin

Copyright (C) - All Rights Reserved
*********************************************************************/

namespace Unique
{
    public interface ITickable: System.IDisposable
    {
        void Tick (float deltaTime);
    }
}