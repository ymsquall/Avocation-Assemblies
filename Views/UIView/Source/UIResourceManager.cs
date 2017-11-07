using Framework.Tools;
using System.Collections;
using System.Collections.Generic;
using UIView.Popup;
using UnityEngine;

namespace UIView
{
    public class UIResourceManager : SingletonMBT<UIResourceManager>
    {
        public delegate bool TexEventHandler(Texture tex);
        public delegate void AtlasEventHandler(UIAtlasType at, UIAtlas a);
        AssetBundle mTempFontBundle = null;
        AssetBundle mTempAtlasBundle = null;
        Dictionary<byte, UIFont> mFontList = new Dictionary<byte, UIFont>();
        //Dictionary<string, UIFont> mFontList = new Dictionary<string, UIFont>();
        Dictionary<string, GameObject> mUIList = null;
        Dictionary<string, TwoData<Texture, int>> mTextureList = new Dictionary<string, TwoData<Texture, int>>();
        Dictionary<int, ThreeData<UIAtlas, string, Vector2>> mAtlasList = null;
        Dictionary<int, int> mAtlasRefCounts = new Dictionary<int, int>();
        Dictionary<string, Shader> mShaderPool = new Dictionary<string, Shader>();
        List<int> mGlobalAtlasTexName = new List<int>() { (int)UIAtlasType.D_jiugongge,
                    (int)UIAtlasType.UI_主界面通用, (int)UIAtlasType.公用数字图包1 };
		Texture2D mWhiteBlockTex = null;
        bool mTextureLoading = false;
        bool mUIAtlasLoading = false;
        bool mFontPacketLoaded = false;
        bool mAtlasPacketLoaded = false;

		public Texture2D WhiteBlockTex{ get { return mWhiteBlockTex; } }

