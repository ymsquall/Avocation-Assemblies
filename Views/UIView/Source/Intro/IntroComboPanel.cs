using UnityEngine;
using System.Collections.Generic;

namespace UIView.Intro
{
    public class IntroComboPanel : IntroBasePanel
    {
        public IntroComboItem 连击 = null;
        public IntroComboItem 连击飞 = null;
        public int 最大连击数 = 99;
        public byte 最大连续击飞数 = 10;
        int mLastCombo = 0;
        byte mLastClobber = 0;
		Dictionary<string, int> mClobberList = new Dictionary<string, int>();
		void Awake()
		{
			if(NGUI2DRootPanel.IsHHDScreen)
			{
				UIAnchor anchor = GetComponent<UIAnchor>();
				if(null != anchor)
				{
					anchor.pixelOffset = new Vector2(-200f, -300f);
					anchor.enabled = true;
				}
			}
		}
        public override void SetParams(params object[] ps)
        {
            if(ps.Length == 1)
            {
                int num = (int)ps[0];
                if (num > 最大连击数) num = 最大连击数;
                if (num > mLastCombo)
                {
                    for (int i = mLastCombo; i <= num; ++i)
                        连击.StartCombo(i, num);
                }
                else
                    连击.StartCombo(num, num);
                mLastCombo = num;
            }
            else if (ps.Length == 2)
            {
                string actID = ps[0] as string;
                int num = (int)ps[1];
                if (num > 最大连续击飞数) num = 最大连续击飞数;
                if (num == 0)
                {
                    if (mClobberList.ContainsKey(actID))
                        mClobberList.Remove(actID);
                    return;
                }
                if (!mClobberList.ContainsKey(actID))
                    mClobberList.Add(actID, num);
                else
                    mClobberList[actID] = num;
                List<KeyValuePair<string, int>> sortList = new List<KeyValuePair<string, int>>(mClobberList);
                sortList.Sort((KeyValuePair<string, int> v1, KeyValuePair<string, int> v2) =>
                {
                    if (v1.Value < v2.Value)
                        return 1;
                    else if (v1.Value > v2.Value)
                        return -1;
                    return 0;
                });
                num = sortList[0].Value;
                if (num > mLastClobber || 连击飞.IsFadeOuted || num >= 最大连续击飞数)
                {
                    连击飞.StartComboFly(num, num);
                    mLastClobber = (byte)num;
                }
            }
		}
		public void RemoveColbber(string id)
		{
			if(mClobberList.ContainsKey(id))
				mClobberList.Remove(id);
		}
		public void ClearClobber()
		{
			mClobberList.Clear ();
		}
    }
}