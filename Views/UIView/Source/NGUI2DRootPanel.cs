using System.Collections.Generic;
using UnityEngine;
using Framework.Tools;
using UIView.Popup;

namespace UIView
{
    public class NGUI2DRootPanel : SingletonMBT<NGUI2DRootPanel>
    {
        static Vector2 mUIViewPortSize = Vector2.zero;
        static bool mUIViewPortGetted = false;
		public static bool IsHHDScreen = false;
        public Camera mUICamera = null;
        public UIRoot mUIRoot = null;
        //public UIPanel 生物Title = null;
        List<UIBaseView> mActivedChilden = new List<UIBaseView>();
        List<UIBaseView> mFadeInedChilden = new List<UIBaseView>();
        List<UIBaseView> mFadeOutedChilden = new List<UIBaseView>();
        List<UIBaseView> mDisableedChilden = new List<UIBaseView>();
#if DEBUG_OUTPUT
        public SimpleRichText 调试信息 = null;
        public static bool 进入场景 = false;
#endif
        RaycastHit[] mTouchsBeginObj = null;
        Vector3 mTouchsBeginWorldPos = Vector3.zero;
        //int mUnlockFuncID = -1;
        object[] mInterchangeParams = null;
        bool mVipExpChanged = false;
		//bool mInWaitUpdateGuide = false;
//		static protected Queue<IconViewItemBase> WaitCheckColliderItemIconQueue = new Queue<IconViewItemBase>();

