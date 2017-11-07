// ***************************************************************
//  RobotBlockedException   version:  1.0   Date: 12/19/2005
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
	/// Summary description for RobotBlockedException.
	/// </summary>
	public class RobotBlockedException : ApplicationException
	{
		private string m_strUrl;

        public RobotBlockedException(string strUrl)
			:base("Blocked by robots.txt")
		{
			m_strUrl = strUrl;
		}

		public string Url
		{
			get
			{
				return this.m_strUrl;
			}
		}
	}
}
