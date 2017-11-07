
/*********************************************************************
created:    2016-01-23
author:     lixianmin

Copyright (C) - All Rights Reserved
*********************************************************************/

namespace Unique
{
	/// <summary>
	/// 对于实现了IIsYieldable的对象的使用必须非常在意，因为yield return obj
    /// 意味着至少一帧，这在性能敏感的地方需要特别注意.
	/// </summary>
    public interface IIsYieldable
    {
        bool isYieldable { get; }
    }
}
