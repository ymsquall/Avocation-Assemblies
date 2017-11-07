using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Tools;
using Networking.TcpIPNetworking;
using Shared.BasicShared;
using System;
#if SERVER_HOST
using NewUI;
using ServerHost.View;
#endif
//using Effect;
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
using UnityEngine.Windows;
#else
//using System.IO;
#endif

namespace Logic.BasicLogic
{
    public abstract class LogicModel : MonoBehaviour, ILogic, ITcpNetworkingModel, ITcpNetLogicModel
    {
        //public int 模拟发送消息延迟时间 = 0;
        //public int 模拟接收消息延迟时间 = 0;
        //public int 模拟网络连接延迟时间 = 0;
        public static bool ApplicationFocusFirst = true;
        public static byte StartupAtlasMaxProg = 100;
        static byte StartupAtlasProg = 0;
        static float BEGIN_LOADING_SCENE_TIME = 0;
        public static float LOADSCENE_TIME = 0;
        public static TcpIPPacketQueue mNetPacket = null;
        bool mLoading = true;
		public PlatformSDKInterface SDKController = null;

        protected LogicModel()
        {
        }
        void OnApplicationFocus(bool focusStatus)
        {
            if (focusStatus)
            {
                if (ApplicationFocusFirst)
                {
                    OnApplicationInit();
                    ApplicationFocusFirst = false;
                }
            }
        }
        public void OnApplicationInit()
        {
            StreamAssetHelper.AssetPath = Application.streamingAssetsPath + "/";
#if UNITY_ANDROID && !UNITY_EDITOR
		        StreamAssetHelper.WWWAssetPath = Application.streamingAssetsPath + "/";
#else
            StreamAssetHelper.WWWAssetPath = "file://" + Application.streamingAssetsPath + "/";
#endif
#if !UNITY_IOS && !UNITY_ANDROID && !UNITY_WINRT
            StreamAssetHelper.WWWNewAssetPath = StreamAssetHelper.WWWAssetPath + "../../Documents/";
            StreamAssetHelper.NewAssetPath = StreamAssetHelper.AssetPath + "../../Documents/";
            if (!Directory.Exists(StreamAssetHelper.NewAssetPath))
                Directory.CreateDirectory(StreamAssetHelper.NewAssetPath);
#else
#if UNITY_IOS
				StreamAssetHelper.NewAssetPath = Application.persistentDataPath + "/Updates/";
				StreamAssetHelper.WWWNewAssetPath = "file://" + StreamAssetHelper.NewAssetPath;
#elif UNITY_ANDROID
                StreamAssetHelper.WWWNewAssetPath = StreamAssetHelper.WWWAssetPath + "../../../Documents/";
				StreamAssetHelper.NewAssetPath = StreamAssetHelper.AssetPath + "../../../Documents/";
#elif NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
	            StreamAssetHelper.AssetPath = StreamAssetHelper.AssetPath.Replace("/", "\\");
                StreamAssetHelper.WWWNewAssetPath = StreamAssetHelper.WWWAssetPath + "../../../Documents/";
				StreamAssetHelper.NewAssetPath = StreamAssetHelper.AssetPath + "..\\..\\..\\Documents\\";
#endif
#endif
            Application.targetFrameRate = 45;
            SharedUserConfig.Inst.InitSoundConfig();
            StartCoroutine(DoLoadUIHeader());
#if SDK_91 || SDK_WX || SDK_PP || SDK_TBT || SDK_AB
				PlatformSDKInterface.SDKInit(OnSDKInited, SDKController.name);
#elif SDK_HM
				//SDKController
#else
            StartCoroutine(InitHardDevices());

            OnSDKInited(true);
#endif
            StartCoroutine("DoCheckUIController");
#if !UNITY_EDITOR && UNITY_IOS
			OCDelegate.ScreenIdleDisable(true);
#endif
        }
        IEnumerator DoCheckUIController()
        {
            //while(null == UIController.Inst)
            //    yield return 0;
            //UIController.Inst.Login.gameObject.SetActive(true);
            yield return 0;
        }
        IEnumerator InitHardDevices()
        {
            //while (null == OCPcycleBridge.Inst)
            //{
            //    yield return 0;
            //}
            //OCPcycleBridge.Inst.Event_PcycleBridge += new OCPcycleBridge.PcycleBridge(PcycleMessageControl.PcycleMsgHandle);
            //OCPcycleBridge.Inst.initDevice();
            //while (null == ConnectAndroidDevice.Inst)
            //{
            //    yield return 0;
            //}
            //ConnectAndroidDevice.Inst.OnDeviceEvent += new OCPcycleBridge.PcycleBridge(PcycleMessageControl.PcycleMsgHandle);
            //ConnectAndroidDevice.Inst.OnPcycleBtnClick += new ConnectAndroidDevice.BtnHandler(PcycleMessageControl.OnPcycleBtnClick);
            yield return 0;
        }
        IEnumerator DoLoadUIHeader()
        {
            //yield return StartCoroutine(DoLoadMonoIndex(ResourceType.UIView, "UIScriptIndex.idx"));
        //    yield return StartCoroutine(DoLoadResourceFinder(ResourceType.Font));
            //UIResourceManager.LoadFont(UIHlp.FontName.SourceHanSans_Regular);
			yield return 0;
        }
		public void OnSDKInited(bool succ)
        {
			if (!succ)
			{
				Debug.LogError("SDK Init Failed, please restart game and try again!");
                return;
			}
            mNetPacket = TcpIPPacketQueue.Inst;
            //GameObject asset = Resources.Load<GameObject>("PrefabObject/" + UIHlp.界面类型.Root2D.ToString());
            //GameObject rootUI = Instantiate(asset) as GameObject;
            //asset = Resources.Load<GameObject>("PrefabObject/" + UIHlp.界面类型.登陆游戏界面.ToString());
            //GameObject updateObj = Instantiate(asset) as GameObject;
            //updateObj.transform.parent = rootUI.transform;
            //updateObj.transform.localScale = Vector3.one;
            //LoginUIView update = updateObj.GetComponent<LoginUIView>();
            //update.更新流程 = true;
            //update.手动删除资源 = true;
            //update.OnUpdateSucessed += new LoginUIView.EventHandler(OnAssetsUpdated);
            //update.FadeIn();
            //asset = null;
			Resources.UnloadUnusedAssets();
            OnAssetsUpdated();
        }
        void OnAssetsUpdated()
		{
			//LuaDataTableProxy.Begining();
   //         LuaLogic.Begining();
            StartCoroutine(DoLoadResourceFiner());
        }
        void OnApplicationPause(bool pauseStatus)
        {
			Debug.Log ("OnApplicationPause[" + pauseStatus.ToString() + "]");
#if UNITY_IOS || UNITY_ANDROID
			if(pauseStatus)
				PlatformSDKInterface.SDKPause();
#elif UNITY_WINRT
#endif
#if !UNITY_EDITOR && UNITY_IOS
			OCDelegate.ScreenIdleDisable(false);
#endif
        }
        void OnApplicationQuit()
        {
            ApplicationFocusFirst = false;
            TcpIPPacketQueue.RecvPacketQueue.Clear();
            if (null != mNetPacket)
                Destroy(mNetPacket.gameObject);
            TcpIPMessageQueue.Clear();
            TcpIPNetwork.Inst.Close();
            TcpIPNetwork.Inst.CloseMatch();
            //P2PNetworkManager.Inst.StopClient();
            PlayerPrefs.Save();
        }
//        public IEnumerator DoLoadResourceFinder(ResourceType i)
//        {
//            WWW file = StreamAssetHelper.LoadAsset(i.ToString() + ".list");
//            yield return file;
//            if (!string.IsNullOrEmpty(file.error) || null == file.bytes || file.bytes.Length <= 0)
//            {
//                PopupDialogView.Popup(弹框类型.错误提示, "找不到文件[" + i.ToString() + ".list" + "]");
//                Debug.LogWarning(file.error);
//            }
//            else
//            {
//                byte[] buffer = file.bytes;
//                switch (i)
//                {
//                    case ResourceType.Model:
//                        {
//                            MemoryStream s = new MemoryStream(buffer);
//                            //ModelResourceManager modelMgr = ModelResourceManager.FindManager();
//                            //modelMgr.Read(s);
//#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
//#else
//                            s.Close();
//#endif
//                            s.Dispose();
//                        }
//                        break;
//                    default:
//                        {
//                            int readIndex = 0;
//                            short fileCount = System.BitConverter.ToInt16(buffer, readIndex); readIndex += sizeof(short);
//                            Dictionary<string, string> finder = new Dictionary<string, string>();
//                            for (int f = 0; f < fileCount; ++f)
//                            {
//                                short fc = System.BitConverter.ToInt16(buffer, readIndex); readIndex += sizeof(short);
//                                List<string> files = new List<string>();
//                                for (int j = 0; j < fc; ++j)
//                                {
//                                    short fnLen = System.BitConverter.ToInt16(buffer, readIndex); readIndex += sizeof(short);
//                                    string fn = PlatformTools.BS2String_UTF8(buffer, readIndex, fnLen); readIndex += fnLen;
//                                    files.Add(fn);
//                                }
//                                short realFnlen = System.BitConverter.ToInt16(buffer, readIndex); readIndex += sizeof(short);
//                                string realFn = PlatformTools.BS2String_UTF8(buffer, readIndex, realFnlen); readIndex += realFnlen;
//                                if (i == ResourceType.UINeedAtlas)
//                                {
//                                    if (files.Count != 1)
//                                    {
//                                        Debug.LogError("读取UI与图包对照表资源索引[" + realFn + "]时，发现索引不唯一：" + files.Count.ToString());
//                                        continue;
//                                    }
//                                    finder.Add(realFn, files[0]);
//                                }
//                                else
//                                {
//                                    for (int j = 0; j < files.Count; ++j)
//                                    {
//                                        if (finder.ContainsKey(files[j]))
//                                        {
//                                            Debug.LogError("读取资源索引" + i.ToString() + "时发现相同的资源名：" + files[j]);
//                                            continue;
//                                        }
//                                        finder.Add(files[j], realFn);
//                                    }
//                                }
//                            }
//                            this[i] = finder;
//                        }
//                        break;
//                }
//            }
//        }
        IEnumerator DoLoadResourceFiner()
		{
            //while (!LuaDataTableProxy.Loaded)
			yield return 0;
            //float startTime = Time.realtimeSinceStartup;
            for (ResourceType i = ResourceType.Sound; i < ResourceType.max; ++i)
            {
                if (i == ResourceType.Sound || i == ResourceType.Font || i == ResourceType.UIView ||
                    i == ResourceType.Material || i == ResourceType.UIAtlas)
					continue;
                //float time = Time.realtimeSinceStartup;
				//yield return StartCoroutine(DoLoadResourceFinder(i));
                //LogicModel.OnStartupProg();
                //Debug.Log("载入资源列表[" + i.ToString() + "]耗时：" + (Time.realtimeSinceStartup - time).ToString("F4"));
            }
            //Debug.Log("载入资源列表总耗时：" + (Time.realtimeSinceStartup - startTime).ToString("F4"));
            //startTime = Time.realtimeSinceStartup;
            //yield return StartCoroutine(DoLoadMonoIndex(ResourceType.Model, "ModelMonoIndex.idx"));
            //LogicModel.OnStartupProg();
            //Debug.Log("载入模型资源脚本索引耗时：" + (Time.realtimeSinceStartup - startTime).ToString("F4"));
            //startTime = Time.realtimeSinceStartup;
            //yield return StartCoroutine(DoLoadSharedMaterials());
            //LogicModel.OnStartupProg();
            //Debug.Log("载入公用材质耗时：" + (Time.realtimeSinceStartup - startTime).ToString("F4"));
            //startTime = Time.realtimeSinceStartup;
            //UIResourceManager.LoadUIPacket();
            //while (!UIResourceManager.Inst.UIPacketLoaded)
            //    yield return 0;
            //LogicModel.OnStartupProg();
            //Debug.Log("载入UI集耗时：" + (Time.realtimeSinceStartup - startTime).ToString("F4"));
            //startTime = Time.realtimeSinceStartup;
            //ModelResourceManager modelLoader = ModelResourceManager.OnLoadCharaModel();
            //while (null != modelLoader && !modelLoader.CharaModelLoadOvered)
            //    yield return 0;
            //LogicModel.OnStartupProg();
            //Debug.Log("载入模型索引耗时：" + (Time.realtimeSinceStartup - startTime).ToString("F4"));
            //startTime = Time.realtimeSinceStartup;
            //EffectResourceManager effectLoader = EffectResourceManager.OnCommonLoad();
            //while (null != effectLoader && !effectLoader.CommonLoaded)
            //    yield return 0;
            //LogicModel.OnStartupProg();
            //// sound
            //{
            //    WWW soundIdx = StreamAssetHelper.LoadAsset("Sound.list");
            //    yield return soundIdx;
            //    SoundController.OnSoundClipListLoaded(soundIdx.assetBundle);
            //    soundIdx.Dispose();
            //    soundIdx = null;
            //    LogicModel.OnStartupProg();
			//}
            AsyncOperation unloadAsync = Resources.UnloadUnusedAssets();
            while (!unloadAsync.isDone || unloadAsync.progress < 0.9f)
				yield return 0;
			//LogicModel.OnStartupProg();
			System.GC.Collect();
			//LogicModel.OnStartupProg();
			Caching.CleanCache();
            OnResFinderLoadOvered();
//#if UNITY_IOS || UNITY_ANDROID
//			PlatformSDKInterface.GameResLoaded();
//#elif UNITY_WINRT

//#endif
        }
		/*
        public IEnumerator DoLoadMonoIndex(ResourceType t, string idxName)
        {
            WWW resScriptIndexFile = StreamAssetHelper.LoadAsset(idxName);
            yield return resScriptIndexFile;
            if (!string.IsNullOrEmpty(resScriptIndexFile.error) || null == resScriptIndexFile.bytes || resScriptIndexFile.bytes.Length <= 0)
            {
                PopupDialogView.Popup(弹框类型.错误提示, "找不到文件[" + idxName + "]");
                Debug.LogWarning(resScriptIndexFile.error);
            }
            else
            {
                AssetBundle bundle = resScriptIndexFile.assetBundle;
                mResourceScriptIndexBundle.Add((byte)t, bundle);
                if (t == ResourceType.Model)
                    bundle.LoadAllAssets();
            }
        }
        IEnumerator DoLoadSharedMaterials()
        {
            mSharedMaterialList.Clear();
            WWW matFile = StreamAssetHelper.LoadAsset("SharedMaterials.pg");
            yield return matFile;
            if (!string.IsNullOrEmpty(matFile.error))
                Debug.LogError(matFile.error);
            else
            {
                AssetBundle ab = matFile.assetBundle;
                if (null == ab)
                {
                    string error = "公用材质的assetBundle为空！";
                    Debug.LogError(error);
                }
                else
                {
                    Object[] objs = ab.LoadAllAssets(typeof(GameObject));
                    for (int i = 0; i < objs.Length; ++i)
                    {
                        if (!System.Enum.IsDefined(typeof(SharedMaterial), objs[i].name))
                        {
                            Debug.LogError("未定义的公用材质类型:" + objs[i].name);
                            continue;
                        }
                        Material mat = Instantiate(objs[i]) as Material;
                        mat.name = objs[i].name;
                        SharedMaterial t = (SharedMaterial)System.Enum.Parse(typeof(SharedMaterial), objs[i].name);
                        mSharedMaterialList.Add((byte)t, mat);
                    }
                    ab.Unload(true);
                }
            }
            matFile.Dispose();
            matFile = null;
        }
        */
        protected virtual void OnResFinderLoadOvered() { }
        public Dictionary<string, string> this[ResourceType t]
        {
            set
            {
                //if (mResourceFinder.ContainsKey((byte)t))
                //    mResourceFinder[(byte)t] = value;
                //else
                //    mResourceFinder.Add((byte)t, value);
            }
        }
        public bool Loading
        {
            set
            {
                if (mLoading != value)
                {
                    if (value)
                        BEGIN_LOADING_SCENE_TIME = Time.realtimeSinceStartup;
                    else
                    {
                        LOADSCENE_TIME = Time.realtimeSinceStartup - BEGIN_LOADING_SCENE_TIME;
                        Debug.Log("scene loading time = " + LOADSCENE_TIME.ToString("F4") + "s!");
                    }
                    mLoading = value;
                }
            }
            get { return mLoading; }
        }
        
