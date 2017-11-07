using UnityEngine;
using System.Collections.Generic;

namespace UIView.Scrolling
{
    public class ScrollTableContainer : UIWidgetContainer
    {
        public delegate void ReposEventHandler(List<Transform> ch, Bounds b);
        public event ReposEventHandler OnReposed;

        protected bool mReposition = false;
        protected Bounds mBounds;

        public Bounds Bounds
        {
            get { return mBounds; }
        }
        public bool repositionNow
        {
            set
            {
                if (value)
                {
                    mReposition = true;
                    enabled = true;
                }
            }
        }
        protected void OnReposition(List<Transform> ch, Bounds b)
        {
            mBounds = b;
            if (null != OnReposed)
                OnReposed(ch, mBounds);
        }
    }
}