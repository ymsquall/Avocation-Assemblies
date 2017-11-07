// ***************************************************************
//  ICollectionSupport   version:  1.0   Date: 12/12/2005
//  -------------------------------------------------------------
//  
//  -------------------------------------------------------------
//  Copyright ?2005 - Winista All Rights Reserved
// ***************************************************************
// 
// ***************************************************************
using System;
using System.Collections;

namespace Winista.Text.HtmlParser.Support
{
	/// <summary>
	/// Summary description for ICollectionSupport.
	/// </summary>
	public class ICollectionSupport
	{
		/// <summary>
		/// Adds a new element to the specified collection.
		/// </summary>
		/// <param name="c">Collection where the new element will be added.</param>
		/// <param name="obj">Object to add.</param>
		/// <returns>true</returns>
		public static bool Add(System.Collections.ICollection c, System.Object obj)
		{
			bool added = false;
			//Reflection. Invoke either the "add" or "Add" method.
			System.Reflection.MethodInfo method;
			try
			{
                //Get the "add" method for proprietary classes
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
                method = Type_WP_8_1.Type.GetMethod(c.GetType(), "Add");
#else
                method = c.GetType().GetMethod("Add");
#endif
                if (method == null)
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
                    method = Type_WP_8_1.Type.GetMethod(c.GetType(), "add");
#else
                    method = c.GetType().GetMethod("add");
#endif
				int index = (int) method.Invoke(c, new System.Object[] {obj});
				if (index >=0)	
					added = true;
			}
			catch (System.Exception e)
			{
				throw e;
			}
			return added;
		}

