using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Framework.Tools;
using UIView.Scrolling;
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
//using Threading_WP_8_1;
#else
#endif

namespace UIView.Control
{
    public class SimpleRichText : MonoBehaviour
    {
        public delegate void ContextEvent();
        public event ContextEvent OnContextChanged;
        public delegate void ParseEvent(Bounds b, UIWidget.Pivot p);
        public event ParseEvent OnContextParseOvered;
        public delegate void SizeEvent(Bounds o, Bounds n, int maxHeight);
        public event SizeEvent OnChangeHeight;
        public string 地址 = "";
        public string 内容 = "";
        public bool 重新解析 = true;
        public bool 重新计算边界 = true;
        public bool 自动换行 = true;
        public int 默认字体大小 = 22;
        public UIFontType FontType = UIFontType.sourcehansans_regular;
        public Color 默认颜色 = Color.black;
        public UILabel.Effect 默认文字效果 = UILabel.Effect.None;
        public Vector2 默认文字效果值 = Vector2.zero;
        public Color 默认文字效果颜色 = Color.white;
        public Vector2 视图尺寸 = Vector2.zero;
        public float 行间距 = 0;
        public float 附加行间距比例 = 0;
        public UIWidget 底板 = null;
        public Bounds 边界;
        public UIWidget 关联高度面板 = null;
        public int 关联附加高度 = 0;
		public int 关联最大高度 = 0;
        public UIWidget.Pivot 内容锚定方向 = UIWidget.Pivot.Center;
        public int 层级 = 0;
		public bool 解析后重置滚动 = true;
        public int 行数限制 = 0;
        //float mLastSizeY = 0;
        protected bool mIsNextTitleText = false;
		string mNowHrefbData = "";
		string mNowHrefbColor = "";
        string mNowEnableData = "";
        protected Stack<int> mFontSizeTagStack = new Stack<int>();
        Stack<Color> mFontColorTagStack = new Stack<Color>();
        Stack<UILabelEffect> mFontEffectTagStack = new Stack<UILabelEffect>();
        Stack<int> mSpaceHorStack = new Stack<int>();
        Stack<int> mSpaceVerStack = new Stack<int>();
        Stack<int> mSpaceSegmentStack = new Stack<int>();
        Stack<int> mTransPosXStack = new Stack<int>();
        Stack<int> mTransPosYStack = new Stack<int>();
        Stack<int> mTransSizeWStack = new Stack<int>();
        Stack<int> mTransSizeHStack = new Stack<int>();
        Vector2 mStartWritePos = Vector2.zero;
        protected Vector2 mNowWritePos = Vector2.zero;
        float mLastFontSize = 0.0f;
        float mLastLineHeight = 0.0f;
        float mNowLineHeight = 0.0f;
        protected List<UIWidget> mNowLineUnits = new List<UIWidget>();
        bool mNextTextIsNotShow = false;
        bool mFirstRunning = true;
        Vector3 mLastViewPos = Vector3.zero;
        List<TwoData<int, int>> mMappingToRichPos = new List<TwoData<int, int>>();
        List<string> mPushParamList = new List<string>();
        List<object> mParamList = new List<object>();
        bool mParsing = false;
        Transform mCacheTrans = null;
        List<UIAtlasType> mHasedAtlas = new List<UIAtlasType>();
        int mNowLineCount = 0;