        protected abstract bool AwakeImpl();
        void Awake()
		{
            if (ApplicationFocusFirst)
                OnApplicationFocus(true);
            if (!AwakeImpl())
            {
                OnPopupDialog(PopupDialogType.Error, "错误", "逻辑初始化失败！！！");
                Debug.LogError(string.Format("[{0}]逻辑初始化失败！！！", GetType().ToString()));
                return;
            }
        }
        protected virtual void Update()
        {
            if (!UpdateBefore())
                return;
            //if (!Loading)
            {
                BEGIN_LOADING_SCENE_TIME = 0;
                TcpIPNetwork.Inst.CheckRecvedMessage(this);
                //IntroTextManager.Inst.Update(Time.deltaTime);
                //FightingMainView.RecordLastLoadingTime(LOADSCENE_TIME);
                LOADSCENE_TIME = 0;
            }
            UpdateAfter();
            //if(Time.frameCount % 50 == 0)
            //    System.GC.Collect(0, GCCollectionMode.Optimized); 
        }
        protected virtual bool UpdateBefore() { return true; }
        protected virtual void UpdateAfter() { }
        public virtual void ActiveLogic() { }
        public virtual void DeActiveLogic() { }
        protected virtual void OnDestroy()
        {
            //TcpIPNetwork.Inst.UnRegisterRecvDelegate(this);
            DestroydImpl();
            //Debug.Log(string.Format("逻辑模块[{0}]已被销毁！", typeof(T).ToString()));
            UnityEngine.Resources.UnloadUnusedAssets();
            //GC.Collect();
        }
        protected virtual void DestroydImpl() { }
        public bool HandleMessage(IS2C_Msg msg, ref bool handled, ref bool breaked)
        {
            if (msg.MessageID == (System.Int32)RecvMsgType.S2C_Hint)
            {
                S2C_Hint prompt = msg as S2C_Hint;
                switch (prompt.mode)
                {
                    case S2CHintType.Error:
                        {
                            TcpIPNetwork.Inst.UnLockNetMsgScreenLocked();
                            HandlePromptError(prompt.mode, prompt.Hints);
							PlatformSDKInterface.LoginSender = false;
                            handled = true;
                        }
                        return true;
                    case S2CHintType.TV:
                        {
                            handled = true;
                        }
                        return true;
                    case S2CHintType.Scroll:
                        {
                            OnAddNotice(prompt.Hints);
#if SERVER_HOST
                            UIResourceBinder.FindView<RoomListUIView>().OnAddLogMsg(prompt.Hints);
#endif
                            handled = true;
                        }
                        return true;
                    case S2CHintType.System:
                        {
                            OnAddSystemMessage(prompt.Hints);
#if SERVER_HOST
                            UIResourceBinder.FindView<RoomListUIView>().OnAddLogMsg(prompt.Hints);
#endif
                            handled = true;
                        }
                        return true;
                    case S2CHintType.Debug:
                        {
                            OnAddDebugText(prompt.Hints);
                            Debug.Log(prompt.Hints);
#if SERVER_HOST
                            UIResourceBinder.FindView<RoomListUIView>().OnAddLogMsg(prompt.Hints);
#endif
                            handled = true;
                        }
                        return true;
                    case S2CHintType.Intro:
                        {
                            OnAddIntro(prompt.Hints);
                            Debug.Log(prompt.Hints);
#if SERVER_HOST
                            UIResourceBinder.FindView<RoomListUIView>().OnAddLogMsg(prompt.Hints);
#endif
                            handled = true;
                        }
                        return true;
                }
            }
            //if (msg.MessageID == (System.Int32)RecvMsgType.S2C_Login)
            //{
            //    if(!HandleLoginedImpl(msg as S2C_Login))
            //    {
            //        Debug.LogWarning("未处理的消息:" + ((接收消息类型)msg.MessageID).ToString() + ", 当前逻辑:" + this.ToString());
            //        return false;
            //    }
            //    handled = true;
            //    return true;
            //}
            if (!HandleMsgImpl(msg, ref handled, ref breaked))
            {
                Debug.LogWarning("未处理的消息:" + ((RecvMsgType)msg.MessageID).ToString() + ", 当前逻辑:" + this.ToString());
                return false;
            }
            return true;
        }
        protected virtual bool HandlePromptError(S2CHintType mode, string hints)
        {
            if (mode == S2CHintType.Error)
            {
                OnPopupDialog(PopupDialogType.Error, "", hints);
            }
            else if(mode == S2CHintType.BackToLogin)
            {
                OnPopupDialog(PopupDialogType.Error, "返回登陆界面", "与服务器断开连接,返回登陆界面");
    //            PopupDialogView.Popup(弹框类型.错误提示, (弹框按钮 t, object[] ps)=>
    //          	{
				//	if(null != InSceneLogicModel.Inst)
				//		InSceneLogicModel.Inst.BackToLoginScene(弹框按钮.确认, ps);
				//	return true;
				//}, Text.SharedShowStrings.返回登陆界面.ToString(), 
				//                      Text.SharedShowStrings.与服务器断开连接.ToString() + "，" + 
				//                      Text.SharedShowStrings.返回登陆界面.ToString());
            }
            return true;
        }
        protected virtual bool HandleMsgImpl(IS2C_Msg msg, ref bool handled, ref bool breaked)
        {
            return false;
        }

        protected virtual void OnPopupDialog(PopupDialogType t, string title, string msg)
        {

        }
        protected virtual void OnAddNotice(string notice)
        {

        }
        protected virtual void OnAddSystemMessage(string message)
        {

        }
        protected virtual void OnAddIntro(string intro)
        {

        }
        protected virtual void OnAddDebugText(string text)
        {

        }
        public virtual void TcpNetLockScreen()
        {
        }

        public virtual void TcpNetUnlockScreen()
        {
        }
        public RecvMsgType[] CanRecvMessages
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
