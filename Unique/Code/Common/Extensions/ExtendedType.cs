
/*********************************************************************
created:    2014-12-17
author:     lixianmin

Copyright (C) - All Rights Reserved
*********************************************************************/
using System;
using System.Collections;

namespace Unique
{
    [Obfuscators.ObfuscatorIgnore]
    public static class ExtendedType
    {
        class TypeItem
        {
            public string name;
            public string assemblyQualifiedName;
        }

        public static bool IsStaticClassEx (this Type type)
        {
            return null != type
                    && type.GetConstructor(Type.EmptyTypes) == null
                    && type.IsAbstract
                    && type.IsSealed;
        }

        private static TypeItem _SetDefaultTypeItem (Type type)
        {
			var item = _typeItems[type] as TypeItem;
            if (null == item)
            {
                item = new TypeItem();
                _typeItems.Add(type, item);
            }
            
            return item;
        }

        public static string GetNameEx (this Type type)
        {
            if (null != type)
            {
                var item = _SetDefaultTypeItem(type);
                if (null == item.name)
                {
                    item.name = type.Name;
                }
                
                return item.name;
            }
            
            return string.Empty;
        }

        public static string GetAssemblyQualifiedNameEx (this Type type)
        {
            if (null != type)
            {
                var item = _SetDefaultTypeItem(type);
                if (null == item.assemblyQualifiedName)
                {
                    item.assemblyQualifiedName = type.AssemblyQualifiedName;
                }
                
                return item.assemblyQualifiedName;
            }

            return string.Empty;
        }

		private static readonly Hashtable _typeItems = new Hashtable();
    }
}