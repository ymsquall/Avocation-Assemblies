using UnityEngine;

namespace UIView.Scrolling
{
    public class ScrollViewport : UIScrollView
    {
        public bool 重置 = false;
        public UIWidget.Pivot 内容锚点 = UIWidget.Pivot.Center;
        public bool 富文本 = false;
        public bool EnabledResetPos = true;
        //Vector3 mStartPos = Vector3.zero;
        //Vector4 mStartClipRegion = Vector4.zero;
        void OnEnable()
        {
            mTrans.localPosition = Vector3.zero;
            Vector4 clip = mPanel.baseClipRegion;
            clip.x = 0; clip.y = 0;
            mPanel.baseClipRegion = clip;
            mPanel.clipOffset = Vector2.zero;
            if (EnabledResetPos)
                ResetPosition();
        }
        public override Bounds bounds
        {
            get
            {
                if (!mCalculatedBounds)
                {
                    mCalculatedBounds = true;
                    mTrans = transform;
                    mBounds = NGUIMath.CalculateRelativeWidgetBounds(mTrans, mTrans);
                    if (null != mPanel)
                    {
                        Vector3 size = mBounds.size;
                        Vector4 clipRegion = mPanel.baseClipRegion;
                        Vector2 clipSoftness = mPanel.clipSoftness;
                        Vector2 clipSize = new Vector2(clipRegion.z - clipSoftness.x * 2, clipRegion.w - clipSoftness.y * 2);
                        Vector3 center = mBounds.center;
                        if (size.x < clipSize.x)
                        {
                            switch (内容锚点)
                            {
                                case UIWidget.Pivot.Left:
                                case UIWidget.Pivot.TopLeft:
                                case UIWidget.Pivot.BottomLeft:
                                    center.x = (clipRegion.z - size.x) * 0.5f + clipSoftness.x;
                                    break;
                                case UIWidget.Pivot.Center:
                                case UIWidget.Pivot.Top:
                                case UIWidget.Pivot.Bottom:
                                    center.x = (clipRegion.z - size.x) * 0.5f - clipSoftness.x;
                                    break;
                                default:
                                    Debug.LogError("ScrollViewport[" + mTrans.parent.name + "]can only use center with context pivot!");
                                    break;
                            }
                            size.x = clipSize.x;
                        }
                        if (size.y < clipSize.y)
                        {
                            switch (内容锚点)
                            {
                                case UIWidget.Pivot.Top:
                                case UIWidget.Pivot.TopLeft:
                                case UIWidget.Pivot.TopRight:
                                    if (富文本)
                                        center.y = clipSoftness.y;
                                    else
                                        center.y = -clipRegion.w * 0.5f + clipSoftness.y;
                                    break;
                                case UIWidget.Pivot.Center:
                                case UIWidget.Pivot.Left:
                                case UIWidget.Pivot.Right:
                                    center.y = (size.y - clipRegion.w) * 0.5f + clipSoftness.y;
                                    break;
                                default:
                                    Debug.LogError("ScrollViewport[" + mTrans.parent.name + "]can only use center with context pivot!");
                                    break;
                            }
                            size.y = clipSize.y;
                        }
                        mBounds.size = size;
                        mBounds.center = center;
                    }
                }
                return mBounds;
            }
        }
        public float GetMinMaxOffset(Vector3 size, UIWidget.Pivot pivot, ref float boundsMin, ref float boundsMax)
        {
            if (null == panel) return 0;
            boundsMin = boundsMax = 0;
            float space = 0;
            switch (movement)
            {
                case Movement.Horizontal:
                    {
                        Vector4 clipRegion = panel.baseClipRegion;
                        Vector2 clipSoftness = panel.clipSoftness;
                        space = size.x - clipRegion.z;
                        switch (pivot)
                        {
                            case UIWidget.Pivot.Left:
                            case UIWidget.Pivot.TopLeft:
                            case UIWidget.Pivot.BottomLeft:
                                {
                                    if (space < 0)
                                    {
                                        boundsMin = -clipRegion.z * 0.5f + clipSoftness.x;
                                        boundsMax = -boundsMin;
                                    }
                                    else
                                    {
                                        boundsMax = -clipRegion.z * 0.5f + clipSoftness.x;
                                        boundsMin = boundsMax - space - (clipSoftness.x * 2f);
                                    }
                                }
                                break;
                            case UIWidget.Pivot.Center:
                            case UIWidget.Pivot.Top:
                            case UIWidget.Pivot.Bottom:
                                {
                                    if (space < 0)
                                    {
                                        boundsMin = space * 0.5f + clipSoftness.x;
                                        boundsMax = -boundsMin;
                                    }
                                    else
                                    {
                                        boundsMax = space * 0.5f + clipSoftness.x;
                                        boundsMin = -boundsMax;
                                    }
                                }
                                break;
                            default:
                                Debug.LogError("ScrollViewport[" + mTrans.parent.name + "]can only use center with context pivot!");
                                break;
                        }
                    }
                    break;
                case Movement.Vertical:
                    {
                        Vector4 clipRegion = panel.baseClipRegion;
                        Vector2 clipSoftness = panel.clipSoftness;
                        space = size.y - clipRegion.w;
                        switch (pivot)
                        {
                            case UIWidget.Pivot.Top:
                            case UIWidget.Pivot.TopRight:
                            case UIWidget.Pivot.TopLeft:
                                {
                                    if (space < 0)
                                    {
                                        boundsMin = -clipRegion.w * 0.5f + clipSoftness.y;
                                        boundsMax = -boundsMin;
                                    }
                                    else
                                    {
                                        boundsMin = clipRegion.w * 0.5f - clipSoftness.y;
                                        boundsMax = boundsMin + space + (clipSoftness.y * 2f);
                                    }
                                }
                                break;
                            case UIWidget.Pivot.Center:
                            case UIWidget.Pivot.Left:
                            case UIWidget.Pivot.Right:
                                {
                                    if (space < 0)
                                    {
                                        boundsMax = -space * 0.5f - clipSoftness.y;
                                        boundsMin = -boundsMax;
                                    }
                                    else
                                    {
                                        boundsMin = -space * 0.5f - clipSoftness.y;
                                        boundsMax = -boundsMin;
                                    }
                                }
                                break;
                            default:
                                Debug.LogError("ScrollViewport[" + mTrans.parent.name + "]can only use center with context pivot!");
                                break;
                        }
                    }
                    break;
            }
            return space;
        }
        public void ScrollNotChcekBounds(float pos, Vector3 size, UIWidget.Pivot pivot)
        {
            if (null == panel) return;
            float boundsMin = 0, boundsMax = 0;
            float space = GetMinMaxOffset(size, pivot, ref boundsMin, ref boundsMax);
            switch (movement)
            {
                case Movement.Horizontal:
                    {
                        Vector3 oriPos = transform.localPosition;
                        Vector2 clipOffset = panel.clipOffset;
                        Vector2 clipSoftness = panel.clipSoftness;
                        Vector4 clipRange = panel.baseClipRegion;
                        float offPos = oriPos.x + clipOffset.x;
                        oriPos.x = offPos + pos;
                        if (size.x != 0)
                        {
                            switch (pivot)
                            {
                                case UIWidget.Pivot.Left:
                                case UIWidget.Pivot.TopLeft:
                                case UIWidget.Pivot.BottomLeft:
                                    {
                                        boundsMax = -clipRange.z * 0.5f + clipSoftness.x;
                                        boundsMin = boundsMax - space - (clipSoftness.x * 2f);
                                    }
                                    break;
                                case UIWidget.Pivot.Center:
                                case UIWidget.Pivot.Top:
                                case UIWidget.Pivot.Bottom:
                                    {
                                        boundsMax = space * 0.5f + clipSoftness.x;
                                        boundsMin = -boundsMin;
                                    }
                                    break;
                                case UIWidget.Pivot.Right:
                                case UIWidget.Pivot.TopRight:
                                case UIWidget.Pivot.BottomRight:
                                    {
                                        boundsMin = clipRange.z * 0.5f - clipSoftness.x;
                                        boundsMax = boundsMin + space + (clipSoftness.x * 2f);
                                    }
                                    break;
                            }
                            if (oriPos.x < boundsMin)
                                oriPos.x = boundsMin;
                            else if (oriPos.x > boundsMax)
                                oriPos.x = boundsMax;
                        }
                        transform.localPosition = oriPos;
                        clipOffset.x = -(oriPos.x - offPos);
                        clipOffset.y = 0;
                        panel.clipOffset = clipOffset;
                    }
                    break;
                case Movement.Vertical:
                    {
                        Vector3 oriPos = transform.localPosition;
                        Vector2 clipOffset = panel.clipOffset;
                        Vector2 clipSoftness = panel.clipSoftness;
                        Vector4 clipRange = panel.baseClipRegion;
                        float offPos = oriPos.y + clipOffset.y;
                        oriPos.y = offPos - pos;
                        if (size.y != 0)
                        {
                            switch (pivot)
                            {
                                case UIWidget.Pivot.Top:
                                case UIWidget.Pivot.TopRight:
                                case UIWidget.Pivot.TopLeft:
                                    {
                                        boundsMin = clipRange.w * 0.5f - clipSoftness.y;
                                        boundsMax = boundsMin + space + (clipSoftness.y * 2f);
                                    }
                                    break;
                                case UIWidget.Pivot.Center:
                                case UIWidget.Pivot.Left:
                                case UIWidget.Pivot.Right:
                                    {
                                        boundsMin = -space * 0.5f - clipSoftness.y;
                                        boundsMax = -boundsMin;
                                    }
                                    break;
                                case UIWidget.Pivot.Bottom:
                                case UIWidget.Pivot.BottomRight:
                                case UIWidget.Pivot.BottomLeft:
                                    {
                                        boundsMax = -clipRange.w * 0.5f + clipSoftness.y;
                                        boundsMin = boundsMax - space - (clipSoftness.y * 2f);
                                    }
                                    break;
                            }
                            if (oriPos.y < boundsMin)
                                oriPos.y = boundsMin;
                            else if (oriPos.y > boundsMax)
                                oriPos.y = boundsMax;
                        }
                        transform.localPosition = oriPos;
                        clipOffset.x = 0;
                        clipOffset.y = -(oriPos.y - offPos);
                        panel.clipOffset = clipOffset;
                    }
                    break;
            }
            UpdateScrollbars(false);
        }
        public void Scroll(float pos, Vector3 size, UIWidget.Pivot pivot)
        {
            if (null == panel) return;
            float boundsMin = 0, boundsMax = 0;
            float space = GetMinMaxOffset(size, pivot, ref boundsMin, ref boundsMax);
            switch (movement)
            {
                case Movement.Horizontal:
                    {
                        Vector3 oriPos = mTrans.localPosition;
                        Vector2 clipOffset = panel.clipOffset;
                        float offPos = oriPos.x + clipOffset.x;
                        clipOffset.x = pos;
                        clipOffset.y = 0;
                        if (space < 0)
                            clipOffset.x = boundsMax;
                        else
                        {
                            if (clipOffset.x < boundsMin)
                                clipOffset.x = boundsMin;
                            else if (clipOffset.x > boundsMax)
                                clipOffset.x = boundsMax;
                        }
                        oriPos.x = offPos + -clipOffset.x;
                        mTrans.localPosition = oriPos;
                        panel.clipOffset = clipOffset;
                    }
                    break;
                case Movement.Vertical:
                    {
                        Vector3 oriPos = mTrans.localPosition;
                        Vector2 clipOffset = panel.clipOffset;
                        float offPos = oriPos.y + clipOffset.y;
                        clipOffset.x = 0;
                        clipOffset.y = pos;
                        if (space < 0)
                            clipOffset.y = boundsMin;
                        else
                        {
                            if (clipOffset.y < boundsMin)
                                clipOffset.y = boundsMin;
                            else if (clipOffset.y > boundsMax)
                                clipOffset.y = boundsMax;
                        }
                        oriPos.y = offPos + -clipOffset.y;
                        mTrans.localPosition = oriPos;
                        panel.clipOffset = clipOffset;
                    }
                    break;
            }
            UpdateScrollbars(false);
        }
        public void Scroll_TaskList(Vector3 size, float pos, bool instant, float strength)
        {
            if (null == panel) return;
            float space = size.y - panel.baseClipRegion.w;
            float minY = panel.baseClipRegion.w * 0.5f + mPanel.clipSoftness.y;
            float maxY = minY + space + mPanel.clipSoftness.y * 2f;
            if (space <= 0)
                maxY = minY;
            if (!instant && dragEffect == DragEffect.MomentumAndSpring)
            {
                // Spring back into place
                Vector3 endPos = mTrans.localPosition + new Vector3(0, pos, 0);
                endPos.x = Mathf.Round(endPos.x);
                endPos.y = Mathf.Round(endPos.y);
                if (endPos.y <= minY)
                {
                    ResetPosition();
                    return;
                }
                if (endPos.y > maxY)
                    endPos.y = maxY;
                mScroll = 0f;
                SpringPanel sp = SpringPanel.Begin(panel.gameObject, endPos, 13f);
                sp.strength = strength;
                sp.onFinished = () =>
                {
                //RestrictWithinBounds(false, canMoveHorizontally, canMoveVertically);
                DisableSpring();
                    UpdateScrollbars(true);
                    Press(false);
                };
            }
            else
            {
                Vector3 movment = new Vector3(0, pos, 0);
                // Jump back into place
                MoveRelative(movment);
                // Clear the momentum in the constrained direction
                if (Mathf.Abs(movment.x) > 0.01f) mMomentum.x = 0;
                if (Mathf.Abs(movment.y) > 0.01f) mMomentum.y = 0;
                if (Mathf.Abs(movment.z) > 0.01f) mMomentum.z = 0;
                mScroll = 0f;
                UpdateScrollbars(true);
                Press(false);
            }
        }
        public void CheckOutOfBounds(Vector3 size, UIWidget.Pivot pivot)
        {
            if (null == panel) return;
            float boundsMin = 0, boundsMax = 0;
            float space = GetMinMaxOffset(size, pivot, ref boundsMin, ref boundsMax);
            Vector3 oriPos = mTrans.localPosition;
            Vector2 clipOffset = panel.clipOffset;
            switch (movement)
            {
                case Movement.Horizontal:
                    {
                        float offPos = oriPos.x + clipOffset.x;
                        if (space < 0)
                            clipOffset.x = boundsMax;
                        else
                        {
                            if (clipOffset.x < boundsMin)
                                clipOffset.x = boundsMin;
                            else if (clipOffset.x > boundsMax)
                                clipOffset.x = boundsMax;
                        }
                        oriPos.x = offPos + -clipOffset.x;
                        mTrans.localPosition = oriPos;
                        panel.clipOffset = clipOffset;
                    }
                    break;
                case Movement.Vertical:
                    {
                        float offPos = oriPos.y + clipOffset.y;
                        if (space < 0)
                            clipOffset.y = boundsMin;
                        else
                        {
                            if (clipOffset.y < boundsMin)
                                clipOffset.y = boundsMin;
                            else if (clipOffset.y > boundsMax)
                                clipOffset.y = boundsMax;
                        }
                        oriPos.y = offPos + -clipOffset.y;
                        mTrans.localPosition = oriPos;
                        panel.clipOffset = clipOffset;
                    }
                    break;
            }
        }
#if UNITY_EDITOR
        void Update()
        {
            if (重置)
            {
                ResetPosition();
                重置 = false;
            }
        }
#endif
    }
}