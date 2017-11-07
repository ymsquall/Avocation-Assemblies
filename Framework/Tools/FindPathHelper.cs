//using System;
//using System.Collections.Generic;
//using System.Text;
//using Tools.Maths;

//namespace Tools
//{
//    class PathNode
//    {
//        public PathNode(Segment2D s)
//        {
//            seg = s;
//        }
//        public void AddChilden(PathNode p, float h, bool g2sMax, bool g2sMin, bool s2gMax, bool s2gMin)
//        {
//            p.height = h;
//            p.g2sMax = g2sMax; p.g2sMin = g2sMin;
//            p.s2gMax = s2gMax; p.s2gMin = s2gMin;
//            p.parent = this;
//            childen.Add(p);
//        }
//        public Segment2D seg = null;
//        public float height = 0;
//        public bool g2sMax = false, g2sMin = false, s2gMax = false, s2gMin = false;
//        public List<PathNode> childen = new List<PathNode>();
//        public PathNode parent = null;
//#if UNITY_EDITOR
//        public void DebugDraw()
//        {
//            if(null != seg)
//            {
//                Color cc = Color.green;
//                if (height > 2f)
//                    cc = Color.yellow;
//                Debug.DrawLine(seg.Min, seg.Max, cc);
//            }
//            for (int i = 0; i < childen.Count; ++i)
//                childen[i].DebugDraw();
//        }
//#endif
//    }
//    public class FindPathHelper
//    {
//        List<Segment2D> mCloseList = new List<Segment2D>();
//        List<Segment2D> mDeathBlockList = new List<Segment2D>();
//        Segment2D mStartSeg = null;
//        Segment2D mEndSeg = null;
//        PathNode mRootNode = null;
//        public int mFindTimes = 0;
//        //Dictionary<int, List<PathNode>> mLayerNodeList = new Dictionary<int, List<PathNode>>();
//        public void FindPath(Segment2D[] grounds, UnityEngine.Vector2 pos2D, Segment2D beginSeg, Segment2D endSeg, float jumpHeight, float secondJH)
//        {
//            mFindTimes = 0;
//            mStartSeg = beginSeg;
//            mEndSeg = endSeg;
//            mCloseList.Clear();
//            mDeathBlockList.Clear();
//            mDeathBlockList.Add(mStartSeg);
//            mRootNode = new PathNode(beginSeg);
//            int layer = 1;
//            EnumPaths(grounds, mRootNode, jumpHeight, secondJH, layer);
//        }
//        bool EnumPaths(Segment2D[] grounds, PathNode node, float jumpHeight, float secondJH, int layer)
//        {
//            mFindTimes++;
//            Segment2D segment = node.seg;
//            for (int i = 0; i < grounds.Length; ++i)
//            {
//                if (mDeathBlockList.Contains(grounds[i]))
//                    continue;
//                if (mCloseList.Contains(grounds[i]))
//                    continue;
//                if (grounds[i].Min.y > segment.Min.y && grounds[i].Min.y < (segment.Min.y + secondJH))
//                {
//                    float height = grounds[i].Min.y - segment.Min.y;
//                    bool g2sMax = grounds[i].Max.x > segment.Min.x && grounds[i].Max.x < segment.Max.x;
//                    bool g2sMin = grounds[i].Min.x > segment.Min.x && grounds[i].Min.x < segment.Max.x;
//                    bool s2gMax = segment.Max.x > grounds[i].Min.x && segment.Max.x < grounds[i].Max.x;
//                    bool s2gMin = segment.Min.x > grounds[i].Min.x && segment.Min.x < grounds[i].Max.x;
//                    if (g2sMax || g2sMin || s2gMax || s2gMin)
//                    {
//                        if (grounds[i] == mEndSeg)
//                        {
//                            node.height = height;
//                            node.g2sMax = g2sMax; node.g2sMin = g2sMin;
//                            node.s2gMax = s2gMax; node.s2gMin = s2gMin;
//                            return true;
//                        }
//                        mCloseList.Add(grounds[i]);
//                        PathNode childNode = new PathNode(grounds[i]);
//                        childNode.parent = node;
//                        if (EnumPaths(grounds, childNode, jumpHeight, secondJH, layer + 1))
//                        {
//                            node.AddChilden(childNode, height, g2sMax, g2sMin, s2gMax, s2gMin);
//                        }
//                        else if (childNode.childen.Count <= 0)
//                            mDeathBlockList.Add(grounds[i]);
//                        mCloseList.Remove(grounds[i]);
//                    }
//                }
//            }
//            //for(int i = 0; i < node.childen.Count; ++ i)
//            //{
//            //    EnumPaths(grounds, node.childen[i], jumpHeight, secondJH);
//            //}
//            return false;
//        }
//#if UNITY_EDITOR
//        public void DebugDraw()
//        {
//            if (null != mRootNode)
//                mRootNode.DebugDraw();
//            for (int i = 0; i < mDeathBlockList.Count; ++i)
//                Debug.DrawLine(mDeathBlockList[i].Min, mDeathBlockList[i].Max, Color.red);
//        }
//#endif
//    }
//}
