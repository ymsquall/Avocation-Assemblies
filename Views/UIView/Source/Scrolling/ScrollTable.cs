using UnityEngine;
using System.Collections.Generic;

namespace UIView.Scrolling
{
    using Direction = UITable.Direction;
    using Sorting = UITable.Sorting;
    public class ScrollTable : ScrollTableContainer
    {
        public bool 重置 = false;
        public int 列数 = 0;     // 0代表无限制
        public Direction 添加方向 = Direction.Down;
        public Sorting 排序方式 = Sorting.None;
        public UIWidget.Pivot 容器锚点 = UIWidget.Pivot.TopLeft;
        public UIWidget.Pivot 元素锚点 = UIWidget.Pivot.TopLeft;
        public Vector2 元素间距 = Vector2.zero;
        /// <summary>
        /// Whether inactive children will be discarded from the table's calculations.
        /// </summary>
        public bool hideInactive = true;
        /// <summary>
        /// Whether the parent container will be notified of the table's changes.
        /// </summary>
        public bool keepWithinPanel = false;
        /// <summary>
        /// Custom sort delegate, used when the sorting method is set to 'custom'.
        /// </summary>
        public System.Comparison<Transform> onCustomSort;

        protected UIPanel mPanel;
        protected bool mInitDone = false;

