using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scripting.UniLua;

namespace Scripting.LuaSupport
{
    public enum LuaTableType
    {
         Role           = 0
        ,Enemy          = 1
        ,Model          = 2
        ,Skill          = 3
        ,Aoe            = 4
        ,Npc            = 5
        ,Animation      = 6
        ,Prop           = 7
        ,SkillDamage    = 8
        ,HideWeapon     = 9
        ,HideWeaponRes  = 10
        ,Const          = 11
        ,Chapter        = 12
        ,Instance       = 13
        ,Item           = 14
        ,Event          = 15
        ,Prefix         = 16
        ,Task           = 17
        ,Buff           = 18
        ,Equip          = 19
        ,InlayItem      = 20
        ,SuitItem       = 21
        ,SkillAdd       = 22
        ,EffectEvent    = 23
        ,EquipUpgrade   = 24
        ,Shop           = 25
        ,VIP            = 26
        ,DropRule       = 27
        ,DropList       = 28
        ,FloatingText   = 29
        ,Vitality       = 30
        ,VitalityGift   = 31
        ,D2DLoginGift   = 32
        ,Sound          = 33
        ,Music          = 34
        ,LoadingTips    = 35
        ,Emoticon       = 36
        ,Title          = 37
        ,UnlockLvFunc   = 38
        ,Notice         = 39
        ,SkillShake     = 40
        ,Guide          = 41
        ,PlayerName     = 42
        ,Hero
        ,RoleExp
        ,HeroExp
        ,RandName
        ,Scene
        ,max
    }
    public interface ILuaTableReader
    {
        bool ReadTable(LuaTable tbl);
        void OnDestroy();
    }
    public class LuaDataTableProxy : LuaScriptDelegate
    {
        static LuaDataTableProxy mInst = null;
        internal static LuaDataTableLoader Loader = null;
        public static bool Loaded = false;
        public static string LUA数据表脚本路径 = "DataTable";
        public static string LUA数据表脚本入口文件 = "main_csvtbl.lua";

