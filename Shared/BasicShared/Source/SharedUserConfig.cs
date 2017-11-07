using Framework.Tools;
using System;
using System.Collections;

namespace Shared.BasicShared
{
    public enum UserConfigType
    {
        motionShowType = 0,
        music_itemMode = 1,
        max
    }
    public class UserConfig
    {
        byte motionShowType = 1;        // 运动数据显示类型 1=当日 2 = 最后一局
        bool music_itemMode = true;     // 休闲模式音乐开关
        IEnumerator EnumValue()
        {
            yield return motionShowType;
            yield return music_itemMode;
        }
        public object this[UserConfigType t]
        {
            set
            {
                switch (t)
                {
                    case UserConfigType.motionShowType:
                        {
                            if (motionShowType != (byte)value)
                            {
                                motionShowType = (byte)value;
                            }
                        }
                        break;
                    case UserConfigType.music_itemMode: music_itemMode = (bool)value; break;
                }
                Write();
            }
            get
            {
                Read();
                switch (t)
                {
                    case UserConfigType.motionShowType: return motionShowType;
                    case UserConfigType.music_itemMode: return music_itemMode;
                }
                return null;
            }
        }
        public void Read()
        {
            string key = SharedUserConfig.Key_UserConfig + SharedUserConfig.Inst[SharedUserConfig.Key_Account];
            if (!UnityEngine.PlayerPrefs.HasKey(key))
                return;
            string config = UnityEngine.PlayerPrefs.GetString(key);
            string[] configStrs = config.Split("#".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            for(int i = 0; i < configStrs.Length; ++ i)
            {
                UserConfigType uct = (UserConfigType)i;
                switch (uct)
                {
                    case UserConfigType.motionShowType: byte.TryParse(configStrs[i], out motionShowType); break;
                    case UserConfigType.music_itemMode: bool.TryParse(configStrs[i], out music_itemMode); break;
                }
            }
        }
        public void Write()
        {
            string str = motionShowType.ToString() + "#" + music_itemMode.ToString();
            UnityEngine.PlayerPrefs.SetString(SharedUserConfig.Key_UserConfig + SharedUserConfig.Inst[SharedUserConfig.Key_Account], str);
            UnityEngine.PlayerPrefs.Save();
        }
    }
    public class SharedUserConfig : AutoSingleT<SharedUserConfig>
    {
        public const string Key_Account = "account";
        public const string Key_Password = "password";
        public const string Key_UserConfig = "user";
		public const string Key_BluetoothUUID = "Key_BluetoothUUID";
        UserConfig mLocalUserConfigure = new UserConfig();
        public object this[object key]
        {
            set
            {
                if (key is UserConfigType)
                {
                    mLocalUserConfigure[(UserConfigType)key] = value;
                    return;
                }
                UnityEngine.PlayerPrefs.SetString(key as string, value.ToString());
                UnityEngine.PlayerPrefs.Save();
            }
            get
            {
                if (key is UserConfigType)
                {
                    object ret = mLocalUserConfigure[(UserConfigType)key];
                    return ret;
                }
                string keyStr = key as string;
                if (!UnityEngine.PlayerPrefs.HasKey(keyStr))
                    return null;
                string str = UnityEngine.PlayerPrefs.GetString(keyStr);
                return str as object;
            }
        }
        public void DeleteConfig(object key)
        {
            if(key is string)
            {
                if (UnityEngine.PlayerPrefs.HasKey(key as string))
                {
                    UnityEngine.PlayerPrefs.DeleteKey(key as string);
                    UnityEngine.PlayerPrefs.Save();
                }
            }
        }
        public void InitSoundConfig()
        {

        }
    }
}
