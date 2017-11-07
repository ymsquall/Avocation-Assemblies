//
///********************************************************************
//created:    2015-03-03
//author:     lixianmin
//
//Copyright (C) - All Rights Reserved
//*********************************************************************/
//using UnityEngine;
//using System;
//
//namespace Unique.Reflection
//{
//	[Flags]
//	public enum ImportAssetOptions
//	{
//		Default = 0,
//		ForceUpdate = 1,
//		ForceSynchronousImport = 8,
//		ImportRecursive = 256,
//		DontDownloadFromCacheServer = 8192,
//		ForceUncompressedImport = 16384
//	}
//
//	public static class AssetDatabase
//	{
//		public static void SaveAssets ()
//		{
//			if (null == _lpfnSaveAssets)
//			{
//				var method = MyType.GetMethod("SaveAssets", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
//				TypeTools.CreateDelegate(method, out _lpfnSaveAssets);
//			}
//
//			_lpfnSaveAssets();
//		}
//
//		private static Type _myType;
//
//		public static Type MyType
//		{
//			get
//			{
//				if (null == _myType)
//				{
//					_myType = System.Type.GetType("UnityEditor.AssetDatabase,UnityEditor");
//				}
//
//				return _myType;
//			}
//		}
//
//		private static Action _lpfnSaveAssets;
//	}
//}
