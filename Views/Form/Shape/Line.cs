using Framework.Maths;
using System;

namespace Views.Form.Shape
{
    public enum DrawLineType
    {
        DDA,
        Bresenham,
    }
    public class Line : ShapeBase
    {
        #region Constructor
        public Line()
        {
            Vertex p1 = new Vertex(Vertex.Origin.Position + new Vector3(-0.5f, -0.5f, 0), Vertex.Origin.Normal, Vertex.Origin.Color);
            Vertex p2 = new Vertex(Vertex.Origin.Position + new Vector3(0.5f, 0.5f, 0), Vertex.Origin.Normal, Vertex.Origin.Color);
            _pointList.Add(p1);
            _pointList.Add(p2);
            Name = "mesh_line";
        }
        public Line(Vertex p1, Vertex p2)
        {
            _pointList.Add(p1);
            _pointList.Add(p2);
            Name = "mesh_line";
        }
        #endregion Constructor

        #region Methods
        /// <summary>
        /// 计算两条直线的交点
        /// </summary>
        /// <param name="lineFirstStar">L1的点1坐标</param>
        /// <param name="lineFirstEnd">L1的点2坐标</param>
        /// <param name="lineSecondStar">L2的点1坐标</param>
        /// <param name="lineSecondEnd">L2的点2坐标</param>
        /// <returns></returns>
        public static Vector2 Intersection(Vector2 lineFirstStar, Vector2 lineFirstEnd, Vector2 lineSecondStar, Vector2 lineSecondEnd)
        {
            /*
             * L1，L2都存在斜率的情况：
             * 直线方程L1: ( y - y1 ) / ( y2 - y1 ) = ( x - x1 ) / ( x2 - x1 ) 
             * => y = [ ( y2 - y1 ) / ( x2 - x1 ) ]( x - x1 ) + y1
             * 令 a = ( y2 - y1 ) / ( x2 - x1 )
             * 有 y = a * x - a * x1 + y1   .........1
             * 直线方程L2: ( y - y3 ) / ( y4 - y3 ) = ( x - x3 ) / ( x4 - x3 )
             * 令 b = ( y4 - y3 ) / ( x4 - x3 )
             * 有 y = b * x - b * x3 + y3 ..........2
             * 
             * 如果 a = b，则两直线平等，否则， 联解方程 1,2，得:
             * x = ( a * x1 - b * x3 - y1 + y3 ) / ( a - b )
             * y = a * x - a * x1 + y1
             * 
             * L1存在斜率, L2平行Y轴的情况：
             * x = x3
             * y = a * x3 - a * x1 + y1
             * 
             * L1 平行Y轴，L2存在斜率的情况：
             * x = x1
             * y = b * x - b * x3 + y3
             * 
             * L1与L2都平行Y轴的情况：
             * 如果 x1 = x3，那么L1与L2重合，否则平等
             * 
            */
            float a = 0, b = 0;
            int state = 0;
            if (lineFirstStar.x != lineFirstEnd.x)
            {
                a = (lineFirstEnd.y - lineFirstStar.y) / (lineFirstEnd.x - lineFirstStar.x);
                state |= 1;
            }
            if (lineSecondStar.x != lineSecondEnd.x)
            {
                b = (lineSecondEnd.y - lineSecondStar.y) / (lineSecondEnd.x - lineSecondStar.x);
                state |= 2;
            }
            switch (state)
            {
                case 0: //L1与L2都平行Y轴
                    {
                        if (lineFirstStar.x == lineSecondStar.x)
                        {
                            //throw new Exception("两条直线互相重合，且平行于Y轴，无法计算交点。");
                            return new Vector2(0, 0);
                        }
                        else
                        {
                            //throw new Exception("两条直线互相平行，且平行于Y轴，无法计算交点。");
                            return new Vector2(0, 0);
                        }
                    }
                case 1: //L1存在斜率, L2平行Y轴
                    {
                        float x = lineSecondStar.x;
                        float y = (lineFirstStar.x - x) * (-a) + lineFirstStar.y;
                        return new Vector2(x, y);
                    }
                case 2: //L1 平行Y轴，L2存在斜率
                    {
                        float x = lineFirstStar.x;
                        //网上有相似代码的，这一处是错误的。你可以对比case 1 的逻辑 进行分析
                        //源code:lineSecondStar * x + lineSecondStar * lineSecondStar.x + p3.y;
                        float y = (lineSecondStar.x - x) * (-b) + lineSecondStar.y;
                        return new Vector2(x, y);
                    }
                case 3: //L1，L2都存在斜率
                    {
                        if (a == b)
                        {
                            // throw new Exception("两条直线平行或重合，无法计算交点。");
                            return new Vector2(0, 0);
                        }
                        float x = (a * lineFirstStar.x - b * lineSecondStar.x - lineFirstStar.y + lineSecondStar.y) / (a - b);
                        float y = a * x - a * lineFirstStar.x + lineFirstStar.y;
                        return new Vector2(x, y);
                    }
            }
            // throw new Exception("不可能发生的情况");
            return new Vector2(0, 0);
        }
        public static void DrawLine(Vertex2D p1, Vertex2D p2, AddPixelHandler handler, DrawLineType drawType, ZTestHandler zTest)
        {
            switch(drawType)
            {
                case DrawLineType.DDA:
                    DrawDDALine(p1, p2, handler, zTest);
                    break;
                case DrawLineType.Bresenham:
                    DrawBresenhamLine(p1, p2, handler, zTest);
                    break;
            }
        }
        public static void DrawDDALine(Vertex2D p1, Vertex2D p2, AddPixelHandler handler, ZTestHandler zTest)
        {
            var dist = (p1 - p2).Length2D;
            // 如果长度小于2像素就只是一个点;
            if (dist < 2)
            {
                if(null != zTest && !zTest(p1.x, p1.y, p1.depth))
                {
                    return;
                }
                handler?.Invoke(p1.x, p1.y, p1.depth, p1.color);
                return;
            }
            // 绘制中心点;
            Vertex2D center = p1 + (p2 - p1) / 2;
            float depth = MathUtil.Interpolate(p1.depth, center.depth, 0.5f);
            if (null != zTest && !zTest(center.x, center.y, depth))
            {
                return;
            }
            // 颜色线性插值;
            Color clr = Color.FromArgb(
                    (p1.color.a + p2.color.a) * 0.5f,
                    (p1.color.r + p2.color.r) * 0.5f,
                    (p1.color.g + p2.color.g) * 0.5f,
                    (p1.color.b + p2.color.b) * 0.5f);
            if (null != handler)
            {
                handler(center.x, center.y, MathUtil.Interpolate(p1.depth, center.depth, 0.5f), clr);
            }
            // 前半段递归;
            DrawDDALine(p1, center, handler, zTest);
            // 后半段递归;
            DrawDDALine(center, p2, handler, zTest);
        }
        public static void DrawBresenhamLine(Vertex2D p1, Vertex2D p2, AddPixelHandler handler, ZTestHandler zTest)
        {
            // Bresenham’s line algorithm;
            int x1 = p1.x;
            int y1 = p1.y;
            int x2 = p2.x;
            int y2 = p2.y;

            var dx = Math.Abs(x2 - x1);
            var dy = Math.Abs(y2 - y1);
            var sx = (x1 < x2) ? 1 : -1;
            var sy = (y1 < y2) ? 1 : -1;
            var err = dx - dy;
            float totalLen = (p1 - p2).SqrLength2D;
            if (totalLen <= 0f)
            {
                return;
            }
            float invTotalLen = 1f / totalLen;
            float p1x = p1.x;
            float p1y = p1.y;
            float p1d = p1.depth;
            float p2d = p2.depth;
            Color p1c = p1.color;
            Color p2c = p2.color;
            while (true)
            {
                float p1x1 = p1x - x1;
                float p1y1 = p1y - y1;
                float len = p1x1 * p1x1 + p1y1 * p1y1;
                float ratio = len * invTotalLen;                        // MathUtil.Clamp(len / totalLen);
                ratio = ratio < 0 ? 0 : ratio;
                ratio = ratio > 1 ? 1 : ratio;
                float z1 = p1d + (p2d - p1d) * ratio;    //MathUtil.Interpolate(p1.Depth, p2.Depth, ratio);
                if (null != zTest && zTest(x1, y1, z1))
                {
                    float a = p1c.a * ratio + p2c.a * (1f - ratio);
                    float r = p1c.r * ratio + p2c.r * (1f - ratio);
                    float g = p1c.g * ratio + p2c.g * (1f - ratio);
                    float b = p1c.b * ratio + p2c.b * (1f - ratio);
                    Color clr = Color.FromArgb(a, r, g, b);
                    if (null != handler)
                    {
                        handler(x1, y1, z1, clr);
                    }
                }
                if ((x1 == x2) && (y1 == y2))
                {
                    break;
                }
                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x1 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y1 += sy;
                }
            }
        }
        #endregion Methods
    }
}
