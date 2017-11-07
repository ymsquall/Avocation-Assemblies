using System.Collections.Generic;
using UnityEngine;

namespace UIView.Scrolling
{
    using Framework.Tools;
    using Movement = UIScrollView.Movement;
    public enum TableItemOpt
	{
		add,
		remove,
		insert,
		update,
		clear,
		max
	}
#if UNITY_EDITOR
    public class ScrollTableView : MonoBehaviour
#else
    public class ScrollTableView : MonoBehaviour
#endif
    {
        public delegate void ItemOptEvent(int index, TableItemOpt opt, GameObject item, params object[] ps);
        public event ItemOptEvent OnItemOpt;
        public delegate void OveredEvent(bool changed, ref bool resetScrollPos);
        public event OveredEvent OnItemOvered;
        public delegate void TableEvent(List<Transform> ch);
        public event TableEvent OnTableBuilded;
        public delegate void ScrollEvent();
        public event ScrollEvent OnDragStarted;
        public event ScrollEvent OnDragOvered;
        public event ScrollEvent OnMovingOvered;

        public GameObject 单元模板;
        // scroll objs
        public ScrollViewport 滚动视图;
        public ScrollTable 表格;
        // data source
        List<object[]> mAddedList = new List<object[]>();
        List<int> mRemovedList = new List<int>();
        List<TwoData<int, object[]>> mInsertedList = new List<TwoData<int, object[]>>();
        List<TwoData<int, object[]>> mUpdatedList = new List<TwoData<int, object[]>>();
        Queue<GameObject> mFreedItemList = new Queue<GameObject>();
        bool mChanged = true;
        int mScrollOnIndex = 0;
        bool mReposedAutoResetScrollPos = false;
        List<Transform> mLastChildenList;
        float mScrollClipStartOffset = 0;
        float mScrollClipEndOffset = 0;

        public int ScrollOnIndex
        {
            set { mScrollOnIndex = value; }
            get { return mScrollOnIndex; }
        }
        public int TableSize
        {
            get { return mLastChildenList.Count; }
        }
        public UIPanel TablePanel
        {
            get
            {
                UIPanel pan = null;
                if (null != 滚动视图)
                    pan = 滚动视图.panel;
                if (null == pan)
                {
                    if (null != 表格)
                        pan = 表格.Panel;
                }
                return pan;
            }
        }
        public bool IsEmpty
        {
            get { if (null == 表格) return true; return 表格.transform.childCount <= 0; }
        }
        public Vector2 ClipSize
        {
            get
            {
                return new Vector2(TablePanel.baseClipRegion.z, TablePanel.baseClipRegion.w);
            }
        }
        public bool CanScrollFront
        {
            get
            {
                Vector3 pos = 滚动视图.transform.localPosition;
                bool can = false;
                switch (滚动视图.movement)
                {
                    case Movement.Horizontal: can = pos.x > mScrollClipStartOffset; break;
                    case Movement.Vertical: can = pos.y > mScrollClipStartOffset; break;
                }
                return can;
            }
        }
        public bool CanScrollBack
        {
            get
            {
                Vector3 pos = 滚动视图.transform.localPosition;
                bool can = false;
                switch (滚动视图.movement)
                {
                    case Movement.Horizontal: can = pos.x < mScrollClipEndOffset; break;
                    case Movement.Vertical: can = pos.y < mScrollClipEndOffset; break;
                }
                return can;
            }
        }
        void Start()
        {
            NGUI2DRootPanel root = NGUI2DRootPanel.Inst;
            if (null == root)
            {
                Debug.LogError("创建ScrollTableView[" + name + "]时NGUIRoot还未创建！");
                Destroy(gameObject);
                return;
            }
            滚动视图.onDragStarted = () => { OnStoppedMoving(); if (null != OnDragStarted) OnDragStarted(); };
            滚动视图.onDragFinished = () => { if (null != OnDragOvered) OnDragOvered(); };
            滚动视图.onStoppedMoving = OnStoppedMoving;
            表格.OnReposed += new ScrollTableContainer.ReposEventHandler(OnTableReposed);
        }
        void OnTableReposed(List<Transform> ch, Bounds b)
        {
            表格.transform.localPosition = Vector3.zero;
            mLastChildenList = ch;
            if (ScrollOnIndex >= 0 && ScrollOnIndex < mLastChildenList.Count)
                ScrollToIndex(ScrollOnIndex, true);
            else if (mReposedAutoResetScrollPos)
            {
                滚动视图.ResetPosition();
                mReposedAutoResetScrollPos = false;
            }
            滚动视图.GetMinMaxOffset(b.size, 表格.容器锚点, ref mScrollClipStartOffset, ref mScrollClipEndOffset);
            if (null != OnTableBuilded)
                OnTableBuilded(mLastChildenList);
        }
        public void AddItem(params object[] ps)
        {
            mAddedList.Add(ps);
            mChanged = true;
        }
        public void RemoveItem(int index)
        {
            if (index < 0)
                return;
            if (!mRemovedList.Contains(index))
                mRemovedList.Add(index);
            mChanged = true;
        }
        public void InsertItem(int index, params object[] ps)
        {
            if (index < 0)
                return;
            if (index < mAddedList.Count)
            {
                mAddedList.Insert(index, ps);
                mChanged = true;
                return;
            }
            for(int i = 0; i < mInsertedList.Count; ++ i)
            {
                if (mInsertedList[i].First == index)
                    return;
            }
            if (mRemovedList.Contains(index))
            {
                mRemovedList.RemoveAt(index);
                mUpdatedList.Add(new TwoData<int, object[]>(index, ps));
                mChanged = true;
                return;
            }
            mInsertedList.Add(new TwoData<int, object[]>(index, ps));
            mChanged = true;
        }
        public void ClearItem()
        {
            mAddedList.Clear();
            mRemovedList.Clear();
            mInsertedList.Clear();
            mUpdatedList.Clear();
			mLastChildenList = null;
            if (null != 表格)
            {
                List<Transform> itemList = 表格.GetChildList();
                for (int i = 0; i < itemList.Count; ++i)
                    FreedItem(itemList[i]);
                if (null != OnItemOpt)
                    OnItemOpt(-1, TableItemOpt.clear, null, null);
            }
			滚动视图.currentMomentum = Vector3.zero;
			滚动视图.Scroll(0);
			if (null != OnDragOvered) OnDragOvered();
			OnStoppedMoving ();
        }
        GameObject NewItem()
        {
            GameObject item = null;
            while (mFreedItemList.Count > 0)
            {
                item = mFreedItemList.Dequeue();
                if (null != item)
                    break;
            }
            if (null == item)
                item = Instantiate(单元模板) as GameObject;
            item.hideFlags = HideFlags.DontSave;
            Transform trans = item.transform;
            trans.parent = null;
            //item.SetActive(true);
            return item;
        }
        void FreedItem(Transform trans)
        {
            trans.hideFlags = HideFlags.HideInHierarchy;
            trans.parent = transform;
            //UIToggle[] ts = trans.GetComponents<UIToggle>();
            //for (int i = 0; i < ts.Length; ++i)
            //    ts[i].value = false;
            //ts = trans.GetComponentsInChildren<UIToggle>();
            //for (int i = 0; i < ts.Length; ++i)
            //    ts[i].value = false;
            trans.gameObject.SetActive(false);
            mFreedItemList.Enqueue(trans.gameObject);
        }
        public void ClearFreeedList()
        {
            while (mFreedItemList.Count > 0)
            {
                GameObject obj = mFreedItemList.Dequeue();
                if (null != obj)
                {
                    obj.hideFlags = HideFlags.DontSave;
                    Destroy(obj);
                }
            }
        }
        int GetItemNameIndex(Transform itemTrans)
        {
            string itemName = "";
            int idx = 0;
            if (null != itemTrans)
            {
                itemName = itemTrans.name;
                if (!int.TryParse(itemName, out idx))
                {
                    itemName = "0000";
                    idx = 0;
                }
                else
                    idx++;
            }
            else
                itemName = "0000";
            return idx;
        }
        void OnAddItem(int index, int nameIdx, Transform parent, object[] ps)
        {
            GameObject item = NewItem();
            item.name = nameIdx.ToString();
            Transform trans = item.transform;
            trans.parent = parent;
            trans.localPosition = Vector3.zero;
            trans.localScale = Vector3.one;
            trans.localRotation = Quaternion.identity;
            item.SetActive(true);
            if (null != OnItemOpt)
                OnItemOpt(index, TableItemOpt.add, item, ps);
        }
        public void PanelFadeSync(UIPanel pan)
        {
            if (null != 表格 && 表格.Inited)
            {
                表格.Panel.alpha = pan.alpha;
            }
        }
        void Update()
        {
            if(mChanged)
            {
                if (null == 表格 || !表格.Inited)
                    return;
                mChanged = false;
                Transform tableTrans = 表格.transform;
                GameObject item = null;
                bool changed = false;
                if (mAddedList.Count > 0)
                {
                    Transform lastItemTrans = 表格.LastChild;
                    int nameIdx = GetItemNameIndex(lastItemTrans);
                    for (int i = 0; i < mAddedList.Count; ++i)
                    {
                        OnAddItem(i, nameIdx++, tableTrans, mAddedList[i]);
                        changed = true;
                    }
                    mAddedList.Clear();
                }
                if(mRemovedList.Count > 0)
                {
                    List<Transform> itemList = 表格.GetChildList();
                    for (int i = 0; i < mRemovedList.Count; ++i)
                    {
                        int index = mRemovedList[i];
                        if (index < 0 || index >= itemList.Count)
                            continue;
                        if (index < mAddedList.Count)
                            mAddedList.RemoveAt(index);
                        for (int j = 0; j < mInsertedList.Count; ++j)
                        {
                            if (mInsertedList[j].First == index)
                            {
                                mInsertedList.RemoveAt(j);
                                break;
                            }
                        }
                        for (int j = 0; j < mUpdatedList.Count; ++j)
                        {
                            if (mUpdatedList[j].First == index)
                            {
                                mUpdatedList.RemoveAt(j);
                                break;
                            }
                        }
                        FreedItem(itemList[index]);
                        if (null != OnItemOpt)
                            OnItemOpt(index, TableItemOpt.remove, item, null);
                        changed = true;
                    }
                    mRemovedList.Clear();
                }
                if (mInsertedList.Count > 0)
                {
                    mInsertedList.Sort((TwoData<int, object[]> v1, TwoData<int, object[]> v2) =>
                        {
                            return v1.First.CompareTo(v2.First);
                        });
                    List<Transform> itemList = 表格.GetChildList();
                    int nameIdx = 0;
                    for (int i = 0; i < itemList.Count; ++ i)
                    {
                        for (int j = 0; j < mInsertedList.Count; ++j)
                        {
                            TwoData<int, object[]> lst = mInsertedList[j];
                            if(lst.First == i)
                            {
                                nameIdx = GetItemNameIndex(itemList[i]);
                                item = NewItem();
                                item.name = nameIdx.ToString();
                                item.transform.parent = tableTrans;
                                for (int k = i; k < itemList.Count; ++k)
                                    itemList[k].name = (nameIdx++).ToString();
                                if (null != OnItemOpt)
                                    OnItemOpt(lst.First, TableItemOpt.insert, item, lst.Second);
                                mInsertedList.RemoveAt(j);
                                changed = true;
                                break;
                            }
                        }
                    }
                    Transform lastItemTrans = 表格.LastChild;
                    nameIdx = GetItemNameIndex(lastItemTrans);
                    for (int i = 0; i < mInsertedList.Count; ++i)
                    {
                        TwoData<int, object[]> lst = mInsertedList[i];
                        OnAddItem(lst.First, nameIdx++, tableTrans, lst.Second);
                        changed = true;
                    }
                }
                if (mUpdatedList.Count > 0)
                {
                    List<Transform> itemList = 表格.GetChildList();
                    for (int i = 0; i < mUpdatedList.Count; ++i)
                    {
                        TwoData<int, object[]> lst = mInsertedList[i];
                        if (lst.First < 0 || lst.First >= itemList.Count)
                            continue;
                        item = itemList[lst.First].gameObject;
                        if (null != OnItemOpt)
                            OnItemOpt(lst.First, TableItemOpt.update, item, lst.Second);
                        changed = true;
                    }
                    mUpdatedList.Clear();
                }
                if (changed)
                    表格.repositionNow = true;
                mReposedAutoResetScrollPos = true;
                if (null != OnItemOvered)
                    OnItemOvered(changed, ref mReposedAutoResetScrollPos);
            }
        }
        public void ResetPosWithIndex(int inIdx)
        {
            if (null == 表格)
                return;
			if (inIdx < 0)
				inIdx = 0;
			ScrollToIndex(inIdx, true);
		}
		public void ScrollFront(bool immediate)
		{
            if (null == 表格)
                return;
			ScrollToIndex(0, immediate);
		}
		public void ScrollBack(bool immediate)
		{
            if (null == 表格)
                return;
            List<Transform> list = 表格.GetChildList();
            int count = list.Count - 1;
			ScrollToIndex(count, immediate);
        }
		public void ScrollToIndex(int index, bool immediate)
		{
            if (index < 0)
                return;
            ScrollOnIndex = index;
			if(!immediate) return;
            if (null == 表格 || null == mLastChildenList || mLastChildenList.Count <= 0)
                return;
            if(index >= mLastChildenList.Count)
				index = mLastChildenList.Count - 1;
			if (index < 0)
				return;
            Transform trans = mLastChildenList[index];
            if (null == trans)
                return;
            switch(滚动视图.movement)
            {
                case UIScrollView.Movement.Horizontal:
                    {
                        float dist = trans.localPosition.x;
                        Scroll(dist);
                    }
                    break;
                case UIScrollView.Movement.Vertical:
                    {
                        float dist = trans.localPosition.y;
                        Scroll(dist);
                    }
                    break;
            }
        }
        void Scroll(float dist)
        {
            Scroll(dist, true);
        }
        void Scroll(float dist, bool reset)
        {
            if (null == 表格 || null == 滚动视图)
                return;
            Vector2 maxSize = 表格.Bounds.size;
            滚动视图.Scroll(dist, maxSize, 表格.容器锚点);
			if (null != OnDragOvered)
				OnDragOvered();
			OnStoppedMoving();
        }
		void OnStoppedMoving()
        {
			if(null == 滚动视图) return;
			if(null == TablePanel) return;
			if(null == mLastChildenList) return;
			switch (滚动视图.movement)
			{
			case Movement.Horizontal:
				{
					float offset = TablePanel.clipOffset.x;
					float neerZero = 999999;
					for(int i = 0; i < mLastChildenList.Count; ++ i)
					{
						Vector3 pos = mLastChildenList[i].localPosition;
						float nowNZ = Mathf.Abs(pos.x - offset);
						if(nowNZ < neerZero)
						{
							neerZero = nowNZ;
							ScrollOnIndex = i;
						}
						else
							break;
					}
				}
				break;
			case Movement.Vertical:
				{
					float offset = TablePanel.clipOffset.y;
					float neerZero = 999999;
					for(int i = 0; i < mLastChildenList.Count; ++ i)
					{
						Vector3 pos = mLastChildenList[i].localPosition;
						float nowNZ = Mathf.Abs(pos.y - offset);
						if(nowNZ < neerZero)
						{
							neerZero = nowNZ;
							ScrollOnIndex = i;
						}
						else
							break;
					}
				}
				break;
			}
			//Debug.Log ("Scroll on index=" + ScrollOnIndex.ToString());
			if(null != OnMovingOvered)
				OnMovingOvered();
        }
        void OnDestroy()
        {
            while (mFreedItemList.Count > 0)
            {
                GameObject obj = mFreedItemList.Dequeue();
                if(null != obj)
                {
                    obj.hideFlags = HideFlags.DontSave;
                    Destroy(obj);
                }
            }
        }
#if UNITY_EDITOR
        public virtual void ExportSettings()
        {
            if (null == 单元模板)
            {
                单元模板 = new GameObject("模板");
                Transform trans = 单元模板.transform;
                trans.parent = transform;
                trans.localPosition = Vector3.zero;
                trans.localScale = Vector3.one;
                trans.localRotation = Quaternion.identity;
            }
            if (单元模板.activeSelf)
                单元模板.SetActive(false);
            if(表格 == null)
                表格 = GetComponentInChildren<ScrollTable>();
            if (表格 == null)
                表格 = GetComponent<ScrollTable>();
			if (null != 滚动视图 && null != 表格)
			{
				switch(滚动视图.movement)
				{
					case Movement.Horizontal:
						{
							switch (表格.容器锚点)
							{
							case UIWidget.Pivot.Center:
							case UIWidget.Pivot.Top:
							case UIWidget.Pivot.Bottom:
								break;
							default:
								{
									UIBaseView view = GetComponentInParent<UIBaseView>();
									Debug.LogError("ScrollViewport[" + view.name + "/" + transform.parent.name + "]can only use center with context pivot!");
								}
								break;
							}
						}
						break;
					case Movement.Vertical:
						{
							switch(表格.容器锚点)
							{
							case UIWidget.Pivot.Center:
							case UIWidget.Pivot.Left:
							case UIWidget.Pivot.Right:
								break;
							default:
								{
									UIBaseView view = GetComponentInParent<UIBaseView>();
									Debug.LogError("ScrollViewport[" + view.name + "/" + transform.parent.name + "]can only use center with context pivot!");
								}
								break;
							}
						}
						break;
				}
			}
			滚动视图.内容锚点 = 表格.容器锚点;
		}
		#endif
    }
}
