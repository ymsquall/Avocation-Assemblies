using System.Collections.Generic;
using Framework.Maths;
using Views.Form.Views;
using Views.Form.Views.Lights;
using System;

namespace Views.Form.Shape
{
    public class Triangle : ShapeBase
    {
        #region Constructor
        public Triangle()
        {
            _pointList.Clear();
            Vertex p1 = new Vertex(new Vector3(-0.5f, -0.5f, 0), Vector3.back, Vertex.Origin.Color);
            Vertex p2 = new Vertex(new Vector3(0, 0.5f, 0), Vector3.back, Vertex.Origin.Color);
            Vertex p3 = new Vertex(new Vector3(0.5f, -0.5f, 0), Vector3.back, Vertex.Origin.Color);
            _pointList.Add(p1);
            _pointList.Add(p2);
            _pointList.Add(p3);
            Name = "mesh_triangle";
        }
        public Triangle(Vertex p1, Vertex p2, Vertex p3)
        {
            _pointList.Clear();
            _pointList.Add(p1);
            _pointList.Add(p2);
            _pointList.Add(p3);
            Name = "mesh_triangle";
        }
        #endregion Constructor

        #region Methods
        protected override void _CollectMesh(Matrix4 transMat, int w, int h,
            RenderType rt, List<IPosLight> lights, AddPixelHandler handler)
        {
            if (_pointList.Count != VertexNumber)
            {
                return;
            }
            GraphPragma.DrawTriangle(transMat,
                _pointList[0], _pointList[1], _pointList[2],
                MaterialName, w, h, rt, lights, handler);
        }
        public static bool CheckAndSortVertexSequence(ref Vertex2D p1, ref Vertex2D p2, ref Vertex2D p3)
        {
            // p1,p2,p3在同一水平面上，这时三角形看起来就是一条线，可以不画它;
            if (p1.y == p2.y && p1.y == p3.y)
            {
                return false;
            }
            // p1,p2,p3在同一垂直面上，同上;
            if (p1.x == p2.x && p1.x == p3.x)
            {
                return false;
            }
            // 按y轴坐标从上到下(从小到大)进行排序;
            Vertex2D tempSort;
            if (p3.y < p2.y)
            {
                tempSort = p2;
                p2 = p3;
                p3 = tempSort;
            }
            if (p3.y < p1.y)
            {
                tempSort = p1;
                p1 = p3;
                p3 = tempSort;
            }
            if (p2.y < p1.y)
            {
                tempSort = p1;
                p1 = p2;
                p2 = tempSort;
            }
            return true;
        }
        public static void SortVertexByViewInfo(bool onePtOutside, Vector3 viewPos0, Vector3 viewPos1, Vector3 viewPos2,
            bool p0Inside, bool p1Inside, bool p2Inside, ref Vertex v0, ref Vertex v1, ref Vertex v2,
            out Vector3 p0, out Vector3 p1, out Vector3 p2)
        {
            // 重新排列顶点顺序;
            p0 = p1 = p2 = Vector3.zero;
            if (onePtOutside)
            {
                // 情况1:有一个点在平面外，裁剪后的三角形会变成四边形，需要将其分割成两个三角形;
                if (p0Inside && p1Inside)
                {
                    // 顺序不变;
                    p0 = viewPos0; p1 = viewPos1; p2 = viewPos2;
                }
                else if (p0Inside && p2Inside)
                {
                    p0 = viewPos0; p1 = viewPos2; p2 = viewPos1;
                    Vertex tmpV2 = v1; v1 = v2; v2 = tmpV2;
                }
                else
                {
                    p0 = viewPos1; p1 = viewPos2; p2 = viewPos0;
                    Vertex tmpV1 = v0; v0 = v1; v1 = v2; v2 = tmpV1;
                }
            }
            else
            {
                // 情况2:有两个点在平面外，裁剪后依然是一个三角形，无需分割;
                if (p0Inside)
                {
                    // 顺序不变;
                    p0 = viewPos0; p1 = viewPos1; p2 = viewPos2;
                }
                else if (p1Inside)
                {
                    p0 = viewPos1; p1 = viewPos2; p2 = viewPos0;
                    Vertex tmpV1 = v0; v0 = v1; v1 = v2; v2 = tmpV1;
                }
                else
                {
                    p0 = viewPos2; p1 = viewPos0; p2 = viewPos1;
                    Vertex tmpV1 = v0; v0 = v2; v1 = v0; v2 = tmpV1;
                }
            }
        }
        public static Vector2 GetEdgePoint(double w, double h, double row, double col, ref Vector2 axis)
        {
            Vector2 pos = new Vector2(col, row);
            var centerPos = new Vector2(w * 0.5f, h * 0.5f);
            var dir = pos - centerPos;
            var dist = dir.Normalize();
            Vector2 pos1 = centerPos + dir * Math.Sqrt(centerPos.x * centerPos.x + centerPos.y * centerPos.y);
            Vector2 pos2 = Vector2.zero;
            // 1象限
            if (col < centerPos.x && row < centerPos.y)
            {
                var upper = new Vector2(centerPos.x, 0);
                var left = new Vector2(0, centerPos.y);
                var p1 = Line.Intersection(centerPos, pos1, upper, Vector2.zero);
                var p2 = Line.Intersection(centerPos, pos1, left, Vector2.zero);
                var d1 = (p1 - centerPos).Length;
                var d2 = (p2 - centerPos).Length;
                if (d1 < d2)
                {
                    pos2 = p1;
                    axis = upper;
                }
                else
                {
                    pos2 = p2;
                    axis = left;
                }
                //int x = (int)col, y = (int)row;
                ////double r = 1, a = 0, b = 0, c = 0;
                //if (x == y)
                //{
                //    // 落在角上(0,0)
                //    pos2 = Vector2.zero;
                //}
                //else
                //{
                //    //if (x > y)
                //    //{
                //    //    // 落在上边(x,0)
                //    //    //r = (float)y / (float)x;
                //    //    //a = centerPos.y;
                //    //    //b = centerPos.x - centerPos.x * r;
                //    //    //c = Math.Sqrt(a * a + b * b);
                //    //    //a = dir.Dot(new Vector2(0, 1));
                //    //    //b = Math.Cos(a);
                //    //    //r = Math.Acos(a) / Math.PI;
                //    //    //pos2 = new Vector2(centerPos.x - centerPos.x * b, 0);
                //    //    pos2 = Line.Intersection(centerPos, pos1, new Vector2(centerPos.x, 0), Vector2.zero);
                //    //}
                //    //else if (x < y)
                //    //{
                //    //    // 落在左边(0,y)    
                //    //    //r = (float)x / (float)y;
                //    //    //a = centerPos.x;
                //    //    //b = centerPos.y * r;
                //    //    ////c = Math.Sqrt(a * a + b * b);
                //    //    //pos2 = new Vector2(0, centerPos.y - b);
                //    //    pos2 = Line.Intersection(centerPos, pos1, new Vector2(0, centerPos.y), Vector2.zero);
                //    //}
                //}
            }
            // 2象限
            if (col >= centerPos.x && row < centerPos.y)
            {
                var upper = new Vector2(centerPos.x, 0);
                var right = new Vector2(w, centerPos.y);
                var p1 = Line.Intersection(centerPos, pos1, upper, new Vector2(w, 0));
                var p2 = Line.Intersection(centerPos, pos1, right, new Vector2(w, 0));
                var d1 = (p1 - centerPos).Length;
                var d2 = (p2 - centerPos).Length;
                if (d1 < d2)
                {
                    pos2 = p1;
                    axis = upper;
                }
                else
                {
                    pos2 = p2;
                    axis = right;
                }
                //int x = (int)(col - centerPos.x), y = (int)row;
                ////double r = 1, a = 0, b = 0, c = 0;
                //if (x == y)
                //{
                //    // 落在角上(w,0)
                //    pos2 = new Vector2(w, 0);
                //}
                //else
                //{
                //    if (x < y)
                //    {
                //        // 落在上边(x,0)
                //        //r = (float)x / (float)y;
                //        //a = centerPos.y;
                //        //b = centerPos.x * r;
                //        //c = Math.Sqrt(a * a + b * b);
                //        //pos2 = new Vector2(c + centerPos.x, 0);
                //        pos2 = Line.Intersection(centerPos, pos1, new Vector2(centerPos.x, 0), new Vector2(w, 0));
                //    }
                //    else if (x > y)
                //    {
                //        // 落在右边(w,y)    
                //        //r = (float)y / (float)x;
                //        //a = centerPos.x;
                //        //b = centerPos.y * r;
                //        //c = Math.Sqrt(a * a + b * b);
                //        //pos2 = new Vector2(w, c);
                //        pos2 = Line.Intersection(centerPos, pos1, new Vector2(w, centerPos.y), new Vector2(w, 0));
                //    }
                //}
            }
            // 3象限
            if (col >= centerPos.x && row >= centerPos.y)
            {
                var down = new Vector2(centerPos.x, h);
                var right = new Vector2(w, centerPos.y);
                var p1 = Line.Intersection(centerPos, pos1, down, new Vector2(w, h));
                var p2 = Line.Intersection(centerPos, pos1, right, new Vector2(w, h));
                var d1 = (p1 - centerPos).Length;
                var d2 = (p2 - centerPos).Length;
                if (d1 < d2)
                {
                    pos2 = p1;
                    axis = down;
                }
                else
                {
                    pos2 = p2;
                    axis = right;
                }
                //int x = (int)(col - centerPos.x), y = (int)(row - centerPos.y);
                ////double r = 1, a = 0, b = 0, c = 0;
                //if (x == y)
                //{
                //    // 落在角上(w,h)
                //    pos2 = new Vector2(w, h);
                //}
                //else
                //{
                //    if (x < y)
                //    {
                //        // 落在下边(x,h)
                //        //r = (float)x / (float)y;
                //        //a = centerPos.y;
                //        //b = centerPos.x * r;
                //        //c = Math.Sqrt(a * a + b * b);
                //        //pos2 = new Vector2(c + centerPos.x, h);
                //        pos2 = Line.Intersection(centerPos, pos1, new Vector2(centerPos.x, h), new Vector2(w, h));
                //    }
                //    else if (x > y)
                //    {
                //        // 落在右边(w,y)    
                //        //r = (float)y / (float)x;
                //        //a = centerPos.x;
                //        //b = centerPos.y * r;
                //        //c = Math.Sqrt(a * a + b * b);
                //        //pos2 = new Vector2(w, c + centerPos.y);
                //        pos2 = Line.Intersection(centerPos, pos1, new Vector2(w, centerPos.y), new Vector2(w, h));
                //    }
                //}
            }
            // 4象限
            if (col < centerPos.x && row >= centerPos.y)
            {
                var left = new Vector2(0, centerPos.x);
                var down = new Vector2(centerPos.x, h);
                var p1 = Line.Intersection(centerPos, pos, left, new Vector2(0, h));
                var p2 = Line.Intersection(centerPos, pos, down, new Vector2(0, h));
                var d1 = (p1 - centerPos).Length;
                var d2 = (p2 - centerPos).Length;
                if (d1 < d2)
                {
                    pos2 = p1;
                    axis = left;
                }
                else
                {
                    pos2 = p2;
                    axis = down;
                }
                //int x = (int)col, y = (int)(row - centerPos.y);
                ////double r = 1, a = 0, b = 0, c = 0;
                //if (x == y)
                //{
                //    // 落在角上(0,h)
                //    pos2 = new Vector2(w, h);
                //}
                //else
                //{
                //    if (x < y)
                //    {
                //        // 落在左边(0,y)
                //        //r = (float)x / (float)y;
                //        //a = centerPos.x;
                //        //b = centerPos.y * r;
                //        //c = Math.Sqrt(a * a + b * b);
                //        //pos2 = new Vector2(0, c + centerPos.y);
                //        pos2 = Line.Intersection(centerPos, pos, new Vector2(0, centerPos.x), new Vector2(0, h));
                //    }
                //    else if (x > y)
                //    {
                //        // 落在下边(x,h)    
                //        //r = (float)y / (float)x;
                //        //a = centerPos.x;
                //        //b = centerPos.y * r;
                //        //c = Math.Sqrt(a * a + b * b);
                //        //pos2 = new Vector2(c, h);
                //        pos2 = Line.Intersection(centerPos, pos, new Vector2(centerPos.x, h), new Vector2(0, h));
                //    }
                //}
            }
            if (pos2.x < 0) pos2.x = 0;
            if (pos2.y < 0) pos2.y = 0;
            if (pos2.x > w) pos2.x = (float)w;
            if (pos2.y > h) pos2.y = (float)h;
            return pos2;
        }
        #endregion Methods

        #region Fields
        public const int VertexNumber = 3;
        #endregion Fields
    }
}
