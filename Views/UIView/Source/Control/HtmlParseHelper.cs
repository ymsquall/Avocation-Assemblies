using UnityEngine;
using System.Collections.Generic;
using System;
using Framework.Tools;
using Winista.Text.HtmlParser;
using Winista.Text.HtmlParser.Lex;
using Winista.Text.HtmlParser.Util;

namespace UIView.Control
{
    public class HtmlParseHelper : MonoBehaviour
    {
        public string 页面地址 = "Task/0001.html";
        public string 富文本内容 = "";
        public bool 重新解析 = false;
        public List<string> 节点列表 = new List<string>();
        string mFileFullPath = "";
        void Awake()
        {
            AddUrl();
            DoParse();
        }
        private void AddUrl()
        {
            //CBUrl.Items.Add("http://www.hao123.com");
            //CBUrl.Items.Add("http://www.sina.com");
            //CBUrl.Items.Add("http://www.heuet.edu.cn");
            mFileFullPath = StreamAssetHelper.AssetsPath(StreamAssetRoot.HTML_ROOT, 页面地址);
        }  
        void DoParse()
        {
            #region 获得网页的html
            try
            {
                富文本内容 = "";
                节点列表.Clear();
                string url = mFileFullPath.Trim();
                富文本内容 = PlatformTools.DownloadWebString(url);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            #endregion

            #region 分析网页html节点
            Lexer lexer = new Lexer(富文本内容);
            Parser parser = new Parser(lexer);
            NodeList htmlNodes = parser.Parse(null);
            节点列表.Clear();
            节点列表.Add("root");
            string treeRoot = 节点列表[0];
            for (int i = 0; i < htmlNodes.Count; i++)
            {
                this.RecursionHtmlNode(treeRoot, htmlNodes[i], false);
            }

            #endregion

        }

        private void RecursionHtmlNode(string treeNode, INode htmlNode, bool siblingRequired)
        {
            if (htmlNode == null || treeNode == null) return;

            string current = treeNode;
            //string content;
            //current node  
            if (htmlNode is ITag)
            {
                string nodeString = "";
                ITag tag = (htmlNode as ITag);
                if (!tag.IsEndTag())
                {
                    nodeString = tag.TagName;
                    if (tag.Attributes != null && tag.Attributes.Count > 0)
                    {
                        if (tag.Attributes["ID"] != null)
                        {
                            nodeString = nodeString + " { id=\"" + tag.Attributes["ID"].ToString() + "\" }";
                        }
                        if (tag.Attributes["HREF"] != null)
                        {
                            nodeString = nodeString + " { href=\"" + tag.Attributes["HREF"].ToString() + "\" }";
                        }
                        if (tag.Attributes["SIZE"] != null)
                        {
                            nodeString = nodeString + " { size=\"" + tag.Attributes["SIZE"].ToString() + "\" }";
                        }
                        if (tag.Attributes["COLOR"] != null)
                        {
                            nodeString = nodeString + " { color=\"" + tag.Attributes["COLOR"].ToString() + "\" }";
                        }
                    }
                }
                else
                {
                    nodeString = tag.TagName + " End";
                }
                current = nodeString;
                节点列表.Add(current);
            }
            //获取节点间的内容  
            if (htmlNode.Children != null && htmlNode.Children.Count > 0)
            {
                this.RecursionHtmlNode(current, htmlNode.FirstChild, true);
                //content = htmlNode.FirstChild.GetText();
                //节点列表.Add(content);
            }
            else if (!(htmlNode is ITag))
            {
                if (htmlNode is IText)
                {
                    IText tex = htmlNode as IText;
                    current = tex.GetText();
                    current = current.Replace("\r\n", "");
                    current = current.Replace("&nbsp;", " ");
                    byte[] utf8Bom = new byte[] { 239, 187, 191 };
                    current = current.Replace(PlatformTools.BS2String_UTF8(utf8Bom), "");
                    bool allIsSpace = true;
                    for (int i = 0; i < current.Length; ++i)
                    {
                        if (current[i] != ' ')
                        {
                            allIsSpace = false;
                            break;
                        }
                    }
                    if (!string.IsNullOrEmpty(current) && current != "\t" && !allIsSpace)
                        节点列表.Add(current);
                }
                else
                {
                    string typestr = htmlNode.GetType().ToString();
                    节点列表.Add(typestr);
                }
            }

            //the sibling nodes  
            if (siblingRequired)
            {
                INode sibling = htmlNode.NextSibling;
                while (sibling != null)
                {
                    this.RecursionHtmlNode(treeNode, sibling, false);
                    sibling = sibling.NextSibling;
                }
            }
        }
        void Update()
        {
            if(重新解析)
            {
                Awake();
                重新解析 = false;
            }
        }
    }
}