        static bool FindManager()
        {
            if (null == Inst)
            {
                GameObject resMgrObj = new GameObject("UIResourceManager");
                DontDestroyOnLoad(resMgrObj);
				UIResourceManager uiResMgr = resMgrObj.AddComponent<UIResourceManager>();
				uiResMgr.mWhiteBlockTex = new Texture2D(2, 2);
				uiResMgr.mWhiteBlockTex.SetPixel(0, 0, Color.white);
				uiResMgr.mWhiteBlockTex.SetPixel(0, 1, Color.white);
				uiResMgr.mWhiteBlockTex.SetPixel(1, 0, Color.white);
				uiResMgr.mWhiteBlockTex.SetPixel(1, 1, Color.white);
				uiResMgr.mWhiteBlockTex.Apply();
				uiResMgr.mWhiteBlockTex.filterMode = FilterMode.Point;
                //uiResMgr.StartCoroutine(uiResMgr.DoCheckAtlasPool());
            }
            if (null == Inst)
            {
                Debug.LogError("为正确创建UIResourceManager！");
                return false;
            }
            return true;
        }
        public static bool LoadTexture(string path, TexEventHandler cb)
        {
            if (!FindManager())
                return false;
            Inst.StartCoroutine(Inst.DoLoadTexture(path, cb));
            return true;
        }
        public static bool AddFont(string fontName, UIFont font)
        {
            if (!FindManager() || null == font)
                return false;
            fontName = fontName.Replace("font/", "");
            if (System.Enum.IsDefined(typeof(UIFontType), fontName))
            {
                UIFontType ft = (UIFontType)System.Enum.Parse(typeof(UIFontType), fontName, true);
                if (!Inst.mFontList.ContainsKey((byte)ft))
                {
                    Inst.mFontList.Add((byte)ft, font);
                    return true;
                }
            }
            return false;
        }
        public static bool RemoveFont(string fontName)
        {
            if (!FindManager())
                return false;
            fontName = fontName.Replace("font/", "");
            if (System.Enum.IsDefined(typeof(UIFontType), fontName))
            {
                UIFontType ft = (UIFontType)System.Enum.Parse(typeof(UIFontType), fontName, true);
                if (Inst.mFontList.ContainsKey((byte)ft))
                {
                    Inst.mFontList.Remove((byte)ft);
                    return true;
                }
            }
            return false;
        }
        public static UIFont LoadFont(UIFontType t)
        {
            if (!FindManager())
                return null;
            UIFont font = Inst[t];
            if (null != font)
                return font;
            GameObject obj = Resources.Load("Font/" + t.ToString(), typeof(GameObject)) as GameObject;
            font = obj.GetComponent<UIFont>();
            obj = null;
            Resources.UnloadUnusedAssets();
            Inst.mFontList.Add((byte)t, font);
            return font;
        }
        //public static UIResourceManager LoadFont(UIHlp.动态字体类型 t, Object lbl)
        //{
        //    if (!FindManager())
        //        return null;
        //    Inst.StartCoroutine(Inst.DoLoadFont(t, lbl));
        //    return Inst;
        //}
        public static bool LoadAtlasPacket()
        {
            if (!FindManager())
                return false;
            Inst.StartCoroutine(Inst.DoLoadAtlasPacket());
            return true;
        }
        public static bool LoadUIPacket()
        {
            if (!FindManager())
                return false;
            Inst.StartCoroutine(Inst.DoLoadUIPacket());
            return true;
        }
    //    public static bool LoadAtlas(UIAtlasType t, UISpriteEx spEx)
    //    {
    //        if (!FindManager())
    //            return false;
    //        if (null != spEx)
    //        {
				//if (spEx.type == UISprite.Type.Simple)
    //            {
    //                if (Inst.mAtlasList.ContainsKey((int)t))
    //                {
    //                    UIAtlas atlas = Inst.mAtlasList[(int)t].Bgein;
    //                    UISpriteData spData = null;
    //                    if (spEx.SpriteIndex != -1)
    //                        spData = atlas.FindSpriteData(spEx.SpriteIndex);
    //                    if (null == spData)
    //                        spData = atlas.GetSprite(spEx.spriteName);
    //                    if (null != spData)
    //                    {
    //                        spEx.width = spData.width;
    //                        spEx.height = spData.height;
    //                    }
    //                }
    //            }
    //        }
    //        Inst.StartCoroutine(Inst.DoLoadAtlas(t, spEx));
    //        return true;
    //    }
        public static bool LoadAtlas(UIAtlasType t, AtlasEventHandler callback)
        {
            if (!FindManager())
                return false;
            Inst.StartCoroutine(Inst.DoLoadAtlas((int)t, callback));
            return true;
        }
        public static NGUI2DRootPanel LoadRootUI()
        {
            if (!FindManager())
                return null;
            return Inst.OnLoadRootUI(UIViewType.Root2D.ToString());
        }
        public static T LoadUI<T>(UIViewType ui, string atlasList)
            where T : UIBaseView
        {
            if(!FindManager())
                return null;
            string uiTypeName = ui.ToString();
            return Inst.OnLoadUI<T>(ui, uiTypeName, atlasList);
        }
        public static PopupDialogView LoadUIDialog(string uiName, string atlasList, PopupDialogType t, Transform parent, string richPath, object[] richPs, PopupDialogView.ButtonEventHandler btnEvent, object[] ps, PopupDialogView.DialogEventHandleer callback)
        {
            if (!FindManager())
                return null;
            return Inst.OnLoadDialog<PopupDialogView>(UIViewType.弹出框, uiName, atlasList, t, parent, richPath, richPs, btnEvent, ps, callback);
        }
        public static T LoadUITemplate<T>(string uiName, string atlasList)
            where T : UIBaseView
        {
            if (!FindManager())
                return null;
            return Inst.OnLoadUI<T>(UIViewType.UI模板, uiName, atlasList);
        }
        //public static UISliderEx LoadUISlider(string uiName, string atlasList)
        //{
        //    if (!FindManager())
        //        return null;
        //    return Inst.OnLoadUISlider(uiName, atlasList);
        //}
        public bool FontPacketLoaded { get { return mFontPacketLoaded; } }
        public bool UIShaderLoaded { get { return mShaderPool.Count > 0; } }
        public bool AtlasPacketLoaded { get { return mAtlasPacketLoaded; } }
        public bool UIPacketLoaded { get { return null != mUIList; } }
        public ThreeData<UIAtlas, string, Vector2> this[UIAtlasType t]
        {
            get
            {
                if (!mAtlasList.ContainsKey((int)t))
                    return null;
                //if (!mGlobalAtlasTexName.Contains(n))
                //    return null;
                return mAtlasList[(int)t];
            }
        }
        public UIFont this[UIFontType t]
        {
            get
            {
                if (mFontList.ContainsKey((byte)t))
                    return mFontList[(byte)t];
                return null;
            }
        }
        NGUI2DRootPanel OnLoadRootUI(string uiName)
        {
            float sTime = Time.realtimeSinceStartup;
            GameObject uiObj = Instantiate(mUIList[uiName]) as GameObject;
            uiObj.name = uiName;
            uiObj.SetActive(true);
            NGUI2DRootPanel ret = uiObj.GetComponent<NGUI2DRootPanel>();
            LogSys.Debug("~~~inst ui[" + uiName + "]used:" + (Time.realtimeSinceStartup - sTime).ToString());
            return ret;
        }
        T OnLoadUI<T>(UIViewType ui, string uiName, string atlasList)
            where T : UIBaseView
        {
            float sTime = Time.realtimeSinceStartup;
            GameObject uiObj = Instantiate(mUIList[uiName]) as GameObject;
            uiObj.name = uiName;
            uiObj.SetActive(true);
            T ret = uiObj.GetComponent<T>();
            if (null != ret)
            {
                ret.UI类型 = ui;
				ret.SetWhiteBlockTex(WhiteBlockTex);
                bool noNeedAtlas = true;
                string[] list = atlasList.Split("#".ToCharArray());
                List<int> aidList = new List<int>();
                for (int i = 0; i < list.Length; ++i)
                {
                    if (string.IsNullOrEmpty(list[i]))
                        continue;
                    if (System.Enum.IsDefined(typeof(UIAtlasType), list[i]))
                    {
                        UIAtlasType at = (UIAtlasType)System.Enum.Parse(typeof(UIAtlasType), list[i]);
                        noNeedAtlas = false;
                        ret.AddRefAtlas(at);
                        aidList.Add((int)at);
                    }
                }
                if (noNeedAtlas)
                    ret.DoReadyShowing();
                else
                {
                    for (int i = 0; i < aidList.Count; ++i)
                        StartCoroutine(DoLoadAtlas(aidList[i], ret));
                }
            }
            LogSys.Debug("~~~inst ui[" + uiName + "]used:" + (Time.realtimeSinceStartup - sTime).ToString());
            return ret;
        }
        T OnLoadDialog<T>(UIViewType ui, string uiName, string atlasList, PopupDialogType t, Transform parent, string richPath, object[] richPs, PopupDialogView.ButtonEventHandler btnEvent, object[] ps, PopupDialogView.DialogEventHandleer callback)
            where T : PopupDialogView
        {
            float sTime = Time.realtimeSinceStartup;
            GameObject uiObj = Instantiate(mUIList[uiName]) as GameObject;
            uiObj.name = uiName;
            uiObj.SetActive(true);
            T ret = uiObj.GetComponent<T>();
            if (null != ret)
            {
				ret.UI类型 = ui;
				ret.SetWhiteBlockTex(WhiteBlockTex);
                if (null != callback)
                    callback(t, ret, parent, richPath, richPs, btnEvent, ps);
                bool noNeedAtlas = true;
                string[] list = atlasList.Split("#".ToCharArray());
                List<int> aidList = new List<int>();
                for (int i = 0; i < list.Length; ++i)
                {
                    if (string.IsNullOrEmpty(list[i]))
                        continue;
                    if (System.Enum.IsDefined(typeof(UIAtlasType), list[i]))
                    {
                        UIAtlasType at = (UIAtlasType)System.Enum.Parse(typeof(UIAtlasType), list[i]);
                        noNeedAtlas = false;
                        ret.AddRefAtlas(at);
                        aidList.Add((int)at);
                    }
                }
                if (noNeedAtlas)
                    ret.DoReadyShowing();
                else
                {
                    for (int i = 0; i < aidList.Count; ++i)
                        StartCoroutine(DoLoadAtlas(aidList[i], ret));
                }
            }
            LogSys.Debug("~~~inst ui[" + uiName + "]used:" + (Time.realtimeSinceStartup - sTime).ToString());
            return ret;
        }
        //UISliderEx OnLoadUISlider(string uiName, string atlasList)
        //{
        //    float sTime = Time.realtimeSinceStartup;
        //    GameObject uiObj = Instantiate(mUIList[uiName]) as GameObject;
        //    uiObj.name = uiName;
        //    uiObj.SetActive(true);
        //    UISliderEx ret = uiObj.GetComponent<UISliderEx>();
        //    if (null != ret)
        //    {
        //        bool noNeedAtlas = true;
        //        string[] list = atlasList.Split("#".ToCharArray());
        //        List<int> aidList = new List<int>();
        //        for (int i = 0; i < list.Length; ++i)
        //        {
        //            if (string.IsNullOrEmpty(list[i]))
        //                continue;
        //            if (System.Enum.IsDefined(typeof(UIAtlasType), list[i]))
        //            {
        //                UIAtlasType at = (UIAtlasType)System.Enum.Parse(typeof(UIAtlasType), list[i]);
        //                noNeedAtlas = false;
        //                ret.AddRefAtlas(at);
        //                aidList.Add((int)at);
        //            }
        //        }
        //        if (noNeedAtlas)
        //            ret.DoReadyShowing();
        //        else
        //        {
        //            for (int i = 0; i < aidList.Count; ++i)
        //                StartCoroutine(DoLoadAtlas(aidList[i], ret));
        //        }
        //    }
        //    LogSys.Debug("~~~inst ui[" + uiName + "]used:" + (Time.realtimeSinceStartup - sTime).ToString());
        //    return ret;
        //}
        IEnumerator DoLoadTexture(string path, TexEventHandler cb)
        {
            if (!mTextureList.ContainsKey(path))
                yield return StartCoroutine(DoLoadTexture(path));
            if (mTextureList.ContainsKey(path))
            {
                mTextureList[path].Second++;
                if (null != cb)
                {
                    if (!cb(mTextureList[path].First))
                        RemoveTexture(path);
                }
            }
        }
        //IEnumerator DoLoadAtlas(UIAtlasType t, UISpriteEx spEx)
        //{
        //    int aid = (int)t;
        //    if (!mAtlasList.ContainsKey(aid) || string.IsNullOrEmpty(mAtlasList[aid].Middle))
        //        Debug.LogError("找不到图包[" + t.ToString() + "]对应的资源！");
        //    else
        //    {
        //        if (!mAtlasRefCounts.ContainsKey(aid))
        //        {
        //            ThreeData<UIAtlas, string, Vector2> data = mAtlasList[aid];
        //            yield return StartCoroutine(DoLoadAtlas(data.Bgein, aid, data.Middle));
        //        }
        //        mAtlasRefCounts[aid]++;
        //        if (null != spEx)
        //            spEx.OnAtlasLoaded(t, mAtlasList[aid].Bgein);
        //    }
        //}
        IEnumerator DoLoadAtlas(int aid, UIBaseView view)
        {
            if (!mAtlasList.ContainsKey(aid) || string.IsNullOrEmpty(mAtlasList[aid].Middle))
            {
                Debug.LogError("找不到图包[" + ((UIAtlasType)aid).ToString() + "]对应的资源！");
            }
            else
            {
                if (!mAtlasRefCounts.ContainsKey(aid))
                {
                    ThreeData<UIAtlas, string, Vector2> data = mAtlasList[aid];
                    yield return StartCoroutine(DoLoadAtlas(data.Bgein, aid, data.Middle));
                }
                mAtlasRefCounts[aid]++;
            }
            if (null != view)
                view.OnAtlasLoaded((UIAtlasType)aid);
        }
        //IEnumerator DoLoadAtlas(int aid, UISliderEx view)
        //{
        //    if (!mAtlasList.ContainsKey(aid) || string.IsNullOrEmpty(mAtlasList[aid].Middle))
        //    {
        //        Debug.LogError("找不到图包[" + ((UIAtlasType)aid).ToString() + "]对应的资源！");
        //    }
        //    else
        //    {
        //        if (!mAtlasRefCounts.ContainsKey(aid))
        //        {
        //            ThreeData<UIAtlas, string, Vector2> data = mAtlasList[aid];
        //            yield return StartCoroutine(DoLoadAtlas(data.Bgein, aid, data.Middle));
        //        }
        //        mAtlasRefCounts[aid]++;
        //    }
        //    if (null != view)
        //        view.OnAtlasLoaded((UIAtlasType)aid);
        //}

