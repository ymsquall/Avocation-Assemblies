using UnityEngine;

namespace UIView.Intro
{
    public class IntroTextItem : MonoBehaviour
    {
        public delegate void EventHandler(IntroTextItem ptr, int height);
        public event EventHandler OnFadeOuted;
        public UILabel 文本 = null;
        public UISprite 底板 = null;
        public TweenAlpha 淡出 = null;
        public float 淡出等待时间 = 0;

        public string Text { set { 文本.text = value; } }
        public int Height { get { return 文本.height; } }
        void Start()
        {
            if(淡出等待时间 > 0)
                淡出.enabled = false;
        }
        void Update()
        {
            底板.height = 文本.height;
            if (淡出等待时间 > 0)
            {
                淡出等待时间 -= Time.deltaTime;
                return;
            }
            if (!淡出.enabled)
                淡出.enabled = true;
            文本.alpha = 淡出.value;
            底板.alpha = 淡出.value;
        }
        public void OnFinisheded()
        {
            if (null != OnFadeOuted)
                OnFadeOuted(this, 文本.height);
            Destroy(gameObject);
        }
    }
}