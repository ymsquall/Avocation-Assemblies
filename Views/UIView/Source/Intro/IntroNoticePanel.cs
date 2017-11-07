using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UIView.Intro
{
    public class IntroNoticePanel : IntroBasePanel
    {
        public UIPanel Panel = null;
        public UIAnchor 左边界 = null;
        public UIAnchor 右边界 = null;
        public UIWidget 背景 = null;
        public UILabel 模板 = null;
        public float 淡入淡出速度 = 0.5f;
        public float 滚动速度 = 10f;
        public float 间隔距离 = 50f;
        UILabel mLastItem = null;
        int mWidth = 0;
        bool mInFadeIn = false;
        float mBGStartX = 0;
        List<UILabel> mTextList = new List<UILabel>();
        void Awake()
        {
            if (null != Panel)
                Panel.alpha = 0;
			if(NGUI2DRootPanel.IsHHDScreen)
			{
				UIAnchor anchor = GetComponent<UIAnchor>();
				if(null != anchor)
				{
					Vector2 offset = anchor.pixelOffset;
					offset.y = -160f;
					anchor.pixelOffset = offset;
					anchor.enabled = true;
				}
			}
        }
        protected override void Start()
        {
            base.Start();
            if (null != Panel)
                Panel.widgetsAreStatic = false;
        }
        void InitAnchor()
        {
            if (null != 左边界 && null != 右边界)
            {
                左边界.gameObject.SetActive(true);
                右边界.gameObject.SetActive(true);
                mWidth = Mathf.RoundToInt(Mathf.Abs(右边界.transform.localPosition.x - 左边界.transform.localPosition.x));
                //Vector3 pos = transform.localPosition;
                float posX = 左边界.transform.localPosition.x + (float)mWidth * 0.5f;
                //transform.localPosition = pos;
                UIAnchor anchor = GetComponent<UIAnchor>();
                if (null != anchor)
                    anchor.pixelOffset.x = posX;
                if (null != Panel)
                {
                    Vector4 cutRange = Panel.baseClipRegion;
                    cutRange.z = mWidth;
                    Panel.baseClipRegion = cutRange;
                }
                左边界.gameObject.SetActive(false);
                右边界.gameObject.SetActive(false);
            }
            if (null != 背景)
                背景.width = mWidth;
            if (null != PanelTrans)
            {
                mBGStartX = (float)mWidth * 0.5f;
                PanelTrans.localPosition = new Vector3(mBGStartX, 0, 0);
            }
        }
        public override void SetParams(params object[] ps)
        {
            if (ps.Length != 1)
                return;
            string t = ps[0] as string;
            GameObject itemObj = NGUITools.AddChild(PanelTrans.gameObject, 模板.gameObject);
            UILabel item = itemObj.GetComponent<UILabel>();
            if (null == item)
            {
                Destroy(itemObj);
                return;
            }
            item.gameObject.SetActive(true);
            item.text = t;
            if (null == mLastItem)
            {
                InitAnchor();
                StopAllCoroutines();
                for (int i = 0; i < mTextList.Count; ++i)
                    Destroy(mTextList[i]);
                mTextList.Clear();
                mLastItem = item;
                mTextList.Add(item);
                StartCoroutine(DoScroll());
            }
            else
            {
                Vector3 pos = PanelTrans.localPosition;
                float posX = (pos.x - mBGStartX) + (mLastItem.transform.localPosition.x + mLastItem.width);
                if (posX < -间隔距离)
                    item.transform.localPosition = new Vector3(-pos.x + mBGStartX, 0, 0);
                else
                    item.transform.localPosition = new Vector3(mLastItem.transform.localPosition.x + mLastItem.width + 间隔距离, 0, 0);
                mLastItem = item;
                mTextList.Add(item);
            }
        }
        IEnumerator DoScroll()
        {
            Vector3 pos = PanelTrans.localPosition;
            if (null != Panel)
            {
                StartCoroutine(DoFadeIn());
                StartCoroutine(DoCheckHideItem());
                float posX = (pos.x - mBGStartX) + mLastItem.transform.localPosition.x + mLastItem.width;
                while (posX > -mWidth || Panel.alpha > 0)
                {
                    yield return 0;
                    if (null != mLastItem)
                    {
                        pos.x -= Time.deltaTime * 滚动速度;
                        PanelTrans.localPosition = pos;
                        posX = (pos.x - mBGStartX) + mLastItem.transform.localPosition.x + mLastItem.width;
                        if (!mInFadeIn && posX <= -mWidth)
                            Panel.alpha -= Time.deltaTime * 淡入淡出速度;
                    }
                    else
                        Panel.alpha -= Time.deltaTime * 淡入淡出速度;
                }
            }
            PanelTrans.localPosition = new Vector3((float)mWidth * 0.5f, 0, 0);
            mLastItem = null;
            背景.gameObject.SetActive(false);
        }
        IEnumerator DoFadeIn()
        {
            mInFadeIn = true;
            Panel.alpha = 0;
            背景.gameObject.SetActive(true);
            while (Panel.alpha < 1)
            {
                yield return 0;
                Panel.alpha += Time.deltaTime * 淡入淡出速度;
            }
            mInFadeIn = false;
        }
        IEnumerator DoCheckHideItem()
        {
            while(mTextList.Count > 0)
            {
                yield return new WaitForSeconds(1f);
                if (mTextList.Count <= 0)
                    break;
                Vector3 pos = PanelTrans.localPosition;
                float posX = (pos.x - mBGStartX) + mTextList[0].transform.localPosition.x + mTextList[0].width;
                if (posX < -mWidth)
                {
                    if (mTextList[0] == mLastItem)
                        mLastItem = null;
                    Destroy(mTextList[0].gameObject);
                    mTextList.RemoveAt(0); 
                }
            }
        }
    }
}