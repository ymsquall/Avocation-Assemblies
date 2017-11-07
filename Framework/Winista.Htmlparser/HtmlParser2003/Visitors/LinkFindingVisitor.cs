// ***************************************************************
//  LinkFindingVisitor   version:  1.0   date: 12/19/2005
//  -------------------------------------------------------------
//  
//  -------------------------------------------------------------
//  Copyright ?2005 Winista - All Rights Reserved
// ***************************************************************
// 
// ***************************************************************
using System;

using Winista.Text.HtmlParser.Tags;

namespace Winista.Text.HtmlParser.Visitors
{
	/// <summary>
	/// Summary description for LinkFindingVisitor.
	/// </summary>
	public class LinkFindingVisitor:NodeVisitor
	{
		private System.String linkTextToFind;
		private int count;
		private System.Globalization.CultureInfo locale;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="linkTextToFind"></param>
		public LinkFindingVisitor(System.String linkTextToFind):this(linkTextToFind, null)
		{
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="linkTextToFind"></param>
		/// <param name="locale"></param>
		public LinkFindingVisitor(System.String linkTextToFind, System.Globalization.CultureInfo locale)
		{
			count = 0;
			this.locale = (null == locale)?new System.Globalization.CultureInfo("en"):locale;
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
			this.linkTextToFind = linkTextToFind.ToUpper();
#else
            this.linkTextToFind = linkTextToFind.ToUpper(this.locale);
#endif
		}

		/// <summary>
		/// 
		/// </summary>
		virtual public int Count
		{
			get
			{
				return (count);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tag"></param>
		public override void VisitTag(ITag tag)
		{
			if (tag is LinkTag)
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
				if (- 1 != ((ATag) tag).LinkText.ToUpper().IndexOf(linkTextToFind))
#else
                if (- 1 != ((ATag) tag).LinkText.ToUpper(locale).IndexOf(linkTextToFind))
#endif
					count++;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual bool LinkTextFound()
		{
			return (0 != count);
		}
	}
}
