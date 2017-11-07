using Framework.Tools;
using System.Collections.Generic;
using UIView.Control;
using UnityEngine;

namespace UIView
{
    public enum UIViewType
    {
        Root2D = 1,
        登陆游戏界面 = 2,
        服务器选择界面 = 3,
        创建角色界面 = 4,
        场景Loading界面 = 5,
        战斗主界面 = 6,
        非战斗主界面 = 7,
        单人副本界面 = 8,
        单人副本结算 = 9,
        我要变强界面 = 10,
        多人副本界面 = 11,
        多人活动界面 = 12,
        副本房间界面 = 13,
        队长分配界面 = 14,
        掷点分配界面 = 15,
        聊天界面 = 16,
        人物背包界面 = 17,
        背包物品介绍界面 = 18,
        装备对比界面 = 19,
        人物属性界面 = 20,
        装备强化界面 = 21,
        装备镶嵌界面 = 22,
        宝石合成界面 = 23,
        装备洗练界面 = 24,
        咒文置换界面 = 25,
        任务界面 = 26,
        系统界面 = 27,
        技能界面 = 28,
        竞技场界面 = 29,
        竞技场结算界面 = 30,
        好友界面 = 31,
        邮件界面 = 32,
        排行榜界面 = 33,
        日常界面 = 34,
        活动界面 = 35,
        商城界面 = 36,
        首充界面 = 37,
        VIP充值界面 = 38,
        赌博商店界面 = 39,
        战前预览界面 = 40,
        战前指导界面 = 41,
        活动结算界面 = 42,
        擂台界面 = 43,
        弹出框 = 1001,
        UI模板 = 1002,
        max
    }
    public enum UITemplateType
    {
        聊天框模板   = 901
        ,物品栏模板  = 902
        ,提示信息模板= 903
        ,max
    }
    public enum UIAtlasType
    {
        公用按钮图包1       = 1
        , 公用数字图包1      = 2
        ,公用图标图包1      = 3
        ,物品图标图包1      = 4
        ,BUFF               = 5
        ,chatface           = 6
        ,loadingpar1        = 7
        ,loadingpar2        = 8
        //,SMapcomponent      = 9
        ,vipdengji          = 10
        ,指导               = 11
        ,称号               = 12
        ,战斗技能图包       = 13
        ,UI_非战斗主界面    = 14
        ,UI_战斗主界面      = 15
        ,UI_主界面通用      = 16
        ,登录背景图         = 17
        ,C_activity         = 51
        ,D_huanyng          = 52
        ,D_baoguo           = 101
        ,D_bianqiang        = 102
        ,D_denglu           = 103
        ,D_duboshangdian    = 104
        ,D_fubenfangjian    = 105
        ,D_fubenjiesuan     = 106
        ,D_fubenleixing     = 107
        ,D_haoyou           = 108
        ,D_huodong          = 109
        ,D_jineng           = 110
        ,D_jingjichang      = 111
        ,D_jiugongge        = 112
        ,D_liaotian         = 113
        ,D_renwu            = 114
        ,D_shangcheng       = 115
        ,D_shouchong        = 116
        ,D_xitong           = 117
        ,D_xunlunzzdianji   = 118
        ,D_youjian          = 119
        ,D_zhuangbei        = 120
        ,D_zhujue           = 121
        ,C_instance1        = 131
        ,C_instance2        = 132
        ,C_instance3        = 133
        ,C_instance4        = 134
		,主界面特效			= 201
		,技能界面特效		= 202
		,装备界面特效		= 203
		,装备边框特效		= 204
        , GameUI01
        , GameUI02
        , GameUI03
        , GameMatch01
        , 暗器               = 1000
        ,max
    }
    public enum UIFontType
    {
        simhie,
        sourcehansans_regular,
        huakangwawaw7,
        huakangwawa5,
        youyuan,
        max
    }
    //public struct MapObjSpriteName
    //{
    //    public const string local = "自己";
    //    public const string player = "队友";
    //    public const string monster = "怪物";
    //    public const string boss = "BOSS";
    //    public const string npc = "NPC";
    //    public const string hwFriend = "暗器友";
    //    public const string hwEnemy = "暗器敌";
    //    public const string tp = "传送阵";
    //    public const string ground = "地面";
    //    public const string wall = "阻挡";
    //}
#if USE_OLD_UILOAD && UNITY_EDITOR
    public enum UI资源模板
    {
        图集模板 = 1,
        字体集模板 = 2,
        进度条集模板 = 3,
        max
    }
    public enum 进度条类型
    {
        生命条,
        气力条,
        max
    }
    public enum UIAtlasType
    {
        公用数字图包1,
        公用图标图包1,
        公用按钮图包1,
        非战斗主界面图包,
        物品图标图包1,
        SMapcomponent,
        D_jiugongge,
        chatface,
        vipdengji,
        max
    }
#endif
    class UIViewHelper : AutoSingleT<UIViewHelper>
    {
        static List<short> UILoadScreenLockList = new List<short>();
        static bool CheckUITypeEnum<T>(UIViewType type)
            where T : UIBaseView
        {
            switch (type)
            {
                case UIViewType.Root2D:           if (typeof(T) == typeof(NGUI2DRootPanel)) return true; break;
                /*case UIViewType.登陆游戏界面:     if (typeof(T) == typeof(LoginUIView)) return true; break;
                case UIViewType.服务器选择界面:   if (typeof(T) == typeof(ServerListUIView)) return true; break;
                case UIViewType.创建角色界面:     if (typeof(T) == typeof(CreateCharaUIView)) return true; break;
                case UIViewType.场景Loading界面:  if (typeof(T) == typeof(LoadingSceneUIView)) return true; break;
                case UIViewType.战斗主界面:       if (typeof(T) == typeof(FightingMainView)) return true; break;
                case UIViewType.非战斗主界面:     if (typeof(T) == typeof(NoFightMainView)) return true; break;
                case UIViewType.单人副本界面:     if (typeof(T) == typeof(SingledCopySceneView)) return true; break;
                case UIViewType.单人副本结算:     if (typeof(T) == typeof(SingledCSWinnerView)) return true; break;
                case UIViewType.我要变强界面:     if (typeof(T) == typeof(GotoStrengthenView)) return true; break;
                case UIViewType.多人副本界面:     if (typeof(T) == typeof(MultiCopySceneView)) return true; break;
                case UIViewType.多人活动界面:     if (typeof(T) == typeof(MultiActivityCopySceneView)) return true; break;
                case UIViewType.副本房间界面:     if (typeof(T) == typeof(CopySceneRoomView)) return true; break;
                case UIViewType.队长分配界面:     if (typeof(T) == typeof(ItemAssigningLeaderView)) return true; break;
                case UIViewType.掷点分配界面:     if (typeof(T) == typeof(ItemAssigningCastDiceView)) return true; break;
                case UIViewType.聊天界面:         if (typeof(T) == typeof(ChatMainView)) return true; break;
                case UIViewType.人物背包界面:     if (typeof(T) == typeof(CharacterBagView)) return true; break;
                case UIViewType.背包物品介绍界面: if (typeof(T) == typeof(BagItemPresentView)) return true; break;
                case UIViewType.装备对比界面:     if (typeof(T) == typeof(EquipCampDetailsView)) return true; break;
                case UIViewType.人物属性界面:     if (typeof(T) == typeof(CharacterAttributeView)) return true; break;
                case UIViewType.装备强化界面:     if (typeof(T) == typeof(EquipIntensifyView)) return true; break;
                case UIViewType.装备镶嵌界面:     if (typeof(T) == typeof(EquipInlayView)) return true; break;
                case UIViewType.宝石合成界面:     if (typeof(T) == typeof(GemComposeView)) return true; break;
                case UIViewType.装备洗练界面:     break;
                case UIViewType.咒文置换界面:     break;
                case UIViewType.任务界面:         if (typeof(T) == typeof(TaskMainUIView)) return true; break;
                case UIViewType.系统界面:         if (typeof(T) == typeof(SystemMainUIView)) return true; break;
                case UIViewType.技能界面:         if (typeof(T) == typeof(SkillUIView)) return true; break;
                case UIViewType.竞技场界面:       if (typeof(T) == typeof(ArenaMainView)) return true; break;
                case UIViewType.竞技场结算界面:   if (typeof(T) == typeof(ArenaEndingView)) return true; break;
                case UIViewType.好友界面:         if (typeof(T) == typeof(FriendView)) return true; break;
                case UIViewType.邮件界面:         if (typeof(T) == typeof(MailView)) return true; break;
                case UIViewType.排行榜界面:       if (typeof(T) == typeof(RankingView)) return true; break;
                case UIViewType.日常界面:         if (typeof(T) == typeof(Day2DayView)) return true; break;
                case UIViewType.活动界面:         if (typeof(T) == typeof(ActivityView)) return true; break;
                case UIViewType.商城界面:         if (typeof(T) == typeof(ItemMallView)) return true; break;
                case UIViewType.首充界面:         if (typeof(T) == typeof(FirstRechargeView)) return true; break;
                case UIViewType.VIP充值界面:      if (typeof(T) == typeof(VIPRechargeView)) return true; break;
                case UIViewType.赌博商店界面:     if (typeof(T) == typeof(GambleShopView)) return true; break;
                case UIViewType.战前预览界面:     if (typeof(T) == typeof(FightingPreviewView)) return true; break;
                case UIViewType.战前指导界面:     if (typeof(T) == typeof(FirstFightGuideView)) return true; break;
                case UIViewType.活动结算界面:     if (typeof(T) == typeof(ActivityCSEndingView)) return true; break;
                case UIViewType.擂台界面:         if (typeof(T) == typeof(RingRoomView)) return true; break;*/
            }
            return false;
        }
       /* public static NGUI2DRootPanel LoadRootView(UnityEngine.Transform parent)
        {
            if (null != NGUI2DRootPanel.Inst)
                return NGUI2DRootPanel.Inst;
            NGUI2DRootPanel view = null;
#if USE_OLD_UILOAD && UNITY_EDITOR
            UnityEngine.GameObject uiObj = null;
            UnityEngine.GameObject prefab = UnityEngine.Resources.LoadAssetAtPath<UnityEngine.GameObject>(NGUIUtil.UIViewPrefabPath + type.ToString()) as UnityEngine.GameObject;
            uiObj = UnityEngine.GameObject.Instantiate(prefab) as UnityEngine.GameObject;
            uiObj.name = prefab.name;
            uiObj.transform.parent = parent;
            uiObj.transform.localScale = UnityEngine.Vector3.one;
            uiObj.transform.localPosition = UnityEngine.Vector3.zero;
            uiObj.transform.localRotation = SharedGlobalParams.IdentityQuat;
            UICamera uiCamera = parent.GetComponentInChildren<UICamera>();
            if(null != uiCamera)
            {
                UIAnchor anchor = uiObj.GetComponent<UIAnchor>();
                if (null != anchor)
                    anchor.uiCamera = uiCamera.GetComponent<UnityEngine.Camera>();
                UIAnchor[] anchors = uiObj.GetComponentsInChildren<UIAnchor>();
                for (int i = 0; i < anchors.Length; ++i)
                    anchors[i].uiCamera = uiCamera.GetComponent<UnityEngine.Camera>();
            }
            view = uiObj.GetComponent<T>();
            if (null != view)
                view.UI类型 = type;
            prefab = null;
            UnityEngine.Resources.UnloadUnusedAssets();
#else
            view = UIResourceManager.LoadRootUI();
            if (null != view)
            {
                view.transform.parent = parent;
                view.transform.localScale = UnityEngine.Vector3.one;
                view.transform.localPosition = UnityEngine.Vector3.zero;
                view.transform.localRotation = SharedGlobalParams.IdentityQuat;
            }
#endif
            return view;
        }
        public static T LoadUIViewRes<T>(UIViewType type, UnityEngine.Transform parent = null)
            where T : UIBaseView
        {
            if (null == NGUI2DRootPanel.Inst)
                return null;
            if(!CheckUITypeEnum<T>(type))
            {
                Debug.LogError("界面类型与界面类不一致，载入失败！" + type.ToString() + " != " + typeof(T).ToString());
                return null;
            }
            T view = null;
            switch(type)
            {
                case UIViewType.登陆游戏界面: if (null != LoginUIView.Inst) view = LoginUIView.Inst as T; break;
                case UIViewType.服务器选择界面: if (null != ServerListUIView.Inst) view = ServerListUIView.Inst as T; break;
                case UIViewType.创建角色界面: if (null != CreateCharaUIView.Inst) view = CreateCharaUIView.Inst as T; break;
                case UIViewType.场景Loading界面: if (null != LoadingSceneUIView.Inst) view = LoadingSceneUIView.Inst as T; break;
                case UIViewType.战斗主界面: if (null != FightingMainView.Inst) view = FightingMainView.Inst as T; break;
                case UIViewType.非战斗主界面: if (null != NoFightMainView.Inst) view = NoFightMainView.Inst as T; break;
                case UIViewType.单人副本界面: if (null != SingledCopySceneView.Inst) view = SingledCopySceneView.Inst as T; break;
                case UIViewType.单人副本结算: if (null != SingledCSWinnerView.Inst) view = SingledCSWinnerView.Inst as T; break;
                case UIViewType.我要变强界面: if (null != GotoStrengthenView.Inst) view = GotoStrengthenView.Inst as T; break;
                case UIViewType.多人副本界面: if (null != MultiCopySceneView.Inst) view = MultiCopySceneView.Inst as T; break;
                case UIViewType.多人活动界面: if (null != MultiActivityCopySceneView.Inst) view = MultiActivityCopySceneView.Inst as T; break;
                case UIViewType.副本房间界面: if (null != CopySceneRoomView.Inst) view = CopySceneRoomView.Inst as T; break;
                case UIViewType.队长分配界面: if (null != ItemAssigningLeaderView.Inst) view = ItemAssigningLeaderView.Inst as T; break;
                case UIViewType.掷点分配界面: if (null != ItemAssigningCastDiceView.Inst) view = ItemAssigningCastDiceView.Inst as T; break;
                case UIViewType.聊天界面: if (null != ChatMainView.Inst) view = ChatMainView.Inst as T; break;
                case UIViewType.人物背包界面: if (null != CharacterBagView.Inst) view = CharacterBagView.Inst as T; break;
                case UIViewType.背包物品介绍界面: if (null != BagItemPresentView.Inst) view = BagItemPresentView.Inst as T; break;
                case UIViewType.装备对比界面: if (null != EquipCampDetailsView.Inst) view = EquipCampDetailsView.Inst as T; break;
                case UIViewType.人物属性界面: if (null != CharacterAttributeView.Inst) view = CharacterAttributeView.Inst as T; break;
                case UIViewType.装备强化界面: if (null != EquipIntensifyView.Inst) view = EquipIntensifyView.Inst as T; break;
                case UIViewType.装备镶嵌界面: if (null != EquipInlayView.Inst) view = EquipInlayView.Inst as T; break;
                case UIViewType.宝石合成界面: if (null != GemComposeView.Inst) view = GemComposeView.Inst as T; break;
                case UIViewType.任务界面: if (null != TaskMainUIView.Inst) view = TaskMainUIView.Inst as T; break;
                case UIViewType.系统界面: if (null != SystemMainUIView.Inst) view = SystemMainUIView.Inst as T; break;
                case UIViewType.技能界面: if (null != SkillUIView.Inst) view = SkillUIView.Inst as T; break;
                case UIViewType.竞技场界面: if (null != ArenaMainView.Inst) view = ArenaMainView.Inst as T; break;
                case UIViewType.竞技场结算界面: if (null != ArenaEndingView.Inst) view = ArenaEndingView.Inst as T; break;
                case UIViewType.好友界面: if (null != FriendView.Inst) view = FriendView.Inst as T; break;
                case UIViewType.邮件界面: if (null != MailView.Inst) view = MailView.Inst as T; break;
                case UIViewType.排行榜界面: if (null != RankingView.Inst) view = RankingView.Inst as T; break;
                case UIViewType.日常界面: if (null != Day2DayView.Inst) view = Day2DayView.Inst as T; break;
                case UIViewType.活动界面: if (null != ActivityView.Inst) view = ActivityView.Inst as T; break;
                case UIViewType.商城界面: if (null != ItemMallView.Inst) view = ItemMallView.Inst as T; break;
                case UIViewType.首充界面: if (null != FirstRechargeView.Inst) view = FirstRechargeView.Inst as T; break;
                case UIViewType.VIP充值界面: if (null != VIPRechargeView.Inst) view = VIPRechargeView.Inst as T; break;
                case UIViewType.赌博商店界面: if (null != GambleShopView.Inst) view = GambleShopView.Inst as T; break;
                case UIViewType.战前预览界面: if (null != FightingPreviewView.Inst) view = FightingPreviewView.Inst as T; break;
                case UIViewType.战前指导界面: if (null != FirstFightGuideView.Inst) view = FirstFightGuideView.Inst as T; break;
                case UIViewType.活动结算界面: if (null != ActivityCSEndingView.Inst) view = ActivityCSEndingView.Inst as T; break;
                case UIViewType.擂台界面: if (null != RingRoomView.Inst) view = RingRoomView.Inst as T; break;
            }
            if (null != view)
            {
                //if (view.IsReadyShow)
                //    view.DoReadyShowing();
                return view;
            }
            if (type != 界面类型.场景Loading界面)
            {
                if (type == 界面类型.背包物品介绍界面)
                    NGUI2DRootPanel.Inst.CloseUI(type, 界面类型.人物背包界面);
                else if (type == 界面类型.赌博商店界面)
                    NGUI2DRootPanel.Inst.CloseUI(type, 界面类型.人物背包界面);
                else if (type == 界面类型.副本房间界面 && null != MultiActivityCopySceneView.Inst)
                    NGUI2DRootPanel.Inst.CloseUI(type, 界面类型.多人活动界面);
                else if (type != 界面类型.装备对比界面 && type != 界面类型.聊天界面)
                    NGUI2DRootPanel.Inst.CloseUI(type);
                if (type == 界面类型.队长分配界面 || type == 界面类型.掷点分配界面 || type == 界面类型.活动结算界面)
                {
                    if (null != DeathPopupDialog.Inst)
						DeathPopupDialog.Inst.OnForceClose();
                    if (null != BufferListDialog.Inst)
                        BufferListDialog.Inst.OnClose();
                }
                if (!UILoadScreenLockList.Contains((short)type))
                    UILoadScreenLockList.Add((short)type);
                if (type != 界面类型.场景Loading界面)
                    ScreenLockedView.Lock(ScreenLockedType.popup_ui, 2f, 3f);
            }
            if (null == parent)
                parent = NGUI2DRootPanel.Inst.transform;
#if USE_OLD_UILOAD && UNITY_EDITOR
            UnityEngine.GameObject uiObj = null;
            UnityEngine.GameObject prefab = UnityEngine.Resources.LoadAssetAtPath<UnityEngine.GameObject>(NGUIUtil.UIViewPrefabPath + type.ToString()) as UnityEngine.GameObject;
            uiObj = UnityEngine.GameObject.Instantiate(prefab) as UnityEngine.GameObject;
            uiObj.name = prefab.name;
            uiObj.transform.parent = parent;
            uiObj.transform.localScale = UnityEngine.Vector3.one;
            uiObj.transform.localPosition = UnityEngine.Vector3.zero;
            uiObj.transform.localRotation = SharedGlobalParams.IdentityQuat;
            UICamera uiCamera = parent.GetComponentInChildren<UICamera>();
            if(null != uiCamera)
            {
                UIAnchor anchor = uiObj.GetComponent<UIAnchor>();
                if (null != anchor)
                    anchor.uiCamera = uiCamera.GetComponent<UnityEngine.Camera>();
                UIAnchor[] anchors = uiObj.GetComponentsInChildren<UIAnchor>();
                for (int i = 0; i < anchors.Length; ++i)
                    anchors[i].uiCamera = uiCamera.GetComponent<UnityEngine.Camera>();
            }
            view = uiObj.GetComponent<T>();
            if (null != view)
                view.UI类型 = type;
            prefab = null;
            UnityEngine.Resources.UnloadUnusedAssets();
#else
            string uiTypeName = type.ToString();
            string atlasList = LogicModel.GetResName(ResourceType.UINeedAtlas, uiTypeName);
            view = UIResourceManager.LoadUI<T>(type, atlasList);
            if (null != view)
            {
                view.transform.parent = parent;
                view.transform.localScale = UnityEngine.Vector3.one;
                view.transform.localPosition = UnityEngine.Vector3.zero;
                view.transform.localRotation = SharedGlobalParams.IdentityQuat;
				view.UI类型 = type;
				if (ItemGridsViewPanel.InstCount > 0)
				{
					switch(view.UI类型)
					{
						case 界面类型.装备对比界面:
						{
							Camera cam = ItemGridsViewPanel.Inst.RenderTargetUpperRoot.GetComponentInChildren<Camera>();
							UIAnchor anchor = view.GetComponent<UIAnchor>();
							if (null != anchor)
								anchor.uiCamera = cam;
							UIAnchor[] anchors = view.GetComponentsInChildren<UIAnchor>();
							for (int i = 0; i < anchors.Length; ++i)
								anchors[i].uiCamera = cam;
						}
						break;
					}
				}
            }
#endif
            return view;
        }
        public static void OnUIReadyed(界面类型 t)
        {
            if(UILoadScreenLockList.Contains((short)t))
                UILoadScreenLockList.Remove((short)t);
            if(UILoadScreenLockList.Count <= 0)
                ScreenLockedView.Unlock(ScreenLockedType.popup_ui);
        }
        //public static UITexture AddBGPanel(UnityEngine.GameObject parent, bool fullScene, bool locked)
        //{
        //    if (null == parent)
        //        parent = NGUI2DRootPanel.Inst.gameObject;
        //    UITexture bg = NGUITools.AddWidget<UITexture>(parent);
        //    if (fullScene)
        //    {
        //        //UnityEngine.Camera camera = NGUI2DRootPanel.Inst.GetComponentInChildren<UnityEngine.Camera>();
        //        //bg.width = (int)UnityEngine.Display.main.renderingWidth;
        //        //bg.height = (int)UnityEngine.Display.main.renderingHeight;
        //        bg.width = (int)NGUI2DRootPanel.UIViewPortSize.x;
        //        bg.height = (int)NGUI2DRootPanel.UIViewPortSize.y;
        //    }
        //    if(locked)
        //    {
        //        UnityEngine.BoxCollider box = NGUITools.AddWidgetCollider(bg.gameObject);
        //        //box.size = new UnityEngine.Vector3(UnityEngine.Display.main.renderingWidth, UnityEngine.Display.main.renderingHeight);
        //        box.size = new UnityEngine.Vector3(NGUI2DRootPanel.UIViewPortSize.x, NGUI2DRootPanel.UIViewPortSize.y);
        //    }
        //    return bg;
        //}*/
		public static UISprite AddUISprite(UnityEngine.GameObject parent, UIAtlasType at, string imageName, UIPanel panel, UISprite.Type spType = UISprite.Type.Simple, int width = 0, int height = 0)
        {
			UISprite sprite = null;
			GameObject go = new GameObject();
			go.layer = parent.layer;
			Transform t = go.transform;
			t.parent = parent.transform;
			t.localPosition = Vector3.zero;
			t.localRotation = Quaternion.identity;
			t.localScale = Vector3.one;
			UISprite spEx = go.AddComponent<UISprite>();
			spEx.type = spType;
			if(spType != UIBasicSprite.Type.Simple)
			{
	            spEx.width = width;
	            spEx.height = height;
			}
            spEx.atlas = UIResourceBinder.FindAtlas(at.ToString());
            spEx.spriteName = imageName;
            sprite = spEx;
            //UIResourceManager.LoadAtlas(at, spEx);
            return sprite;
        }/*

		public static UISprite AddUISprite(UnityEngine.GameObject parent, UIAtlasType at, int imageIdx, UIPanel panel, UISprite.Type spType = UISprite.Type.Simple, int width = 0, int height = 0)
        {
			GameObject go = new GameObject();
			go.layer = parent.layer;
            Transform t = go.transform;
            t.parent = parent.transform;
            t.localPosition = Vector3.zero;
            t.localRotation = SharedGlobalParams.IdentityQuat;
            t.localScale = Vector3.one;
            UISpriteEx spEx = go.AddComponent<UISpriteEx>();
            if (null != panel)
			    spEx.SetPanel(panel);
			spEx.type = spType;
			if(spType != UIBasicSprite.Type.Simple)
			{
				spEx.width = width;
				spEx.height = height;
			}
            spEx.SpriteIndex = imageIdx;
            UIResourceManager.LoadAtlas(at, spEx);
            return spEx;
        }
		public static UISprite[] AddUISprites(UnityEngine.GameObject parent, UIAtlasType at, string imageNames, int ast, UIWidget.Pivot pivot, Vector3 startPos, UIPanel panel, UISprite.Type spType = UISprite.Type.Simple, int width = 0, int height = 0)
        {
            UISprite[] ret = null;
            if (!string.IsNullOrEmpty(imageNames))
            {
                int maxWidth = 0;
                int maxHeight = 0;
                ret = new UISprite[imageNames.Length];
                UISpriteEx spEx = null;
                for (int i = 0; i < imageNames.Length; ++i)
				{
					GameObject go = new GameObject();
					go.layer = parent.layer;
					Transform t = go.transform;
					t.parent = parent.transform;
					t.localPosition = Vector3.zero;
					t.localRotation = SharedGlobalParams.IdentityQuat;
					t.localScale = Vector3.one;
					spEx = go.AddComponent<UISpriteEx>();
                    if (null != panel)
						spEx.SetPanel(panel);
					spEx.type = spType;
					if(spType != UIBasicSprite.Type.Simple)
					{
						spEx.width = width;
						spEx.height = height;
					}
                    switch(imageNames[i].ToString())
                    {
                        case "0": spEx.SpriteIndex = ast + 0; break;
                        case "1": spEx.SpriteIndex = ast + 1; break;
                        case "2": spEx.SpriteIndex = ast + 2; break;
                        case "3": spEx.SpriteIndex = ast + 3; break;
                        case "4": spEx.SpriteIndex = ast + 4; break;
                        case "5": spEx.SpriteIndex = ast + 5; break;
                        case "6": spEx.SpriteIndex = ast + 6; break;
                        case "7": spEx.SpriteIndex = ast + 7; break;
                        case "8": spEx.SpriteIndex = ast + 8; break;
                        case "9": spEx.SpriteIndex = ast + 9; break;
                        case "+": spEx.SpriteIndex = ast + 10; break;
                        case "/": spEx.SpriteIndex = ast + 11; break;
                        default: spEx.spriteName = imageNames[i].ToString(); break;
                    }
                    spEx.pivot = pivot;
                    UIResourceManager.LoadAtlas(at, spEx);
                    maxWidth += spEx.width;
                    if (maxHeight < spEx.height)
                        maxHeight = spEx.height;
                    ret[i] = spEx;
                }
                switch (pivot)
                {
                    case UIWidget.Pivot.Center:
                        {
                            Vector3 pos = startPos;
                            pos.x -= (float)maxWidth * 0.5f;
                            for (int i = 0; i < ret.Length; ++i)
                            {
                                if(i == 0)
                                    pos.x += (float)ret[i].width * 0.5f;
                                ret[i].transform.localPosition = pos;
                                pos.x += ret[i].width;
                            }
                        }
                        break;
                    case UIWidget.Pivot.Right:
                    case UIWidget.Pivot.BottomRight:
                    case UIWidget.Pivot.TopRight:
                        {
                            Vector3 pos = startPos;
                            for (int i = ret.Length - 1; i >= 0; --i)
                            {
                                ret[i].transform.localPosition = pos;
                                pos.x -= ret[i].width;
                            }
                        }
                        break;
                }
            }
            return ret;
        }*/
        public static UILabel AddUILabel(UnityEngine.GameObject parent, UIFontType ft, int fontSize, UnityEngine.Color color)
        {
			UILabel lbl = null;
			GameObject go = new GameObject();
			go.layer = parent.layer;
			Transform t = go.transform;
			t.parent = parent.transform;
			t.localPosition = Vector3.zero;
			t.localRotation = Quaternion.identity;
			t.localScale = Vector3.one;
			UILabel lblEx = go.AddComponent<UILabel>();
            lblEx.ambigiousFont = UIResourceManager.LoadFont(ft);
            lbl = lblEx;
            lbl.fontSize = fontSize;
            lbl.color = color;
            lbl.pivot = UIWidget.Pivot.Center;
            lbl.keepCrispWhenShrunk = UILabel.Crispness.Never;
            lbl.AssumeNaturalSize();
            return lbl;
        }
        public static UIHyperLinkLabel AddHyperLinkLabel(UnityEngine.GameObject parent, UIFontType ft, int fontSize, UnityEngine.Color color)
		{
			GameObject go = new GameObject();
			go.layer = parent.layer;
			go.name = "UIHyperLinkLabel";
			Transform t = go.transform;
			t.parent = parent.transform;
			t.localPosition = Vector3.zero;
			t.localRotation = Quaternion.identity;
			t.localScale = Vector3.one;
			UIHyperLinkLabel lbl = go.AddComponent<UIHyperLinkLabel>();
			lbl.ambigiousFont = UIResourceManager.LoadFont(ft);
            lbl.fontSize = fontSize;
            lbl.color = color;
            lbl.pivot = UIWidget.Pivot.Center;
            lbl.overflowMethod = UILabel.Overflow.ResizeFreely;
            lbl.keepCrispWhenShrunk = UILabel.Crispness.Never;
            return lbl;
        }/*
		public static UISprite AddEquipIntensifyEffect(UI图包类型 at, string n, Vector3 pos, Vector3 scale, Vector3 rota, Transform parent, UIPanel panel)
		{
			GameObject effObj = new GameObject("effect");
			effObj.layer = parent.gameObject.layer;
			Transform effTrans = effObj.transform;
			effTrans.parent = parent;
			effTrans.localScale = scale;
			effTrans.localPosition = pos;
			effTrans.localRotation = Quaternion.Euler(rota);
			UISprite sp = AddUISprite(effObj, at, n + "01", panel);
			UISpriteEx spEx = sp as UISpriteEx;
			if (null == sp || null == spEx)
				return null;
			UIFrameSprite fsp = sp.gameObject.AddComponent<UIFrameSprite>();
			spEx.OnAtlasLoadOvered += new UISpriteEx.EventHandler(fsp.OnSpriteAtlasLoaded);
			fsp.namePrefix = n;
			fsp.framesPerSecond = 30;
			return sp;
		}
        public static IconViewStaticItem AddStaticItemIcon(int id, int count, UnityEngine.GameObject parent, UIPanel panel = null, bool showName = true, int nameFtSize = 17, int width = 0, int height = 0)
        {
            ItemLuaTable.ValDesc data = ItemLuaTable.Inst[id];
            if (null == data)
                return null;
			GameObject icon = new GameObject();
			icon.layer = parent.layer;
			icon.name = data.Name;
			Transform t = icon.transform;
			t.parent = parent.transform;
			t.localPosition = Vector3.zero;
			t.localRotation = SharedGlobalParams.IdentityQuat;
			t.localScale = Vector3.one;
            IconViewStaticItem comp = icon.AddComponent<IconViewStaticItem>();
            comp.底板 = AddUISprite(icon, UIAtlasType.物品图标图包1, (int)ItemAtlasSprite.ItemGrid, panel);
            if (null == comp.底板)
            {
				GameObject.Destroy(icon);
                return null;
            }
            comp.图标 = AddUISprite(icon, UIAtlasType.物品图标图包1, data.ImageIndex, panel,
			                      UISprite.Type.Sliced, 70, 70);
            if (null == comp.图标)
            {
                PopupDialogView.Popup(PopupDialogType.错误提示, "错误的图标名：" + data.ImageName + "用默认图标代替！");
				comp.图标 = AddUISprite(icon, UIAtlasType.物品图标图包1, (int)ItemAtlasSprite.ErrorIcon, panel,
				                      UISprite.Type.Sliced, 70, 70);
                if (null == comp.图标)
                {
					GameObject.Destroy(icon);
                    return null;
                }
            }
            if (!string.IsNullOrEmpty(data.QualityImageName))
            {
                comp.边框 = AddUISprite(icon, UIAtlasType.物品图标图包1, data.QualityImageIndex, panel);
                if (null == comp.边框)
                {
					GameObject.Destroy(icon);
                    return null;
                }
            }
            float nameOffY = 0;
            if (count > 1)
            {
                comp.数值 = AddUISprites(icon, UIAtlasType.物品图标图包1, count.ToString(), (int)ItemAtlasSprite.NumberBegin, UIWidget.Pivot.BottomRight,
                    new UnityEngine.Vector3((float)comp.底板.width * 0.5f - 5, -(float)comp.底板.height * 0.5f + 5), panel,
				                       UISprite.Type.Sliced, 16, 26);
            }
            if (showName)
            {
                comp.物品名 = AddUILabel(icon, UIHlp.动态字体类型.SIMHEI, nameFtSize, data.QualityColor);
                comp.物品名.overflowMethod = UILabel.Overflow.ResizeFreely;
                UILabelEffect.公用文字描边.FillLabel(comp.物品名);
                comp.物品名.pivot = UIWidget.Pivot.Top;
                comp.物品名.transform.localPosition = new UnityEngine.Vector3(0, -((float)comp.底板.height * 0.5f) - nameOffY - 5.0f, 0);
                comp.物品名.text = data.Name;
                nameOffY += comp.物品名.height;
            }
            return comp;
        }
        public static IconViewGambleEquip AddGambleEquipIcon(Text.装备物品类型 part, UnityEngine.GameObject parent, UIPanel panel = null, string equipName = "", int nameFtSize = 17)
        {
            if (part < Text.装备物品类型.武器 || part >= Text.装备物品类型.max)
				return null;
			GameObject icon = new GameObject();
			icon.layer = parent.layer;
			icon.name = "GambleEquipIcon";
			Transform t = icon.transform;
			t.parent = parent.transform;
			t.localPosition = Vector3.zero;
			t.localRotation = SharedGlobalParams.IdentityQuat;
			t.localScale = Vector3.one;
            IconViewGambleEquip comp = icon.AddComponent<IconViewGambleEquip>();
            comp.底板 = AddUISprite(icon, UIAtlasType.物品图标图包1, (int)ItemAtlasSprite.ItemGrid, panel);
            if (null == comp.底板)
            {
				GameObject.Destroy(icon);
                return null;
            }
            //string iconName = EquipLuaTable.GetGambleEquipIcon(part);
			comp.图标 = AddUISprite(icon, UIAtlasType.物品图标图包1, (int)ItemAtlasSprite.ErrorIcon, panel,
			                      UISprite.Type.Sliced, 70, 70);
            return comp;
        }
        public static IconViewDynamicItem AddDynamicItemIcon(Shared.ItemBaseData itemData, UnityEngine.GameObject parent, UIPanel panel = null, bool showName = true, int nameFtSize = 16, int count = -1, bool showLv = false, string prefix = "", string baseAttr = "")
        {
            ItemLuaTable.ValDesc metaData = ItemLuaTable.Inst[itemData.id];
            if (null == metaData)
				return null;
			GameObject icon = new GameObject();
			icon.layer = parent.layer;
			icon.name = metaData.Name;
			Transform t = icon.transform;
			t.parent = parent.transform;
			t.localPosition = Vector3.zero;
			t.localRotation = SharedGlobalParams.IdentityQuat;
			t.localScale = Vector3.one;
            IconViewDynamicItem comp = icon.AddComponent<IconViewDynamicItem>();
            comp.底板 = AddUISprite(icon, UIAtlasType.物品图标图包1, (int)ItemAtlasSprite.ItemGrid, panel);
            if (null == comp.底板)
            {
				GameObject.Destroy(icon);
                return null;
            }
			comp.图标 = AddUISprite(icon, UIAtlasType.物品图标图包1, metaData.ImageIndex, panel,
			                      UISprite.Type.Sliced, 70, 70);
            if (null == comp.图标)
            {
                PopupDialogView.Popup(PopupDialogType.错误提示, "错误的图标名：" + metaData.ImageName + "用默认图标代替！");
				comp.图标 = AddUISprite(icon, UIAtlasType.物品图标图包1, (int)ItemAtlasSprite.ErrorIcon, panel,
				                      UISprite.Type.Sliced, 70, 70);
                if (null == comp.图标)
                {
					GameObject.Destroy(icon);
                    return null;
                }
            }
            if (!string.IsNullOrEmpty(metaData.QualityImageName))
            {
                comp.边框 = AddUISprite(icon, UIAtlasType.物品图标图包1, metaData.QualityImageIndex, panel);
                if (null == comp.边框)
                {
                    UnityEngine.GameObject.Destroy(icon);
                    return null;
                }
            }
            if (count == -1)
                count = itemData.count;
            if (count > 1)
            {
                comp.数值 = AddUISprites(icon, UIAtlasType.物品图标图包1, count.ToString(), (int)ItemAtlasSprite.NumberBegin, UIWidget.Pivot.BottomRight,
                    new UnityEngine.Vector3((float)comp.底板.width * 0.5f - 5, -(float)comp.底板.height * 0.5f + 5), panel,
				                       UISprite.Type.Sliced, 16, 26);
            }
            float nameOffY = 0;
            Shared.EquipItemData equipData = null;
            if (metaData.IsEquip)
            {
				equipData = itemData as Shared.EquipItemData;
                if (null != equipData)
                {
                    if (equipData.Lv > 0)
                    {
                        comp.强化等级 = AddUISprites(icon, UIAtlasType.物品图标图包1, "+" + equipData.Lv.ToString(),
					                         (int)ItemAtlasSprite.NumberBegin, UIWidget.Pivot.Center, UnityEngine.Vector3.zero, panel,
					                         UISprite.Type.Sliced, 22, 34);
                        if(null != comp.强化等级 && comp.强化等级.Length > 0)
                        {
                            Color sc = EquipLuaTable.GetStLvColor(equipData.Lv);
                            for (int i = 0; i < comp.强化等级.Length; ++i)
                                comp.强化等级[i].color = sc;
                        }
                        UI图包类型 intEffectAtlas = UI图包类型.装备边框特效;
                        Vector3 intPos = Vector3.zero, intScale = Vector3.one, intRota = Vector3.zero;
                        string intEffect = ConstLuaTable.Inst.GetEquipIntensifyLVEffect(equipData.Lv, ref intEffectAtlas, ref intPos, ref intScale, ref intRota);
                        if(!string.IsNullOrEmpty(intEffect))
                        {
                            comp.强化特效 = AddEquipIntensifyEffect(intEffectAtlas, intEffect, intPos, intScale, intRota, icon.transform, panel);
                            if (null == comp.强化特效)
                            {
                                GameObject.Destroy(comp.gameObject);
                                return null;
                            }
                        }
                    }
                }
            }
            if(showName)
            {
                comp.物品名 = AddUILabel(icon, UIHlp.动态字体类型.SIMHEI, nameFtSize, metaData.QualityColor);
                comp.物品名.overflowMethod = UILabel.Overflow.ResizeFreely;
                UILabelEffect.公用文字描边.FillLabel(comp.物品名);
                comp.物品名.pivot = UIWidget.Pivot.Top;
                comp.物品名.transform.localPosition = new UnityEngine.Vector3(0, -((float)comp.底板.height * 0.5f) - nameOffY, 0);
                if (showLv && metaData.IsEquip)
                    comp.物品名.text = metaData.Name + " +" + equipData.Lv;
                else
                    comp.物品名.text = metaData.Name;
                nameOffY += comp.物品名.height;
            }
            LocalPlayer lp = LocalPlayer.Inst;
            if (metaData.LimitJob != 职业类型.所有 && metaData.LimitJob != (职业类型)lp.Job)
            {
                comp.蒙版 = AddUISprite(icon, UIAtlasType.物品图标图包1, (int)ItemAtlasSprite.Mask1, panel,
				                      UISprite.Type.Sliced, 70, 70);
                if (null != comp.蒙版)
                    comp.蒙版.color = new UnityEngine.Color(1f, 0, 0, 0.3f);
            }
            else if (metaData.LimitLV > lp.Lvl)
            {
                if (metaData.ItemType == 物品类型.装备 || metaData.ItemType == 物品类型.恢复 || metaData.ItemType == 物品类型.礼包)
                {
                    comp.等级不足 = AddUISprite(icon, UIAtlasType.物品图标图包1, (int)ItemAtlasSprite.DiffJob, panel);
                    if (null != comp.等级不足)
                        comp.等级不足.transform.localPosition = new UnityEngine.Vector3(24f, 24f);
                }
            }
            if (itemData.binded)
            {
                comp.绑定角标 = AddUISprite(icon, UIAtlasType.物品图标图包1, (int)ItemAtlasSprite.BindLock, panel,
				                        UISprite.Type.Sliced, 25, 25);
                if(null != comp.绑定角标)
                    comp.绑定角标.transform.localPosition = new UnityEngine.Vector3(-25f, -24f);
            }
            if (metaData.ItemType == 物品类型.前后缀)
            {
                if ((前后缀类型)metaData.SubType == 前后缀类型.前缀)
                    comp.左上角标 = AddUISprite(icon, UIAtlasType.物品图标图包1, (int)ItemAtlasSprite.PrePrefix, panel,
					                        UISprite.Type.Sliced, 25, 25);
                else
					comp.左上角标 = AddUISprite(icon, UIAtlasType.物品图标图包1, (int)ItemAtlasSprite.SufPrefix, panel,
					                        UISprite.Type.Sliced, 25, 25);
                comp.左上角标.transform.localPosition = new UnityEngine.Vector3(-24, 24);
            }
            return comp;
        }
        public static InlayItemGridView AddInlayItemIcon(UnityEngine.Vector2 size, UnityEngine.GameObject parent, bool cursesPart, UIPanel panel = null)
		{
			GameObject icon = new GameObject();
			icon.layer = parent.layer;
			icon.name = "InlayItemGridView";
			Transform t = icon.transform;
			t.parent = parent.transform;
			t.localPosition = Vector3.zero;
			t.localRotation = SharedGlobalParams.IdentityQuat;
			t.localScale = Vector3.one;
			InlayItemGridView grid = icon.AddComponent<InlayItemGridView>();
			grid.width = (int)size.x;
			grid.height = (int)size.y;
            grid.底板 = AddUISprite(grid.gameObject, UIAtlasType.物品图标图包1, (int)ItemAtlasSprite.ItemGrid, panel,
			                      UISprite.Type.Sliced, (int)size.x, (int)size.y);
            if (null == grid.底板)
            {
				GameObject.Destroy(grid.gameObject);
                return null;
            }
            if (cursesPart)
            {
				grid.咒语特效 = AddUISprite(grid.gameObject, UIAtlasType.物品图标图包1, (int)ItemAtlasSprite.YellowSel, panel,
				                        UISprite.Type.Sliced, (int)size.x + 8, (int)size.y + 8);
                if (null == grid.咒语特效)
                {
					GameObject.Destroy(grid.gameObject);
                    return null;
                }
            }
            return grid;
        }
        public static Dictionary<System.Enum, ValT> GenEnumDictValues<ValT>(UnityEngine.GameObject parent, List<System.Enum> keys, out Dictionary<System.Enum, ValT> dict)
            where ValT : UnityEngine.Component
        {
            dict = new Dictionary<System.Enum, ValT>();
            ValT[] allVals = parent.GetComponentsInChildren<ValT>();
            for (int i = 0; i < allVals.Length; ++i)
            {
                ValT val = allVals[i];
                for (int e = 0; e < keys.Count; ++e)
                {
                    if (val.name == keys[e].ToString())
                    {
                        dict.Add(keys[e], val);
                        keys.RemoveAt(e);
                        break;
                    }
                }
            }
            return dict;
        }
        public static UILabel UpdateAttributeLabel<T1, T2>(System.Enum type, T1 min, T2 max, ref Dictionary<System.Enum, UILabel> dict, UnityEngine.GameObject parent = null)
            where T1 : System.IFormattable
            where T2 : System.IFormattable
        {
            string value = string.Format("{0}-{1}", min.ToString(), max.ToString());
            return UpdateAttributeLabel(type, value, ref dict, parent);
        }
        public static UILabel UpdateAttributeLabel<T1, T2>(System.Enum type, T1 min, T2 max, string sign, ref Dictionary<System.Enum, UILabel> dict, UnityEngine.GameObject parent = null)
            where T1 : System.IFormattable
            where T2 : System.IFormattable
        {
            string value = string.Format("{0}{1}{2}", min.ToString(), max.ToString(), sign);
            return UpdateAttributeLabel(type, value, ref dict, parent);
        }
        public static UILabel UpdateAttributeLabel<T>(System.Enum type, T value, ref Dictionary<System.Enum, UILabel> dict, UnityEngine.GameObject parent = null)
        //    where T : System.IConvertible
        {
            UILabel label = null;
            if (dict.ContainsKey(type))
            {
                label = dict[type];
                label.text = value.ToString();
            }
            else if (null != parent)
            {
                UILabel[] allLabels = parent.GetComponentsInChildren<UILabel>();
                for (int i = 0; i < allLabels.Length; ++i)
                {
                    UILabel lab = allLabels[i];
                    if (lab.name == type.ToString())
                    {
                        label = lab;
                        dict.Add(type, label);
                        label.text = value.ToString();
                        break;
                    }
                }
            }
            return label;
        }
        public static UILabel AddCharacterName(string name, UnityEngine.GameObject parent)
        {
            UILabel lbl = null;
#if USE_OLD_UILOAD && UNITY_EDITOR
            UIFont font = NGUIUtil.FindFont(UIHlp.动态字体类型.SIMHEI);
            lbl = NGUITools.AddWidget<UILabel>(parent);
            lbl.ambigiousFont = font.dynamicFont;
#else
            UILabelEx lblEx = NGUITools.AddWidget<UILabelEx>(parent);
            lblEx.ambigiousFont = UIResourceManager.Inst[UIHlp.动态字体类型.SIMHEI];
            //UIResourceManager.LoadFont(UIHlp.动态字体类型.SIMHEI, lblEx);
            lbl = lblEx;
#endif
            lbl.depth = 11;
            lbl.overflowMethod = UILabel.Overflow.ResizeFreely;
            lbl.text = name;
            lbl.fontSize = 20;
            return lbl;
        }
        public static UISlider AddSlider(UIHlp.UI模板类型 t, UnityEngine.GameObject parent)
        {
            UISlider s = null;
#if USE_OLD_UILOAD && UNITY_EDITOR
            s = NGUIUtil.FindSlider(t, parent);
#else
            string tempName = t.ToString();
            string atlasList = LogicModel.GetResName(ResourceType.UINeedAtlas, tempName);
            s = UIResourceManager.LoadUISlider(tempName, atlasList);
            if (null != s)
            {
                s.transform.parent = parent.transform;
                s.transform.localScale = UnityEngine.Vector3.one;
                s.transform.localPosition = UnityEngine.Vector3.zero;
                s.transform.localRotation = SharedGlobalParams.IdentityQuat;
            }
#endif
            return s;
        }
        public static ItemGridsViewPanel AddItemGridPanel(UnityEngine.GameObject parent)
        {
            ItemGridsViewPanel view = null;
#if USE_OLD_UILOAD && UNITY_EDITOR
            string tempName = NGUIUtil.UIViewPrefabPath + "Templates/物品栏模板";
            UnityEngine.Object assetObject = UnityEngine.Resources.LoadAssetAtPath<Object>(tempName);
            UnityEngine.GameObject newObj = NGUITools.AddChild(parent, assetObject as UnityEngine.GameObject);
            view = newObj.GetComponent<ItemGridsViewPanel>();
            if (null != view)
            {
                view.transform.parent = parent.transform;
                view.transform.localScale = UnityEngine.Vector3.one;
                view.transform.localPosition = UnityEngine.Vector3.zero;
                view.transform.localRotation = SharedGlobalParams.IdentityQuat;
            }
            assetObject = null;
            UnityEngine.Resources.UnloadUnusedAssets();
#else
            string tempName = UIHlp.UI模板类型.物品栏模板.ToString();
            string atlasList = LogicModel.GetResName(ResourceType.UINeedAtlas, tempName);
            view = UIResourceManager.LoadUITemplate<ItemGridsViewPanel>(tempName, atlasList);
            if (null != view)
            {
                view.transform.parent = parent.transform;
                view.transform.localScale = UnityEngine.Vector3.one;
                view.transform.localPosition = UnityEngine.Vector3.zero;
                view.transform.localRotation = SharedGlobalParams.IdentityQuat;
            }
#endif
            return view;
        }
        public static ChatViewPanel AddChatPanel(UnityEngine.GameObject parent)
        {
            ChatViewPanel view = null;
#if USE_OLD_UILOAD && UNITY_EDITOR
            string tempName = NGUIUtil.UIViewPrefabPath + "Templates/聊天框模板";
            UnityEngine.Object assetObject = UnityEngine.Resources.LoadAssetAtPath<Object>(tempName);
            UnityEngine.GameObject newObj = NGUITools.AddChild(parent, assetObject as UnityEngine.GameObject);
            view = newObj.GetComponent<ChatViewPanel>();
            if (null != view)
            {
                view.transform.parent = parent.transform;
                view.transform.localScale = UnityEngine.Vector3.one;
                view.transform.localPosition = UnityEngine.Vector3.zero;
                view.transform.localRotation = SharedGlobalParams.IdentityQuat;
            }
            assetObject = null;
            UnityEngine.Resources.UnloadUnusedAssets();
#else
            string tempName = UIHlp.UI模板类型.聊天框模板.ToString();
            string atlasList = LogicModel.GetResName(ResourceType.UINeedAtlas, tempName);
            view = UIResourceManager.LoadUITemplate<ChatViewPanel>(tempName, atlasList);
            if (null != view)
            {
                view.transform.parent = parent.transform;
                view.transform.localScale = UnityEngine.Vector3.one;
                view.transform.localPosition = UnityEngine.Vector3.zero;
                view.transform.localRotation = SharedGlobalParams.IdentityQuat;
            }
#endif
            return view;
        }
        public static IntroRootTemplate AddIntroPanel(UnityEngine.GameObject parent = null)
        {
            if (null == parent)
                parent = NGUI2DRootPanel.Inst.gameObject;
            IntroRootTemplate view = null;
            string tempName = UIHlp.UI模板类型.提示信息模板.ToString();
            string atlasList = LogicModel.GetResName(ResourceType.UINeedAtlas, tempName);
            view = UIResourceManager.LoadUITemplate<IntroRootTemplate>(tempName, atlasList);
            if (null != view)
            {
                view.transform.parent = parent.transform;
                view.transform.localScale = UnityEngine.Vector3.one;
                view.transform.localPosition = UnityEngine.Vector3.zero;
                view.transform.localRotation = SharedGlobalParams.IdentityQuat;
            }
            return view;
        }*/
    }
}
