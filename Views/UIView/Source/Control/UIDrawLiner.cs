using UnityEngine;
using System.Collections.Generic;

namespace UIView.Control
{
    [ExecuteInEditMode]
    [AddComponentMenu("NGUI/Ext/UIDrawLiner")]
    public class UIDrawLiner : UIWidget
    {
        [HideInInspector][SerializeField] float mLineWeight = 1f;
        [HideInInspector][SerializeField] bool mAutoClesed = false;
        [HideInInspector][SerializeField] List<Vector2> mPointList = new List<Vector2>();
        [HideInInspector][SerializeField] List<float> mPointDistList = new List<float>();
        [HideInInspector][SerializeField] float mTotalLength = 0;
        Material mMat = null;
        public override Material material
        {
            get
            {
                if(null == mMat)
                {
                    mMat = new Material(Shader.Find("Unlit/Transparent Colored"));
                    mMat.mainTexture = Texture2D.whiteTexture;
                }
                return mMat;
            }
        }
        public override Shader shader
        {
            get
            {
                return material.shader;
            }
            set
            {
                throw new System.NotImplementedException(GetType() + " has no shader setter");
            }
        }
        public override Texture mainTexture
        {
            get
            {
                Material mat = material;
                return (mat != null) ? mat.mainTexture : null;
            }
            set
            {
                throw new System.NotImplementedException(GetType() + " has no mainTexture setter");
            }
        }
        protected override void OnInit()
        {
            if (null == geometry)
                geometry = new UIGeometry();
            base.OnInit();
            //OnFill(geometry.verts, geometry.uvs, geometry.cols);
        }
        public void Rebuild()
        {
            if (null == geometry)
                geometry = new UIGeometry();
            OnFill(geometry.verts, geometry.uvs, geometry.cols);
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }
        public Vector2 GetPositionOnLine(float p)
        {
            p = Mathf.Clamp(p, 0f, 1f);
            float queryDist = p * mTotalLength;
            Vector2 ret = Vector2.zero, queryPos1 = Vector2.zero, queryPos2 = Vector2.zero;
            for (int i = 0; i < mPointDistList.Count; ++ i)
            {
                bool queryed = false;
                if (mPointDistList[i] == queryDist)
                {
                    if (i >= mPointList.Count - 1)
                    {
                        queryPos1 = mPointList[i];
                        queryPos2 = mPointList[0];
                    }
                    else
                    {
                        queryPos1 = mPointList[i];
                        queryPos2 = mPointList[i + 1];
                    }
                    queryDist = 0;
                    queryed = true;
                }
                else if (mPointDistList[i] > queryDist)
                {
                    if (i == 0)
                    {
                        queryPos1 = mPointList[mPointList.Count - 1];
                        queryPos2 = mPointList[0];
                        queryDist = queryDist - mPointDistList[mPointList.Count - 1];
                    }
                    else
                    {
                        queryPos1 = mPointList[i - 1];
                        queryPos2 = mPointList[i];
                        queryDist = queryDist - mPointDistList[i - 1];
                    }
                    queryed = true;
                }
                if(queryed)
                {
                    Vector2 dir = (queryPos2 - queryPos1).normalized;
                    ret = queryPos1 + dir * queryDist;
                }
            }
            return ret;
        }
        public List<Vector2> PointList { get { return mPointList; } }
        float Fill(Vector2 offset, float helfWeight, bool first, Vector2 start, Vector2 end, BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color> cols)
        {
            Vector2 p0, p1, p2, p3;
            Vector2 dir = end - start;
            float len = dir.sqrMagnitude;
            dir.Normalize();
            Vector2 right = Vector3.Cross(dir, Vector3.forward);
            right.Normalize();
            if (first)
            {
                p0 = start - right * helfWeight + offset;
                p3 = start + right * helfWeight + offset;
            }
            else
            {
                p0 = verts[verts.size - 3];
                p3 = verts[verts.size - 2];
            }
            p1 = end - right * helfWeight + offset;
            p2 = end + right * helfWeight + offset;
            verts.Add(p0);
            verts.Add(p1);
            verts.Add(p2);
            verts.Add(p3);

            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(0, 1));
            uvs.Add(new Vector2(1, 1));
            uvs.Add(new Vector2(1, 0));

            cols.Add(color);
            cols.Add(color);
            cols.Add(color);
            cols.Add(color);
            return len;
        }
        public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color> cols)
        {
            mTotalLength = 0;
            mPointDistList.Clear();
            int offset = verts.size;
            if (PointList.Count > 0)
            {
                Vector2 startPos = cachedTransform.localPosition;
                float helfWeight = mLineWeight * 0.5f;
                int i = 0;
                for (i = 0; i < PointList.Count - 1; ++i)
                {
                    mTotalLength += Fill(startPos, helfWeight, i == 0, PointList[i], PointList[i + 1], verts, uvs, cols);
                    mPointDistList.Add(mTotalLength);
                }
                if(mAutoClesed)
                {
                    mTotalLength += Fill(startPos, helfWeight, false, PointList[0], PointList[PointList.Count - 1], verts, uvs, cols);
                }
                mPointDistList.Add(mTotalLength);
            }
            if (onPostFill != null)
                onPostFill(this, offset, verts, uvs, cols);
        }
    }
}