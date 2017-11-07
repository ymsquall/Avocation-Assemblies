
/********************************************************************
created:    2014-11-19
author:     lixianmin

purpose:    assert
Copyright (C) - All Rights Reserved
*********************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Unique
{
    public static class TypeTools
    {
		private class AssemblyComparer :IComparer<System.Reflection.Assembly>
		{
			public int Compare (System.Reflection.Assembly lhs, System.Reflection.Assembly rhs)
			{
				// version = "0.0.0.0" means this is an our own assembly.
				var leftVersion = lhs.FullName.Split(_splitter)[1];
				var rightVersion = rhs.FullName.Split(_splitter)[1];
				
				var result = leftVersion.CompareTo(rightVersion);
				if (result == 0)
				{
					result = lhs.FullName.CompareTo(rhs.FullName);
				}
				
				return result;
			}
			
			private readonly char[] _splitter = new char[] { '=' };
		}

        public static Type SearchType (string typeFullName)
        {
            if (!string.IsNullOrEmpty(typeFullName))
            {
				if (null == _currentAssemblies)
				{
					_currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();
					Array.Sort(_currentAssemblies, new AssemblyComparer());
				}

				var count = _currentAssemblies.Length;
                for (int i= 0; i< count; ++i)
                {
					var assembly = _currentAssemblies[i];
                    var type = assembly.GetType(typeFullName);
                    
                    if (null != type)
                    {
                        return type;
                    }
                }
            }
            
            return null ;
        }

		public static void CreateDelegate<T> (System.Reflection.MethodInfo method, out T lpfnMethod) where T: class
		{
			lpfnMethod = Delegate.CreateDelegate(typeof(T), method) as T;
		}

		public static void CreateDelegate<T> (object target, string name, out T lpfnMethod) where T: class
		{
			lpfnMethod = Delegate.CreateDelegate(typeof(T), target, name) as T;
		}

		public static void CreateDelegate<T> (Type targetType, string name, out T lpfnMethod) where T: class
		{
			lpfnMethod = Delegate.CreateDelegate(typeof(T), targetType, name) as T;
		}

		public static System.Reflection.Assembly GetEditorAssembly ()
		{
			if (null == _editorAssembly)
			{
				foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
				{
					var fullname = assembly.FullName;
					if (fullname.StartsWithEx("Assembly-CSharp-Editor", CompareOptions.Ordinal))
					{
						_editorAssembly = assembly;
						break;
					}
				}
			}
			
			return _editorAssembly;
		}

		private static System.Reflection.Assembly _editorAssembly;
		private static System.Reflection.Assembly[] _currentAssemblies;
    }
}