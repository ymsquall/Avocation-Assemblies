using UnityEngine;
using System.Collections.Generic;
using Scripting.UniLua;
using Framework.Tools;

namespace Scripting.LuaSupport.DataTable
{
    public enum ConstName
    {
        // 一般常量
        MAX_LEVEL,                  // 等级开放上限
        MAX_BAG_SIZE,               // 背包格子上限
        MAX_STORE_SIZE,             // 仓库格子上限
        DAILY_DROP_COUNT,           // 每日掉落计数
        WORLD_DROP_COUNT,           // 全服掉落计数
        MAX_FRIEND_NUM,             // 好友数量上限
        JOB0_BORN_SCRIPT,	        // 职业0出生脚本
        JOB1_BORN_SCRIPT,	        // 职业1出生脚本
        JOB2_BORN_SCRIPT,	        // 职业2出生脚本
        WRITE_MAIL_LEVEL,           // 写邮件等级
        SKILL_RANKUP_COST,          // 技能进阶消耗，技能经验：铜钱
        MIN_MAIL_ATTACH_LEVEL,      // 邮件附带钱币的最小等级
        MIN_MAIL_ATTACH_VLEVEL,     // 邮件附带钱币的最小VIP等级
        MAX_ACTOR_NUM_PER_LINE,     // 每条线人数上限
        OEXP_OPEN_LEVEL,            // 挂机开启等级
        OEXP_MAX_HOUR,              // 挂机最大有效小时
        // 战斗相关
        DEBUFF_TRIG_CONST,          // 三系debuff触发参数，攻击/防御大于这个值则触发debuff
        STRICK_FLY_V_SPEED,         // 击飞水平速度像素/S
        PIG_1000KG_RECOVER_TIME,    // 猪八戒千金坠落地恢复时间MS
        CONF_PROTECT_TIME,          // 碰撞怪物保护时间MS
        STRICK_FLY_MIN_LOST_TIME,   // 击飞状态最少失控时间MS，过了后可以使用反控技能
        BUFF_3S_DEALY_FIRE,         // 三系buff持续时间MS火
        BUFF_3S_DEALY_WATER,	    // 三系buff持续时间MS水
        BUFF_3S_DEALY_THUND,	    // 三系buff持续时间MS雷
        WEAK_BUFF_DEALY,            // 虚弱buff持续时间(分钟)
        WEAK_BUFF_CLEAR_COST,       // 清除虚弱buff每分钟消耗游戏币
        RELIFE_COST_ITEM_ID,        // 副本复活消耗道具ID
        RELIFE_COST_ITEM_NUM,       // 副本复活消耗道具数量
        RELIFE_COST_GOLD_NUM,       // 副本复活消耗元宝数量
        MUT_STRICK_DELAY,           // 连击最短判断间隔MS
        // 战斗公式相关
        MIN_HIT_RATIO,              // 基础命中率
        MIN_FATAL_RATIO,            // 基础致命率
        MIN_CRIT_RATIO,             // 基础暴击率
        HIT_BASE_PARAM,             // 命中基准参数
        HIT_LEVEL_FIX_PARAM,        // 命中等级修正参数
        FATAL_BASE_PARAM,           // 致命基准参数
        FATAL_LEVEL_FIX_PARAM,      // 致命等级修正参数
        CRIT_BASE_PARAM,            // 暴击基准参数
        CRIT_LEVEL_FIX_PARAM,       // 暴击等级修正参数
        THUND_DEBUFF_PARAM,         // 雷系debuff参数
        ATK_VERSION_FIX_PARAM,      // 普通攻击版本修正参数
        ATK_DEC_FIX_PARAM,          // 普通攻击减法修正参数
        ELEM_VERSION_FIX_PARAM,     // 属性攻击版本修正参数
        ELEM_FIX_PARAM,             // 属性攻击修正参数
        BASE_CRIT_PARAM,            // 基本暴击强度参数
        FLYTIMES_2_DAMAGE_RATIO,    // 击飞伤害比对照表
        // 怪物相关
        ENEMY_WATCH_RADIO,          // 巡逻半径px
        ENEMY_LEVEL_EXP_MAP_G,      // 怪物等级高于自己，获得经验的百分比
        ENEMY_LEVEL_EXP_MAP_L,      // 怪物等级低于自己，获得经验的百分比
        ENEMY_SHOTER_DISTANCE,      // 远程怪发动攻击距离
        BOSS_CRAZY_BUFFID,          // boss狂暴buffID
        // 副本相关	
        INSTANCE_NORMAL_DROP_RATIO, // 常规副本掉率控制，随着刷福本次数的提升，副本掉率降低系数
        INSTANCE_ELITE_DROP_RATIO,  // 精英副本掉率控制，随着刷福本次数的提升，副本掉率降低系数
        INSTANCE_TRAIN_DROP_RATIO,  // 试练副本掉率控制，随着刷福本次数的提升，副本掉率降低系数
        INSTANCE_2PLAYER_COIN_PCT,  // 2人加入副本经济类加成
        INSTANCE_3PLAYER_COIN_PCT,  // 3人加入副本经济类加成
        BORN_INSTANCE_ID,           // 各职业出生副本ID
        TRAIN_INSTANCE_RESET_DAYS,  // 试练副本重置周期天数
        ELITE_INSTANCE_RESET_DAYS,  // 精英副本重置周期天数
        // 物品相关
        GAMBLE_DROPRULE_ID,         // 赌博商店贩卖物品的掉落ID
        GAMBLE_TIMES_PRICE,         // 赌博商店购买次数价格增幅表，从第2次购买开始计算
        GAMBLE_OPEN_LEVEL,          // 赌博商店开放等级
        GAMBLE_MAX_BUY_TIMES,       // 赌博商店每日购买次数上限
        FIRST_RECHARGE_GIFT_ID,     // 首冲礼包ID
        AUTO_ITEM_ID,               // 托管消耗道具ID
        GAMBLE_DROP_ID,             // 赌博商店额外掉落ID
        EQUIP_EFFECT_LV,            // 客户端专用:装备图标特效出现的强化等级序列
        EQUIP_EFFECT_ID,            // 客户端专用:装备图标特效的ID序列
        // 竞技场相关
        START_TIME,                 // 竞技场开启时间
        END_TIME,                   // 竞技场结束时间
        JOIN_STOP_TIME,             // 报名结束时间，一般在结束前几分钟
        MAX_ARENA_DELAY,            // 单场最大持续时间(分)
        MAX_ARENA_ACTOR_NUM,        // 比赛人数上限
        MIN_ARENA_ACTOR_NUM,        // 最小比赛人数
        MIN_ARENA_WINNER,           // 胜利判断存活人数
        JOIN_DELAY,                 // 参与经济场CD(分)
        ARENA_MAPS,                 // 竞技场地图序列
        ARENA_JOIN_LEVEL,           // 参加竞技场等级
        DEAD_HOUNOR,                // 失败给与的荣誉
        ARENA_START_WAIT_TIME,      // 战斗开始等待时间秒
        ARENA_START_SCRIPT,
        //活动副本相关
        FOREVER_FIGHT_INSTANCE_ID,  // 无尽挑战副本ID
        FOREVER_FIGHT_MAP_ID,       // 无尽挑战副本地图序列
        GOLD_GOD_INSTANCE_ID,       // 喜从天降副本ID
        GOLD_GOD_MAP_ID,            // 喜从天降副本地图序列
        GG_ACT_MIN_KILL,            // 财神活动最小参与活动存活时间秒
        FF_ACT_MIN_KILL,            // 无尽挑战活动最小参与活动杀怪数
        GG_ACT_START_WAIT_TIME,     // 开始降落箱子等待时间秒
        //冲值相关
        FIRST_RMB_2_GOLD_RATIO,     // 首冲获得元宝百分比
        RMB_2_GOLD_RATIO,           // 1人民币兑换X元宝
        RECHARGE_STEP_RATIO,        // 单比冲值人民币数额阶段所对应的元宝比例
        RECHARGE_STEP_ICON,         // 单比冲值人民币数额阶段所对应图标
        //擂台相关
        RING_MIN_LEVEL,             // 打擂最小等级
        RING_MIN_GOLD,              // 最小礼金（元宝）
        RING_MAX_GOLD,              // 最大礼金（元宝）
        RING_MIN_SILVER,            // 最小礼金（铜钱）
        RING_MAX_SILVER,            // 最大礼金（铜钱）
        RING_FV_DIS,                // 回应挑战的战斗力差距
        RING_SYSTEM_COST,           // 系统分成
        max
    }
    public class ConstLuaTable : LuaTableReader<ConstLuaTable, ConstLuaTable.KeyDesc, ConstLuaTable.ValDesc>
    {
        public const LuaTableType TableType = LuaTableType.Const;
        //List<EquipIntensifyEffectDesc> mEquipIntensifyEffectDestList = new List<EquipIntensifyEffectDesc>();
        public static ConstLuaTable Inst
        {
            get { return LuaDataTableProxy.Inst[TableType] as ConstLuaTable; }
        }
        //class EquipIntensifyEffectDesc
        //{
        //    public byte minLv = 0;
        //    public byte maxLv = 0;
        //    public UIHlp.UI图包类型 atlas = UIHlp.UI图包类型.装备边框特效;
        //    public string spName = "";
        //    public Vector3 pos = Vector3.zero;
        //    public Vector3 scale = Vector3.one;
        //    public Vector3 rota = Vector3.zero;
        //}
        public class KeyDesc : LuaTableKeyValue
        {
            public override int Count { get { return 1; } }
            static KeyDesc CackeKey = new KeyDesc();
            public override bool Init(StkId[] ps)
            {
                string name = LuaDataReadHelper.ReadUtf8String(ps[0].V);
                if (!System.Enum.IsDefined(typeof(ConstName), name))
                {
                    Debug.LogError("[Const表读取失败:]ConstName中不包含名为:" + name + "的定义！");
                    return false;
                }
                type = (ConstName)System.Enum.Parse(typeof(ConstName), name);
                GenHashCode();
                return true;
            }
            void GenHashCode()
            {
                mHashCode = (int)type;
            }
            internal static KeyDesc GenKey(ConstName type)
            {
                CackeKey.type = type;
                CackeKey.GenHashCode();
                return CackeKey;
            }
            internal ConstName type;
        }
        public class ValDesc : LuaTableKeyValue
        {
            public override int Count { get { return 1; } }
            public override bool Init(StkId[] ps, ILuaTableElement oth)
            {
                KeyDesc k = oth as KeyDesc;
                if (k.type == ConstName.RELIFE_COST_ITEM_NUM ||
                    k.type == ConstName.RELIFE_COST_GOLD_NUM ||
                    k.type == ConstName.FIRST_RECHARGE_GIFT_ID ||
                    k.type == ConstName.WEAK_BUFF_CLEAR_COST ||
                    k.type == ConstName.RECHARGE_STEP_ICON ||
                    k.type == ConstName.EQUIP_EFFECT_LV ||
                    k.type == ConstName.EQUIP_EFFECT_ID)
                {
                    values = LuaDataReadHelper.ReadStringArray(ps[0].V);
                }
                else if (k.type == ConstName.RECHARGE_STEP_RATIO ||
                    k.type == ConstName.SKILL_RANKUP_COST)
                {
                    string[] strs = LuaDataReadHelper.ReadStringArray(ps[0].V);
                    List<TwoData<string, string>> v2s = new List<TwoData<string,string>>();
                    for(int i = 0; i < strs.Length; ++ i)
                    {
                        if(string.IsNullOrEmpty(strs[i]))
                            continue;
                        string[] ss = strs[i].Split(":".ToCharArray());
                        if (ss.Length != 2)
                            continue;
                        v2s.Add(new TwoData<string, string>(ss[0], ss[1]));
                    }
                    value2s = v2s.ToArray();
                }
                else
                    value = LuaDataReadHelper.ReadUtf8String(ps[0].V);
                return true;
            }
            internal string value = "";
            internal string[] values = null;
            internal TwoData<string, string>[] value2s = null;
        }
        public ValDesc this[ConstName type]
        {
            get
            {
                KeyDesc k = KeyDesc.GenKey(type);
                if (!mTableDatas.ContainsKey(k))
                    return null;
                ValDesc val = mTableDatas[k];
                return val;
            }
        }
        public float SingleValue(ConstName type)
        {
            float ret = 0.0f;
            ValDesc data = this[type];
            if (float.TryParse(data.value, out ret))
                return ret;
            return 0.0f;
        }
        public float SingleValue_MS(ConstName type)
        {
            float ret = 0.0f;
            ValDesc data = this[type];
            if (float.TryParse(data.value, out ret))
                return ret * 0.001f;
            return 0.0f;
        }
        public int IntValue(ConstName type)
        {
            int ret = -1;
            ValDesc data = this[type];
            if (int.TryParse(data.value, out ret))
                return ret;
            return -1;
        }
        public int[] IntArrayValue(ConstName type)
        {
            ValDesc data = this[type];
            List<int> ret = new List<int>();
            for (int i = 0; i < data.values.Length; ++ i)
            {
                int r = 0;
                if (!int.TryParse(data.values[i], out r))
                    return null;
                ret.Add(r);
            }
            if (ret.Count > 0)
                return ret.ToArray();
            return ret.ToArray();
        }
        public bool ArrayValue2s<T1, T2>(ConstName type, out TwoData<T1, T2>[] rets)
        //    where T1 : System.IConvertible
        //    where T2 : System.IConvertible
        {
            ValDesc data = this[type];
            rets = null;
            List<TwoData<T1, T2>> retList = new List<TwoData<T1, T2>>();
            for (int i = 0; i < data.value2s.Length; ++ i)
            {
                T1 r1 = default(T1);
                T2 r2 = default(T2);
                if (!StringUtils.ToValueT(data.value2s[i].First, out r1))
                    return false;
                if (!StringUtils.ToValueT(data.value2s[i].Second, out r2))
                    return false;
                retList.Add(new TwoData<T1, T2>(r1, r2));
            }
            rets = retList.ToArray();
            return true;
        }
        public bool GetDateValue(ConstName type, ref int hor, ref int min,ref int sec)
        {
            ValDesc data = this[type];
            string[] ss = data.value.Split(":".ToCharArray());
            if(ss.Length >= 2)
            {
                int index = 0;
                if(ss.Length == 3)
                {
                    if(!int.TryParse(ss[index++], out hor))
                        return false;
                }
                if (!int.TryParse(ss[index++], out min))
                    return false;
                if (!int.TryParse(ss[index++], out sec))
                    return false;
                return true;
            }
            return false;
        }
        //protected override void OnTableReadOvered()
        //{
        //    int[] lvs = IntArrayValue(ConstName.EQUIP_EFFECT_LV);
        //    if (null == lvs || lvs.Length <= 0)
        //        return;
        //    ValDesc data = this[ConstName.EQUIP_EFFECT_ID];
        //    if (null == data)
        //        return;
        //    string[] images = data.values;
        //    if (null == images || images.Length != lvs.Length)
        //        return;
        //    for (int i = 0; i < lvs.Length; ++i)
        //    {
        //        string[] rets = images[i].Split("/".ToCharArray());
        //        if (rets.Length < 2 || string.IsNullOrEmpty(rets[1]))
        //            continue;
        //        if (!System.Enum.IsDefined(typeof(UIHlp.UI图包类型), rets[0]))
        //            continue;
        //        EquipIntensifyEffectDesc last = new EquipIntensifyEffectDesc();
        //        last.minLv = (byte)lvs[i];
        //        last.atlas = (UIHlp.UI图包类型)System.Enum.Parse(typeof(UIHlp.UI图包类型), rets[0]);
        //        last.spName = rets[1];
        //        if (rets.Length > 2)
        //            last.pos = StringUtils.V3Str2U3DVector3(rets[2]);
        //        if (rets.Length > 3)
        //            last.scale = StringUtils.V3Str2U3DVector3(rets[3]);
        //        if (rets.Length > 4)
        //            last.rota = StringUtils.V3Str2U3DVector3(rets[4]);
        //        if ((i + 1) < lvs.Length)
        //            last.maxLv = (byte)(lvs[i + 1] - 1);
        //        mEquipIntensifyEffectDestList.Add(last);
        //    }
        //}
        //public string GetEquipIntensifyLVEffect(int lv, ref UIHlp.UI图包类型 t, ref Vector3 pos, ref Vector3 scale, ref Vector3 rota)
        //{
        //    string ret = "";
        //    for(int i = 0; i < mEquipIntensifyEffectDestList.Count; ++ i)
        //    {
        //        EquipIntensifyEffectDesc last = mEquipIntensifyEffectDestList[i];
        //        if (lv >= last.minLv && (lv <= last.maxLv || last.maxLv == 0))
        //        {
        //            t = last.atlas;
        //            ret = last.spName;
        //            pos = last.pos;
        //            scale = last.scale;
        //            rota = last.rota;
        //            break;
        //        }
        //    }
        //    return ret;
        //}
    }
}
