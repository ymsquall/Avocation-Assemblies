using Framework.Tools;
using UnityEngine;

namespace UIView.Control
{
    [ExecuteInEditMode]
    [AddComponentMenu("NGUI/UI/UIHyperLinkLabel")]
    public class UIHyperLinkLabel : UILabel
    {
        public delegate void LinkEventHandler(UIHyperLinkLabel sender, object data);
        public event LinkEventHandler OnHyperLinkClicked;

        public bool 重置下划线长度 = true;
        //Rect mInnerUV = new Rect();
        //Rect mOuterUV = new Rect();
        Vector2 mUnderlineSize = Vector2.one;
		object mLinkDataSource = null;
		string mUnderLineColor = "";
        object mLinkDataEnable = null;
        BoxCollider mCollider = null;
        BoxCollider2D mCollider2D = null;
        UISprite mUnderLine = null;
        Color mEnableColor = Color.white;
        Vector2 UnderlineSize
        {
            get
            {
                if (重置下划线长度)
                {
                    mUnderlineSize = new Vector2(width+2, 2);
                    重置下划线长度 = false;
                }
                return mUnderlineSize;
            }
        }
        public object LinkDataSource
        {
            set { mLinkDataSource = value; }
            get { return mLinkDataSource; }
		}
		public string UnderLineColor
		{
			set { mUnderLineColor = value; }
			get { return mUnderLineColor; }
		}
        public object LinkDataEnable
        {
            set
            {
                if (mLinkDataEnable != value)
                {
                    mLinkDataEnable = value;
                    CheckCanClick();
                }
            }
            get { return mLinkDataEnable; }
        }
        public float Alpha
        {
            set
            {
                Color c = color;
                c.a = value;
                color = c;
                if(null != mUnderLine)
                {
                    c = mUnderLine.color;
                    c.a = value;
                    mUnderLine.color = c;
                }
            }
        }
	    protected override void OnStart ()
        {
            mEnableColor = color;
            base.OnStart();
			mUnderLine = UIViewHelper.AddUISprite(gameObject, UIAtlasType.D_jiugongge, "分割线1", null,
			                                                UISprite.Type.Sliced, (int)UnderlineSize.x, (int)UnderlineSize.y);
			if(string.IsNullOrEmpty(mUnderLineColor))
				mUnderLine.color = color;
			else
			{
				Color c = StringUtils.ColorStr2U3DColor(mUnderLineColor);
				mUnderLine.color = c;
			}
            mUnderLine.transform.localPosition = new Vector3(0, -height * 0.5f - 2);
            mUnderLine.depth = 1;
            CheckCanClick();
            if (null == mCollider && null == mCollider2D)
            {
                UIWidget w = NGUITools.AddWidget<UIWidget>(gameObject);
                w.width = width;
                w.height = height + (null != mUnderLine ? mUnderLine.height : 0);
                NGUIUtil.UpdateWidgetCollider(w, out mCollider, out mCollider2D, true); ;
            }
            else
            {
                UIWidget w = null;
                if(null != mCollider)
                    w = mCollider.GetComponent<UIWidget>();
                else
                    w = mCollider2D.GetComponent<UIWidget>();
                w.width = width;
                w.height = height + (null != mUnderLine ? mUnderLine.height : 0);
                if (null != mCollider)
                    NGUITools.UpdateWidgetCollider(mCollider, false);
                else
                    NGUITools.UpdateWidgetCollider(mCollider2D, false);
            }
            UIButtonMessage btnMsg = null;
            if (null != mCollider)
            {
                mCollider.gameObject.AddComponent<UIDragScrollView>();
                btnMsg = mCollider.GetComponent<UIButtonMessage>();
                if (null == btnMsg)
                    btnMsg = mCollider.gameObject.AddComponent<UIButtonMessage>();

            }
            if (null != mCollider2D)
            {
                mCollider2D.gameObject.AddComponent<UIDragScrollView>();
                btnMsg = mCollider2D.GetComponent<UIButtonMessage>();
                if (null == btnMsg)
                    btnMsg = mCollider2D.gameObject.AddComponent<UIButtonMessage>();
            }
            btnMsg.target = gameObject;
            btnMsg.functionName = "OnTigger";
        }
        void CheckCanClick()
        {
            if (null == mCollider && null == mCollider2D)
            {
                UIWidget w = NGUITools.AddWidget<UIWidget>(gameObject);
                w.width = width;
                w.height = height + (null != mUnderLine ? mUnderLine.height : 0);
                NGUIUtil.UpdateWidgetCollider(w, out mCollider, out mCollider2D, true);
            }
            if (null != mLinkDataEnable)
            {
                string e = mLinkDataEnable as string;
                if (!string.IsNullOrEmpty(e))
                {
                    if (e.Contains("false"))
                    {
                        if(null != mCollider)
                            mCollider.enabled = false;
                        if (null != mCollider2D)
                            mCollider2D.enabled = false;
                        mEnableColor = color;
                        color = Color.gray;
                        if (null != mUnderLine)
						{
							if(string.IsNullOrEmpty(mUnderLineColor))
								mUnderLine.color = color;
							else
							{
								Color c = StringUtils.ColorStr2U3DColor(mUnderLineColor);
								mUnderLine.color = c;
							}
						}
                    }
                    else if (!mCollider.enabled)
                    {
                        if (null != mCollider)
                            mCollider.enabled = true;
                        color = mEnableColor;
                        if (null != mUnderLine)
						{
							if(string.IsNullOrEmpty(mUnderLineColor))
								mUnderLine.color = color;
							else
							{
								Color c = StringUtils.ColorStr2U3DColor(mUnderLineColor);
								mUnderLine.color = c;
							}
						}
                    }
                    else if (!mCollider2D.enabled)
                    {
                        if (null != mCollider2D)
                            mCollider2D.enabled = true;
                        color = mEnableColor;
                        if (null != mUnderLine)
						{
							if(string.IsNullOrEmpty(mUnderLineColor))
								mUnderLine.color = color;
							else
							{
								Color c = StringUtils.ColorStr2U3DColor(mUnderLineColor);
								mUnderLine.color = c;
							}
						}
                    }
                }
            }
        }
        void OnTigger()
        {
            if (null != OnHyperLinkClicked)
                OnHyperLinkClicked(this, mLinkDataSource);
        }
    }
}