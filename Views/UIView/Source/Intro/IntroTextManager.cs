using Framework.Tools;
using System.Collections.Generic;

namespace UIView.Intro
{
    public enum IntroPanelArea
    {
        center,
        left,
        combo,
        notice,
        debug,
        max
    }
    public class IntroTextManager : AutoSingleT<IntroTextManager>
    {
        Dictionary<byte, IntroBasePanel> mIntroPanelList = new Dictionary<byte, IntroBasePanel>();
        //List<TwoData<属性项类型, double>> mLPAttrChangeList = new List<TwoData<属性项类型, double>>();
        public IntroBasePanel this[IntroPanelArea t]
        {
            set
            {
                if (mIntroPanelList.ContainsKey((byte)t) || null == value)
                    mIntroPanelList.Remove((byte)t);
                mIntroPanelList.Add((byte)t, value);
            }
            get
            {
                if (mIntroPanelList.ContainsKey((byte)t))
                    return mIntroPanelList[(byte)t];
                return null;
            }
        }
        public void SetCombo(int cobmo)
        {
            IntroBasePanel panel = this[IntroPanelArea.combo];
            if (null != panel)
                panel.SetParams(cobmo);
        }
        public void SetClobber(string n, int num)
		{
            IntroBasePanel panel = this[IntroPanelArea.combo];
            if (null != panel)
                panel.SetParams(n, num);
		}
		public void ClearClobber()
		{
			IntroBasePanel panel = this[IntroPanelArea.combo];
			if (null != panel && panel is IntroComboPanel)
				(panel as IntroComboPanel).ClearClobber();
		}
		public void RemoveColbber(string id)
		{
			IntroBasePanel panel = this[IntroPanelArea.combo];
			if (null != panel && panel is IntroComboPanel)
				(panel as IntroComboPanel).RemoveColbber(id);
		}
        public void AddIntro(string s, params object[] ps)
		{
            AddIntro(IntroPanelArea.center, s, ps);
        }
        public void AddIntroTimer(string s, float timer, params object[] ps)
        {
            AddIntro(IntroPanelArea.center, s, timer, ps);
        }
        public void AddLeftIntro(string s, params object[] ps)
		{
            AddIntro(IntroPanelArea.left, s, ps);
        }
        public void AddNotice(string s, params object[] ps)
		{
            AddIntro(IntroPanelArea.notice, s, ps);
        }
        public void AddDebugIntro(string s, params object[] ps)
        {
#if DEBUG_OUTPUT
            AddIntro(IntroPanelArea.debug, s, ps);
#endif
        }
        void AddIntro(IntroPanelArea t, string s, params object[] ps)
		{
            IntroBasePanel panel = this[t];
            if(null != panel)
            {
                string text = string.Format(s, ps);
                panel.SetParams(text);
            }
        }
        void AddIntro(IntroPanelArea t, string s, float timer, params object[] ps)
        {
            IntroBasePanel panel = this[t];
            if (null != panel)
            {
                string text = string.Format(s, ps);
                panel.SetParams(text, timer);
            }
        }
   //     public void AddLPAttrChange(属性项类型 t, double v)
   //     {
   //         for (int i = 0; i < mLPAttrChangeList.Count; ++i)
   //         {
   //             if (t == mLPAttrChangeList[i].First)
   //             {
   //                 mLPAttrChangeList[i].Second += v;
   //                 return;
   //             }
   //         }
   //         mLPAttrChangeList.Add(new TwoData<属性项类型, double>(t, v));
   //     }
   //     public void Update(float deltaTime)
   //     {
   //         if(mLPAttrChangeList.Count > 0 && null != InSceneLogicModel.Inst)
   //         {
   //             场景类型 st = InSceneLogicModel.Inst.SceneType;
   //             mLPAttrChangeList.Sort((TwoData<属性项类型, double> v1, TwoData<属性项类型, double> v2) =>
   //             {
   //                 if ((int)v1.First < (int)v2.First)
   //                     return -1;
   //                 else if ((int)v1.First > (int)v2.First)
   //                     return 1;
   //                 return 0;
   //             });
   //             for (int i = 0; i < mLPAttrChangeList.Count; ++ i)
   //             {
   //                 TwoData<属性项类型, double> v = mLPAttrChangeList[i];
   //                 if (v.Second == 0)
   //                     continue;
   //                 PropLuaTable.ValDesc data = PropLuaTable.Inst[v.First];
   //                 if (null == data || !data.IsIntroShow(st))
   //                     continue;
   //                 if (v.Second > 0 && string.IsNullOrEmpty(data.AttrIncText))
   //                     continue;
   //                 if (v.Second < 0 && string.IsNullOrEmpty(data.AttrDecText))
   //                     continue;
   //                 if (v.Second > 0)
   //                 {
   //                     if (data.ShowType == 属性值显示类型.百分比)
   //                         AddIntro(data.AttrIncText, v.Second *= 100);
   //                     else
   //                         AddIntro(data.AttrIncText, (long)v.Second);
   //                 }
   //                 else if (v.Second < 0)
   //                 {
   //                     if (data.ShowType == 属性值显示类型.百分比)
   //                         AddIntro(data.AttrDecText, System.Math.Abs(v.Second *= 100));
   //                     else
   //                         AddIntro(data.AttrDecText, (long)System.Math.Abs(v.Second));
   //                 }
   //             }
   //             mLPAttrChangeList.Clear();
   //         }
			//LocalPlayer.Inst.CheckClobberNum ();
   //     }
    }
}