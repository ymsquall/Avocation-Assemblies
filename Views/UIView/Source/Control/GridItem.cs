using UnityEngine;

namespace UIView.Control
{
    [ExecuteInEditMode]
    public class GridItem : UIWidget
    {
        public delegate void SizeEventHandler();
        public event SizeEventHandler OnSizeChanged;
        [HideInInspector][SerializeField] GridLayoutView mContainer = null;
        [HideInInspector][SerializeField] float mRatioWidth = 0;
        [HideInInspector][SerializeField] float mRatioHeight = 0;
        [HideInInspector][SerializeField] bool mAutoSizeW = false;
        [HideInInspector][SerializeField] bool mAutoSizeH = false;
        [HideInInspector][SerializeField] int mRealWidth = 100;
        [HideInInspector][SerializeField] int mRealHeight = 100;
        public int mRowIndex = 0, mColIndex = 0;
        public GridLayoutView Container { set { mContainer = value; } get { return mContainer; } }
        public float RatioW
        {
            set
            {
                if(mRatioWidth != value)
                {
                    mRatioWidth = value;
                    if (mRatioWidth > 1)
                        mRatioWidth = 1;
#if UNITY_EDITOR
                    mContainer.DoLayoutChanged();
#else
                    mContainer.StartCoroutine(mContainer.DoLayoutChanged());
#endif
                }
            }
            get { return mRatioWidth; }
        }
        public float RatioH
        {
            set
            {
                if (mRatioHeight != value)
                {
                    mRatioHeight = value;
                    if (mRatioHeight > 1)
                        mRatioHeight = 1;
#if UNITY_EDITOR
                    mContainer.DoLayoutChanged();
#else
                    mContainer.StartCoroutine(mContainer.DoLayoutChanged());
#endif
                }
            }
            get { return mRatioHeight; }
        }
        public bool AutoSizeW
        {
            get { return mAutoSizeW; }
        }
        public bool AutoSizeH
        {
            get { return mAutoSizeH; }
        }
        public int RealWidth
        {
            set
            {
                if(mRealWidth != value)
                {
                    mRealWidth = value;
#if UNITY_EDITOR
                    mContainer.DoLayoutChanged();
#else
                    mContainer.StartCoroutine(mContainer.DoLayoutChanged());
#endif
                }
            }
            get { return mRealWidth; }
        }
        public int RealHeight
        {
            set
            {
                if(mRealHeight != value)
                {
                    mRealHeight = value;
#if UNITY_EDITOR
                    mContainer.DoLayoutChanged();
#else
                    mContainer.StartCoroutine(mContainer.DoLayoutChanged());
#endif
                }
            }
            get { return mRealHeight; }
        }
        public void SizeChanged()
        {
            if(null != OnSizeChanged)
                OnSizeChanged();
        }
    }
}