        public bool Inited { get { return mInitDone; } }
        public UIPanel Panel { get { return mPanel; } }
        /// <summary>
        /// Get the current list of the grid's children.
        /// </summary>
        public Transform LastChild
        {
            get
            {
                Transform myTrans = transform;
                int count = myTrans.childCount;
                if (count > 0)
                    return myTrans.GetChild(count - 1);
                return null;
            }
        }
        public List<Transform> GetChildList()
        {
            Transform myTrans = transform;
            List<Transform> list = new List<Transform>();
            for (int i = 0; i < myTrans.childCount; ++i)
            {
                Transform t = myTrans.GetChild(i);
                if (!hideInactive || (t && NGUITools.GetActive(t.gameObject)))
                    list.Add(t);
            }
            // Sort the list using the desired sorting logic
            if (排序方式 != Sorting.None)
            {
                if (排序方式 == Sorting.Alphabetic) list.Sort(UIGrid.SortByName);
                else if (排序方式 == Sorting.Horizontal) list.Sort(UIGrid.SortHorizontal);
                else if (排序方式 == Sorting.Vertical) list.Sort(UIGrid.SortVertical);
                else if (onCustomSort != null) list.Sort(onCustomSort);
                else Sort(list);
            }
            return list;
        }
        public void Clear()
        {
            List<Transform> itemList = GetChildList();
            for (int i = 0; i < itemList.Count; ++i)
                Destroy(itemList[i].gameObject);
            Reposition();
        }
        /// <summary>
        /// Want your own custom sorting logic? Override this function.
        /// </summary>
        protected virtual void Sort(List<Transform> list) { list.Sort(UIGrid.SortByName); }
        /// <summary>
        /// Position the grid's contents when the script starts.
        /// </summary>
        protected virtual void Start()
        {
#if UNITY_EDITOR
            UIBaseView bv = GetComponentInParent<UIBaseView>();
            Transform pparent = transform.parent.parent;
            if (keepWithinPanel)
            {
                Debug.LogError("ScrollTable[" + bv.name + "/" + pparent.name + "]sure enable panel moving?");
            }
#endif
            Init();
            Reposition();
            enabled = false;
        }
        /// <summary>
        /// Find the necessary components.
        /// </summary>
        protected virtual void Init()
        {
            mInitDone = true;
            mPanel = NGUITools.FindInParents<UIPanel>(gameObject);
        }
        /// <summary>
        /// Is it time to reposition? Do so now.
        /// </summary>
        protected virtual void LateUpdate()
        {
#if UNITY_EDITOR
            if (重置)
            {
                enabled = true;
                mReposition = true;
                重置 = false;
            }
#endif
            if (mReposition) Reposition();
            enabled = false;
        }
        /// <summary>
        /// Reposition the content on inspector validation.
        /// </summary>
        void OnValidate() { if (!Application.isPlaying && NGUITools.GetActive(this)) Reposition(); }
        /// <summary>
        /// Positions the grid items, taking their own size into consideration.
        /// </summary>
        protected Bounds RepositionVariableSize(List<Transform> children)
        {
            Bounds selfBounds = new Bounds();
            float xOffset = 0;
            float yOffset = 0;

            int cols = 列数 > 0 ? children.Count / 列数 + 1 : 1;
            int rows = 列数 > 0 ? 列数 : children.Count;

            Bounds[,] bounds = new Bounds[cols, rows];
            Bounds[] boundsRows = new Bounds[rows];
            Bounds[] boundsCols = new Bounds[cols];

            int x = 0;
            int y = 0;

            for (int i = 0, imax = children.Count; i < imax; ++i)
            {
                Transform t = children[i];
                Bounds b = NGUIMath.CalculateRelativeWidgetBounds(t, !hideInactive);

                Vector3 scale = t.localScale;
                b.min = Vector3.Scale(b.min, scale);
                b.max = Vector3.Scale(b.max, scale);
                bounds[y, x] = b;

                boundsRows[x].Encapsulate(b);
                boundsCols[y].Encapsulate(b);

                if (++x >= 列数 && 列数 > 0)
                {
                    x = 0;
                    ++y;
                }
            }

            x = 0;
            y = 0;

            Vector2 po = NGUIMath.GetPivotOffset(元素锚点);

            for (int i = 0, imax = children.Count; i < imax; ++i)
            {
                Transform t = children[i];
                Bounds b = bounds[y, x];
                Bounds br = boundsRows[x];
                Bounds bc = boundsCols[y];

                Vector3 pos = t.localPosition;
                pos.x = xOffset + b.extents.x - b.center.x;
                pos.x -= Mathf.Lerp(0f, b.max.x - b.min.x - br.max.x + br.min.x, po.x) - 元素间距.x;

                if (添加方向 == Direction.Down)
                {
                    pos.y = -yOffset - b.extents.y - b.center.y;
                    pos.y += Mathf.Lerp(b.max.y - b.min.y - bc.max.y + bc.min.y, 0f, po.y) - 元素间距.y;
                }
                else
                {
                    pos.y = yOffset + b.extents.y - b.center.y;
                    pos.y -= Mathf.Lerp(0f, b.max.y - b.min.y - bc.max.y + bc.min.y, po.y) - 元素间距.y;
                }

                xOffset += br.size.x + 元素间距.x * 2f;

                t.localPosition = pos;

                if (++x >= 列数 && 列数 > 0)
                {
                    x = 0;
                    ++y;

                    xOffset = 0f;
                    yOffset += bc.size.y + 元素间距.y * 2f;
                }
            }

            // Apply the origin offset
            if (容器锚点 != UIWidget.Pivot.TopLeft)
            {
                po = NGUIMath.GetPivotOffset(容器锚点);

                float fx, fy;

                selfBounds = NGUIMath.CalculateRelativeWidgetBounds(transform);

                fx = Mathf.Lerp(0f, selfBounds.size.x, po.x);
                fy = Mathf.Lerp(-selfBounds.size.y, 0f, po.y);

                Transform myTrans = transform;

                for (int i = 0; i < myTrans.childCount; ++i)
                {
                    Transform t = myTrans.GetChild(i);
                    SpringPosition sp = t.GetComponent<SpringPosition>();

                    if (sp != null)
                    {
                        sp.target.x -= fx;
                        sp.target.y -= fy;
                    }
                    else
                    {
                        Vector3 pos = t.localPosition;
                        pos.x -= fx;
                        pos.y -= fy;
                        t.localPosition = pos;
                    }
                }
            }
            else
                selfBounds = NGUIMath.CalculateRelativeWidgetBounds(transform);
            return selfBounds;
        }
        /// <summary>
        /// Recalculate the position of all elements within the table, sorting them alphabetically if necessary.
        /// </summary>
        [ContextMenu("Execute")]
        public virtual void Reposition()
        {
            if (Application.isPlaying && !mInitDone && NGUITools.GetActive(this))
                Init();
            Bounds selfBounds = new Bounds();
            mReposition = false;
            Transform myTrans = transform;
            List<Transform> ch = GetChildList();
            if (ch.Count > 0)
                selfBounds = RepositionVariableSize(ch);
            if (keepWithinPanel && mPanel != null)
            {
                mPanel.ConstrainTargetToBounds(myTrans, true);
                UIScrollView sv = mPanel.GetComponent<UIScrollView>();
                if (sv != null)
                    sv.UpdateScrollbars(true);
                //            else
                //            {
                //                ScrollCameraViewport sc = mPanel.GetComponent<ScrollCameraViewport>();
                //                if (sc != null)
                //                    sc.UpdateScrollbars(true);
                //            }
            }
            OnReposition(ch, selfBounds);
        }
    }
}