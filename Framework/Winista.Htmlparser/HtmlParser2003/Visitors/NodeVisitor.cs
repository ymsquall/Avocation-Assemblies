// ***************************************************************
//  NodeVisitor   version:  1.0   date: 12/18/2005
//  -------------------------------------------------------------
//  
//  -------------------------------------------------------------
//  Copyright � 2005 Winista - All Rights Reserved
// ***************************************************************
// 
// ***************************************************************
using System;

namespace Winista.Text.HtmlParser.Visitors
{
	/// <summary> The base class for the 'Visitor' pattern.
	/// Classes that wish to use <code>visitAllNodesWith()</code> will subclass
	/// this class and provide implementations for methods they are interested in
	/// processing.<p>
	/// The operation of <code>visitAllNodesWith()</code> is to call
	/// <code>beginParsing()</code>, then <code>visitXXX()</code> according to the
	/// types of nodes encountered in depth-first order and finally
	/// <code>finishedParsing()</code>.<p>
	/// Typical code to print all the link tags:
	/// <pre>
	/// 
	/// public class MyVisitor:NodeVisitor
	/// {
	/// public MyVisitor ()
	/// {
	/// }
	/// 
	/// public void visitTag (Tag tag)
	/// {
	///		System.Console.WriteLine ("\n" + tag.GetTagName () + tag.GetStartPosition ());
	/// }
	/// 
	/// public void VisitStringNode (Text string)
	/// {
	///		System.Console.WriteLine (string);
	/// }
	/// 
	/// public static void main (String[] args) throws ParserException
	/// {
	///		Parser parser = new Parser ("http://cbc.ca");
	///		Visitor visitor = new MyVisitor ();
	///		parser.VisitAllNodesWith (visitor);
	/// }
	/// }
	/// </pre>
	/// If you want to handle more than one tag type with the same visitor
	/// you will need to check the tag type in the VisitTag method. You can
	/// do that by either checking the tag name:
	/// <pre>
	/// public void VisitTag (Tag tag)
	/// {
	/// if (tag.GetName ().Equals ("BODY"))
	/// ... do something with the BODY tag
	/// else if (tag.GetName ().equals ("FRAME"))
	/// ... do something with the FRAME tag
	/// }
	/// </pre>
	/// or you can use <code>instanceof</code> if all the tags you want to handle
	/// have a Tag registered}
	/// tag (i.e. they are generated by the NodeFactory):
	/// <pre>
	/// public void VisitTag (ITag tag)
	/// {
	/// if (tag instanceof BodyTag)
	/// {
	/// BodyTag body = (BodyTag)tag;
	/// ... do something with body
	/// }
	/// else if (tag instanceof FrameTag)
	/// {
	/// FrameTag frame = (FrameTag)tag;
	/// ... do something with frame
	/// }
	/// else // other specific tags and generic TagNode objects
	/// {
	/// }
	/// }
	[Serializable]
	public abstract class NodeVisitor
	{
		[NonSerialized]
		private bool m_bRecurseChildren;
		[NonSerialized]
		private bool m_bRecurseSelf;

		/// <summary> Creates a node visitor that recurses itself and it's children.</summary>
		public NodeVisitor():this(true)
		{
		}

		/// <summary> Creates a node visitor that recurses itself and it's children
		/// only if <code>recurseChildren</code> is <code>true</code>.
		/// </summary>
		/// <param name="recurseChildren">If <code>true</code>, the visitor will
		/// visit children, otherwise only the top level nodes are recursed.
		/// </param>
		public NodeVisitor(bool recurseChildren):this(recurseChildren, true)
		{
		}

		/// <summary> Creates a node visitor that recurses itself only if
		/// <code>recurseSelf</code> is <code>true</code> and it's children
		/// only if <code>recurseChildren</code> is <code>true</code>.
		/// </summary>
		/// <param name="recurseChildren">If <code>true</code>, the visitor will
		/// visit children, otherwise only the top level nodes are recursed.
		/// </param>
		/// <param name="recurseSelf">If <code>true</code>, the visitor will
		/// visit the top level node.
		/// </param>
		public NodeVisitor(bool recurseChildren, bool recurseSelf)
		{
			m_bRecurseChildren = recurseChildren;
			m_bRecurseSelf = recurseSelf;
		}

		/// <summary> Override this method if you wish to do special
		/// processing prior to the start of parsing.
		/// </summary>
		public virtual void BeginParsing()
		{
		}
		
		/// <summary> Called for each <code>Tag</code> visited.</summary>
		/// <param name="tag">The tag being visited.
		/// </param>
		public virtual void  VisitTag(ITag tag)
		{
		}
		
		/// <summary> Called for each <code>Tag</code> visited that is an end tag.</summary>
		/// <param name="tag">The end tag being visited.
		/// </param>
		public virtual void  VisitEndTag(ITag tag)
		{
		}
		
		/// <summary> Called for each <code>StringNode</code> visited.</summary>
		/// <param name="string">The string node being visited.
		/// </param>
		public virtual void  VisitStringNode(IText string_Renamed)
		{
		}
		
		/// <summary> Called for each <code>RemarkNode</code> visited.</summary>
		/// <param name="remark">The remark node being visited.
		/// </param>
		public virtual void  VisitRemarkNode(IRemark remark)
		{
		}
		
		/// <summary> Override this method if you wish to do special
		/// processing upon completion of parsing.
		/// </summary>
		public virtual void  FinishedParsing()
		{
		}
		
		/// <summary> Depth traversal predicate.</summary>
		/// <returns> <code>true</code> if children are to be visited.
		/// </returns>
		public virtual bool ShouldRecurseChildren()
		{
			return (m_bRecurseChildren);
		}
		
		/// <summary> Self traversal predicate.</summary>
		/// <returns> <code>true</code> if a node itself is to be visited.
		/// </returns>
		public virtual bool ShouldRecurseSelf()
		{
			return (m_bRecurseSelf);
		}
	}
}
