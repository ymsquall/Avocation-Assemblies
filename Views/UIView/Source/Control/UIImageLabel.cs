using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UIView.Control
{
    [ExecuteInEditMode]
    [AddComponentMenu("NGUI/Ext/UIImageLabel")]
    public class UIImageLabel : UIWidget
    {
        // Cached and saved values
        [HideInInspector][SerializeField] UIAtlas mAtlas = null;
        [HideInInspector][SerializeField] string mFormat = "u_Gpingfen_";
        [HideInInspector][SerializeField] string mText = "";
        [HideInInspector][SerializeField] int mSizeX = 0;
        [HideInInspector][SerializeField] int mSizeY = 0;
        List<UISprite> mSpriteList = new List<UISprite>();
        Pivot mOldPivot = Pivot.Center;
        bool mReseted = true;
        string mLastText = "";
        Color mCacheColor = Color.white;
        public UIAtlas Atlas { get { return mAtlas; } }
        List<UISprite> mFreeSprites = new List<UISprite>();
        public string Format
        {
            set
            {
                if (mFormat != value)
                {
                    mFormat = value;
                    mReseted = true;
                }
            }
            get { return mFormat; }
        }
        public int SizeX { set { mSizeX = value; } get { return mSizeX; } }
        public int SizeY { set { mSizeY = value; } get { return mSizeY; } }
        public string Text
        {
            set
            {
                if(mText != value)
                {
                    mText = value;
                    mReseted = true;
                }
            }
            get { return mText; }
        }
        public int Count
        {
            get
            {
                return mSpriteList.Count;
            }
        }
        public UISprite this[int idx]
        {
            set
            {
                if (idx < 0 || idx >= mSpriteList.Count)
                    return;
                if(null == value)
                    return;
                UISprite old = mSpriteList[idx];
                mSpriteList[idx] = value;
                Destroy(old.gameObject);
            }
            get
            {
                if (idx < 0 || idx >= mSpriteList.Count)
                    return null;
                return mSpriteList[idx];
            }
        }
        public void ScrollText(string s, float d, bool scrollSame)
        {
            int idx = 0;
            for (idx = 0; idx < mText.Length; ++idx)
            {
                if (idx >= s.Length)
                    continue;
                if (scrollSame)
                    StartCoroutine(DoScrollText(idx, s[idx], d));
                else if (mText[idx] != s[idx])
                    StartCoroutine(DoScrollText(idx, s[idx], d));
            }
            for (int i = idx; i < s.Length; ++i)
            {
                StartCoroutine(DoScrollText(idx, s[idx], d));
            }
        }
        IEnumerator DoScrollText(int idx, char t, float d)
        {
            UISprite sp = this[idx];
            string imgName = NGUIUtil.Number2ImageName(t, mFormat);
            UISpriteData spData = mAtlas.GetSprite(imgName);
            if(null != spData)
            {
                if (null == sp)
                {
                    sp = NGUITools.AddSprite(gameObject, mAtlas, "");
                    sp.pivot = pivot;
                    sp.color = color;
                    //Vector2 size = Vector2.zero;
                    if (mSizeX <= 0 || mSizeY <= 0)
                    {
                        sp.type = UISprite.Type.Simple;
                        sp.width = spData.width;
                        sp.height = spData.height;
                    }
                    else
                    {
                        sp.type = UISprite.Type.Sliced;
                        sp.width = mSizeX;
                        sp.height = mSizeY;
                    }
                    Vector3 pos = Vector3.zero;
                    if(mSpriteList.Count > 0)
                    {
                        UISprite lastSp = mSpriteList[mSpriteList.Count - 1];
                        switch (pivot)
                        {
                            case Pivot.Bottom:
                            case Pivot.Top:
                            case Pivot.Center:
                                pos.x += lastSp.width * 0.5f;
                                break;
                            case Pivot.Right:
                            case Pivot.BottomRight:
                            case Pivot.TopRight:
                                pos.x += lastSp.width;
                                break;
                        }
                    }
                    switch (pivot)
                    {
                        case Pivot.Bottom:
                        case Pivot.Top:
                        case Pivot.Center:
                            pos.x += sp.width * 0.5f;
                            break;
                        case Pivot.Right:
                        case Pivot.BottomRight:
                        case Pivot.TopRight:
                            pos.x += sp.width;
                            break;
                    }
                    sp.transform.localPosition = pos;
                    mSpriteList.Add(sp);
                }
                if (null != sp)
                {
                    UISprite sprite = NGUITools.AddSprite(gameObject, mAtlas, imgName);
                    if (null != sprite)
                    {
                        sprite.pivot = pivot;
                        sprite.color = color;
                        if (mSizeX <= 0 || mSizeY <= 0)
                        {
                            sprite.type = UISprite.Type.Simple;
                            sprite.width = spData.width;
                            sprite.height = spData.height;
                        }
                        else
                        {
                            sprite.type = UISprite.Type.Sliced;
                            sprite.width = mSizeX;
                            sprite.height = mSizeY;
                        }
                        sprite.MakePixelPerfect();
                        Vector3 pos = sp.transform.localPosition;
                        Vector3 spPos = pos;
                        float oriPosY = pos.y;
                        switch (pivot)
                        {
                            case Pivot.Bottom:
                            case Pivot.BottomLeft:
                            case Pivot.BottomRight:
                                pos.y -= sprite.height;
                                break;
                            case Pivot.Top:
                            case Pivot.TopLeft:
                            case Pivot.TopRight:
                                pos.y -= sp.height;
                                break;
                            case Pivot.Center:
                            case Pivot.Left:
                            case Pivot.Right:
                                pos.y -= (sp.height + sprite.height) * 0.5f;
                                break;
                        }
                        sprite.transform.localPosition = pos;
                        float dist = oriPosY - pos.y;
                        while (pos.y < oriPosY)
                        {
                            yield return 0;
                            if (null == sp)
                                break;
                            float deltaDist = Time.deltaTime * (dist / d);
                            pos.y += deltaDist;
                            spPos.y += deltaDist;
                            sp.transform.localPosition = spPos;
                            sprite.transform.localPosition = pos;
                        }
                        pos.y = oriPosY;
                        sprite.transform.localPosition = pos;
                        this[idx] = sprite;
                    }
                }
            }
        }
        public void ResetShowing(bool noImmediate)
        {
            StopAllCoroutines();
            for(int i = 0; i < mSpriteList.Count; ++ i)
            {
                if (null == mSpriteList[i])
                    continue;
//#if UNITY_EDITOR
//                GameObject.DestroyImmediate(mSpriteList[i].gameObject);
//#else
//                GameObject.Destroy(mSpriteList[i].gameObject);
//#endif
                mSpriteList[i].gameObject.SetActive(false);
                mFreeSprites.Add(mSpriteList[i]);
            }
            mSpriteList.Clear();
            UISprite[] splst = GetComponentsInChildren<UISprite>();
            for (int i = 0; i < splst.Length; ++ i)
            {
#if UNITY_EDITOR
                GameObject.DestroyImmediate(splst[i].gameObject);
#else
                GameObject.Destroy(splst[i].gameObject);
#endif
            }
            width = 0;
            height = 0;
            mCacheColor = color;
            if (null == mAtlas)
                return;
            Vector2 size = Vector2.zero;
            for (int i = 0; i < mText.Length; ++i)
            {
                string imgName = NGUIUtil.Number2ImageName(mText[i], mFormat);
                UISpriteData spData = mAtlas.GetSprite(imgName);
                if (null == spData)
                    continue;
                UISprite sprite = null;
                if (mFreeSprites.Count > 0)
                {
                    sprite = mFreeSprites[0];
                    mFreeSprites.RemoveAt(0);
                    sprite.atlas = mAtlas;
                    sprite.spriteName = imgName;
                    sprite.gameObject.SetActive(true);
                }
                else
                    sprite = NGUITools.AddSprite(gameObject, mAtlas, imgName);
                sprite.depth = depth;
                sprite.pivot = pivot;
                sprite.color = color;
                if (mSizeX <= 0 || mSizeY <= 0)
                {
                    sprite.type = UISprite.Type.Simple;
                    sprite.width = spData.width;
                    sprite.height = spData.height;
                }
                else
                {
                    sprite.type = UISprite.Type.Sliced;
                    sprite.width = mSizeX;
                    sprite.height = mSizeY;
                }
                sprite.MakePixelPerfect();
                size.x += sprite.width;
                if (size.y < sprite.height)
                    size.y = sprite.height;
                mSpriteList.Add(sprite);
            }
            width = (int)size.x;
            height = (int)size.y;
            Vector3 pos = Vector3.zero;
            switch(pivot)
            {
                case Pivot.Bottom:
                case Pivot.Top:
                case Pivot.Center:
                    pos.x = -size.x * 0.5f;
                    break;
                case Pivot.Left:
                case Pivot.BottomLeft:
                case Pivot.TopLeft:
                    pos.x = 0;
                    break;
                case Pivot.Right:
                case Pivot.BottomRight:
                case Pivot.TopRight:
                    pos.x = -size.x;
                    break;
            }
            for (int i = 0; i < mSpriteList.Count; ++i)
            {
                UISprite sprite = mSpriteList[i];
                if (null == sprite)
                    continue;
                switch (pivot)
                {
                    case Pivot.Bottom:
                    case Pivot.Top:
                    case Pivot.Center:
                        pos.x += sprite.width * 0.5f;
                        break;
                    case Pivot.Right:
                    case Pivot.BottomRight:
                    case Pivot.TopRight:
                        pos.x += sprite.width;
                        break;
                }
                sprite.transform.localPosition = pos;
                switch (pivot)
                {
                    case Pivot.Bottom:
                    case Pivot.Top:
                    case Pivot.Center:
                        pos.x += sprite.width * 0.5f;
                        break;
                    case Pivot.Left:
                    case Pivot.BottomLeft:
                    case Pivot.TopLeft:
                        pos.x += sprite.width;
                        break;
                }
            }
            mReseted = false;
            mOldPivot = pivot;
            mLastText = mText;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (mReseted || mOldPivot != pivot || mLastText != mText)
            {
                ResetShowing(false);
            }
            if(mCacheColor != color)
            {
                for(int i = 0; i < mSpriteList.Count; ++ i)
                    mSpriteList[i].color = color;
                mCacheColor = color;
            }
        }
    }
}