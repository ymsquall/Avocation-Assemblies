using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UIView.Control;

namespace UIView.Popup
{
    public enum PopupDialogType
    {
        Confirm,
        CancelOrConfirm,
        Bubble,
        FightResult,
        QuestTypeSelect,
        UserInfo,
    }
    public enum PopupDialogButtonType
    {
        Confirm,
        Cancel,
        Retry,
    }
    public partial class PopupDialogView : UIViewFadePanel
    {
        const string PrefixName = "UIView/Popup_";
        public delegate bool ButtonEventHandler(PopupDialogButtonType bt, params object[] ps);
        public event ButtonEventHandler OnButtonClicked = null;
        public delegate void DialogEventHandleer(PopupDialogType t, PopupDialogView view, Transform parent, string richPath, object[] richPs, ButtonEventHandler callback, object[] ps);
        public UILabel 标题 = null;
        public SimpleRichText 内容 = null;
        public UILabel 内容1 = null;
        public PopupDialogType DialogType = PopupDialogType.Confirm;
        public List<PopupDialogType> 关联类型 = new List<PopupDialogType>();
        public bool 唯一 = false;
        public bool 唯一等待 = false;
        public float 倒计时 = 0;
        public UILabel 时间 = null;
        public UILabel 按钮1 = null;
        public UILabel 按钮2 = null;
        public UILabel 按钮3 = null;
        object[] mParams = null;
        static List<PopupDialogView> mShowDialogs = new List<PopupDialogView>();
        static List<object[]> mWaitShowDialogs = new List<object[]>();
        protected override bool InitView()
        {
            if (!base.InitView())
                return false;
            if (null != 时间)
            {
                if (倒计时 > 0.001f)
                {
                    时间.gameObject.SetActive(true);
                    StartCoroutine(DoTimer());
                }
                else
                    时间.gameObject.SetActive(false);
            }
            return true;
        }
        IEnumerator DoTimer()
        {
            int timer = Mathf.CeilToInt(倒计时);
            时间.text = "(" + timer.ToString() + "S)";
            while(timer > 0)
            {
                yield return new WaitForSeconds(1);
                timer--;
                时间.text = "(" + timer.ToString() + "S)";
            }
            OnClose();
        }
        public static int ShowingCount
        {
            get { return mShowDialogs.Count; }
        }
        public static void CloseDialogs(params PopupDialogType[] ps)
        {
            for(int i = 0; i < mShowDialogs.Count; ++ i)
            {
                bool close = true;
                for(int j = 0; j < ps.Length; ++ j)
                {
                    if (null == mShowDialogs[i])
                        continue;
                    if(ps[j] == mShowDialogs[i].DialogType)
                    {
                        close = false;
                        break;
                    }
                }
                if (close && null != mShowDialogs[i])
                    mShowDialogs[i].FadeOut();
            }
        }
        public static PopupDialogView Popup(PopupDialogType t, params object[] ps)
        {
            return Popup(t, null, null, ps);
        }
        public static PopupDialogView Popup(PopupDialogType t, ButtonEventHandler callback, params object[] ps)
        {
            return Popup(t, null, callback, ps);
        }
        public static PopupDialogView Popup(PopupDialogType t, string richPath, object[] richPs, ButtonEventHandler callback, params object[] ps)
        {
            return Popup(t, null, richPath, richPs, callback, ps);
        }
        public static PopupDialogView Popup(PopupDialogType t, GameObject parent, params object[] ps)
        {
            return Popup(t, parent, null, ps);
        }
        public static PopupDialogView Popup(PopupDialogType t, GameObject parent, ButtonEventHandler callback, params object[] ps)
        {
            return Popup(t, parent, "", new object[] { }, callback, ps);
        }
        public static PopupDialogView Popup(PopupDialogType t, GameObject parent, string richPath, object[] richPs, ButtonEventHandler callback, params object[] ps)
        {
            if (null == NGUI2DRootPanel.Inst)
				return null;
#if SDK_HM
			PlatformSDKInterface.LoginLocked = false;
#endif
            for (int i = 0; i < mShowDialogs.Count; ++ i)
            {
                if ((t == mShowDialogs[i].DialogType || mShowDialogs[i].关联类型.Contains(t)) && mShowDialogs[i].唯一)
                {
                    if (mShowDialogs[i].唯一等待)
                    {
                        mWaitShowDialogs.Add(new object[]{t, parent, richPath, richPs, callback, ps});
                    }
                    return mShowDialogs[i];
                }
            }
            if (null == parent)
                parent = NGUI2DRootPanel.Inst.gameObject;
            PopupDialogView dialog = null;
            string dialogName = PrefixName + t.ToString();
            GameObject dialogObj = Resources.Load(dialogName) as GameObject;
            if(null == dialogObj)
                return null;
            GameObject dlg = Instantiate(dialogObj);
            dialog = dlg.GetComponent<PopupDialogView>();
            if (null != dialog)
                OnDialogLoadOvered(t, dialog, parent.transform, richPath, richPs, callback, ps);
            dialogObj = null;
            Resources.UnloadUnusedAssets();
            dialog.FadeIn();
//            string atlasList = LogicModel.GetResName(ResourceType.UINeedAtlas, dialogName);
//            dialog = UIResourceManager.LoadUIDialog(dialogName, atlasList, t, parent.transform, richPath, richPs, callback, ps, OnDialogLoadOvered);
            return dialog;
        }
        public static void OnDialogLoadOvered(PopupDialogType t, PopupDialogView view, Transform parent, string richPath, object[] richPs, ButtonEventHandler callback, object[] ps)
        {
            if (null != view)
            {
                view.transform.parent = parent;
                view.transform.localScale = UnityEngine.Vector3.one;
                view.transform.localPosition = UnityEngine.Vector3.zero;
                view.transform.localRotation = Quaternion.identity;
                view.UI类型 = UIViewType.弹出框;
                if (null != view.内容 && !string.IsNullOrEmpty(richPath))
                {
                    view.内容.地址 = richPath;
                    view.内容.SetParam(richPs);
                }
                //				if (ItemGridsViewPanel.InstCount > 0)
                //				{
                //					Camera cam = ItemGridsViewPanel.Inst.RenderTargetUpperRoot.GetComponentInChildren<Camera>();
                //					UIAnchor anchor = view.GetComponent<UIAnchor>();
                //					if (null != anchor)
                //						anchor.uiCamera = cam;
                //					UIAnchor[] anchors = view.GetComponentsInChildren<UIAnchor>();
                //					for (int i = 0; i < anchors.Length; ++i)
                //						anchors[i].uiCamera = cam;
                //				}
            }
            switch (t)
            {
                case PopupDialogType.Confirm:
                case PopupDialogType.CancelOrConfirm:
                    {
                        if (null != view)
                        {
                            int index = 0;
                            if (ps.Length >= 1)
                            {
                                if (ps.Length >= 2)
                                {
                                    if (!(ps[index] is string))
                                        view.标题.text = ps[index].ToString();
                                    else
                                        view.标题.text = ps[index] as string;
                                    index++;
                                }
                                if (ps[index] is string && !string.IsNullOrEmpty(ps[index] as string))
                                {
                                    if (null != view.内容)
                                    {
                                        if (!(ps[index] is string))
                                            view.内容.SetText(ps[index].ToString());
                                        else
                                            view.内容.SetText(ps[index] as string);
                                    }
                                    if (null != view.内容1)
                                    {
                                        if (!(ps[index] is string))
                                            view.内容1.text = ps[index].ToString();
                                        else
                                            view.内容1.text = ps[index] as string;
                                    }
                                	index++;
                                }
                                if (ps.Length >= index + 1)
                                {
                                    string[] btnText = ps[index] as string[];
                                    if (null != btnText)
                                    {
                                        int btnIndex = 0;
                                        if (null != view.按钮1 && btnIndex < btnText.Length)
                                            view.按钮1.text = btnText[btnIndex++] as string;
                                        if (null != view.按钮2 && btnIndex < btnText.Length)
                                            view.按钮2.text = btnText[btnIndex++] as string;
                                        if (null != view.按钮3 && btnIndex < btnText.Length)
                                            view.按钮3.text = btnText[btnIndex++] as string;
                                        index++;
                                    }
                                }
                                if (ps.Length >= index + 1)
                                {
                                    if (ps[index] is float)
                                    {
                                        view.倒计时 = (float)ps[index];
                                        index++;
                                    }
                                }
                                if (ps.Length >= index + 1)
                                {
                                    if (ps[index] is object[])
                                    {
                                        view.SetParam(ps[index] as object[]);
                                    }
                                    else
                                    {
                                        List<object> psList = new List<object>();
                                        for (int i = index; i < ps.Length; ++i)
                                            psList.Add(ps[i]);
                                        view.SetParam(psList.ToArray() as object[]);
                                    }
                                }
                            }
                        }
                    }
                    break;
                default:
                    {
                        if (null != view)
                            view.SetParam(ps);
                    }
                    break;
            }
            if (null != view)
            {
                if (null != callback)
                    view.OnButtonClicked += new ButtonEventHandler(callback);
                view.DialogType = t;
                //if (view.唯一)
                    mShowDialogs.Add(view);
            }
        }
        public static void ResetDialogsCamera()
        {
           // NoFightMainView mainView = NoFightMainView.Inst;
            NGUI2DRootPanel uiRoot = NGUI2DRootPanel.Inst;
            Camera cam = uiRoot.mUICamera.GetComponent<Camera>();
            for (int i = 0; i < mShowDialogs.Count; ++i)
            {
                if (mShowDialogs[i].DialogType == PopupDialogType.Bubble && mShowDialogs[i].DialogType == PopupDialogType.Bubble)
                {
//                    if (null != mainView)
//                        mShowDialogs[i].transform.parent = mainView.transform;
                }
                else
                    mShowDialogs[i].transform.parent = uiRoot.transform;
                UIAnchor anchor = mShowDialogs[i].GetComponent<UIAnchor>();
                if(null != anchor)
                {
                    anchor.uiCamera = cam;
                    anchor.enabled = true;
                }
                UIAnchor[] anchors = mShowDialogs[i].GetComponentsInChildren<UIAnchor>();
                for (int n = 0; n < anchors.Length; ++n)
                {
                    anchors[n].uiCamera = cam;
                    anchors[n].enabled = true;
                }
            }
        }
        public virtual void SetParam(params object[] ps)
        {
            mParams = ps;
        }
        public static void CloseDialog(PopupDialogType t, bool force, bool all)
        {
            for (int i = 0; i < mShowDialogs.Count; ++ i)
            {
                if(t == mShowDialogs[i].DialogType && (mShowDialogs[i].唯一 || force))
                {
                    mShowDialogs[i].OnClose();
                    if(!all)
                        return;
                }
            }
            for (int i = 0; i < mWaitShowDialogs.Count;)
            {
                if (t == (PopupDialogType)mWaitShowDialogs[i][0])
                {
                    mWaitShowDialogs.RemoveAt(i);
                    if (!all)
                        return;
                }
                else
                    i++;
            }
        }
		public static void ClearWaitDialogs()
		{
			mWaitShowDialogs.Clear ();
		}
        protected virtual object[] GetParams(PopupDialogButtonType t)
        {
            if (null != mParams)
                return mParams;
            return new object[] { };
        }
        void CheckDialogClose()
        {
            if (mShowDialogs.Contains(this))
                mShowDialogs.Remove(this);
            for (int i = 0; i < mWaitShowDialogs.Count; ++i)
            {
                object[] ps = mWaitShowDialogs[i];
                if (DialogType == (PopupDialogType)ps[0])
                {
                    if (null != PopupDialogView.Popup((PopupDialogType)ps[0], ps[1] as GameObject, ps[2] as string, ps[3] as object[], ps[4] as ButtonEventHandler, ps[5] as object[]))
                    {
                        mWaitShowDialogs.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        public virtual void OnConfirm()
        {
            if (null != OnButtonClicked)
            {
                if (OnButtonClicked(PopupDialogButtonType.Confirm, GetParams(PopupDialogButtonType.Confirm)))
                {
                    CheckDialogClose();
                    base.OnClose();
                }
            }
        }
        void OnRetry()
        {
            if (null != OnButtonClicked)
            {
                if (OnButtonClicked(PopupDialogButtonType.Retry, GetParams(PopupDialogButtonType.Retry)))
                {
                    CheckDialogClose();
                    base.OnClose();
                }
            }
        }
        public override void OnClose()
        {
            if (null != OnButtonClicked)
            {
                if (OnButtonClicked(PopupDialogButtonType.Cancel, GetParams(PopupDialogButtonType.Cancel)))
                {
                    CheckDialogClose();
                    base.OnClose();
                }
            }
            else
            {
                CheckDialogClose();
                base.OnClose();
            }
        }
        protected override void DestroyImpl()
        {
            if (mShowDialogs.Contains(this))
                mShowDialogs.Remove(this);
            //if (null != NGUI2DRootPanel.Inst)
              //  NGUI2DRootPanel.Inst.CheckAutoLastUIs();
        }
    }
}