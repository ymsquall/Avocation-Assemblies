// ***************************************************************
//  ResourceGoneException   version:  1.0   Date: 12/19/2005
//  -------------------------------------------------------------
//  
//  -------------------------------------------------------------
//  Copyright ?2005 - Winista All Rights Reserved
// ***************************************************************
// 
// ***************************************************************
using System;

namespace Winista.Text.HtmlParser.Http
{
	/// <summary>
	/// Summary description for ResourceGoneException.
	/// </summary>
	public class ResourceGoneException : System.ApplicationException
	{
		private string m_Url;

        public ResourceGoneException(string url)
			:base("Http resource gone")
		{
			m_Url = url;
		}

        public ResourceGoneException(string url, String strMsg)
			:base(strMsg)
		{
			m_Url = url;
		}

        public string Url
		{
			get
			{
				return this.m_Url;
			}
		}
	}
}
