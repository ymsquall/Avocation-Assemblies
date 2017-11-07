using UnityEngine;
using UIView.Control;

namespace UIView.Intro
{
    public class IntroRootTemplate : UIBaseView
    {
        public UIAnchor 左侧锚点 = null;
        public GameObject 调试信息面板 = null;
        public SimpleRichText 调试信息 = null;
        bool mMarkNotOnMainUIRoot = false;
        void Awake()
        {
#if DEBUG_OUTPUT
            调试信息面板.SetActive(true);
#else
            调试信息面板.SetActive(false);
#endif
        }
  //      void Update()
		//{
		//	if(mMarkNotOnMainUIRoot)
		//	{
		//		if (ItemGridsViewPanel.InstCount <= 0)
		//		{
		//			NGUI2DRootPanel root = NGUI2DRootPanel.Inst;
		//			if(null == root || null == root.mUICamera || null == root.mUICamera.GetComponent<Camera>())
		//				return;
		//			Camera cam = root.mUICamera.GetComponent<Camera>();
		//			UIAnchor anchor = GetComponent<UIAnchor>();
		//			if (null != anchor)
		//			{
		//				anchor.uiCamera = cam;
		//				anchor.enabled = true;
		//			}
		//			UIAnchor[] anchors = GetComponentsInChildren<UIAnchor>();
		//			for (int i = 0; i < anchors.Length; ++i)
		//			{
		//				anchors[i].uiCamera = cam;
		//				anchors[i].enabled = true;
		//			}
		//			mMarkNotOnMainUIRoot = false;
		//		}
		//	}
		//	else
		//	{
		//		if (ItemGridsViewPanel.InstCount > 0)
		//		{
		//			ItemGridsViewPanel root = ItemGridsViewPanel.Inst;
		//			if(null == root || null == root.RenderTargetUpperRoot)
		//				return;
		//			Camera cam = root.RenderTargetUpperRoot.GetComponentInChildren<Camera>();
		//			if(null == cam)
		//				return;
		//			UIAnchor anchor = GetComponent<UIAnchor>();
		//			if (null != anchor)
		//			{
		//				anchor.uiCamera = cam;
		//				anchor.enabled = true;
		//			}
		//			UIAnchor[] anchors = GetComponentsInChildren<UIAnchor>();
		//			for (int i = 0; i < anchors.Length; ++i)
		//			{
		//				anchors[i].uiCamera = cam;
		//				anchors[i].enabled = true;
		//			}
		//			mMarkNotOnMainUIRoot = true;
		//		}
		//	}
		//}
        protected override void OnReadyShowing()
        {
            RootPanel.alpha = 1;
		}
		public override void SetWhiteBlockTex (Texture2D tex)
		{
		}
    }
}