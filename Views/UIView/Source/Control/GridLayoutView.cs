using System.Collections;
using UnityEngine;

namespace UIView.Control
{
    [ExecuteInEditMode]
    [AddComponentMenu("NGUI/Ext/GridLayoutView")]
    public class GridLayoutView : UIWidget
    {
        [HideInInspector][SerializeField] int mRowCount = 0;
        [HideInInspector][SerializeField] int mColCount = 0;
        [HideInInspector][SerializeField] GridItem[,] mGrids = null;
#if !UNITY_EDITOR
        bool mLayoutChanged = false;
#endif
        protected override void OnStart()
        {
            base.OnStart();
#if UNITY_EDITOR
            DoLayoutChanged();
#else
            StartCoroutine(DoLayoutChanged());
#endif
        }
        void Building()
        {
            mGrids = new GridItem[mRowCount, mColCount];
            for (int i = 0; i < mRowCount; ++i)
            {
                string lineName = "Row_" + i.ToString();
                Transform rowTrans = transform.Find(lineName);
                if (null != rowTrans)
                {
                    for (int j = 0; j < mColCount; ++j)
                    {
                        string itemName = "Item_" + i.ToString() + "_" + j.ToString();
                        Transform itemTrans = rowTrans.Find(itemName);
                        if(null != itemTrans)
                            mGrids[i, j] = itemTrans.GetComponent<GridItem>();
                    }
                }
            }
        }
#if UNITY_EDITOR
        //void OnDisable()
        //{
        //    for (int i = 0; i < mRowCount; ++i)
        //    {
        //        string lineName = "Row_" + i.ToString();
        //        Transform rowTrans = transform.Find(lineName);
        //        if (null != rowTrans)
        //            DestroyImmediate(rowTrans.gameObject);
        //    }
        //    mRowCount = 0;
        //    mColCount = 0;
        //}
        public void DoUpdateItems(GridItem item, float w, float h)
        {
            int countRow = 0, countCol = 0;
            for (int j = 0; j < mColCount; ++j)
            {
                if (mGrids[item.mRowIndex, j] == item || !mGrids[item.mRowIndex, j].AutoSizeW)
                    continue;
                countCol++;
            }
            for (int i = 0; i < mRowCount; ++i)
            {
                if (mGrids[i, item.mColIndex] == item || !mGrids[i, item.mColIndex].AutoSizeH)
                    continue;
                countRow++;
            }

            float fixw = w / countCol, fixh = h / countRow;
            for (int j = 0; j < mColCount; ++j)
            {
                if (mGrids[item.mRowIndex, j] == item || !mGrids[item.mRowIndex, j].AutoSizeW)
                    continue;
                mGrids[item.mRowIndex, j].RatioW += fixw;
            }
            for (int i = 0; i < mRowCount; ++i)
            {
                if (mGrids[i, item.mColIndex] == item || !mGrids[i, item.mColIndex].AutoSizeH)
                    continue;
                mGrids[i, item.mColIndex].RatioH += fixh;
            }
        }
        public void DoLayoutChanged()
        {
#else
        public IEnumerator DoLayoutChanged()
        {
            if(!mLayoutChanged)
            {
                yield return 0;
                mLayoutChanged = true;
#endif
                if (null == mGrids)
                    Building();
                Vector2 pos = new Vector2(-(float)width * 0.5f, -(float)height * 0.5f);
                for (int i = 0; i < mRowCount; ++i)
                {
                    float canAutoSizeWidth = width;
                    for (int jj = 0; jj < mColCount; ++jj)
                    {
                        GridItem tmpItem = mGrids[i, jj];
                        if (!tmpItem.AutoSizeW)
                            canAutoSizeWidth -= tmpItem.width;
                    }
                    float lineHeight = 0;
                    pos.x = -(float)width * 0.5f;
                    for (int j = 0; j < mColCount; ++j)
                    {
                        float canAutoSizeHeight = height;
                        for (int ii = 0; ii < mRowCount; ++ii)
                        {
                            GridItem tmpItem = mGrids[ii, j];
                            if (!tmpItem.AutoSizeH)
                                canAutoSizeHeight -= tmpItem.height;
                        }
                        GridItem item = mGrids[i, j];
                        float w = 0, h = 0;
                        if (item.AutoSizeW)
                        {
                            w = item.RatioW * (float)canAutoSizeWidth;
                            item.width = (int)w;
                        }
                        else
                        {
                            w = item.RealWidth;
                            item.width = (int)w;
                        }
                        if (item.AutoSizeH)
                        {
                            h = item.RatioH * (float)canAutoSizeHeight;
                            item.height = (int)h;
                        }
                        else
                        {
                            h = item.RealHeight;
                            item.height = (int)h;
                        }
                        item.transform.localPosition = new Vector3(pos.x + w * 0.5f, pos.y + h * 0.5f);
                        pos.x += w;
                        if (lineHeight < h)
                            lineHeight = h;
                        item.SizeChanged();
                    }
                    pos.y += lineHeight;
                }
#if !UNITY_EDITOR
                mLayoutChanged = false;
            }
#endif
        }
#if UNITY_EDITOR
        public void ResetLayout()
        {
            GridItem[,] oldGrids = mGrids;
            int rowCount = 0, colCount = 0;
            if (null != oldGrids)
            {
                rowCount = oldGrids.GetLength(0);
                colCount = oldGrids.GetLength(1);
            }
            else if (mRowCount == 0 && mColCount == 0)
                return;
            mGrids = new GridItem[mRowCount, mColCount];
            for (int i = 0; i < mRowCount; ++i)
            {
                string lineName = "Row_" + i.ToString();
                Transform rowTrans = transform.Find(lineName);
                if (null == rowTrans)
                {
                    GameObject rowObj = new GameObject(lineName);
                    rowTrans = rowObj.transform;
                    rowTrans.parent = transform;
                    rowTrans.localPosition = Vector3.zero;
                    rowTrans.localScale = Vector3.one;
                }
                for (int j = 0; j < mColCount; ++j)
                {
                    string itemName = "Item_" + i.ToString() + "_" + j.ToString();
                    Transform itemTrans = rowTrans.Find(itemName);
                    GridItem item = null;
                    if(null == itemTrans)
                    {
                        item = NGUITools.AddWidget<GridItem>(rowTrans.gameObject);
                        item.name = itemName;
                    }
                    else
                        item = itemTrans.GetComponent<GridItem>();
                    item.Container = this;
                    item.mRowIndex = i; item.mColIndex = j;
                    mGrids[i, j] = item;
                }
            }
            for (int i = mRowCount; i < rowCount; ++i)
            {
                string lineName = "Row_" + i.ToString();
                Transform rowTrans = transform.Find(lineName);
                if (null != rowTrans)
                    DestroyImmediate(rowTrans.gameObject);
            }
            for (int i = 0; i < mRowCount; ++i)
            {
                for (int j = mColCount; j < colCount; ++j)
                {
                    DestroyImmediate(oldGrids[i, j].gameObject);
                }
            }
            float preWidth = 1f / mColCount;
            float preHeight = 1f / mRowCount;
            for (int i = 0; i < mRowCount; ++i)
            {
                for (int j = 0; j < mColCount; ++j)
                {
                    GridItem item = mGrids[i, j];
                    item.RatioW = preWidth;
                    item.RatioH = preHeight;
                }
            }
        }
#endif
    }
}