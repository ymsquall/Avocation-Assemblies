using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UIView.Control;

namespace UIView.Popup
{
    public partial class PopupDialogView : UIViewFadePanel
    {
        public static PopupDialogView PopupAB(PopupDialogType t, AssetBundle bundle, params object[] ps)
        {
            return PopupAB(t, bundle, null, null, ps);
        }
        public static PopupDialogView PopupAB(PopupDialogType t, AssetBundle bundle, ButtonEventHandler callback, params object[] ps)
        {
            return PopupAB(t, bundle, null, callback, ps);
        }
        public static PopupDialogView PopupAB(PopupDialogType t, AssetBundle bundle, string richPath, object[] richPs, ButtonEventHandler callback, params object[] ps)
        {
            return PopupAB(t, bundle, null, richPath, richPs, callback, ps);
        }
        public static PopupDialogView PopupAB(PopupDialogType t, AssetBundle bundle, GameObject parent, params object[] ps)
        {
            return PopupAB(t, bundle, parent, null, ps);
        }
        public static PopupDialogView PopupAB(PopupDialogType t, AssetBundle bundle, GameObject parent, ButtonEventHandler callback, params object[] ps)
        {
            return PopupAB(t, bundle, parent, "", new object[] { }, callback, ps);
        }
        public static PopupDialogView PopupAB(PopupDialogType t, AssetBundle bundle, GameObject parent, string richPath, object[] richPs, ButtonEventHandler callback, params object[] ps)
        {
            if (null == NGUI2DRootPanel.Inst)
                return null;
#if SDK_HM
			PlatformSDKInterface.LoginLocked = false;
#endif
            for (int i = 0; i < mShowDialogs.Count; ++i)
            {
                if ((t == mShowDialogs[i].DialogType || mShowDialogs[i].关联类型.Contains(t)) && mShowDialogs[i].唯一)
                {
                    if (mShowDialogs[i].唯一等待)
                    {
                        mWaitShowDialogs.Add(new object[] { t, parent, richPath, richPs, callback, ps });
                    }
                    return mShowDialogs[i];
                }
            }
            if (null == parent)
            {
                parent = NGUI2DRootPanel.Inst.gameObject;
            }
            PopupDialogView dialog = null;
            string dialogName = "Popup_" + t.ToString();
            dialogName = dialogName.ToLower();
            GameObject dialogObj = bundle.LoadAsset<GameObject>(dialogName);
            if (null == dialogObj)
            {
                return null;
            }
            GameObject dlg = Instantiate(dialogObj);
            dialog = dlg.GetComponent<PopupDialogView>();
            if (null != dialog)
            {
                OnDialogLoadOvered(t, dialog, parent.transform, richPath, richPs, callback, ps);
            }
            //bundle.Unload(false);
            //bundle = null;
            dialogObj = null;
            Resources.UnloadUnusedAssets();
            dialog.FadeIn();
            return dialog;
        }
    }
}