
/*********************************************************************
created:    2014-05-26
author:     lixianmin

Copyright (C) - All Rights Reserved
*********************************************************************/
using System;
using System.Collections.Generic;

namespace Unique
{
	public class ReversedStringComparer: IComparer<string>
	{
        static ReversedStringComparer ()
        {
        }

        private ReversedStringComparer ()
        {
        }

		public int Compare (string a, string b)
        {
            return a.ReversedCompareToEx(b);
        }

        public static readonly ReversedStringComparer Instance = new ReversedStringComparer();
	}
}