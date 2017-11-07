using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UIView.Intro
{
    public class IntroTextPanel : IntroBasePanel
    {
        public IntroTextItem 模板 = null;
        public float 滚动速度 = 10f;
        float mStartAnchorOffY = 0;
        bool mScrolling = false;
        Queue<IntroTextItem> mQueue = new Queue<IntroTextItem>();
        List<IntroTextItem> mList = new List<IntroTextItem>();
        protected override void Start()
        {
            base.Start();
            if (null != PanelTrans)
                mStartAnchorOffY = PanelTrans.localPosition.y;
        }
        public override void SetParams(params object[] ps)
        {
            if (ps.Length < 1)
                return;
            string t = ps[0] as string;
            GameObject itemObj = NGUITools.AddChild(PanelTrans.gameObject, 模板.gameObject);
            IntroTextItem item = itemObj.GetComponent<IntroTextItem>();
            if (null == item)
            {
                Destroy(itemObj);
                return;
            }
            if (ps.Length > 1)
                item.淡出等待时间 = (float)ps[1];
            item.OnFadeOuted += new IntroTextItem.EventHandler(OnItemOvered);
            item.Text = t;
            if (mQueue.Count <= 0 && mList.Count <= 0)
                item.gameObject.SetActive(true);
            mQueue.Enqueue(item);
            StartCoroutine(DoAddText());
        }
        IEnumerator DoAddText()
        {
            while (mScrolling)
                yield return 0;
            mScrolling = true;
            while (mQueue.Count > 0)
            {
                IntroTextItem item = mQueue.Dequeue();
                if(mList.Count > 0)
                {
                    Vector3 pos = PanelTrans.localPosition;
                    float endPosY = pos.y + item.Height;
                    while (pos.y < endPosY)
                    {
                        yield return 0;
                        pos.y += Time.deltaTime * 滚动速度;
                        PanelTrans.localPosition = pos;
                    }
                    pos.y = endPosY;
                    PanelTrans.localPosition = pos;
                    if (null != item)
                    {
                        pos = item.transform.localPosition;
                        pos.y = -endPosY;
                        item.transform.localPosition = pos;
                        item.gameObject.SetActive(true);
                    }
                }
                mList.Add(item);
            }
            mScrolling = false;
        }
        void OnItemOvered(IntroTextItem ptr, int height)
        {
            if (mList.Contains(ptr))
                mList.Remove(ptr);
            if (mList.Count <= 0 && mQueue.Count <= 0)
            {
                Vector3 pos = PanelTrans.localPosition;
                pos.y = mStartAnchorOffY;
                PanelTrans.localPosition = pos;
            }
        }
    }
}