        public IEnumerator DoLoadAtlas(int aid, AtlasEventHandler callback)
        {
            if (!mAtlasList.ContainsKey(aid) || string.IsNullOrEmpty(mAtlasList[aid].Middle))
            {
                Debug.LogError("找不到图包[" + ((UIAtlasType)aid).ToString() + "]对应的资源！");
            }
            else
            {
                if (!mAtlasRefCounts.ContainsKey(aid))
                {
                    ThreeData<UIAtlas, string, Vector2> data = mAtlasList[aid];
                    yield return StartCoroutine(DoLoadAtlas(data.Bgein, aid, data.Middle));
                }
                mAtlasRefCounts[aid]++;
            }
            if (null != callback)
                callback((UIAtlasType)aid, mAtlasList[aid].Bgein);
        }
        IEnumerator DoLoadAtlas(int aid)
        {
            if (!mAtlasList.ContainsKey(aid) || string.IsNullOrEmpty(mAtlasList[aid].Middle))
            {
                Debug.LogError("找不到图包[" + ((UIAtlasType)aid).ToString() + "]对应的资源！");
            }
            else
            {
                if (!mAtlasRefCounts.ContainsKey(aid))
                {
                    ThreeData<UIAtlas, string, Vector2> data = mAtlasList[aid];
                    yield return StartCoroutine(DoLoadAtlas(data.Bgein, aid, data.Middle));
                }
                mAtlasRefCounts[aid]++;
            }
        }
        //IEnumerator DoLoadFont(UIHlp.动态字体类型 t, Object lbl)
        //{
        //    string fontName = t.ToString();
        //    if (!mFontList.ContainsKey(fontName))
        //    {
        //        string fontResName = LogicModel.GetResName(ResourceType.Font, fontName);
        //        if (string.IsNullOrEmpty(fontResName))
        //            Debug.LogError("找不到字体[" + t.ToString() + "]对应的资源！");
        //        else
        //            yield return StartCoroutine(DoLoadFont(fontName, fontResName));
        //    }
        //    if (null != lbl)
        //    {
        //        //lblEx.OnFontLoaded(fontName, mFontList[fontName]);
        //    }
        //}
        IEnumerator DoLoadTexture(string path)
        {
            while (mTextureLoading)
                yield return new WaitForFixedUpdate();
            mTextureLoading = true;
            if (!mTextureList.ContainsKey(path))
            {
                WWW file = StreamAssetHelper.LoadAsset(StreamAssetRoot.TEXTURE_ROOT, path);
                file.threadPriority = ThreadPriority.High;
                yield return file;
                if (!string.IsNullOrEmpty(file.error))
                {
                    PopupDialogView.PopupAB(PopupDialogType.Confirm, SharedResources.Inst.UIPopup, "错误", file.error);
                    Debug.LogError(file.error);
                }
                else
                {
                    AssetBundle ab = file.assetBundle;
                    Texture2D tex = file.texture;
                    if (null == tex)
                    {
                        string error = "贴图" + path + "的texture为空！";
                        PopupDialogView.PopupAB(PopupDialogType.Confirm, SharedResources.Inst.UIPopup, "错误", error);
                        Debug.LogError(error);
                    }
                    else
                    {
                        mTextureList.Add(path, new TwoData<Texture, int>(tex, 0));
                        if (null != ab)
                            ab.Unload(false);
                        LogSys.Debug("成功载入贴图:" + path);
                    }
                }
                file.Dispose();
            }
            mTextureLoading = false;
        }
        //IEnumerator DoLoadFont(string fontName, string fontResName)
        //{
        //    WWW ui = StreamAssetHelper.LoadAsset(StreamAssetRoot.FONT_ROOT, fontResName + ".font");
        //    yield return ui;
        //    if (!string.IsNullOrEmpty(ui.error))
        //    {
        //        PopupDialogView.PopupAB(PopupDialogType.错误提示, SharedResources.Inst.UIPopup, ui.error);
        //        Debug.LogError(ui.error);
        //    }
        //    else
        //    {
        //        mTempFontBundle = ui.assetBundle;
        //        if (null == mTempFontBundle)
        //        {
        //            string error = "字体" + fontResName + "的assetBundle为空！";
        //            PopupDialogView.PopupAB(PopupDialogType.错误提示, SharedResources.Inst.UIPopup, error);
        //            Debug.LogError(error);
        //        }
        //        else
        //        {
        //            // font
        //            Object[] ftObjs = mTempFontBundle.LoadAllAssets(typeof(GameObject));
        //            for (int i = 0; i < ftObjs.Length; ++i)
        //            {
        //                GameObject ftObj = ftObjs[i] as GameObject;
        //                ftObj.name = ftObjs[i].name;
        //                ftObj.transform.parent = transform;
        //                UIFont uifont = ftObj.GetComponent<UIFont>();
        //                if (null != uifont)
        //                    mFontList.Add(ftObj.name, uifont);
        //            }
        //            Object[] shaderObjs = mTempFontBundle.LoadAllAssets(typeof(Shader));
        //            for (int i = 0; i < shaderObjs.Length; ++i)
        //            {
        //                Shader sh = Instantiate(shaderObjs[i]) as Shader;
        //                mShaderPool.Add(sh.name, sh);
        //            }
        //            mFontPacketLoaded = true;
        //            LogSys.Debug("成功载入字体:" + fontResName);
        //        }
        //    }
        //    ui.Dispose();
        //    ui = null;
        //}
        IEnumerator DoLoadAtlasPacket()
        {
            WWW ui = StreamAssetHelper.LoadAsset("UIAtlas.ats");
            yield return ui;
            if (!string.IsNullOrEmpty(ui.error))
                Debug.LogError(ui.error);
            else
            {
                mTempAtlasBundle = ui.assetBundle;
                if (null == mTempAtlasBundle)
                {
                    string error = "图包集UIAtlas.ats的assetBundle为空！";
                    Debug.LogError(error);
                }
                else
                {
                    mAtlasList = new Dictionary<int, ThreeData<UIAtlas, string, Vector2>>();
                    Dictionary<string, Vector2> texSizeList = new Dictionary<string, Vector2>();
                    Object[] texSizeObjList = mTempAtlasBundle.LoadAllAssets(typeof(TextAsset));
                    for (int i = 0; i < texSizeObjList.Length; ++i)
                    {
                        TextAsset tex = texSizeObjList[i] as TextAsset;
                        if (string.IsNullOrEmpty(tex.text))
                            continue;
                        string[] ss = tex.text.Split(":".ToCharArray());
                        int sizeX = int.Parse(ss[0]);
                        int sizeY = int.Parse(ss[1]);
                        texSizeList.Add(tex.name, new Vector2(sizeX, sizeY));
                    }
					/*
                    Object[] atlasObjList = mTempAtlasBundle.LoadAllAssets(typeof(GameObject));
                    Dictionary<string, string> finder = LogicModel.GetResFinder(ResourceType.UIAtlas);
                    for (int i = 0; i < atlasObjList.Length; ++i)
                    {
                        GameObject srcObj = atlasObjList[i] as GameObject;
                        if (null == srcObj || !finder.ContainsKey(srcObj.name))
                        {
                            Debug.LogWarning("不被使用的UI图包：" + srcObj.name);
                            continue;
                        }
                        UIAtlas atlas = srcObj.GetComponent<UIAtlas>();
                        if(System.Enum.IsDefined(typeof(UIAtlasType), atlas.name))
                        {
                            UIAtlasType at = (UIAtlasType)System.Enum.Parse(typeof(UIAtlasType), atlas.name);
                            int aid = (int)at;
                            atlas.spriteMaterial = new Material(Shader.Find("Unlit/Transparent Colored"));
                            mAtlasList.Add(aid, new ThreeData<UIAtlas, string, Vector2>(atlas, finder[atlas.name], texSizeList[atlas.name]));
                            if (at == UIAtlasType.物品图标图包1)
                            {
                                // 缓存物品图标索引
                                List<UISpriteData> list = atlas.spriteList;
                                for (int ii = 0, imax = list.Count; ii < imax; ++ii)
                                {
                                    int key = SharedAtlasMapper.GenItemIconAtlasKey(list[ii].name);
                                    atlas.AddIndexMapper(key, ii);
                                }
                            }
                            if (at == UIAtlasType.UI_主界面通用)
                            {
                                // 缓存主界面图标索引
                                List<UISpriteData> list = atlas.spriteList;
                                for (int ii = 0, imax = list.Count; ii < imax; ++ii)
                                {
                                    int key = SharedAtlasMapper.GenCommonMainKey(list[ii].name);
                                    atlas.AddIndexMapper(key, ii);
                                }
                            }
                        }
                    }
                    texSizeList.Clear();
                    for (int i = 0; i < texSizeObjList.Length; ++i)
                    {
                        Resources.UnloadAsset(texSizeObjList[i]);
                        //DestroyImmediate(texSizeObjList[i]);
                        texSizeObjList[i] = null;
                    }
                    texSizeObjList = null;
                    mAtlasPacketLoaded = true;
                    LogSys.Debug("成功载入图包集UIAtlas.ats");
                    */
                }
            }
            ui.Dispose();
        }
        IEnumerator DoLoadUIPacket()
        {
            WWW ui = StreamAssetHelper.LoadAsset("UIView.nui");
            yield return ui;
            if (!string.IsNullOrEmpty(ui.error))
                Debug.LogError(ui.error);
            else
            {
                AssetBundle assetBundle = ui.assetBundle;
                if (null == assetBundle)
                {
                    string error = "UI集UIView.nui的assetBundle为空！";
                    Debug.LogError(error);
                }
                else
                {
                    Object[] objs = assetBundle.LoadAllAssets(typeof(GameObject));
                    mUIList = new Dictionary<string, GameObject>();
                    for (int i = 0; i < objs.Length; ++i)
                        mUIList.Add(objs[i].name, objs[i] as GameObject);
                    assetBundle.Unload(false);
                    mTempFontBundle.Unload(false);
                    mTempAtlasBundle.Unload(false);
                    assetBundle = mTempFontBundle = mTempAtlasBundle = null;
                    for (int i = 0; i < mGlobalAtlasTexName.Count; ++i)
                        yield return StartCoroutine(DoLoadAtlas(mGlobalAtlasTexName[i]));
                    LogSys.Debug("成功载入UI集UIView.nui");
                }
            }
            ui.Dispose();
            ui = null;
            Resources.UnloadUnusedAssets();
        }
        IEnumerator DoLoadAtlas(UIAtlas atlas, int aid, string atlasResName)
        {
            while (mUIAtlasLoading)
                yield return new WaitForFixedUpdate();
            mUIAtlasLoading = true;
            if (null == atlas.texture)
            {
                WWW ui = StreamAssetHelper.LoadAsset(StreamAssetRoot.UIATLASTEX_ROOT, atlasResName + ".tex", 1);
                ui.threadPriority = ThreadPriority.High;
                yield return ui;
                AssetBundle ab = ui.assetBundle;
                if (null == atlas.texture)
                {
                    if (!string.IsNullOrEmpty(ui.error))
                    {
                        PopupDialogView.PopupAB(PopupDialogType.Confirm, SharedResources.Inst.UIPopup, "错误", ui.error);
                        Debug.LogError(ui.error);
                    }
                    else
                    {
                        if (null == ab)
                        {
                            string error = "图包:" + atlas.name + "[" + atlasResName + "]的assetBundle为空！";
                            PopupDialogView.PopupAB(PopupDialogType.Confirm, SharedResources.Inst.UIPopup, "错误", error);
                            Debug.LogError(error);
                        }
                        else
                        {
                            //float sTime = Time.realtimeSinceStartup;
                            atlas.spriteMaterial.mainTexture = ab.mainAsset as Texture2D;
                            ab.Unload(false);
                            //LogSys.Debug("~~~inst atlas[" + atlasObj.name + "]used:" + (Time.realtimeSinceStartup - sTime).ToString());
                            //mAtlasRefCounts.Add(atlas.name, 0);
                            LogSys.Debug("成功载入图包:" + atlas.name);
                        }
                    }
                }
                else if (null != ab)
                    ab.Unload(false);
                ui.Dispose();
            }
            if(!mAtlasRefCounts.ContainsKey(aid))
                mAtlasRefCounts.Add(aid, 0);
            mUIAtlasLoading = false;
        }
        public void RemoveAtlasList(UIAtlasType[] atlasList)
        {
            if (null == atlasList)
                return;
            for (int i = 0; i < atlasList.Length; ++i)
            {
                int aid = (int)atlasList[i];
                if (mGlobalAtlasTexName.Contains(aid))
                    continue;
                if (!mAtlasRefCounts.ContainsKey(aid))
                {
                    Debug.LogError("尝试检索不存在的图包[" + atlasList[i].ToString() + "]的引用计数");
                    continue;
                }
                if (--mAtlasRefCounts[aid] <= 0)
                {
                    if (!mAtlasList.ContainsKey(aid))
                    {
                        Debug.LogError("尝试移除不存在的图包：" + atlasList[i].ToString());
                        continue;
                    }
                    UIAtlas atlas = mAtlasList[aid].Bgein;
                    DestroyImmediate(atlas.texture, true);
                    Resources.UnloadAsset(atlas.texture);
                    atlas.spriteMaterial.mainTexture = null;
                    mAtlasRefCounts.Remove(aid);
                    //LogSys.Debug("成功移除UI图包:" + atlasList[i]);
                }
            }
        }
        public bool RemoveAtlas(UIAtlasType t)
        {
            int aid = (int)t;
            if (mGlobalAtlasTexName.Contains(aid))
                return false;
            if (!mAtlasRefCounts.ContainsKey(aid))
            {
                Debug.LogError("尝试检索不存在的图包[" + t.ToString() + "]的引用计数");
                return false;
            }
            if (--mAtlasRefCounts[aid] <= 0)
            {
                if (!mAtlasList.ContainsKey(aid))
                {
                    Debug.LogError("尝试移除不存在的图包：" + t.ToString());
                    return false;
                }
                UIAtlas atlas = mAtlasList[aid].Bgein;
                DestroyImmediate(atlas.texture, true);
                Resources.UnloadAsset(atlas.texture);
                atlas.spriteMaterial.mainTexture = null;
                mAtlasRefCounts.Remove(aid);
                //LogSys.Debug("成功移除UI图包:" + atlasList[i]);
            }
            return true;
        }
        public bool RemoveTexture(string path)
        {
            if(mTextureList.ContainsKey(path))
            {
                if(--mTextureList[path].Second <= 0)
                {
                    Texture tex = mTextureList[path].First;
                    if (null != tex)
                        DestroyImmediate(tex);
                    mTextureList.Remove(path);
                    Resources.UnloadUnusedAssets();
                }
                return true;
            }
            return false;
        }
    }
}