        internal static Dictionary<byte, ILuaTableReader> mDataReader = null;
        static Dictionary<byte, ILuaTableReader> NewDataReader
        {
            get 
            {
                return new Dictionary<byte, ILuaTableReader>()
                        {
                            //{ (byte)LuaTableType.Role, new RoleLuaTable() },
                            //{ (byte)LuaTableType.Enemy, new EnemyLuaTable() },
                            //{ (byte)LuaTableType.Model, new ModelLuaTable() },
                            //{ (byte)LuaTableType.Scene, new SceneLuaTable() },
                            //{ (byte)LuaTableType.Npc, new NpcLuaTable() },
                            //{ (byte)LuaTableType.Animation, new AnimationLuaTable() },
                            //{ (byte)LuaTableType.Prop, new PropLuaTable() },
                            //{ (byte)LuaTableType.SkillDamage, new SkillDamageLuaTable() },
                            //{ (byte)LuaTableType.HideWeapon, new HideWeaponLuaTable() },
                            //{ (byte)LuaTableType.HideWeaponRes, new HideWeaponResLuaTable() },
                            //{ (byte)LuaTableType.Const, new ConstLuaTable() },
                            //{ (byte)LuaTableType.Chapter, new ChapterLuaTable() },
                            //{ (byte)LuaTableType.Instance, new InstanceLuaTable() },
                            //{ (byte)LuaTableType.Item, new ItemLuaTable() },
                            //{ (byte)LuaTableType.Event, new EventLuaTable() },
                            //{ (byte)LuaTableType.Prefix, new PrefixLuaTable() },
                            //{ (byte)LuaTableType.Task, new TaskLuaTable() },
                            //{ (byte)LuaTableType.Equip, new EquipLuaTable() },
                            //{ (byte)LuaTableType.InlayItem, new InlayItemLuaTable() },
                            //{ (byte)LuaTableType.SuitItem, new SuitItemLuaTable() },
                            //{ (byte)LuaTableType.SkillAdd, new SkillAddLuaTable() },
                            //{ (byte)LuaTableType.EffectEvent, new EffectEventLuaTable() },
                            //{ (byte)LuaTableType.EquipUpgrade, new EquipUpgradeLuaTable() },
                            //{ (byte)LuaTableType.Shop, new ShopLuaTable() },
                            //{ (byte)LuaTableType.VIP, new VIPLuaTable() },
                            //{ (byte)LuaTableType.DropRule, new DropRuleLuaTable() },
                            //{ (byte)LuaTableType.DropList, new DropListLuaTable() },
                            //{ (byte)LuaTableType.FloatingText, new FloatingTextLuaTable() },
                            //{ (byte)LuaTableType.Vitality, new VitalityLuaTable() },
                            //{ (byte)LuaTableType.VitalityGift, new VitalityGiftLuaTable() },
                            //{ (byte)LuaTableType.D2DLoginGift, new D2DLoginGiftLuaTable() },
                            //{ (byte)LuaTableType.Sound, new SoundLuaTable() },
                            //{ (byte)LuaTableType.Music, new MusicLuaTable() },
                            //{ (byte)LuaTableType.LoadingTips, new LoadingTipsLuaTable() },
                            //{ (byte)LuaTableType.Emoticon, new EmoticonLuaTable() },
                            //{ (byte)LuaTableType.Title, new TitleLuaTable() },
                            //{ (byte)LuaTableType.Notice, new NoticeLuaTable() },
                            //{ (byte)LuaTableType.SkillShake, new SkillShakeLuaTable() },
                            //{ (byte)LuaTableType.Guide, new GuideLuaTable() },
                            //{ (byte)LuaTableType.PlayerName, new PlayerNameLuaTable() },
                };
            }
        }
        public static void Begining()
        {
            if (mInst == null)
            {
                mDataReader = NewDataReader;
                mInst = new LuaDataTableProxy();
                mInst.Init();
            }
        }
        public static LuaDataTableProxy Inst
        {
            get
            {
                if (mInst == null)
                {
                    mDataReader = NewDataReader;
                    mInst = new LuaDataTableProxy();
                    mInst.Init();
                }
                return mInst;
            }
        }
        public static void Destroy()
        {
            if (null != mInst)
            {
                foreach (ILuaTableReader tbl in mDataReader.Values)
                    tbl.OnDestroy();
                mDataReader.Clear();
                mDataReader = null;
                mInst = null;
            }
        }
        public ILuaTableReader this[LuaTableType t]
        {
            get
            {
                if (mDataReader.ContainsKey((byte)t))
                    return mDataReader[(byte)t];
                return null;
            }
        }
        protected override string LUAEntryFileName
        {
            get
            {
//#if UNITY_EDITOR
                return string.Format("{0}/{1}", LUA数据表脚本路径,
                                                LUA数据表脚本入口文件);
//#else
//                return LUA数据表脚本入口文件;
//#endif
            }
        }
        protected override string[] MethodNameList()
        {
            List<string> ret = new List<string>(0);
            return ret.ToArray();
        }
        void Init()
        {
            base.InitLuaLib();
            if (!Application.isPlaying)
            {
                // load all data table
                foreach (byte t in mDataReader.Keys)
                {
                    string tn = "__" + ((LuaTableType)t).ToString() + "Table";
                    mLuaState.GetGlobal(tn);
                    int top = mLuaState.GetTop();
                    if (mLuaState.IsTable(top))
                    {
                        if (!mDataReader[t].ReadTable(mLuaState.ToObject(top) as LuaTable))
                        {
                            //PopupDialogView.Popup(弹框类型.错误提示, "读取LUA数据表" + tn + "失败");
                            Debug.LogWarning("读取LUA数据表" + t + "失败");
                        }
                        mLuaState.Pop(1);
                    }
                    //LogicModel.OnStartupProg();
                }
                Loader = null;
                Loaded = true;
            }
            else if (null == Loader)
            {
                GameObject loadObj = new GameObject(typeof(LuaDataTableLoader).ToString());
                Loader = loadObj.AddComponent<LuaDataTableLoader>();
            }
        }
    }

    class LuaDataTableLoader : MonoBehaviour
    {
        IEnumerator Start()
        {
            // load all data table
            foreach (byte t in LuaDataTableProxy.mDataReader.Keys)
            {
                string tn = "__" + ((LuaTableType)t).ToString() + "Table";
                LuaDataTableProxy.State.GetGlobal(tn);
                int top = LuaDataTableProxy.State.GetTop();
                if (LuaDataTableProxy.State.IsTable(top))
                {
                    if (!LuaDataTableProxy.mDataReader[t].ReadTable(LuaDataTableProxy.State.ToObject(top) as LuaTable))
                    {
                        Debug.LogError("读取LUA数据表" + tn + "失败");
                        //PopupDialogView.Popup(弹框类型.错误提示, "读取LUA数据表" + tn + "失败");
                    }
                    LuaDataTableProxy.State.Pop(1);
                }
                yield return 0;
                //LogicModel.OnStartupProg();
            }
            Destroy(gameObject);
            LuaDataTableProxy.Loader = null;
            LuaDataTableProxy.Loaded = true;
        }
    }
}
