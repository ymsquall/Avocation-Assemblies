
/********************************************************************
created:    2016-03-23
author:     lixianmin

Copyright (C) - All Rights Reserved
*********************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;

namespace Unique.Collections
{
    internal static class ArrayTools
    {
        public static int EnsureCapacity (int currentCapacity, int minCapacity)
		{
			var result = 0;

			if (currentCapacity <= 0)
			{
				result = 4;
			}
			else
			{
				const int maxIncrement = 256;
				if (currentCapacity <= maxIncrement)
				{
					result = currentCapacity << 1;
				}
				else
				{
					result = currentCapacity + maxIncrement;
				}
			}

			if (result < minCapacity)
			{
				result = minCapacity;
			}

			return result;
		}
    }
}