		/// <summary>
		/// Adds all of the elements of the "c" collection to the "target" collection.
		/// </summary>
		/// <param name="target">Collection where the new elements will be added.</param>
		/// <param name="c">Collection whose elements will be added.</param>
		/// <returns>Returns true if at least one element was added, false otherwise.</returns>
		public static bool AddAll(System.Collections.ICollection target, System.Collections.ICollection c)
		{
			System.Collections.IEnumerator e = new System.Collections.ArrayList(c).GetEnumerator();
			bool added = false;

			//Reflection. Invoke "addAll" method for proprietary classes
			System.Reflection.MethodInfo method;
			try
            {
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
                method = Type_WP_8_1.Type.GetMethod(target.GetType(), "addAll");
#else
                method = target.GetType().GetMethod("addAll");
#endif
				if (method != null)
					added = (bool) method.Invoke(target, new System.Object[] {c});
				else
                {
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
                    method = Type_WP_8_1.Type.GetMethod(target.GetType(), "Add");
#else
                    method = target.GetType().GetMethod("Add");
#endif
					while (e.MoveNext() == true)
					{
						bool tempBAdded =  (int) method.Invoke(target, new System.Object[] {e.Current}) >= 0;
						added = added ? added : tempBAdded;
					}
				}
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
			return added;
		}

		/// <summary>
		/// Removes all the elements from the collection.
		/// </summary>
		/// <param name="c">The collection to remove elements.</param>
		public static void Clear(System.Collections.ICollection c)
		{
			//Reflection. Invoke "Clear" method or "clear" method for proprietary classes
			System.Reflection.MethodInfo method;
			try
            {
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
                method = Type_WP_8_1.Type.GetMethod(c.GetType(), "Clear");
#else
                method = c.GetType().GetMethod("Clear");
#endif
                if (method == null)
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
                    method = Type_WP_8_1.Type.GetMethod(c.GetType(), "clear");
#else
                    method = c.GetType().GetMethod("clear");
#endif

				method.Invoke(c, new System.Object[] {});
			}
			catch (System.Exception e)
			{
				throw e;
			}
		}

		/// <summary>
		/// Determines whether the collection contains the specified element.
		/// </summary>
		/// <param name="c">The collection to check.</param>
		/// <param name="obj">The object to locate in the collection.</param>
		/// <returns>true if the element is in the collection.</returns>
		public static bool Contains(System.Collections.ICollection c, System.Object obj)
		{
			bool contains = false;

			//Reflection. Invoke "contains" method for proprietary classes
			System.Reflection.MethodInfo method;
			try
            {
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
                method = Type_WP_8_1.Type.GetMethod(c.GetType(), "Contains");
#else
                method = c.GetType().GetMethod("Contains");
#endif
                if (method == null)
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
                    method = Type_WP_8_1.Type.GetMethod(c.GetType(), "contains");
#else
                    method = c.GetType().GetMethod("contains");
#endif

				contains = (bool)method.Invoke(c, new System.Object[] {obj});
			}
			catch (System.Exception e)
			{
				throw e;
			}

			return contains;
		}

		/// <summary>
		/// Determines whether the collection contains all the elements in the specified collection.
		/// </summary>
		/// <param name="target">The collection to check.</param>
		/// <param name="c">Collection whose elements would be checked for containment.</param>
		/// <returns>true id the target collection contains all the elements of the specified collection.</returns>
		public static bool ContainsAll(System.Collections.ICollection target, System.Collections.ICollection c)
		{						
			System.Collections.IEnumerator e =  c.GetEnumerator();

			bool contains = false;

			//Reflection. Invoke "containsAll" method for proprietary classes or "Contains" method for each element in the collection
			System.Reflection.MethodInfo method;
			try
            {
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
                method = Type_WP_8_1.Type.GetMethod(target.GetType(), "containsAll");
#else
                method = target.GetType().GetMethod("containsAll");
#endif
				if (method != null)
					contains = (bool)method.Invoke(target, new Object[] {c});
				else
                {
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
                    method = Type_WP_8_1.Type.GetMethod(target.GetType(), "Contains");
#else
                    method = target.GetType().GetMethod("Contains");
#endif
					while (e.MoveNext() == true)
					{
						if ((contains = (bool)method.Invoke(target, new Object[] {e.Current})) == false)
							break;
					}
				}
			}
			catch (System.Exception ex)
			{
				throw ex;
			}

			return contains;
		}

		/// <summary>
		/// Removes the specified element from the collection.
		/// </summary>
		/// <param name="c">The collection where the element will be removed.</param>
		/// <param name="obj">The element to remove from the collection.</param>
		public static bool Remove(System.Collections.ICollection c, System.Object obj)
		{
			bool changed = false;

			//Reflection. Invoke "remove" method for proprietary classes or "Remove" method
			System.Reflection.MethodInfo method;
			try
            {
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
                method = Type_WP_8_1.Type.GetMethod(c.GetType(), "remove");
#else
                method = c.GetType().GetMethod("remove");
#endif
				if (method != null)
					method.Invoke(c, new System.Object[] {obj});
				else
                {
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
                    method = Type_WP_8_1.Type.GetMethod(c.GetType(), "Contains");
#else
                    method = c.GetType().GetMethod("Contains");
#endif
                    changed = (bool)method.Invoke(c, new System.Object[] { obj });
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
                    method = Type_WP_8_1.Type.GetMethod(c.GetType(), "Remove");
#else
                    method = c.GetType().GetMethod("Remove");
#endif
					method.Invoke(c, new System.Object[] {obj});
				}
			}
			catch (System.Exception e)
			{
				throw e;
			}

			return changed;
		}

		/// <summary>
		/// Removes all the elements from the specified collection that are contained in the target collection.
		/// </summary>
		/// <param name="target">Collection where the elements will be removed.</param>
		/// <param name="c">Elements to remove from the target collection.</param>
		/// <returns>true</returns>
		public static bool RemoveAll(System.Collections.ICollection target, System.Collections.ICollection c)
		{
			System.Collections.ArrayList al = ToArrayList(c);
			System.Collections.IEnumerator e = al.GetEnumerator();

			//Reflection. Invoke "removeAll" method for proprietary classes or "Remove" for each element in the collection
			System.Reflection.MethodInfo method;
			try
            {
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
                method = Type_WP_8_1.Type.GetMethod(target.GetType(), "removeAll");
#else
                method = target.GetType().GetMethod("removeAll");
#endif
				if (method != null)
					method.Invoke(target, new System.Object[] {al});
				else
                {
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
                    method = Type_WP_8_1.Type.GetMethod(target.GetType(), "Remove");
#else
                    method = target.GetType().GetMethod("Remove");
#endif
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
                    System.Reflection.MethodInfo methodContains = Type_WP_8_1.Type.GetMethod(target.GetType(), "Contains");
#else
                    System.Reflection.MethodInfo methodContains = target.GetType().GetMethod("Contains");
#endif

					while (e.MoveNext() == true)
					{
						while ((bool) methodContains.Invoke(target, new System.Object[] {e.Current}) == true)
							method.Invoke(target, new System.Object[] {e.Current});
					}
				}
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
			return true;
		}

		/// <summary>
		/// Retains the elements in the target collection that are contained in the specified collection
		/// </summary>
		/// <param name="target">Collection where the elements will be removed.</param>
		/// <param name="c">Elements to be retained in the target collection.</param>
		/// <returns>true</returns>
		public static bool RetainAll(System.Collections.ICollection target, System.Collections.ICollection c)
		{
			System.Collections.IEnumerator e = new System.Collections.ArrayList(target).GetEnumerator();
			System.Collections.ArrayList al = new System.Collections.ArrayList(c);

			//Reflection. Invoke "retainAll" method for proprietary classes or "Remove" for each element in the collection
			System.Reflection.MethodInfo method;
			try
            {
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
                method = Type_WP_8_1.Type.GetMethod(c.GetType(), "retainAll");
#else
                method = c.GetType().GetMethod("retainAll");
#endif
				if (method != null)
					method.Invoke(target, new System.Object[] {c});
				else
                {
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
                    method = Type_WP_8_1.Type.GetMethod(c.GetType(), "Remove");
#else
                    method = c.GetType().GetMethod("Remove");
#endif
					while (e.MoveNext() == true)
					{
						if (al.Contains(e.Current) == false)
							method.Invoke(target, new System.Object[] {e.Current});
					}
				}
			}
			catch (System.Exception ex)
			{
				throw ex;
			}

			return true;
		}

		/// <summary>
		/// Returns an array containing all the elements of the collection.
		/// </summary>
		/// <returns>The array containing all the elements of the collection.</returns>
		public static System.Object[] ToArray(System.Collections.ICollection c)
		{	
			int index = 0;
			System.Object[] objects = new System.Object[c.Count];
			System.Collections.IEnumerator e = c.GetEnumerator();

			while (e.MoveNext())
				objects[index++] = e.Current;

			return objects;
		}

		/// <summary>
		/// Obtains an array containing all the elements of the collection.
		/// </summary>
		/// <param name="objects">The array into which the elements of the collection will be stored.</param>
		/// <returns>The array containing all the elements of the collection.</returns>
		public static System.Object[] ToArray(System.Collections.ICollection c, System.Object[] objects)
		{	
			int index = 0;

			System.Type type = objects.GetType().GetElementType();
			System.Object[] objs = (System.Object[]) Array.CreateInstance(type, c.Count );

			System.Collections.IEnumerator e = c.GetEnumerator();

			while (e.MoveNext())
				objs[index++] = e.Current;

			//If objects is smaller than c then do not return the new array in the parameter
			if (objects.Length >= c.Count)
				objs.CopyTo(objects, 0);

			return objs;
		}

		/// <summary>
		/// Converts an ICollection instance to an ArrayList instance.
		/// </summary>
		/// <param name="c">The ICollection instance to be converted.</param>
		/// <returns>An ArrayList instance in which its elements are the elements of the ICollection instance.</returns>
		public static System.Collections.ArrayList ToArrayList(System.Collections.ICollection c)
		{
			System.Collections.ArrayList tempArrayList = new System.Collections.ArrayList();
			System.Collections.IEnumerator tempEnumerator = c.GetEnumerator();
			while (tempEnumerator.MoveNext())
				tempArrayList.Add(tempEnumerator.Current);
			return tempArrayList;
		}
	}
}
