using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UIView.Control;
using Scripting.LuaSupport.DataTable;

namespace UIView.Intro
{
    public class IntroComboItem : MonoBehaviour
    {
        public UIImageLabel 数字 = null;
        public UISprite 标题 = null;
        //public TweenAlpha 淡出 = null;
        public TweenScale 动画 = null;
        public float 滚动速度 = 5f;
        bool mInAnim = false;
        bool mInFadeIn = false;
        Queue<int> mQueue = new Queue<int>();
        float mFreeTimer = 0;
        void Awake()
		{
            //if (null != 数字)
            //    数字.alpha = 0;
            if (null != 标题)
                标题.alpha = 0;
			UIPanel pan = GetComponentInChildren<UIPanel>();
			if(null != pan)
				pan.alpha = 1;
        }
        public bool IsFadeOuted
        {
            get
            {
                return !mInFadeIn && 标题.alpha <= 0f;
            }
        }
        public void StartCombo(int now, int max)
        {
            //if (null != 淡出)
            //    淡出.duration = ConstLuaTable.Inst.SingleValue_MS(ConstName.MUT_STRICK_DELAY);
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
            if(now > 0)
                StartCoroutine(DoFadeIn(5f));
            else
            {
                StopAllCoroutines();
                mQueue.Clear();
                //if (null != 数字)
                //    数字.alpha = 0;
                if (null != 标题)
                    标题.alpha = 0;
                StartCoroutine(DoFadeIn(2f));
            }
            mQueue.Enqueue(now);
            StartCoroutine(DoStartScroll(now, 1f / 滚动速度));
            mFreeTimer = 0;
        }
        IEnumerator DoStartScroll(int num, float d)
        {
            if (!mInAnim)
            {
                mInAnim = true;
                //淡出.Reset();
				//淡出.enabled = false;
				StopCoroutine("DoFadeOut");
				while (mQueue.Count > 0)
                {
                    float speed = d / mQueue.Count;
                    num = mQueue.Peek();
                    //Debug.Log(num);
                    if (speed > 0.03f)
                    {
                        数字.ScrollText(num.ToString(), speed, false);
                        yield return new WaitForSeconds(speed);
                    }
                    数字.Text = num.ToString();
                    mQueue.Dequeue();
                    mFreeTimer = 0;
                }
                //淡出.PlayForward();
				StartCoroutine("DoFadeOut");
                mInAnim = false;
            }
        }
        public void StartComboFly(int now, int max)
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
            if (now > 1)
            {
                mQueue.Enqueue(now);
                StartCoroutine(DoFadeIn(5f));
                StartCoroutine(DoStartJump());
            }
            else
            {
                StopAllCoroutines();
                mQueue.Clear();
                //if (null != 数字)
                //    数字.alpha = 0;
                if (null != 标题)
                    标题.alpha = 0;
				数字.Text = now.ToString();
				StartCoroutine(DoFadeIn(2f));
				StopCoroutine("DoFadeOut");
				StartCoroutine("DoFadeOut");
			}
			mFreeTimer = 0;
        }
		/*
        public void OnFadeOut()
        {
            //淡出.Reset();
            淡出.enabled = false;
            淡出.PlayForward();
        }
        */
        IEnumerator DoStartJump()
        {
            if(null != 动画)
            {
                if (!mInAnim)
                {
                    mInAnim = true;
                    while (mQueue.Count > 0)
                    {
                        int num = mQueue.Peek();
                        //IntroTextManager.Inst.AddDebugIntro("s:" + num.ToString());
                        //Debug.Log("s:" + num.ToString());
                        //动画.Reset();
                        动画.PlayForward();
                        yield return new WaitForSeconds(动画.duration * 0.5f);
                        if (num >= 10)
                            num = 0;
                        数字.Text = num.ToString();
                        yield return 0;
                        while (动画.enabled)
                            yield return 0;
                        //动画.Reset();
                        mQueue.Dequeue();
                        //IntroTextManager.Inst.AddDebugIntro("e:" + num.ToString());
                        //Debug.Log("e:" + num.ToString());
                        mFreeTimer = 0;
					}
					StopCoroutine("DoFadeOut");
					StartCoroutine("DoFadeOut");
					mInAnim = false;
                }
            }
        }
        IEnumerator DoFadeIn(float speed)
        {
            mInFadeIn = true;
            float alpha = 0;
            //if (null != 数字)
            //    alpha = 数字.alpha;
            if (null != 标题)
                alpha = 标题.alpha;
            while (alpha < 1)
            {
                yield return 0;
                alpha += Time.deltaTime * speed;
                //if (null != 数字)
                //    数字.alpha = alpha;
                if (null != 标题)
                    标题.alpha = alpha;
            }
            //if (null != 数字)
            //    数字.alpha = 1;
            if (null != 标题)
                标题.alpha = 1;
            mInFadeIn = false;
        }
		IEnumerator DoFadeOut()
		{
			float duration = ConstLuaTable.Inst.SingleValue_MS(ConstName.MUT_STRICK_DELAY);
			float timer = duration * 0.5f;
			yield return new WaitForSeconds (timer);
			duration = timer;
			while(timer > 0)
			{
				yield return 0;
				标题.alpha = timer / duration;
				timer -= Time.deltaTime;
			}
			if (null != 数字)
				数字.Text = "";
			if (null != 标题)
				标题.alpha = 0;
		}
		void Update()
		{
			/*
            if (!mInFadeIn)
            {
                //if (null != 数字)
                //    数字.alpha = 淡出.value;
                if (null != 标题)
                    标题.alpha = 淡出.value;
            }
            */
			if (标题.alpha > 0)
            {
                if (mFreeTimer >= 10f)
                {
					StopAllCoroutines();
					StopCoroutine("DoFadeOut");
					StartCoroutine("DoFadeOut");
					mFreeTimer = 0;
                }
                else
                    mFreeTimer += Time.deltaTime;
            }
        }
    }
}