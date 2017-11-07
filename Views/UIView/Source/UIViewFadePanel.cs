using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIView
{
    public class UIViewFadePanel : UIBaseView
    {
        public UITexture Background = null;
        public BoxCollider LockScreenCollider = null;
        public BoxCollider2D LockScreenCollider2D = null;
        public GameObject LockMeCollider = null;
        public Color 淡入背景色 = new Color(0, 0, 0, 200);
        public Color 淡出背景色 = new Color(0, 0, 0, 0);
        public bool 淡入后锁定 = true;
        public bool 淡出后禁用 = true;
        public bool 淡出后自动销毁 = true;
        public UIPanel[] 关联淡入淡出面板 = null;
		public List<UIPanel> 忽略淡入关联面板 = new List<UIPanel> ();
        public float 淡入时间 = 0.5f;
		public float 淡出时间 = 0.5f;
		public UIAnchor RootAnchor = null;
        float mFadeTime = 0.0f;
        float mFadeSpeed = 0.0f;
        protected bool mFadeIn = false;
        protected bool mFadeOut = false;
        protected bool mFadeOuted = false;
        protected bool mFadeIned = false;
        bool mFadeOutedIdel = false;
        bool mAutoDestroyedInGoingTo = false;
        public override bool IsFadePanel { get { return true; } }
        public bool InFadeIn { get { return mFadeIn; } }
        public bool InFadeOut { get { return mFadeOut; } }
        public bool FadeOuted { get { return mFadeOuted || mFadeOutedIdel; } }
        public bool FadeIned { get { return mFadeIned; } }
        public override bool Closeing { get { return mFadeOut; } }
        protected override void OnReadyShowing()
        {
            StartCoroutine(DoCheckFadeIn());
        }
        IEnumerator DoCheckFadeIn()
        {
            while (!CheckCanFadeIn())
                yield return 0;
            OnEnterFadeIn();
        }
        protected virtual bool CheckCanFadeIn() { return mInited; }
        protected virtual void OnEnterFadeIn()
        {
            FadeIn();
        }
        public override void OnFadeOuted()
        {
            if (淡出后自动销毁)
            {
                GameObject.DestroyObject(gameObject);
                return;
            }
            else
            {
                if (null != LockScreenCollider)
                    LockScreenCollider.enabled = false;
                if (null != LockScreenCollider2D)
                    LockScreenCollider2D.enabled = false;
                if (淡出后禁用)
                {
                    NGUI2DRootPanel.Inst.OnUIDisabled(this);
                    gameObject.SetActive(false);
                }
            }
            mFadeOuted = false;
            mFadeOutedIdel = true;
        }
		public override void SetWhiteBlockTex (Texture2D tex)
		{
			if (null != Background)
			{
				Background.mainTexture = tex;
				Background.color = 淡出背景色;
			}
		}
#if UNITY_EDITOR
        public override void ExportSettings()
        {
            base.ExportSettings();
            if (null != LockScreenCollider)
                LockScreenCollider.enabled = true;
            if (null != LockScreenCollider2D)
                LockScreenCollider2D.enabled = true;
            if (null != LockMeCollider)
                LockMeCollider.SetActive(true);
        }
#endif
        public override void OnGoTo()
        {
            mAutoDestroyedInGoingTo = 淡出后自动销毁;
            淡出后自动销毁 = false;
            FadeOut();
        }
        public override void OnGoBack()
        {
            淡出后自动销毁 = mAutoDestroyedInGoingTo;
            FadeIn();
        }
        public void FadeIn(float t)
        {
            if (mFadeIn)
                return;
            float ot = 淡入时间;
            淡入时间 = t;
            FadeIn();
            淡入时间 = ot;
        }
        public void FadeIn()
        {
            mFadeIned = false;
            if (mFadeIn)
                return;
            if (!mInited)
                InitView();
            RootPanel.alpha = 0;
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
            关联淡入淡出面板 = GetComponentsInChildren<UIPanel>();
            if (淡入时间 <= 0)
            {
                RootPanel.alpha = 淡入背景色.a;
                Background.color = 淡入背景色;
                if (null != 关联淡入淡出面板)
				{
                    for (int i = 0; i < 关联淡入淡出面板.Length; ++i)
					{
						if(!忽略淡入关联面板.Contains(关联淡入淡出面板[i]))
                        	关联淡入淡出面板[i].alpha = RootPanel.alpha;
					}
				}
                OnFadeOvered(true);
                return;
			}
			if (null != 关联淡入淡出面板)
			{
				for (int i = 0; i < 关联淡入淡出面板.Length; ++i)
				{
					if(!忽略淡入关联面板.Contains(关联淡入淡出面板[i]))
						关联淡入淡出面板[i].alpha = RootPanel.alpha;
				}
			}
            mFadeTime = 0;
            mFadeSpeed = 1.0f / 淡入时间;
            mFadeIn = true;
            mFadeOut = false;
            if (null != LockScreenCollider)
                LockScreenCollider.enabled = true;
            if (null != LockScreenCollider2D)
                LockScreenCollider2D.enabled = true;
            if (null != LockMeCollider)
                LockMeCollider.SetActive(true);
            if (null != Background)
                Background.color = 淡出背景色;
        }
        public void FadeOut(float t)
        {
            if (mFadeOut)
                return;
            float ot = 淡出时间;
            淡出时间 = t;
            FadeOut();
            淡出时间 = ot;
        }
        public virtual void FadeOut()
        {
            mFadeIned = false;
            if (mFadeOut)
                return;
            if (!mInited)
                InitView();
            RootPanel.alpha = 1;
            关联淡入淡出面板 = GetComponentsInChildren<UIPanel>();
            if (淡出时间 <= 0)
            {
                if (mFadeOuted)
                    OnFadeOuted();
                else
                {
                    RootPanel.alpha = 淡出背景色.a;
                    Background.color = 淡出背景色;
                    if (null != 关联淡入淡出面板)
                        for (int i = 0; i < 关联淡入淡出面板.Length; ++i)
                            关联淡入淡出面板[i].alpha = RootPanel.alpha;
                    OnFadeOvered(false);
                }
                return;
            }
            mFadeTime = 0;
            mFadeSpeed = 1.0f / 淡出时间;
            mFadeIn = false;
            mFadeOut = true;
            if (null != LockScreenCollider)
                LockScreenCollider.enabled = true;
            if (null != LockScreenCollider2D)
                LockScreenCollider2D.enabled = true;
            if (null != LockMeCollider)
                LockMeCollider.SetActive(true);
            if (null != Background)
                Background.color = 淡入背景色;
            if (null != 关联淡入淡出面板)
                for (int i = 0; i < 关联淡入淡出面板.Length; ++i)
                    关联淡入淡出面板[i].alpha = RootPanel.alpha;
        }
        protected override void OnFadeOvered(bool fadeIn)
        {
            mFadeIn = false;
            mFadeOut = false;
            if (null != LockMeCollider)
                LockMeCollider.SetActive(false);
            if (fadeIn)
            {
                if (null != LockScreenCollider)
                    LockScreenCollider.enabled = 淡入后锁定;
                if (null != LockScreenCollider2D)
                    LockScreenCollider2D.enabled = 淡入后锁定;
                mFadeIned = true;
            }
            base.OnFadeOvered(fadeIn);
            mFadeOuted = !fadeIn;
        }
        protected virtual void UpdateBefore() { }
        protected virtual void UpdateAfter() { }
        void Update()
		{
			if(null != RootAnchor && null == RootAnchor.uiCamera)
			{
				NGUI2DRootPanel root = NGUI2DRootPanel.Inst;
				if(null == root || null == root.mUICamera || null == root.mUICamera.GetComponent<Camera>())
					return;
				Camera cam = root.mUICamera.GetComponent<Camera>();
				RootAnchor.uiCamera = cam;
				RootAnchor.enabled = true;
				UIAnchor[] anchors = GetComponentsInChildren<UIAnchor>();
				for (int i = 0; i < anchors.Length; ++i)
				{
					anchors[i].uiCamera = cam;
					anchors[i].enabled = true;
				}
			}
            UpdateBefore();
            if (mFadeIn)
            {
                RootPanel.alpha = 1.0f * (mFadeTime * mFadeSpeed);
				if (RootPanel.alpha > 1.0f) RootPanel.alpha = 1.0f;
				if (null != 关联淡入淡出面板)
				{
					for (int i = 0; i < 关联淡入淡出面板.Length; ++i)
					{
						if(!忽略淡入关联面板.Contains(关联淡入淡出面板[i]))
							关联淡入淡出面板[i].alpha = RootPanel.alpha;
					}
				}
                if(null != Background)
                {
                    Color color1 = 淡出背景色;
                    Color color2 = 淡入背景色;
                    color1.r = color1.r + Mathf.Abs(color2.r - color1.r) * (mFadeTime * mFadeSpeed);
                    color1.g = color1.g + Mathf.Abs(color2.g - color1.g) * (mFadeTime * mFadeSpeed);
                    color1.b = color1.b + Mathf.Abs(color2.b - color1.b) * (mFadeTime * mFadeSpeed);
                    color1.a = color1.a + Mathf.Abs(color2.a - color1.a) * (mFadeTime * mFadeSpeed);
                    if (color1.r > color2.r) color1.r = color2.r;
                    if (color1.g > color2.g) color1.g = color2.g;
                    if (color1.b > color2.b) color1.b = color2.b;
                    if (color1.a > color2.a) color1.a = color2.a;
                    Background.color = color1;
                    Background.alpha = Background.color.a;
                    if (color1.r >= color2.r && color1.g >= color2.g
                         && color1.b >= color2.b && color1.a >= color2.a && RootPanel.alpha >= 1)
                    {
                        OnFadeOvered(true);
                    }
                }
                else if (RootPanel.alpha >= 1)
                    OnFadeOvered(true);
                mFadeTime += Time.deltaTime;
            }
            else if (mFadeOut)
            {
                RootPanel.alpha = 1.0f - mFadeTime * mFadeSpeed;
                if (RootPanel.alpha < 0) RootPanel.alpha = 0;
                if (null != 关联淡入淡出面板)
                    for (int i = 0; i < 关联淡入淡出面板.Length; ++i)
                        关联淡入淡出面板[i].alpha = RootPanel.alpha;
                if (null != Background)
                {
                    Color color1 = 淡入背景色;
                    Color color2 = 淡出背景色;
                    color1.r = color1.r - Mathf.Abs(color2.r - color1.r) * (mFadeTime * mFadeSpeed);
                    color1.g = color1.g - Mathf.Abs(color2.g - color1.g) * (mFadeTime * mFadeSpeed);
                    color1.b = color1.b - Mathf.Abs(color2.b - color1.b) * (mFadeTime * mFadeSpeed);
                    color1.a = color1.a - Mathf.Abs(color2.a - color1.a) * (mFadeTime * mFadeSpeed);
                    if (color1.r < color2.r) color1.r = color2.r;
                    if (color1.g < color2.g) color1.g = color2.g;
                    if (color1.b < color2.b) color1.b = color2.b;
                    if (color1.a < color2.a) color1.a = color2.a;
                    Background.color = color1;
                    Background.alpha = Background.color.a;
                    if (color1.r <= color2.r && color1.g <= color2.g
                         && color1.b <= color2.b && color1.a <= color2.a && RootPanel.alpha <= 0)
                    {
                        OnFadeOvered(false);
                    }
                }
                else if (RootPanel.alpha <= 0)
                    OnFadeOvered(false);
                mFadeTime += Time.deltaTime;
            }
            else if(mFadeOuted)
            {
                OnFadeOuted();
            }
            UpdateAfter();
        }
        public override void OnClose()
        {
            FadeOut();
        }
    }
}
