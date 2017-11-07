using System;
using System.Collections.Generic;
using UnityEngine;

namespace UIView
{
#if UNITY_EDITOR
    public abstract class UIBaseView : MonoBehaviour
#else
    public abstract class UIBaseView : MonoBehaviour
#endif
    {
        public delegate void FadeEventHandler(UIBaseView sender);
        public event FadeEventHandler OnFadeInOvered;
        public event FadeEventHandler OnFadeOutOvered;
        public delegate void DestroyEventHandler(UIBaseView sender);
        public event DestroyEventHandler OnDestroyed;
        public UIViewType UI类型 = UIViewType.max;
        public bool 面板区域移动 = false;
		public UIPanel RootPanel = null;
        //public bool 忽略面板 = false;
        protected bool mInited = false;
        protected bool mReadyShowing = false;
        List<UIAtlasType> mRefAtlasList = new List<UIAtlasType>();
        List<UIAtlasType> mWaitAtlasList = new List<UIAtlasType>();
        public bool IsReadyShow { get { return mReadyShowing; } }
        public virtual bool Closeing { get { return false; } }
        public void AddRefAtlas(UIAtlasType t)
        {
            mRefAtlasList.Add(t);
            mWaitAtlasList.Add(t);
        }
        public void OnAtlasLoaded(UIAtlasType t)
        {
            if (mWaitAtlasList.Contains(t))
                mWaitAtlasList.Remove(t);
            if (mWaitAtlasList.Count <= 0)
            {
                mReadyShowing = true;
                DoReadyShowing();
            }
        }
        public void DoReadyShowing()
        {
            OnReadyShowing();
//            UIViewHelper.OnUIReadyed(UI类型);
        }
        protected abstract void OnReadyShowing();
		public abstract void SetWhiteBlockTex (Texture2D tex);
#if UNITY_EDITOR
        public virtual void ExportSettings()
        {
            UI类型 = UIViewType.Root2D;
            //if (忽略面板)
            //    return;
            if (null == RootPanel)
                RootPanel = GetComponent<UIPanel>();
            if (null == RootPanel)
            {
                RootPanel = gameObject.AddComponent<UIPanel>();
                //RootPanel.CacheComps();
            }
        }
#endif
        protected virtual void Start()
        {
            if (!mInited)
                InitView();
        }
        protected virtual bool InitView()
        {
            if (mInited)
                return true;
            if (null == NGUI2DRootPanel.Inst || null == RootPanel)
            {
                if (null == NGUI2DRootPanel.Inst)
                {
                    Debug.LogError("创建UI[" + name + "]时NGUI2DRootPanel还未创建！");
                    NGUITools.Destroy(gameObject);
                    return false;
                }
                //if (null == RootPanel && !忽略面板)
                if (null == RootPanel)
                {
                    Debug.LogError("创建UI[" + name + "]时发现该UI没有挂载面板！");
                    NGUITools.Destroy(gameObject);
                    return false;
                }
            }
            //if (!忽略面板)
                RootPanel.widgetsAreStatic = 面板区域移动;
            if (!InitViewImpl())
            {
                NGUITools.Destroy(gameObject);
                return false;
            }
            mInited = true;
            NGUI2DRootPanel.Inst.OnUIActived(this);
            return true;
        }
        protected virtual bool InitViewImpl()
        {
            return true;
        }
        public virtual bool IsFadePanel { get { return false; } }
        public virtual bool OnRefushUI(params object[] ps) { return true; }
        public virtual void OnGoTo() { }
        public virtual void OnGoBack() { }
        public virtual void OnFadeOuted() { }
        protected virtual void OnFadeOvered(bool fadeIn)
        {
            if (fadeIn)
            {
                if (null != OnFadeInOvered)
                    OnFadeInOvered(this);
            }
            else
            {
                if (null != OnFadeOutOvered)
                    OnFadeOutOvered(this);
            }
        }
        public virtual void OnClose() { }
        public virtual void OnClosed() { }
        protected virtual void DestroyImpl() { }
        void OnDestroy()
        {
            DestroyImpl();
            if (null != OnDestroyed)
                OnDestroyed(this);
//            if (null != UIResourceManager.Inst)
//                UIResourceManager.Inst.RemoveAtlasList(mRefAtlasList.ToArray());
//            UIViewHelper.OnUIReadyed(UI类型);
        }
    }
}