        public static Vector2 UIViewPortSize
        {
            get
            {
                if(!mUIViewPortGetted)
                {
                    if(null != Inst)
                    {
#if !UNITY_EDITOR
                        if(Screen.height > 768)
                        {
						    Inst.mUIRoot.manualHeight = 768;
						    Inst.mUIRoot.minimumHeight = 768;
							IsHHDScreen = true;
                        }
                        else
                        {
						    Inst.mUIRoot.manualHeight = Screen.height;
						    Inst.mUIRoot.minimumHeight = Screen.height;
							IsHHDScreen = false;
                        }
#endif
                        mUIViewPortSize.y = Inst.mUIRoot.activeHeight;
                        mUIViewPortSize.x = mUIViewPortSize.y * Inst.mUICamera.aspect;
                        mUIViewPortGetted = true;
                    }
                }
                return mUIViewPortSize;
            }
        }
        public float InSceneScale
        {
            get
            {
                if (null == mUIRoot || mUIRoot.activeHeight <= 0)
                    return 1;
                return 2f / mUIRoot.activeHeight;
            }
        }
        void Start()
        {
            //UICamera.genericEventHandler = gameObject;
#if DEBUG_OUTPUT
            AddDebugText(string.Format("UIViewPortSize1 = {0},{1}", UIViewPortSize.x, UIViewPortSize.y));
#endif
        }
#if DEBUG_OUTPUT
        public void AddDebugText(string s)
        {
            AddDebugText(s, false);
        }
        public void AddDebugText(string s, bool clear)
        {
            if (调试信息.gameObject.activeSelf)
            {
                if (clear)
                    调试信息.Clear();
                else if (调试信息.RichCount > 100)
                    调试信息.Clear();
                调试信息.AddText(s + "<br>");
            }
            Debug.LogWarning(s);
        }
#endif
        //public void ReadyUnlockFunc(int ulvID)
        //{
        //    mUnlockFuncID = ulvID;
        //    OnGoToUnlockFunc();
        //}
        //void OnGoToUnlockFunc()
        //{
        //    if (-1 == mUnlockFuncID)
        //        return;
        //    if (null == InSceneLogicModel.Inst || InSceneLogicModel.Inst.SceneType != 场景类型.主城)
        //        return;
        //    if (mActivedChilden.Count > 1 || mFadeInedChilden.Count > 1)
        //        return;
        //    if (PopupDialogView.ShowingCount > 0)
        //        return;
        //    PopupDialogView.Popup(PopupDialogType.新功能开启, mUnlockFuncID);
        //    mUnlockFuncID = -1;
        //}
/*        public void ReadyInterchange(string sender, 职业类型 job, int lv, int fv)
        {
            mInterchangeParams = new object[] { sender, job, lv, fv };
            OnOpenInterchangeDialog();
        }
        public void OnOpenInterchangeDialog()
        {
            if (null == mInterchangeParams || mInterchangeParams.Length != 4)
                return;
            if (null == InSceneLogicModel.Inst || InSceneLogicModel.Inst.SceneType != 场景类型.主城)
                return;
            if (mActivedChilden.Count > 1 || mFadeInedChilden.Count > 1)
                return;
            if (PopupDialogView.ShowingCount > 0)
                return;
            string sender = mInterchangeParams[0] as string;
            职业类型 job = (职业类型)mInterchangeParams[1];
            int lv = (int)mInterchangeParams[2];
            int fv = (int)mInterchangeParams[3];
            string richPath = "Intro/InterchangeJob" + ((int)job).ToString() + ".html";
            object[] richPs = new object[] { sender, lv.ToString(), fv.ToString() };
            PopupDialogView.Popup(PopupDialogType.确认提示, richPath, richPs, OnAcceptInterchange, Text.SharedShowStrings.申请切磋.ToString(), "");
            mInterchangeParams = null;
        }
        bool OnAcceptInterchange(弹框按钮 t, params object[] ps)
        {
            if (t == 弹框按钮.确认)
            {
                if (LocalPlayer.Inst.InRingState)
                    return true;
            }
            TcpIPNetwork.Inst.SendMessage(C2S_InterchangeResult.Create(t == 弹框按钮.确认));
            return true;
        }
        public void ReadyVipExpChanged()
        {
            mVipExpChanged = true;
            OnVipExpChanged();
        }
        void OnVipExpChanged()
        {
            if (!mVipExpChanged)
                return;
            if (null == InSceneLogicModel.Inst || InSceneLogicModel.Inst.SceneType != 场景类型.主城)
                return;
            if (mActivedChilden.Count > 1 || mFadeInedChilden.Count > 1)
                return;
            if (PopupDialogView.ShowingCount > 0)
                return;
            PopupDialogView.Popup(PopupDialogType.错误提示, OnGoToVipPage, Text.SharedShowStrings.查看VIP特权,
                Text.SharedShowStrings.您的VIP经验已增加.ToString() + "，"
                + Text.SharedShowStrings.可前往查看VIP特权.ToString() + "！",
                new string[] { Text.SharedShowStrings.查看.ToString() });
            mVipExpChanged = false;
        }
        bool OnGoToVipPage(弹框按钮 t, params object[] ps)
        {
            VIPRechargeView view = UIViewHelper.LoadUIViewRes<VIPRechargeView>(UIViewType.VIP充值界面);
            view.跳到VIP页 = true;
            return true;
        }*/
        void OnPress(bool pressed)
        {
            if (null == UICamera.currentTouch)
                return;
            if (UICamera.currentTouch.pressed != gameObject)
                return;
            Camera mainCam = Camera.main;
            if (null != mainCam)
            {
                Vector3 worldPos = mainCam.ScreenToWorldPoint(UICamera.currentTouch.pos);
                worldPos.z = 100;
                RaycastHit[] hits = Physics.RaycastAll(new Ray(worldPos, Vector3.back), 200);
                if (pressed)
                {
                    if (null != mTouchsBeginObj)
//                        OnTouchsObjectEnded(mTouchsBeginObj, worldPos);
                    mTouchsBeginObj = hits;
                    mTouchsBeginWorldPos = worldPos;
                    OnTouchsObjectBegin(mTouchsBeginObj, worldPos);
                }
                else
                {
  //                  OnTouchsObjectEnded(hits, worldPos);
                    mTouchsBeginObj = null;
                    mTouchsBeginWorldPos = Vector3.zero;
                }
            }
        }
        void OnTouchsObjectBegin(RaycastHit[] cs, Vector3 worldPos)
        {
            if (null == mTouchsBeginObj || mTouchsBeginObj.Length <= 0)
                return;
        }
       /* void OnTouchsObjectEnded(RaycastHit[] cs, Vector3 worldPos)
        {
            if (null == mTouchsBeginObj || mTouchsBeginObj.Length <= 0)
                return;
            if ((mTouchsBeginWorldPos - worldPos).magnitude < 0.5f)
            {
                List<Player> pls = new List<Player>();
                List<Npc> npcs = new List<Npc>();
                for (int i = 0; i < mTouchsBeginObj.Length; ++i)
                {
                    if (null == mTouchsBeginObj[i].collider)
                        continue;
                    ControllerBase ctrl = mTouchsBeginObj[i].collider.GetComponent<ControllerBase>();
                    if (null == ctrl || null == ctrl.SceneObject)
                        continue;
                    if (!(ctrl.SceneObject is ICharacter))
                        continue;
                    ICharacter chara = ctrl.SceneObject as ICharacter;
                    if (chara.IsPlayer && !chara.IsLocalPlayer)
                        pls.Add(chara as Player);
                    else if (chara.IsNpc)
                        npcs.Add(chara as Npc);
                }
                if (pls.Count > 0 || npcs.Count > 0)
                {
                    if(null != NoFightMainView.Inst)
                    {
                        if (pls.Count == 1 && npcs.Count <= 0)
                        {
                            if (pls[0].BaseInfo.InRing)
                            {
                                RingRoomView view = UIViewHelper.LoadUIViewRes<RingRoomView>(UIViewType.擂台界面);
                                view.BindPlayer(pls[0]);
                            }
                            else
                                PopupDialogView.Popup(PopupDialogType.玩家功能列表, NoFightMainView.Inst.gameObject, pls[0].ActorID, pls[0].ActorName);
                        }
                        else  if (npcs.Count == 1 && pls.Count <= 0)
                        {
                            NpcLuaTable.ValDesc data = NpcLuaTable.Inst[npcs[0].NpcID];
                            if (null != data)
                            {
                                if(data.TaskData.Count > 0 || (null != data.Functions && data.Functions.Length > 0))
                                    PopupDialogView.Popup(PopupDialogType.NPC功能列表, NoFightMainView.Inst.gameObject, npcs[0]);
                            }
                        }
                        else
                            PopupDialogView.Popup(PopupDialogType.场景对象列表, NoFightMainView.Inst.gameObject, worldPos, npcs, pls);
                    }
                }
            }
        }*/
        public void OnUIActived(UIBaseView ui)
        {
            if (null == ui || ui.UI类型 <= UIViewType.Root2D || ui.UI类型 >= UIViewType.弹出框)
                return;
            if (!mActivedChilden.Contains(ui))
            {
                if(ui.IsFadePanel)
                {
//                    ui.OnFadeInOvered += new UIBaseView.FadeEventHandler(OnUIFadeIned);
                    ui.OnFadeOutOvered += new UIBaseView.FadeEventHandler(OnUIViewFadeOuted);
  //                  ui.OnDestroyed += new UIBaseView.DestroyEventHandler(OnUIDestroyed);
                }
                mActivedChilden.Add(ui);
            }
        }
        public void CloseUI(params UIViewType[] ps)
        {
            List<UIViewType> list = new List<UIViewType>();
            list.Add(UIViewType.战斗主界面);
            list.Add(UIViewType.非战斗主界面);
            list.AddRange(ps);
            for(int i = 0; i < mActivedChilden.Count;)
            {
                if (!list.Contains(mActivedChilden[i].UI类型))
                {
                    if (null != mActivedChilden[i] && !mActivedChilden[i].Closeing)
                        mActivedChilden[i].OnClose();
                    mActivedChilden.RemoveAt(i);
                }
                else
                    i++;
            }
            for (int i = 0; i < mFadeInedChilden.Count; )
            {
                if (!list.Contains(mFadeInedChilden[i].UI类型))
                {
                    if (null != mFadeInedChilden[i] && !mFadeInedChilden[i].Closeing)
                        mFadeInedChilden[i].OnClose();
                    mFadeInedChilden.RemoveAt(i);
                }
                else
                    i++;
            }
            for (int i = 0; i < mFadeOutedChilden.Count; )
            {
                if (!list.Contains(mFadeOutedChilden[i].UI类型))
                {
                    if (null != mFadeOutedChilden[i] && !mFadeOutedChilden[i].Closeing)
                        mFadeOutedChilden[i].OnClose();
                    mFadeOutedChilden.RemoveAt(i);
                }
                else
                    i++;
            }
        }
        public void ClearUI(params UIViewType[] ps)
        {
            List<UIViewType> list = new List<UIViewType>();
            list.AddRange(ps);
            UIBaseView[] uis = mActivedChilden.ToArray();
            for (int i = 0; i < uis.Length; ++i)
            {
                if (uis[i].UI类型 > UIViewType.Root2D && uis[i].UI类型 < UIViewType.弹出框)
                {
                    if (list.Contains(uis[i].UI类型))
                        continue;
                    if (null != uis[i])
                        Destroy(uis[i].gameObject);
                }
            }
            uis = mFadeInedChilden.ToArray();
            for (int i = 0; i < uis.Length; ++i)
            {
                if (uis[i].UI类型 > UIViewType.Root2D && uis[i].UI类型 < UIViewType.弹出框)
                {
                    if (list.Contains(uis[i].UI类型))
                        continue;
                    if (null != uis[i])
                        Destroy(uis[i].gameObject);
                }
            }
            uis = mFadeOutedChilden.ToArray();
            for (int i = 0; i < uis.Length; ++i)
            {
                if (uis[i].UI类型 > UIViewType.Root2D && uis[i].UI类型 < UIViewType.弹出框)
                {
                    if (list.Contains(uis[i].UI类型))
                        continue;
                    if (null != uis[i])
                        Destroy(uis[i].gameObject);
                }
            }
            uis = mDisableedChilden.ToArray();
            for (int i = 0; i < uis.Length; ++i)
            {
                if (uis[i].UI类型 > UIViewType.Root2D && uis[i].UI类型 < UIViewType.弹出框)
                {
                    if (list.Contains(uis[i].UI类型))
                        continue;
                    if (null != uis[i])
                        Destroy(uis[i].gameObject);
                }
            }
            mActivedChilden.Clear();
            mFadeInedChilden.Clear();
            mFadeOutedChilden.Clear();
            mDisableedChilden.Clear();
            PopupDialogView.CloseDialogs();
            //mUnlockFuncID = -1;
            mInterchangeParams = null;
            //BetterDataCacheing<Vector2>.ClearCache();
            //BetterDataCacheing<Vector3>.ClearCache();
            //BetterDataCacheing<Vector4>.ClearCache();
            //BetterDataCacheing<Color32>.ClearCache();
        }
        public void ForceDestroyUI(UIViewType t)
        {
            for (int i = 0; i < mActivedChilden.Count; )
            {
                if ((null != mActivedChilden[i]) && (t == mActivedChilden[i].UI类型) && !mActivedChilden[i].Closeing)
                {
                    mActivedChilden[i].RootPanel.alpha = 0;
                    Destroy(mActivedChilden[i].gameObject);
                    mActivedChilden.RemoveAt(i);
                }
                else
                    i++;
            }
            for (int i = 0; i < mFadeInedChilden.Count; )
            {
                if ((null != mFadeInedChilden[i]) && (t == mFadeInedChilden[i].UI类型) && !mFadeInedChilden[i].Closeing)
                {
                    mFadeInedChilden[i].RootPanel.alpha = 0;
                    Destroy(mFadeInedChilden[i].gameObject);
                    mFadeInedChilden.RemoveAt(i);
                }
                else
                    i++;
            }
            for (int i = 0; i < mFadeOutedChilden.Count; )
            {
                if ((null != mFadeOutedChilden[i]) && (t == mFadeOutedChilden[i].UI类型) && !mFadeOutedChilden[i].Closeing)
                {
                    mFadeOutedChilden[i].RootPanel.alpha = 0;
                    Destroy(mFadeOutedChilden[i].gameObject);
                    mFadeOutedChilden.RemoveAt(i);
                }
                else
                    i++;
            }
        }
        /*public void OnUIFadeIned(UIBaseView ui)
        {
            if (null == ui || ui.UI类型 <= UIViewType.Root2D || ui.UI类型 >= UIViewType.弹出框)
                return;
            if (!mFadeInedChilden.Contains(ui))
                mFadeInedChilden.Add(ui);
            if (mActivedChilden.Contains(ui))
				mActivedChilden.Remove(ui);
			NoFightMainView view = NoFightMainView.Inst;
			if (null != view && !(ui is NoFightMainView) && !(ui is FightingMainView))
				view.OnUpdateGuide(false, false);
        }*/
        void OnUIViewFadeOuted(UIBaseView sender)
        {
            if (null == sender || sender.UI类型 <= UIViewType.Root2D || sender.UI类型 >= UIViewType.弹出框)
                return;
            if (mFadeInedChilden.Contains(sender))
                mFadeInedChilden.Remove(sender);
			mFadeOutedChilden.Add(sender);
        }
        public void OnUIDisabled(UIBaseView ui)
        {
            if (!mDisableedChilden.Contains(ui))
                mDisableedChilden.Add(ui);
        }
//        void OnUIDestroyed(UIBaseView ui)
//        {
//            if (mActivedChilden.Contains(ui))
//                mActivedChilden.Remove(ui);
//            if (mFadeInedChilden.Contains(ui))
//                mFadeInedChilden.Remove(ui);
//            if (mFadeOutedChilden.Contains(ui))
//                mFadeOutedChilden.Remove(ui);
//            if (mDisableedChilden.Contains(ui))
//                mDisableedChilden.Remove(ui);
//			CheckAutoLastUIs();
//			if(null != H2DLocalPlayerController.LocalPlayerController)
//			{
//				if(!(ui is GuideListDialog) && !(ui is LoadingSceneUIView))
//				{
//					NoFightMainView view = NoFightMainView.Inst;
//					if (null != view)
//					{
//						bool autoMoving = H2DLocalPlayerController.LocalPlayerController.AutoMoving;
//						view.OnUpdateGuide(!autoMoving, false);
//					}
//				}
//			}
//        }
//        public void CheckAutoLastUIs()
//        {
//            //OnGoToUnlockFunc();
//            OnOpenInterchangeDialog();
//            OnVipExpChanged();
//            //if (null == this || !gameObject.activeSelf)
//            //    return;
//            //if (!mInWaitUpdateGuide)
//			//    StartCoroutine(DnUpdateGuide());
//        }
        //IEnumerator DnUpdateGuide()
        //{
        //    mInWaitUpdateGuide = true;
        //    if (mActivedChilden.Count <= 1 && mFadeInedChilden.Count <= 1)
        //    {
        //        yield return new WaitForSeconds(1);
        //        if (mActivedChilden.Count <= 1 && mFadeInedChilden.Count <= 1)
        //        {
        //            NoFightMainView view = NoFightMainView.Inst;
        //            if (null != view && !view.任务指引锁屏.activeSelf)
        //                view.OnUpdateGuide(true, false);
        //        }
        //    }
        //    mInWaitUpdateGuide = false;
		//}
//		public void AddItemIcon(IconViewItemBase icon)
//		{
//			WaitCheckColliderItemIconQueue.Enqueue (icon);
//		}
//		void Update()
//		{
//			IconViewItemBase icon = null;
//			for(int i = 0; i < 8; ++ i)
//			{
//				if(WaitCheckColliderItemIconQueue.Count > 0)
//				{
//					icon = WaitCheckColliderItemIconQueue.Dequeue();
//					if(null != icon)
//						icon.CheckCollider();
//				}
//				else
//					break;
//			}
//		}
    }
}
