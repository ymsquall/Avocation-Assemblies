using UnityEngine;

namespace UIView.Control
{
    public class GridItemSizeListener : MonoBehaviour
    {
        public GridItem 格子 = null;
        public UIWidget 监听者1 = null;
		public SimpleRichText 监听者2 = null;
		public UIPanel 监听者3 = null;
        public GameObject 响应区 = null;
        public Vector2 附加尺寸 = Vector2.zero;
        public bool 监听宽度 = false;
        public bool 监听高度 = false;
        void Start()
        {
            格子.OnSizeChanged += new GridItem.SizeEventHandler(OnGridSizeChanged);
            OnGridSizeChanged();
        }

        void OnGridSizeChanged()
        {
            if (null != 监听者1)
            {
                if (监听宽度)
                    监听者1.width = (int)((float)格子.width + 附加尺寸.x);
                if (监听高度)
                    监听者1.height = (int)((float)格子.height + 附加尺寸.y);
            }
            if (null != 监听者2)
            {
                if (监听宽度)
                    监听者2.视图尺寸.x = (int)((float)格子.width + 附加尺寸.x);
                if (监听高度)
                    监听者2.视图尺寸.y = (int)((float)格子.height + 附加尺寸.y);
				//监听者2.transform.localPosition = new Vector3(0, (float)格子.height * 0.5f - Mathf.Abs(附加尺寸.y * 0.5f));
                监听者2.重新计算边界 = true;
                监听者2.重新解析 = true;
			}
			if (null != 监听者3)
			{
				Vector4 clipRange = 监听者3.baseClipRegion;
				if (监听宽度)
					clipRange.z = (int)((float)格子.width + 附加尺寸.x);
				if (监听高度)
					clipRange.w = (int)((float)格子.height + 附加尺寸.y);
				监听者3.baseClipRegion = clipRange;
			}
            if(null != 响应区)
                NGUITools.UpdateWidgetCollider(响应区);
        }
    }
}