        public bool NextTextIsNotShow
        {
            set { mNextTextIsNotShow = value; }
            get { return mNextTextIsNotShow; }
        }
        public bool FirstRunning
        {
            set { mFirstRunning = value; }
            get { return mFirstRunning; }
        }
        public string this[int index]
        {
            get
            {
                if (index >= 0 && index < mPushParamList.Count)
                    return mPushParamList[index];
                return "";
            }
        }
		public Vector2 CursorPos
		{
			get
			{
				Vector2 parentPos = transform.localPosition;
				return parentPos + mNowWritePos;
			}
		}
        void Awake()
        {
            mCacheTrans = transform;
        }
        public void ClearParam()
        {
            mParamList.Clear();
            mPushParamList.Clear();
        }
        public void SetParam(params object[] ps)
        {
            mParamList.AddRange(ps);
        }
        public void Reset()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying)
                return;
#endif
            StopAllCoroutines();
            mParsing = false;
            mNowLineUnits.Clear();
            mLastLineHeight = 0;
            mFontSizeTagStack.Clear();
            mFontColorTagStack.Clear();
            mFontEffectTagStack.Clear();
            mSpaceHorStack.Clear();
            mSpaceVerStack.Clear();
            mSpaceSegmentStack.Clear();
            mTransPosXStack.Clear();
            mTransPosYStack.Clear();
            mTransSizeWStack.Clear();
            mTransSizeHStack.Clear();
            mFontSizeTagStack.Push(默认字体大小);
            mLastFontSize = 默认字体大小;
            mFontColorTagStack.Push(默认颜色);
            mFontEffectTagStack.Push(new UILabelEffect(默认文字效果, 默认文字效果值.x, 默认文字效果值.y, 默认文字效果颜色));
            mSpaceHorStack.Push(0);
            mSpaceVerStack.Push(0);
            mSpaceSegmentStack.Push(0);
            mTransPosXStack.Push(0);
            mTransPosYStack.Push(0);
            mTransSizeWStack.Push(0);
            mTransSizeHStack.Push(0);
            NextTextIsNotShow = false;
            mNowLineCount = 0;
            if (null != 底板)
            {
                Destroy(底板.gameObject);
                底板 = null;
            }
            底板 = NGUITools.AddWidget<UIWidget>(gameObject);
//            底板.NotDrawGeometry = true;
//            底板.CalcRelativeBounds = false;
            底板.pivot = UIWidget.Pivot.Top;
            底板.name = "底板";
            Vector3 size = new Vector3(视图尺寸.x, 视图尺寸.y);
            UpdatePivot(size);
            mNowWritePos = Vector2.zero;
            mStartWritePos = mNowWritePos;
        }
        void Parse(string text)
		{
            if (mParsing)
                return;
            if (!FirstRunning)
                mCacheTrans.localPosition = mLastViewPos;
            else
                mLastViewPos = mCacheTrans.localPosition;
            if (地址.Length > 0 || text.Length > 0)
            {
                mParsing = true;
                HtmlParse reader = new HtmlParse();
                reader.OnReadTagBegin += OnHandleTagBegin;
                reader.OnReadTagEnd += OnHandleTagEnd;
                reader.OnReadText += OnCreateText;
                if (地址.Length > 0)
                {
                    byte[] buffer = null;
#if UNITY_ANDROID && !UNITY_EDITOR
                    WWW file = StreamAssetHelper.LoadAsset(StreamAssetRoot.HTML_ROOT, 地址);
                    while (!file.isDone && (file.progress < 0.9f))
                    {
                        Thread.Sleep(100);
                    }
                    if (file.bytes != null)
                    {
                        buffer = file.bytes;
						内容 = PlatformTools.BS2String_UTF8(buffer);
                    }
                    AssetBundle bdl = file.assetBundle;
                    if (null != bdl)
                        bdl.Unload(true);
                    file.Dispose();
                    file = null;
#else
                    Stream stream = StreamAssetHelper.LoadFile(StreamAssetRoot.HTML_ROOT, 地址);
                    buffer = new byte[stream.Length];
                    int len = stream.Read(buffer, 0, (int)stream.Length);
                    if (len != (int)stream.Length)
                    {
                        Debug.LogError("读取页面地址[" + 地址 + "]失败，读取的长度["
                            + len.ToString() + "]与文件长度[" + stream.Length + "]不匹配！");
                        return;
                    }
                    内容 = PlatformTools.BS2String_UTF8(buffer);
#endif
                }
                else
                    内容 = text;
                StrTreeNode treeNode = null;
                if (内容.Length > 0)
                    reader.DoParseString(内容, out treeNode);
                重新计算边界 = true;
                ClearParam();
                if (null != OnContextChanged)
                    OnContextChanged();
                GetBounds();
                mParsing = false;
                ReposNowLine();
                mNowLineUnits.Clear();
            }
			OnParseOvered ();
            if (null != OnContextParseOvered)
                OnContextParseOvered(Bounds, 内容锚定方向);
        }
		protected virtual void OnParseOvered()
		{
			if(解析后重置滚动)
			{
				ScrollViewport spv = GetComponentInParent<ScrollViewport>();
				if(null != spv)
					spv.ResetPosition();
			}
		}
        protected virtual void Update()
        {
            if (重新解析 || FirstRunning)
            {
                string text = 内容;
                Clear();
                Parse(text);
                重新解析 = false;
                FirstRunning = false;
            }
        }
        protected virtual bool OnHandleTagBegin(HtmlTagType tag, string value)
        {
            switch(tag)
            {
                case HtmlTagType.TITLE:
                    mIsNextTitleText = true;
                    return true;
                case HtmlTagType.BR:
                    {
                        if (NextTextIsNotShow)
                        {
                            NextTextIsNotShow = false;
                            return true;
                        }
                        DoChangeLine();
                    }
                    return true;
                case HtmlTagType.A_HREF:
                    mNowHrefbData = value;
                    return true;
                case HtmlTagType.A_COLOR:
                    mNowHrefbColor = value;
                    return true;
                case HtmlTagType.A_ENABLE:
                    mNowEnableData = value;
                    return true;
                case HtmlTagType.FONT_SIZE:
                    {
						int size = 0;
						if(int.TryParse(value, out size))
						{
							if(size > 100) size = 100;
	                        mFontSizeTagStack.Push(size);
	                        float height = mFontSizeTagStack.Peek();
	                        mLastFontSize = height;
						}
                    }
                    return true;
                case HtmlTagType.FONT_COLOR:
                    {
                        Color color = StringUtils.ColorStr2U3DColor(value);
                        mFontColorTagStack.Push(color);
                    }
                    return true;
                case HtmlTagType.FONT_EFFECT:
                    {
                        string[] ps = value.Split(",".ToCharArray());
                        if (ps.Length >= 3)
                        {
                            UILabel.Effect et = UILabel.Effect.None;
                            if (ps[0] == "s")
                                et = UILabel.Effect.Shadow;
                            else if (ps[0] == "o")
                                et = UILabel.Effect.Outline;
                            else
                                return true;
                            float distX = 0, distY = 0;
                            if (!float.TryParse(ps[1], out distX))
                                return false;
                            if (!float.TryParse(ps[2], out distY))
                                return false;
                            if(ps.Length == 4)
                            {
								Color c = StringUtils.ColorStr2U3DColor(ps[3]);
                                mFontEffectTagStack.Push(new UILabelEffect(et, distX, distY, c));
                            }
                            else
                                mFontEffectTagStack.Push(new UILabelEffect(et, distX, distY));
                        }
                    }
                    return true;
                case HtmlTagType.SPACE_HOR:
                    {
                        int val = 0;
                        if (int.TryParse(value, out val))
                            mSpaceHorStack.Push(val);
                        else
                            mSpaceHorStack.Push(0);
                    }
                    return true;
                case HtmlTagType.SPACE_VER:
                    {
                        int val = 0;
                        if (int.TryParse(value, out val))
                            mSpaceVerStack.Push(val);
                        else
                            mSpaceVerStack.Push(0);
                    }
                    return true;
                case HtmlTagType.SPACE_SEGMENT:
                    {
                        int val = 0;
                        if (int.TryParse(value, out val))
                            mSpaceSegmentStack.Push(val);
                        else
                            mSpaceSegmentStack.Push(0);
                    }
                    return true;
                case HtmlTagType.TRANS_SIZE_W:
                    {
                        int val = 0;
                        if (int.TryParse(value, out val))
                            mTransSizeWStack.Push(val);
                        else
                            mTransSizeWStack.Push(0);
                    }
                    return true;
                case HtmlTagType.TRANS_SIZE_H:
                    {
                        int val = 0;
                        if (int.TryParse(value, out val))
                            mTransSizeHStack.Push(val);
                        else
                            mTransSizeHStack.Push(0);
                    }
                    return true;
                case HtmlTagType.TRANS_POS_X:
                    {
                        int val = 0;
                        if (int.TryParse(value, out val))
                            mTransPosXStack.Push(val);
                        else
                            mTransPosXStack.Push(0);
                    }
                    return true;
                case HtmlTagType.TRANS_POS_Y:
                    {
                        int val = 0;
                        if (int.TryParse(value, out val))
                            mTransPosYStack.Push(val);
                        else
                            mTransPosYStack.Push(0);
                    }
                    return true;
                case HtmlTagType.IMAGE_ID:
                    {
                        string[] names = value.Split("/".ToCharArray());
                        if (names.Length != 2)
                            return false;
                        if (!DoAddImage(names[0], names[1]))
                            return false;
                    }
                    return true;
                case HtmlTagType.IMGANIM_ID:
                    {
                        string[] names = value.Split("/".ToCharArray());
                        if (names.Length != 4)
                            return false;
                        int fps = 0;
                        if(!int.TryParse(names[3], out fps))
                            return false;
                        if (!DoAddImageAnim(names[0], names[1], names[2], fps))
                            return false;
                    }
                    return true;
                case HtmlTagType.NUMBER_ID:
                    {
                        if (!DoAddNumber(value))
                            return false;
                    }
                    return true;
                case HtmlTagType.PARAM_ID:
                    {
                        int idx = int.Parse(value) - 1;
                        if (idx < 0 || idx >= mParamList.Count)
                            return false;
                        if (mParamList[idx] is string)
                            OnCreateText(mParamList[idx] as string);
                        else
                            OnCreateText(mParamList[idx].ToString());
                    }
                    return true;
                case HtmlTagType.PARAM_PUSH:
                    {
                        mPushParamList.Add(value);
                    }
                    return true;
            }
            return true;
        }
        protected virtual bool OnHandleTagEnd(HtmlTagType tag)
        {
            switch (tag)
			{
				case HtmlTagType.AEND:
				case HtmlTagType.A:
					{
						mNowHrefbData = "";
						mNowHrefbColor = "";
						//mNowEnableData = "";
					}
					return true;
				case HtmlTagType.A_HREF:
					mNowHrefbData = "";
					return true;
				case HtmlTagType.A_COLOR:
					mNowHrefbColor = "";
					return true;
				case HtmlTagType.A_ENABLE:
					mNowEnableData = "";
					return true;
                case HtmlTagType.FONT_SIZE:
                    {
                        if (mFontSizeTagStack.Count > 1)
                            mFontSizeTagStack.Pop();
                    }
                    return true;
                case HtmlTagType.FONT_COLOR:
                    {
                        if (mFontColorTagStack.Count > 1)
                            mFontColorTagStack.Pop();
                    }
                    return true;
                case HtmlTagType.FONT_EFFECT:
                    {
                        if (mFontEffectTagStack.Count > 1)
                            mFontEffectTagStack.Pop();
                    }
                    return true;
                case HtmlTagType.SPACE_HOR:
                    {
                        if (mSpaceHorStack.Count > 1)
                            mSpaceHorStack.Pop();
                    }
                    return true;
                case HtmlTagType.SPACE_VER:
                    {
                        if (mSpaceVerStack.Count > 1)
                            mSpaceVerStack.Pop();
                    }
                    return true;
                case HtmlTagType.SPACE_SEGMENT:
                    {
                        if (mSpaceSegmentStack.Count > 1)
                            mSpaceSegmentStack.Pop();
                    }
                    return true;
                case HtmlTagType.TRANS_SIZE_W:
                    {
                        if (mTransSizeWStack.Count > 1)
                            mTransSizeWStack.Pop();
                    }
                    return true;
                case HtmlTagType.TRANS_SIZE_H:
                    {
                        if (mTransSizeHStack.Count > 1)
                            mTransSizeHStack.Pop();
                    }
                    return true;
                case HtmlTagType.TRANS_POS_X:
                    {
                        if (mTransPosXStack.Count > 1)
                            mTransPosXStack.Pop();
                    }
                    return true;
                case HtmlTagType.TRANS_POS_Y:
                    {
                        if (mTransPosYStack.Count > 1)
                            mTransPosYStack.Pop();
                    }
                    return true;
            }
            return true;
        }
        protected virtual bool OnCreateText(string value)
        {
            if (NextTextIsNotShow)
                return true;
            OnUnitAddBefore();
			DoAddText(value, mNowHrefbData, mNowHrefbColor, mNowEnableData, mIsNextTitleText);
            mIsNextTitleText = false;
            //mNowHrefbData = "";
            mNowEnableData = "";
            return true;
        }
        float CalcLineHeight(float height)
        {
            height += height * 附加行间距比例;
            float space_ver = mSpaceVerStack.Peek();
            return height + 行间距 + space_ver;
        }
        float CalcLineFirstPos()
        {
            float pos_x = 0;
            float space_seg = mSpaceSegmentStack.Peek();
            if (space_seg != 0)
                pos_x += space_seg;
            else
                pos_x += mSpaceHorStack.Peek();
            return pos_x;
        }
       protected Vector2 CalcUnitSize(Vector2 showSize)
        {
            Vector2 outPos = showSize;
            float size_w = mTransSizeWStack.Peek();
            float size_h = mTransSizeHStack.Peek();
            if (size_w > outPos.x && size_h <= outPos.y)
            {
                outPos.x = size_w;
            }
            else if (size_h > outPos.y && size_w <= outPos.x)
            {
                outPos.y = size_h;
            }
            else if (size_w > outPos.x && size_h > outPos.y)
            {
                outPos.x = size_w;
                outPos.y = size_h;
            }
            return outPos;
        }
        UILabel.Overflow CalcTextSize(ref Vector2 showSize)
        {
            float size_w = mTransSizeWStack.Peek();
            float size_h = mTransSizeHStack.Peek();
            UILabel.Overflow overflow = UILabel.Overflow.ResizeFreely;
            if (size_w > showSize.x && size_h <= showSize.y)
            {
                showSize.x = size_w;
                overflow = UILabel.Overflow.ResizeHeight;
            }
            else if (size_h > showSize.y && size_w <= showSize.x)
            {
                showSize.y = size_h;
                overflow = UILabel.Overflow.ClampContent;
            }
            else if (size_w > showSize.x && size_h > showSize.y)
            {
                showSize.x = size_w;
                showSize.y = size_h;
                overflow = UILabel.Overflow.ShrinkContent;
            }
            return overflow;
        }
		protected virtual void DoChangeLineAfter(){}
        protected bool DoChangeLine()
        {
            if (行数限制 > 0 && mNowLineCount >= 行数限制)
                return false;
            float height = 0;
            if(mLastLineHeight <= 0)
                height = CalcLineHeight(mLastFontSize);
            else
                height = CalcLineHeight(mLastLineHeight);
            mNowLineCount++;
            if (行数限制 > 0 && mNowLineCount >= 行数限制)
                DoReplaceLimitText();
            mNowWritePos.x = CalcLineFirstPos();
            mNowWritePos.y -= height;
            mLastLineHeight = 0;
			mNowLineUnits.Clear();
			DoChangeLineAfter ();
            return true;
        }
        public static Vector2 CalcTextPrintSize(string text, Font ft, int fs, FontStyle style, bool b, int regionX, int regionY)
        {
            Font oldFt = NGUIText.dynamicFont;
            int oldFs = NGUIText.fontSize;
            FontStyle oldStyle = NGUIText.fontStyle;
            bool oldEncoding = NGUIText.encoding;
            int oldRegionX = NGUIText.regionWidth;
            int oldRegionY = NGUIText.regionHeight;
            NGUIText.dynamicFont = ft;
            NGUIText.fontSize = fs;
            NGUIText.fontStyle = style;
			NGUIText.encoding = b;
			NGUIText.rectWidth = regionX;
			NGUIText.rectHeight = regionY;
            NGUIText.regionWidth = regionX;
			NGUIText.regionHeight = regionY;
			NGUIText.Update(false);
			NGUIText.WrapText(text, out text, true);
            Vector2 ret = NGUIText.CalculatePrintedSize(text);
            NGUIText.dynamicFont = oldFt;
            NGUIText.fontSize = oldFs;
            NGUIText.fontStyle = oldStyle;
            NGUIText.encoding = oldEncoding;
            NGUIText.regionWidth = oldRegionX;
            NGUIText.regionHeight = oldRegionY;
            return ret;
        }
        protected void DoAddText(string text, string hrefb, string hrefColor, string hrefEnable, bool isTitle)
        {
            DoAddText(text, hrefb, hrefColor, hrefEnable, isTitle, false);
        }
        protected void DoAddText(string text, string hrefb, string hrefColor, string hrefEnable, bool isTitle, bool limitReplace)
        {
            if (!limitReplace && 行数限制 > 0 && mNowLineCount >= 行数限制)
                return;
            UILabel lbl = null;
            UIFont font = UIResourceManager.LoadFont(FontType);
            int fontSize = mFontSizeTagStack.Peek();
            Color color = mFontColorTagStack.Peek();
            UILabelEffect effect = mFontEffectTagStack.Peek();
            if(isTitle)
            {
                if (string.IsNullOrEmpty(hrefb))
                {
                    lbl = UIViewHelper.AddUILabel(底板.gameObject, FontType, fontSize, color);
                }
                else
                {
                    UIHyperLinkLabel linkLbl = UIViewHelper.AddHyperLinkLabel(底板.gameObject, FontType, fontSize, color);
                    linkLbl.OnHyperLinkClicked += OnLinkClicked;
                    linkLbl.LinkDataSource = hrefb;
					linkLbl.UnderLineColor = hrefColor;
                    linkLbl.LinkDataEnable = hrefEnable;
                    lbl = linkLbl;
                }
                Vector2 showSize = new Vector2(lbl.width, lbl.height);
                lbl.overflowMethod = CalcTextSize(ref showSize);
                if (effect.type != UILabel.Effect.None)
                {
                    lbl.effectStyle = effect.type;
                    lbl.effectDistance = effect.dist;
                    lbl.effectColor = effect.color;
                }
                lbl.depth = 层级 + 11;
                lbl.text = text;
                if (mNowWritePos.x > 0)
                    DoChangeLine();
                float x = 0;
                lbl.transform.localPosition = new Vector3(x, mNowWritePos.y - lbl.height * 0.5f);
                //OnUnitAdded(lbl);
                DoChangeLine();
            }
            else
            {
                UILabel.Overflow overflow = UILabel.Overflow.ResizeFreely;
                Vector2 showSize = Vector2.zero;
                if(自动换行)
                {
                    if (视图尺寸.x < fontSize)
                    {
                        Debug.LogError("富文本设置为自动换行后，视图尺寸至少大于等于一个字的宽度");
                        return;
                    }
                    string nextLineText = "";
                    string splitText = text;
                    showSize = CalcTextPrintSize(splitText, font.dynamicFont, fontSize, FontStyle.Normal, true, 1000000, 1000000);
                    overflow = CalcTextSize(ref showSize);
                    float canUseSpace = CalcNowLineCanUsedSpace();
                    if (showSize.x > canUseSpace)
                    {
                        while (showSize.x > canUseSpace)
                        {
                            for (int i = text.Length - 1; i >= 0; --i)
                            {
                                splitText = splitText.Substring(0, i);
                                showSize = CalcTextPrintSize(splitText, font.dynamicFont, fontSize, FontStyle.Normal, true, 1000000, 1000000);
                                overflow = CalcTextSize(ref showSize);
                                if (showSize.x <= canUseSpace)
                                    break;
                            }
                            if(string.IsNullOrEmpty(splitText))
                            {
                                if(DoChangeLine())
								    DoAddText(text, hrefb, hrefColor, hrefEnable, false);
                                return;
                            }
                        }
                        nextLineText = text.Substring(splitText.Length);
						DoAddText(splitText, hrefb, hrefColor, hrefEnable, false);
                        if (DoChangeLine())
                            DoAddText(nextLineText, hrefb, hrefColor, hrefEnable, false);
                        return;
                    }
                }
                if (string.IsNullOrEmpty(hrefb))
                {
                    lbl = UIViewHelper.AddUILabel(底板.gameObject, FontType, fontSize, color);
                }
                else
                {
                    UIHyperLinkLabel linkLbl = UIViewHelper.AddHyperLinkLabel(底板.gameObject, FontType, fontSize, color);
                    linkLbl.OnHyperLinkClicked += OnLinkClicked;
					linkLbl.LinkDataSource = hrefb;
					linkLbl.UnderLineColor = hrefColor;
                    linkLbl.LinkDataEnable = hrefEnable;
                    lbl = linkLbl;
                }
                lbl.width = (int)showSize.x;
                lbl.height = (int)showSize.y;
                lbl.overflowMethod = overflow;
                if (effect.type != UILabel.Effect.None)
                {
                    lbl.effectStyle = effect.type;
                    lbl.effectDistance = effect.dist;
                    lbl.effectColor = effect.color;
                }
                lbl.depth = 层级 + 11;
                lbl.text = text;
                Vector3 pos = new Vector3(mNowWritePos.x - (视图尺寸.x - (float)lbl.width) * 0.5f, mNowWritePos.y - lbl.height * 0.5f);
                lbl.transform.localPosition = pos;
                OnUnitAdded(lbl);
            }
        }
        protected virtual void DoReplaceLimitText()
        {
        }
        protected float CalcNowLineCanUsedSpace()
        {
            return 视图尺寸.x - mNowWritePos.x - mSpaceSegmentStack.Peek();
        }
        protected virtual void OnUnitAddBefore()
        {
            if (mNowLineUnits.Count <= 0)
                mNowWritePos.x = CalcLineFirstPos();
            else
                mNowWritePos.x += mSpaceHorStack.Peek();
            mNowWritePos.y -= mSpaceVerStack.Peek();
            float pos_x = mTransPosXStack.Peek();
            float pos_y = mTransPosYStack.Peek();
            if (pos_x != 0)
                mNowWritePos.x = pos_x;
            if (pos_y != 0)
                mNowWritePos.y = pos_y;
        }
        protected virtual void OnUnitAdded(UIWidget widget)
        {
            mNowWritePos.x += widget.width;
            if (widget.height > mLastLineHeight)
            {
                //ReposNowLine(widget);
                mLastLineHeight = widget.height;
            }
            mNowLineHeight = mLastLineHeight;
            mNowLineUnits.Add(widget);
        }
        void ReposNowLine()
        {
            if (mLastLineHeight == 0)
                return;
            for (int i = 0; i < mNowLineUnits.Count; ++i)
            {
                UIWidget ui = mNowLineUnits[i];
                if (null == ui)
                    continue;
                float offY = (ui.height - mLastLineHeight) * 0.5f;
                //mNowWritePos.y -= offY;
                Vector3 pos = ui.transform.localPosition;
                pos.y += offY;
                ui.transform.localPosition = pos;
            }
        }
        protected virtual void OnLinkClicked(UIHyperLinkLabel sender, object data)
        {

        }
        public int RichCount { get { return mMappingToRichPos.Count; } }
        public void SetText(string text)
        {
            if (内容 != text)
            {
                Clear();
                内容 = text;
                Parse(内容);
                重新解析 = false;
                FirstRunning = false;
            }
        }
        public void AddText(string text)
        {
            if (null == this)
                return;
            mLastLineHeight = 0;
            int fPos = 内容.Length;
            内容 += text;
            int lPos = 内容.Length - 1;
            TwoData<int, int> mapping = new TwoData<int, int>(fPos, lPos);
            mMappingToRichPos.Add(mapping);
            if (!FirstRunning)
                Parse(text);
        }

        public void RemoveTextAt(int index)
        {
            if (mMappingToRichPos.Count <= 0 || index < 0 || index >= mMappingToRichPos.Count)
                return;
            TwoData<int, int> mapping = mMappingToRichPos[index];
            string s1 = "", s2 = "";
            if (mapping.First > 0)
                s1 = 内容.Substring(0, mapping.First);
            if (mapping.Second < 内容.Length - 1)
                s2 = 内容.Substring(mapping.Second + 1);
            内容 = s1 + s2;
            int removeCount = mapping.Second - mapping.First;
            for (int i = index; i < mMappingToRichPos.Count; ++i)
            {
                mMappingToRichPos[i].First -= removeCount;
                mMappingToRichPos[i].Second -= removeCount;
            }
            mMappingToRichPos.RemoveAt(0);
            重新解析 = true;
        }
        void ClearText()
        {
            内容 = "";
            mMappingToRichPos.Clear();
            Reset();
        }
        public virtual void Clear()
        {
            StopAllCoroutines();
            ClearText();
            mParsing = false;
        }
        public void ResetPos(Vector3 pos)
        {
            重新计算边界 = true;
            FirstRunning = true;
            mCacheTrans.localPosition = pos;
        }
        bool DoAddImage(string p, string n)
        {
            if (NextTextIsNotShow)
                return true;
            if (行数限制 > 0 && mNowLineCount >= 行数限制)
                return true;
            UIAtlasType at = UIAtlasType.max;
            for (UIAtlasType i = UIAtlasType.公用数字图包1; i < UIAtlasType.max; ++i)
            {
                if(p == i.ToString())
                {
                    at = i;
                    break;
                }
            }
            if (at < UIAtlasType.公用数字图包1 || at >= UIAtlasType.max)
                return false;
            //Color color = mFontColorTagStack.Peek();
            UISprite sp = UIViewHelper.AddUISprite(底板.gameObject, at, n, null);
            if (null == sp)
                return false;
            float canUseSpace = CalcNowLineCanUsedSpace();
            if(sp.width > canUseSpace)
            {
                Destroy(sp.gameObject);
                if (DoChangeLine())
                    DoAddImage(p, n);
                return true;
            }
            int depth = 0;
            if (!mHasedAtlas.Contains(at))
            {
                mHasedAtlas.Add(at);
                depth = mHasedAtlas.Count - 1;
            }
            else
                depth = mHasedAtlas.IndexOf(at);
            float size_w = mTransSizeWStack.Peek();
            float size_h = mTransSizeHStack.Peek();
            bool sliced = false;
            if (size_w > 0)
            {
                sp.width = (int)size_w;
                sliced = true;
            }
            if (size_h > 0)
            {
                sp.height = (int)size_h;
                sliced = true;
            }
            if (sliced)
                sp.type = UISprite.Type.Sliced;
            else
                sp.MakePixelPerfect();
            sp.depth = 层级 + depth;
            OnUnitAddBefore();
            Vector3 pos = new Vector3(mNowWritePos.x - (视图尺寸.x - (float)sp.width) * 0.5f, mNowWritePos.y - sp.height * 0.5f);
            sp.transform.localPosition = pos;
            OnUnitAdded(sp);
            return true;
        }
        bool DoAddImageAnim(string p, string n, string m, int fps)
        {
            if (NextTextIsNotShow)
                return true;
            if (行数限制 > 0 && mNowLineCount >= 行数限制)
                return true;
            UIAtlasType at = UIAtlasType.max;
            for (UIAtlasType i = UIAtlasType.公用数字图包1; i < UIAtlasType.max; ++i)
            {
                if (p == i.ToString())
                {
                    at = i;
                    break;
                }
            }
            if (at < UIAtlasType.公用数字图包1 || at >= UIAtlasType.max)
                return false;
            //Color color = mFontColorTagStack.Peek();
            UISprite sp = UIViewHelper.AddUISprite(底板.gameObject, at, n + m, null);
            if (null == sp)
                return false;
            float canUseSpace = CalcNowLineCanUsedSpace();
            if (sp.width > canUseSpace)
            {
                Destroy(sp.gameObject);
                if (DoChangeLine())
                    DoAddImageAnim(p, n, m, fps);
                return true;
            }
            int depth = 0;
            if (!mHasedAtlas.Contains(at))
            {
                mHasedAtlas.Add(at);
                depth = mHasedAtlas.Count - 1;
            }
            else
                depth = mHasedAtlas.IndexOf(at);
            UISpriteAnimation spanim = sp.gameObject.AddComponent<UISpriteAnimation>();
            spanim.namePrefix = n;
            spanim.framesPerSecond = fps;
            sp.depth = 层级 + depth;
//			if(sp is UISprite)
//				(sp as UISprite).OnAtlasLoadOvered0 += new UISpriteEx.EventHandler0(spanim.RebuildSpriteList);
            OnUnitAddBefore();
            Vector3 pos = new Vector3(mNowWritePos.x - (视图尺寸.x - (float)sp.width) * 0.5f, mNowWritePos.y - sp.height * 0.5f);
            sp.transform.localPosition = pos;
            OnUnitAdded(sp);
            return true;
        }
        bool DoAddNumber(string t)
        {
            if (NextTextIsNotShow)
                return true;
//            string text = SharedRichNumberText.FindNumber(t);
//            if (string.IsNullOrEmpty(text))
//                return false;
//            return OnCreateText(text);
            return OnCreateText(t);
        }
        void UpdatePivot(Vector3 size)
        {
            if (null != 底板)
            {
                底板.width = (int)size.x;
                底板.height = (int)size.y;
                Vector3 ctxPos = Vector3.zero;
                switch (内容锚定方向)
                {
					case UIWidget.Pivot.Top:
						边界.center = new Vector3(0, -size.y * 0.5f, 0);
						break;
                    case UIWidget.Pivot.Center:
                        ctxPos.y = size.y * 0.5f;
                        break;
                    case UIWidget.Pivot.Bottom:
                        ctxPos.y = size.y;
                        break;
                }
                底板.transform.localPosition = ctxPos;
            }
        }
        public Bounds GetBounds()
        {
            if (重新计算边界)
            {
                重新计算边界 = false;
                Bounds old = 边界;
                Transform trans = null;
                if (null != 底板)
                    trans = 底板.transform;
                else
                    trans = mCacheTrans;
				边界 = NGUIMath.CalculateRelativeWidgetBounds(trans, false);
                if (边界.size == Vector3.zero)
                {
                    边界.size = new Vector3(视图尺寸.x, 视图尺寸.y);
                    边界.center = new Vector3(0, -视图尺寸.y * 0.5f);
                }
                else
                {
                    Vector3 bundSize = 边界.size;
                    bundSize.x = 视图尺寸.x;
                    if (bundSize.y > ((-mNowWritePos.y) + mNowLineHeight))
                        bundSize.y = (-mNowWritePos.y) + mNowLineHeight;
                    边界.size = bundSize;
                    边界.center = new Vector3(0, -边界.size.y * 0.5f);
                }
                UpdatePivot(边界.size);
                //if (old != 边界)
                {
                    if (null != OnChangeHeight)
						OnChangeHeight(old, 边界, 关联最大高度);
                }
                if (null != 关联高度面板)
				{
					int h = Mathf.CeilToInt(边界.size.y);
					if(关联最大高度 > 0)
					{
						if(h > 关联最大高度)
						{
							关联高度面板.height = 关联最大高度 + 关联附加高度;
							UIAnchor anchor = GetComponent<UIAnchor>();
							if(null != anchor)
								anchor.enabled = false;
						}
						else
							关联高度面板.height = h + 关联附加高度;
					}
					else
						关联高度面板.height = h + 关联附加高度;
				}
            }
            return 边界;
        }
        public Bounds Bounds { get { return GetBounds(); } }
#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            Bounds b = Bounds;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = new Color(0f, 0.4f, 1f);
            Vector3 size = b.size;
            if (size.x <= 0)
                size.x = 2;
            if (size.y <= 0)
                size.y = 2;
            b.size = size;
            Gizmos.DrawWireCube(new Vector3(b.center.x, b.center.y, b.min.z), new Vector3(b.size.x, b.size.y, 0f));
        }
#endif
    }
}