using UnityEngine;

namespace UIView.Intro
{
    public class IntroBasePanel : MonoBehaviour
    {
        public IntroPanelArea Area = IntroPanelArea.max;
        public Transform PanelTrans = null;
        protected virtual void Start()
        {
            UIAnchor anchor = GetComponent<UIAnchor>();
            if (null != anchor)
                anchor.uiCamera = NGUI2DRootPanel.Inst.mUICamera;
            UIPanel panel = GetComponent<UIPanel>();
            if (null != panel)
                panel.alpha = 1;
            IntroTextManager.Inst[Area] = this;
        }
        void OnDestroy()
        {
            IntroTextManager.Inst[Area] = null;
        }
        public virtual void SetParams(params object[] ps) { }
    }
}