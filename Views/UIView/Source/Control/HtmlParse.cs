using Framework.Tools;
using System.Collections;
using System.Collections.Generic;
using Winista.Text.HtmlParser;
using Winista.Text.HtmlParser.Lex;
using Winista.Text.HtmlParser.Util;

namespace UIView
{
    public enum 页面解析类型
    {
        任务,
        max
    }
    public enum HtmlTagType
    {
		A,
		AEND,
        FONT,
        SPACE,
        TRANS,
        TITLE,          // no endable
        ITEM,
        BR,             // no endable
        A_HREF,         // no endable
		A_COLOR,		//
        A_ENABLE,
        FONT_SIZE,      // has endable
        FONT_COLOR,     // has endable
        FONT_EFFECT,     // has endable
        SPACE_HOR,      // has endable
        SPACE_VER,      // has endable
        SPACE_SEGMENT,  // has endable
        TRANS_SIZE_W,   // has endable
        TRANS_SIZE_H,   // has endable
        TRANS_POS_X,    // has endable
        TRANS_POS_Y,    // has endable
        IMAGE,          // no endable
        IMGANIM,        
        NUMBER,         // no endable
        PARAM,
        IMAGE_ID,
        IMGANIM_ID,
        NUMBER_ID,
        PARAM_ID,
        PARAM_PUSH,
        ITEM_INDEX,
        ITEM_EXCHANGE1,
        ITEM_EXCHANGE2,
        // task
        TASK_NAME,
        TASK_DESC,
        TASK_COND,
        TASK_AWARD,
        TASK_AWARDITEM,
        // equip
        EQUIP_BASE_ATTR_TITLE,
        EQUIP_BASE_ATTR,
        EQUIP_PREFIX1_TITLE,
        EQUIP_PREFIX1,
        EQUIP_PREFIX2_TITLE,
        EQUIP_PREFIX2,
        EQUIP_INLAY_TITLE,
        EQUIP_INLAY_ICON,
        EQUIP_INLAY_ATTR,
        EQUIP_SUIT_TITLE,
        EQUIP_SUIT_PART,
        EQUIP_SUIT_ATTR,
        // equip inlay
        EQUIP_INLAY_ATTR1,
        EQUIP_INLAY_ATTR2,
        EQUIP_INLAY_ATTR3,
        EQUIP_INLAY_ATTR4,
        EQUIP_INLAY_ATTR5,
        EQUIP_INLAY_ATTR6,
        EQUIP_INLAY_ATTR7,
        EQUIP_INLAY_ATTR8,
        // skill
        SKILL_DESC_NAME,
        SKILL_DESC_LV1,
        SKILL_DESC_LV2,
        SKILL_DESC_TITLE,
        SKILL_DESC_ATTR,
        SKILL_NEXT_DESC_TITLE,
        SKILL_NEXT_DESC_ATTR,
        SKILL_ATTR,
        SKILL_ATTR_ID,
        SKILL_ATTR_LINK,
        SKILL_HW,
        SKILL_HW_ID,
        SKILL_HW_PROP,
        SKILL_DMG,
        SKILL_DMG_ID,
        SKILL_DMG_PROP,
        // gem item
        GEM_INLAY_PART0,
        GEM_INLAY_PART1,
        GEM_INLAY_PART2,
        GEM_INLAY_PART3,
        GEM_INLAY_PART4,
        GEM_INLAY_PART5,
        GEM_INLAY_PART6,
        GEM_INLAY_PART7,
        // gift item
        GIFT_GIVE_COIN_TITLE,
        GIFT_GIVE_COIN_TYPE,
        GIFT_GIVE_ITEM_TITLE,
        GIFT_GIVE_ITEM,
        GIFT_RANDOM_ITEM_TITLE1,
        GIFT_RANDOM_ITEM_TITLE2,
        GIFT_RANDOM_ITEM,
        // a spell
        ASPELL_INLAY_ATTR,
        CURSE_NEED_SPELL_PART,
        // copy scene
        SCS_GETED_ITEMS,        // 单人副本胜利，获得的物品
        SCS_AWARD_ITEMS,        // 单人副本胜利，奖励的物品
        MCS_UNLOCK_LV_TITLE,    // 多人副本解锁条件-达到等级
        MCS_UNLOCK_LV,
        MCS_UNLOCK_CS_TITLE,
        MCS_UNLOCK_CS,          // 多人副本解锁条件-通关副本
        MCS_UNLOCK_TASK_TITLE,
        MCS_UNLOCK_TASK,        // 多人副本解锁条件-完成任务
        // npc
        USERNAME,
        // vip
        VIP_AWARD_GIFT_TITLE,
        VIP_AWARD_GIFT_NONE,
		VIP_FREEDOM,
		// chat
		CHAT_NEXT_LINE_NOT_ADD_SPACE,
        /*
        VIP_FREEDOM_TITLE,
        VIP_FREEDOM_AF,
        VIP_FREEDOM_00,
        VIP_FREEDOM_01,
        VIP_FREEDOM_02,
        VIP_FREEDOM_03,
        VIP_FREEDOM_04,
        VIP_FREEDOM_05,
        VIP_FREEDOM_NONE,
        */
        /// <summary>
        /// new
        /// </summary>
		NEXT_LINE_ADD_SPACE,
        HERO_SELECT_SKILL_ICONS,
        max
    };
    public enum HtmlTagAttrType
    {
        ID,
        PROP,
        INDEX,
        EXCHANGE1,
        EXCHANGE2,
        HREF,
        ENABLE,
        SIZE,
        COLOR,
        EFFECT,
        HOR,
        VER,
        SEGMENT,
        SIZE_W,
        SIZE_H,
        POS_X,
        POS_Y,
        LINK,
        PUSH,
        max
    };
    public class HtmlParse
    {
        public delegate bool TagEventHandler(HtmlTagType tag, string value);
        public delegate bool TagEndEventHandler(HtmlTagType tag);
        public delegate bool TextEventHandler(string value);
        public event TagEventHandler OnReadTagBegin;
        public event TagEndEventHandler OnReadTagEnd;
        public event TextEventHandler OnReadText;
        Stack<object> mFontSizeStack = new Stack<object>();
        Stack<object> mFontColorStack = new Stack<object>();
        Stack<object> mFontEffectStack = new Stack<object>();
		Stack<object> mHrefbStack = new Stack<object>();
		Stack<object> mHrefbColorStack = new Stack<object>();
        Stack<object> mEnableStack = new Stack<object>();
        Stack<object> mSpaceHORStack = new Stack<object>();
        Stack<object> mSpaceVERStack = new Stack<object>();
        Stack<object> mSpaceSegmentStack = new Stack<object>();
        Stack<object> mTransSizeWStack = new Stack<object>();
        Stack<object> mTransSizeHStack = new Stack<object>();
        Stack<object> mTransPosXStack = new Stack<object>();
        Stack<object> mTransPosYStack = new Stack<object>();
//#region 获得html文件内容
//        public string DoParseURL(string url, out StrTreeNode tree)
//        {
//            string html = "";
//            try
//            {
//                //System.Net.WebClient aWebClient = new System.Net.WebClient();
//                //aWebClient.Encoding = System.Text.Encoding.UTF8;
//                //html = aWebClient.DownloadString(url);
//                //aWebClient.Dispose();
//                FileStream stream = File.Open(url, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
//                byte[] buffer = new byte[stream.Length];
//                stream.Read(buffer, 0, buffer.Length);
//                html = PlatformTools.BS2String_UTF8(buffer);
//            }
//            catch (Exception ex)
//            {
//                PopupDialogView.Popup(弹框类型.错误提示, ex.Message);
//                Debug.LogException(ex);
//            }
//            DoParseString(html, out tree);
//            return html;
//        }
//#endregion

#region 分析html节点
        public void DoParseString(string str, out StrTreeNode tree)
        {
            tree = new StrTreeNode("root");
            Lexer lexer = new Lexer(str);
            Parser parser = new Parser(lexer);
            NodeList htmlNodes = parser.Parse(null);
            for (int i = 0; i < htmlNodes.Count; i++)
            {
                RecursionHtmlNode(tree, htmlNodes[i], false);
            }
        }
#endregion
        HtmlTagType GetTagType(string tag)
        {
            for(HtmlTagType t = HtmlTagType.A; t < HtmlTagType.max; ++t)
            {
                if(t.ToString() == tag)
                    return t;
            }
            return HtmlTagType.max;
        }
        HtmlTagAttrType GetTagAttrType(string attr)
        {
            for(HtmlTagAttrType t = HtmlTagAttrType.ID; t < HtmlTagAttrType.max; ++t)
            {
                if(t.ToString() == attr)
                    return t;
            }
            return HtmlTagAttrType.max;
        }
        HtmlTagType[] GetFullTagInfo(ITag tag, out string[] value)
        {
            value = null;
            List<HtmlTagType> ret = new List<HtmlTagType>();
            HtmlTagType tagType = GetTagType(tag.TagName);
            switch(tagType)
            {
                case HtmlTagType.FONT:
                    {
                        if(tag.IsEndTag())
                        {
                            if (mFontSizeStack.Count > 0)
                            {
                                if(null != mFontSizeStack.Peek())
                                    ret.Add(HtmlTagType.FONT_SIZE);
                                mFontSizeStack.Pop();
                            }
                            if (mFontColorStack.Count > 0)
                            {
                                if(null != mFontColorStack.Peek())
                                    ret.Add(HtmlTagType.FONT_COLOR);
                                mFontColorStack.Pop();
                            }
                            if (mFontEffectStack.Count > 0)
                            {
                                if (null != mFontEffectStack.Peek())
                                    ret.Add(HtmlTagType.FONT_EFFECT);
                                mFontEffectStack.Pop();
                            }
                        }
                        else
                        {
                            object size = tag.Attributes[HtmlTagAttrType.SIZE.ToString()];
                            object color = tag.Attributes[HtmlTagAttrType.COLOR.ToString()];
                            object effect = tag.Attributes[HtmlTagAttrType.EFFECT.ToString()];
                            mFontSizeStack.Push(size);
                            mFontColorStack.Push(color);
                            mFontEffectStack.Push(effect);
                            if (null != size)
                                ret.Add(HtmlTagType.FONT_SIZE);
                            if (null != color)
                                ret.Add(HtmlTagType.FONT_COLOR);
                            if (null != effect)
                                ret.Add(HtmlTagType.FONT_EFFECT);
                            if (ret.Count > 0)
                            {
                                value = new string[ret.Count];
                                int index = 0;
                                if (null != tag.Attributes && tag.Attributes.Count > 0)
                                {
                                    if (null != size)
                                    {
                                        string attrStr = size.ToString();
                                        string newStr = attrStr.Replace("px", "");
                                        if (newStr == attrStr)
                                        {
                                            newStr = attrStr;
                                        }
                                        value[index++] = newStr;
                                    }
                                    if (null != color)
                                    {
                                        string attrStr = color.ToString();
                                        if (attrStr.Length < 8)
                                        {
                                            for (int i = 0; i < 8 - attrStr.Length + 1; ++i)
                                                attrStr = "f" + attrStr;
                                        }
                                        //if ('#' != attrStr[0])
                                        //    attrStr = "#" + attrStr;
                                        value[index++] = attrStr;
                                    }
                                    if (null != effect)
                                    {
                                        string attrStr = effect.ToString();
                                        value[index++] = attrStr;
                                    }
                                }
                            }
                        }
                    }
                    break;
                case HtmlTagType.AEND:
					{
						if (tag.IsEndTag())
						{
							if (mHrefbStack.Count > 0)
							{
								if (null != mHrefbStack.Peek())
									ret.Add(HtmlTagType.A_HREF);
								mHrefbStack.Pop();
							}
							if(mHrefbColorStack.Count > 0)
							{
								if (null != mHrefbColorStack.Peek())
									ret.Add(HtmlTagType.A_COLOR);
								mHrefbColorStack.Pop();
							}
							if(mEnableStack.Count > 0)
							{
								if (null != mEnableStack.Peek())
									ret.Add(HtmlTagType.A_ENABLE);
								mEnableStack.Pop();
							}
						}
					}
					break;
                case HtmlTagType.A:
                    {
						/*
						ATag httpTag = tag as ATag;
						httpTag.IsEndTag();
						*/
                        if (tag.IsEndTag())
                        {
                            if (mHrefbStack.Count > 0)
                            {
                                if (null != mHrefbStack.Peek())
                                    ret.Add(HtmlTagType.A_HREF);
                                mHrefbStack.Pop();
                            }
							if(mHrefbColorStack.Count > 0)
                            {
                                if (null != mHrefbColorStack.Peek())
                                    ret.Add(HtmlTagType.A_COLOR);
                                mHrefbColorStack.Pop();
                            }
                            if(mEnableStack.Count > 0)
                            {
                                if (null != mEnableStack.Peek())
                                    ret.Add(HtmlTagType.A_ENABLE);
                                mEnableStack.Pop();
                            }
                        }
                        else
                        {
                            object href = tag.Attributes[HtmlTagAttrType.HREF.ToString()];
                            object color = tag.Attributes[HtmlTagAttrType.COLOR.ToString()];
                            object enable = tag.Attributes[HtmlTagAttrType.ENABLE.ToString()];
                            mHrefbStack.Push(href);
                            mHrefbColorStack.Push(color);
                            mEnableStack.Push(enable);
                            if (null != href)
                                ret.Add(HtmlTagType.A_HREF);
                            if (null != color)
                                ret.Add(HtmlTagType.A_COLOR);
                            if (null != enable)
                                ret.Add(HtmlTagType.A_ENABLE);
                            if (ret.Count > 0)
                            {
                                value = new string[ret.Count];
                                int index = 0;
                                if (null != tag.Attributes && tag.Attributes.Count > 0)
                                {
                                    if (null != href)
                                        value[index++] = href.ToString();
                                    if (null != color)
                                        value[index++] = color.ToString();
                                    if (null != enable)
                                        value[index++] = enable.ToString();
                                }
                            }
                        }
                    }
                    break;
                case HtmlTagType.SPACE:
                    {
                        if (tag.IsEndTag())
                        {
                            if (mSpaceHORStack.Count > 0)
                            {
                                if (null != mSpaceHORStack.Peek())
                                    ret.Add(HtmlTagType.SPACE_HOR);
                            }
                            if (mSpaceVERStack.Count > 0)
                            {
                                if (null != mSpaceVERStack.Peek())
                                    ret.Add(HtmlTagType.SPACE_VER);
                            }
                            if (mSpaceSegmentStack.Count > 0)
                            {
                                if (null != mSpaceSegmentStack.Peek())
                                    ret.Add(HtmlTagType.SPACE_SEGMENT);
                            }
                        }
                        else
                        {
                            object hor = tag.Attributes[HtmlTagAttrType.HOR.ToString()];
                            object ver = tag.Attributes[HtmlTagAttrType.VER.ToString()];
                            object segment = tag.Attributes[HtmlTagAttrType.SEGMENT.ToString()];
                            mSpaceHORStack.Push(hor);
                            mSpaceVERStack.Push(ver);
                            mSpaceSegmentStack.Push(segment);
                            if (null != hor)
                                ret.Add(HtmlTagType.SPACE_HOR);
                            if (null != ver)
                                ret.Add(HtmlTagType.SPACE_VER);
                            if (null != segment)
                                ret.Add(HtmlTagType.SPACE_SEGMENT);
                            if (ret.Count > 0)
                            {
                                value = new string[ret.Count];
                                int index = 0;
                                if (null != hor)
                                    value[index++] = hor.ToString();
                                if (null != ver)
                                    value[index++] = ver.ToString();
                                if (null != segment)
                                    value[index++] = segment.ToString();
                            }
                        }
                    }
                    break;
                case HtmlTagType.TRANS:
                    {
                        if (tag.IsEndTag())
                        {
                            if (mTransSizeWStack.Count > 0)
                            {
                                if (null != mTransSizeWStack.Peek())
                                    ret.Add(HtmlTagType.TRANS_SIZE_W);
                            }
                            if (mTransSizeHStack.Count > 0)
                            {
                                if (null != mTransSizeHStack.Peek())
                                    ret.Add(HtmlTagType.TRANS_SIZE_H);
                            }
                            if (mTransPosXStack.Count > 0)
                            {
                                if (null != mTransPosXStack.Peek())
                                    ret.Add(HtmlTagType.TRANS_POS_X);
                            }
                            if (mTransPosYStack.Count > 0)
                            {
                                if (null != mTransPosYStack.Peek())
                                    ret.Add(HtmlTagType.TRANS_POS_Y);
                            }
                        }
                        else
                        {
                            object size_w = tag.Attributes[HtmlTagAttrType.SIZE_W.ToString()];
                            object size_h = tag.Attributes[HtmlTagAttrType.SIZE_H.ToString()];
                            object pos_x = tag.Attributes[HtmlTagAttrType.POS_X.ToString()];
                            object pos_y = tag.Attributes[HtmlTagAttrType.POS_Y.ToString()];
                            mTransSizeWStack.Push(size_w);
                            mTransSizeHStack.Push(size_h);
                            mTransPosXStack.Push(pos_x);
                            mTransPosYStack.Push(pos_y);
                            if (null != size_w)
                                ret.Add(HtmlTagType.TRANS_SIZE_W);
                            if (null != size_h)
                                ret.Add(HtmlTagType.TRANS_SIZE_H);
                            if (null != pos_x)
                                ret.Add(HtmlTagType.TRANS_POS_X);
                            if (null != pos_y)
                                ret.Add(HtmlTagType.TRANS_POS_Y);
                            if (ret.Count > 0)
                            {
                                value = new string[ret.Count];
                                int index = 0;
                                if (null != size_w)
                                    value[index++] = size_w.ToString();
                                if (null != size_h)
                                    value[index++] = size_h.ToString();
                                if (null != pos_x)
                                    value[index++] = pos_x.ToString();
                                if (null != pos_y)
                                    value[index++] = pos_y.ToString();
                            }
                        }
                    }
                    break;
                case HtmlTagType.ITEM:
                    {
                        object idVal = tag.Attributes[HtmlTagAttrType.INDEX.ToString()];
                        object e1Val = tag.Attributes[HtmlTagAttrType.EXCHANGE1.ToString()];
                        object e2Val = tag.Attributes[HtmlTagAttrType.EXCHANGE2.ToString()];
                        if (null != idVal)
                            ret.Add(HtmlTagType.ITEM_INDEX);
                        if (null != e1Val)
                            ret.Add(HtmlTagType.ITEM_EXCHANGE1);
                        if (null != e2Val)
                            ret.Add(HtmlTagType.ITEM_EXCHANGE2);
                        if (ret.Count > 0)
                        {
                            value = new string[ret.Count];
                            int index = 0;
                            if (null != idVal)
                                value[index++] = idVal.ToString();
                            if (null != e1Val)
                                value[index++] = e1Val.ToString();
                            if (null != e2Val)
                                value[index++] = e2Val.ToString();
                        }
                    }
                    break;
                case HtmlTagType.IMAGE:
                    {
                        object idVal = tag.Attributes[HtmlTagAttrType.ID.ToString()];
                        if (null != idVal)
                            ret.Add(HtmlTagType.IMAGE_ID);
                        if (ret.Count > 0)
                        {
                            value = new string[ret.Count];
                            int index = 0;
                            if (null != idVal)
                                value[index++] = idVal.ToString();
                        }
                    }
                    break;
                case HtmlTagType.IMGANIM:
                    {
                        object idVal = tag.Attributes[HtmlTagAttrType.ID.ToString()];
                        if (null != idVal)
                            ret.Add(HtmlTagType.IMGANIM_ID);
                        if (ret.Count > 0)
                        {
                            value = new string[ret.Count];
                            int index = 0;
                            if (null != idVal)
                                value[index++] = idVal.ToString();
                        }
                    }
                    break;
                case HtmlTagType.NUMBER:
                    {
                        object idVal = tag.Attributes[HtmlTagAttrType.ID.ToString()];
                        if (null != idVal)
                            ret.Add(HtmlTagType.NUMBER_ID);
                        if (ret.Count > 0)
                        {
                            value = new string[ret.Count];
                            int index = 0;
                            if (null != idVal)
                                value[index++] = idVal.ToString();
                        }
                    }
                    break;
                case HtmlTagType.PARAM:
                    {
                        object idVal = tag.Attributes[HtmlTagAttrType.ID.ToString()];
                        object pushVal = tag.Attributes[HtmlTagAttrType.PUSH.ToString()];
                        if (null != idVal)
                            ret.Add(HtmlTagType.PARAM_ID);
                        if (null != pushVal)
                            ret.Add(HtmlTagType.PARAM_PUSH);
                        if (ret.Count > 0)
                        {
                            value = new string[ret.Count];
                            int index = 0;
                            if (null != idVal)
                                value[index++] = idVal.ToString();
                            if (null != pushVal)
                                value[index++] = pushVal.ToString();
                        }
                    }
                    break;
                case HtmlTagType.SKILL_ATTR:
                    {
                        object skillAttrID = tag.Attributes[HtmlTagAttrType.ID.ToString()];
                        object skillLinkAttrID = tag.Attributes[HtmlTagAttrType.LINK.ToString()];
                        if (null != skillAttrID)
                            ret.Add(HtmlTagType.SKILL_ATTR_ID);
                        if (null != skillLinkAttrID)
                            ret.Add(HtmlTagType.SKILL_ATTR_LINK);
                        if (ret.Count > 0)
                        {
                            value = new string[ret.Count];
                            int index = 0;
                            if (null != skillAttrID)
                                value[index++] = skillAttrID.ToString();
                            if (null != skillLinkAttrID)
                                value[index++] = skillLinkAttrID.ToString();
                        }
                    }
                    break;
                case HtmlTagType.SKILL_HW:
                    {
                        object hwID = tag.Attributes[HtmlTagAttrType.ID.ToString()];
                        object hwPropID = tag.Attributes[HtmlTagAttrType.PROP.ToString()];
                        if (null != hwID)
                            ret.Add(HtmlTagType.SKILL_HW_ID);
                        if (null != hwPropID)
                            ret.Add(HtmlTagType.SKILL_HW_PROP);
                        if (ret.Count > 0)
                        {
                            value = new string[ret.Count];
                            int index = 0;
                            if (null != hwID)
                                value[index++] = hwID.ToString();
                            if (null != hwPropID)
                                value[index++] = hwPropID.ToString();
                        }
                    }
                    break;
                case HtmlTagType.SKILL_DMG:
                    {
                        object dmgID = tag.Attributes[HtmlTagAttrType.ID.ToString()];
                        object dmgPropID = tag.Attributes[HtmlTagAttrType.PROP.ToString()];
                        if (null != dmgID)
                            ret.Add(HtmlTagType.SKILL_DMG_ID);
                        if (null != dmgPropID)
                            ret.Add(HtmlTagType.SKILL_DMG_PROP);
                        if (ret.Count > 0)
                        {
                            value = new string[ret.Count];
                            int index = 0;
                            if (null != dmgID)
                                value[index++] = dmgID.ToString();
                            if (null != dmgPropID)
                                value[index++] = dmgPropID.ToString();
                        }
                    }
                    break;
                default:
                    {
                        if (null != tag.Attributes && tag.Attributes.Count > 0)
                        {
                            List<string> keys = new List<string>();
                            List<string> vals = new List<string>();
                            IEnumerator ks = tag.Attributes.Keys.GetEnumerator();
                            while (ks.MoveNext())
                            {
                                keys.Add(ks.Current.ToString());
                                vals.Add(tag.Attributes[ks.Current.ToString()].ToString());
                            }
                            value = new string[vals.Count];
                            for (int i = 0; i < keys.Count; ++i)
                            {
                                ret.Add(tagType);
                                value[i] = vals[i];
                            }
                        }
                        else
                        {
                            ret.Add(tagType);
                            value = new string[] { "" };
                        }
                    }
                    break;
            }
            return ret.ToArray();
        }
        void RecursionHtmlNode(StrTreeNode treeNode, INode htmlNode, bool siblingRequired)
        {
            if (htmlNode == null || treeNode == null) return;

            StrTreeNode current = treeNode;
            //current node  
            if (htmlNode is ITag)
            {
                string nodeString = "";
                ITag tag = (htmlNode as ITag);
                string[] values = null;
                HtmlTagType[] types = GetFullTagInfo(tag, out values);
                if (!tag.IsEndTag())
                {
                    for (int i = 0; i < types.Length; ++ i)
                    {
                        if (null != OnReadTagBegin)
                            OnReadTagBegin(types[i], values[i]);
                        nodeString += types[i].ToString() + "={" + values[i] + "} ";
                    }
                    //if (tag.Attributes != null && tag.Attributes.Count > 0)
                    //{
                    //    if (tag.Attributes["ID"] != null)
                    //    {
                    //        if (null != OnReadTagBegin)
                    //            OnReadTagBegin(tag.TagName, "ID", tag.Attributes["ID"].ToString());
                    //        nodeString = nodeString + " { id=\"" + tag.Attributes["ID"].ToString() + "\" }";
                    //    }
                    //    if (tag.Attributes["HREF"] != null)
                    //    {
                    //        if (null != OnReadTagBegin)
                    //            OnReadTagBegin(tag.TagName, "HREF", tag.Attributes["HREF"].ToString());
                    //        nodeString = nodeString + " { href=\"" + tag.Attributes["HREF"].ToString() + "\" }";
                    //    }
                    //    if (tag.Attributes["SIZE"] != null)
                    //    {
                    //        if (null != OnReadTagBegin)
                    //            OnReadTagBegin(tag.TagName, "SIZE", tag.Attributes["SIZE"].ToString());
                    //        nodeString = nodeString + " { size=\"" + tag.Attributes["SIZE"].ToString() + "\" }";
                    //    }
                    //    if (tag.Attributes["COLOR"] != null)
                    //    {
                    //        if (null != OnReadTagBegin)
                    //            OnReadTagBegin(tag.TagName, "COLOR", tag.Attributes["COLOR"].ToString());
                    //        nodeString = nodeString + " { color=\"" + tag.Attributes["COLOR"].ToString() + "\" }";
                    //    }
                    //}
                }
                else
                {
                    for (int i = 0; i < types.Length; ++i)
                    {
                        if (null != OnReadTagEnd)
                            OnReadTagEnd(types[i]);
                    }
                    nodeString = tag.TagName + " End";
                }
                current = treeNode.AddItem(nodeString);
            }
            //获取节点间的内容  
            if (htmlNode.Children != null && htmlNode.Children.Count > 0)
            {
                RecursionHtmlNode(current, htmlNode.FirstChild, true);
                //content = htmlNode.FirstChild.GetText();
                //节点列表.Add(content);
            }
            else if (!(htmlNode is ITag))
            {
                if (htmlNode is IText)
                {
                    IText tex = htmlNode as IText;
                    string nodeString = tex.GetText();
                    nodeString = nodeString.Replace("\r\n", "");
                    nodeString = nodeString.Replace("&nbsp;", " ");
                    nodeString = nodeString.Replace("\t", "    ");
                    byte[] utf8Bom = new byte[] { 239, 187, 191 };
                    nodeString = nodeString.Replace(PlatformTools.BS2String_UTF8(utf8Bom), "");
                    bool allIsSpace = true;
                    for (int i = 0; i < nodeString.Length; ++i)
                    {
                        if (nodeString[i] != ' ')
                        {
                            allIsSpace = false;
                            break;
                        }
                    }
                    if (!string.IsNullOrEmpty(nodeString) && !allIsSpace)
                    {
                        if (null != OnReadText)
                            OnReadText(nodeString);
                        current = treeNode.AddItem(nodeString);
                    }
                }
                else
                {
                    string typestr = htmlNode.GetType().ToString();
                    if (null != OnReadText)
                        OnReadText(typestr);
                    current = treeNode.AddItem(typestr);
                }
            }

            //the sibling nodes  
            if (siblingRequired)
            {
                INode sibling = htmlNode.NextSibling;
                while (sibling != null)
                {
                    RecursionHtmlNode(current, sibling, false);
                    sibling = sibling.NextSibling;
                }
            }
        }
    }
}