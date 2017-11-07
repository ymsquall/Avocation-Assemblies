//
///*********************************************************************
//created:    2014-08-02
//author:     lixianmin
//
//Copyright (C) - All Rights Reserved
//*********************************************************************/
//using System;
//using System.Collections.Generic;
//
//namespace Unique
//{
//	internal static class ComparerFactory
//	{
//        public static IComparer<T> Create<T> ()
//        {
//            var comparer = ReversedStringComparer.Instance as IComparer<T>;
//            if(null != comparer)
//            {
//                return comparer;
//            }
//            
//            return Comparer<T>.Default;
//        }
//	}
//}