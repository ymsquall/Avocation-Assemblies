// ***************************************************************
//  NodeClassFilter   version:  1.0   date: 12/18/2005
//  -------------------------------------------------------------
//  
//  -------------------------------------------------------------
//  Copyright ?2005 Winista - All Rights Reserved
// ***************************************************************
// 
// ***************************************************************
using System;

namespace Winista.Text.HtmlParser.Filters
{
	/// <summary> This class accepts all tags of a given class.</summary>
	public class NodeClassFilter : NodeFilter
	{
		/// <summary> The class to match.</summary>
		protected internal System.Type mClass;

		/// <summary>
		/// 
		/// </summary>
		public NodeClassFilter():this(typeof(Winista.Text.HtmlParser.Tags.Html))
		{
		}

		/// <summary> Creates a NodeClassFilter that accepts tags of the given class.</summary>
		/// <param name="cls">The class to match.
		/// </param>
		public NodeClassFilter(System.Type cls)
		{
			mClass = cls;
		}

		/// <summary> Get the class to match.</summary>
		/// <returns> Returns the class.
		/// </returns>
		/// <summary> Set the class to match.</summary>
		/// <param name="cls">The node class to match.
		/// </param>
		virtual public System.Type MatchClass
		{
			get
			{
				return (mClass);
			}
			
			set
			{
				mClass = value;
			}
			
		}

		/// <summary> Accept nodes that are assignable from the class provided in
		/// the constructor.
		/// </summary>
		/// <param name="node">The node to check.
		/// </param>
		/// <returns> <code>true</code> if the node is the right class,
		/// <code>false</code> otherwise.
		/// </returns>
		public virtual bool Accept(INode node)
        {
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
            return ((null != mClass) && Type_WP_8_1.Type.IsAssignableFrom(mClass, node.GetType()));
#else
            return ((null != mClass) && mClass.IsAssignableFrom(node.GetType()));
#endif
		}

		virtual public System.Object Clone()
		{
			return null;
		}
	}
}
