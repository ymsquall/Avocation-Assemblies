//
///*********************************************************************
//created:    2016-07-02
//author:     lixianmin
//
//Copyright (C) - All Rights Reserved
//*********************************************************************/
//using System;
//using System.Collections;
//using System.Collections.Generic;
//
//namespace Unique.Collections
//{
//	/// <summary>
//	/// http://igoro.com/archive/skip-lists-are-fascinating/
//	/// </summary>
//	public partial class SkipList<TKey, TValue>
//	{
//		public SkipList () : this (null)
//		{
//
//		}
//
//		public SkipList (IComparer<TKey> comparer)
//		{
//			_SetComparer(comparer);
//		}
//
//		/// <summary>
//		/// Inserts a value into the skip list.
//		/// </summary>
//		public void Insert (TKey key, TValue value)
//		{
//			if (_isKeyNullable && null == key)
//			{
//				throw new ArgumentNullException("key is null, value=" + value);
//			}
//
//			// Determine the level of the new node. Generate a random number R. The number of
//			// 1-bits before we encounter the first 0-bit is the level of the node. Since R is
//			// 32-bit, the level can be at most 32.
//			int level = 0;
//			for (int R = _rand.Next(); (R & 1) == 1; R >>= 1)
//			{
//				++level;
//
//				if (level == _levels)
//				{
//					++_levels;
//					break;
//				}
//			}
//			
//			// Insert this node into the skip list
//			var newNode = new Node (key, value, level + 1);
//			var cur = _head;
//			for (int i = _levels - 1; i >= 0; i--)
//			{
//				for (; cur.Next[i] != null; cur = cur.Next[i])
//				{
//					var test = _comparer.Compare (cur.Next[i].Key, key);
//					if (test > 0)
//					{
//						break;
//					}
//				}
//				
//				if (i <= level)
//				{
//					newNode.Next[i] = cur.Next[i];
//					cur.Next[i] = newNode;
//				}
//			}
//		}
//		
//		/// <summary>
//		/// Returns whether a particular value already exists in the skip list
//		/// </summary>
//		public bool Contains (TKey key)
//		{
//			Node cur = _head;
//			for (int i = _levels - 1; i >= 0; i--)
//			{
//				for (; cur.Next[i] != null; cur = cur.Next[i])
//				{
//					var test = _comparer.Compare (cur.Next[i].Key, key);
//					if (test > 0)
//					{
//						break;
//					}
//
//					if (test == 0)
//					{
//						return true;
//					}
//				}
//			}
//
//			return false;
//		}
//		
//		/// <summary>
//		/// Attempts to remove one occurence of a particular value from the skip list. Returns
//		/// whether the value was found in the skip list.
//		/// </summary>
//		public bool Remove (TKey key)
//		{
//			var cur = _head;
//			
//			bool found = false;
//			for (int i = _levels - 1; i >= 0; i--)
//			{
//				for (; cur.Next[i] != null; cur = cur.Next[i])
//				{
//					var test = _comparer.Compare (cur.Next[i].Key, key);
//					if (test == 0)
//					{
//						found = true;
//						cur.Next[i] = cur.Next[i].Next[i];
//						break;
//					}
//					
//					if (test > 0)
//					{
//						break;
//					}
//				}
//			}
//			
//			return found;
//		}
//		
//		/// <summary>
//		/// Produces an enumerator that iterates over elements in the skip list in order.
//		/// </summary>
//		public IEnumerable<KeyValuePair<TKey, TValue>> Enumerate ()
//		{
//			var cur = _head.Next[0];
//			while (cur != null)
//			{
//				var pair = new KeyValuePair<TKey, TValue>(cur.Key, cur.Value);
//				yield return pair;
//				cur = cur.Next[0];
//			}
//		}
//
//		private void _SetComparer (IComparer<TKey> inputComparer)
//		{
//			_comparer = inputComparer ?? Comparer<TKey>.Default;
//		}
//
//
//		private readonly Node _head = new Node (default(TKey), default(TValue), _kMaxLevel);
//		private readonly Random _rand = new Random ();
//
//		private int _levels = 1;
//		private IComparer<TKey> _comparer;
//
//		// The max. number of levels is 33
//		private const int _kMaxLevel = 33;
//		private static readonly bool _isKeyNullable = !typeof(TKey).IsSubclassOf(typeof(ValueType));
//	}
//}
