using Framework.Maths;
using System.Collections.Generic;
using System.Threading.Tasks;
using Views.Form.Materials;
using Views.Form.Shape;
using Views.Form.Views.Lights;

namespace Views.Form.Views
{
    public static class GraphPragma
    {
        #region Helper
        // 3d坐标投影到2d屏幕坐标;
        public static Vertex2D ProjectToScreen(Vector3 coord, Matrix4 transMat, float w, float h)
        {
            var point = transMat * coord;
            var x = point.x * w + w / 2.0f;
            var y = -point.y * h + h / 2.0f;
            return (new Vertex2D((int)x, (int)y, point.z));
        }
        public static Vertex2D BuildVertex2D(Vertex vertex, Matrix4 transMat, Vector3 cameraPos, List<IPosLight> lights, int w, int h)
        {
            Vertex2D result = default(Vertex2D);
            SoftDevice device = SoftDevice.Default;
            if(null == device)
            {
                return result;
            }
            // 将3d坐标投影到2d空间;
            result = ProjectToScreen(vertex.position, transMat, (float)w, (float)h);
            // 顶点光照;
            LightUtil.ApplyVertexLightings(lights, cameraPos, vertex.worldPosition, vertex.worldNormal, ref result);
            // 填充顶点信息;
            result.FillRenderData(vertex.uv0, vertex.color, device.PerspectiveCorrection);
            return result;
        }
        #endregion Helper

        #region DrawTriangle
        public static void DrawTriangle(Matrix4 transMat, Vertex v0, Vertex v1, Vertex v2,
            Texture tex, int width, int height, RenderType rt, List<IPosLight> lights, AddPixelHandler handler)
        {
            Camera camera = Camera.Default;
            if (null == camera)
            {
                return;
            }
            // 将3d坐标投影到2d空间;
            Vertex2D p0 = BuildVertex2D(v0, transMat, camera.Pos, lights, width, height);
            Vertex2D p1 = BuildVertex2D(v1, transMat, camera.Pos, lights, width, height);
            Vertex2D p2 = BuildVertex2D(v2, transMat, camera.Pos, lights, width, height);
            switch (rt)
            {
                case RenderType.WireFrame:
                    // 计算线段;
                    Line.DrawLine(p0, p1, handler, DrawLineType.Bresenham, SoftDevice.Default.ZTest);
                    Line.DrawLine(p1, p2, handler, DrawLineType.Bresenham, SoftDevice.Default.ZTest);
                    Line.DrawLine(p2, p0, handler, DrawLineType.Bresenham, SoftDevice.Default.ZTest);
                    break;
                case RenderType.Solid:
                    // 填充三角形;
                    _VertexPragma(tex, p0, p1, p2, handler);
                    break;
            }
        }
        public static void DrawTriangle(Matrix4 transMat,
            Vertex v0, Vertex v1, Vertex v2, string matName, int w, int h,
            RenderType rt, List<IPosLight> lights, AddPixelHandler handler)
        {
            Camera camera = Camera.Default;
            if (null == camera || (rt != RenderType.WireFrame &&
                camera.BackfaceBeCulled(v0.worldPosition, v1.worldPosition, v2.worldPosition)))
            {
                // 剔除掉了v0-v1-v2组成的三角形;
                return;
            }
            Material mat = MaterialManager.Instance[matName];
            // 裁剪,在摄像机空间中进行;
            // 将顶点变换到摄像机空间;
            Vector3 viewPos0 = camera.viewMatrix * v0.worldPosition;
            Vector3 viewPos1 = camera.viewMatrix * v1.worldPosition;
            Vector3 viewPos2 = camera.viewMatrix * v2.worldPosition;
            bool p0Inside = false, p1Inside = false, p2Inside = false;
            int outsideNum = camera.VisibleTest(viewPos0, viewPos1, viewPos2,
                ref p0Inside, ref p1Inside, ref p2Inside);
            if (outsideNum >= Shape.Triangle.VertexNumber)
            {
                // 三个点都在视距外，整个裁剪掉;
                return;
            }
            // 只会有2种情况;
            if (outsideNum == 1)
            {
                Vector3 newv0, newv1, newv2;
                // 重排顶点顺序;
                Shape.Triangle.SortVertexByViewInfo(true, viewPos0, viewPos1, viewPos2,
                    p0Inside, p1Inside, p2Inside, ref v0, ref v1, ref v2, out newv0, out newv1, out newv2);
                // v0-v2,插值计算v02;
                Vertex newv02 = Vertex.Origin;
                MathUtil.IntersectVertexByDist(v0, v2, newv0, newv2, camera.Near,
                    (pos, norm, uv, color) => { newv02 = new Vertex(pos, norm, color, uv); });
                // v1-v2,插值计算v12;
                Vertex newv12 = Vertex.Origin;
                MathUtil.IntersectVertexByDist(v1, v2, newv1, newv2, camera.Near,
                    (pos, norm, uv, color) => { newv02 = new Vertex(pos, norm, color, uv); });
                // 新三角形 v0-v02-v12;
                DrawTriangle(transMat, v0, newv02, newv12, mat.Texture, w, h, rt, lights, handler);
                // 新三角形 v0-v12-v1;
                DrawTriangle(transMat, v0, newv12, v1, mat.Texture, w, h, rt, lights, handler);
            }
            else if (outsideNum == 2)
            {
                Vector3 newv0, newv1, newv2;
                // 重排顶点顺序
                Shape.Triangle.SortVertexByViewInfo(false, viewPos0, viewPos1, viewPos2,
                    p0Inside, p1Inside, p2Inside, ref v0, ref v1, ref v2, out newv0, out newv1, out newv2);
                // v0-v1,插值计算v01;
                MathUtil.IntersectVertexByDist(v0, v1, newv0, newv1, camera.Near,
                    (pos, norm, uv, color) => { v1 = new Vertex(pos, norm, color, uv); });
                // v0-v2,插值计算v02;
                MathUtil.IntersectVertexByDist(v0, v2, newv0, newv2, camera.Near,
                    (pos, norm, uv, color) => { v2 = new Vertex(pos, norm, color, uv); });
                // 新三角形 v0-v1-v2;
                DrawTriangle(transMat, v0, v1, v2, mat.Texture, w, h, rt, lights, handler);
            }
            else
            {
                // 无需裁剪，三角形 v0-v1-v2;
                DrawTriangle(transMat, v0, v1, v2, mat.Texture, w, h, rt, lights, handler);
            }
        }
        #endregion DrawTriangle

        #region Pragmas
        private static void _PixelPragma(Texture tex, int y,
            Vertex2D pa, Vertex2D pb, Vertex2D pc, Vertex2D pd, AddPixelHandler handler)
        {
            // 计算y位置上横向扫描线的起始和结束;
            SoftDevice device = SoftDevice.Default;
            if (null == device)
            {
                return;
            }
            HorizonScanLine scanLine = default(HorizonScanLine);
            if (!HorizonScanLine.InterpolateWithVertex2D(y, pa, pb, pc, pd, LightUtil.enableLightings, ref scanLine))
            {
                return;
            }
            int leftX = scanLine.left;
            int rightX = scanLine.right;
            // 有可能出现pb.x > pd.x的情况，这里做下容错;
            if (leftX > rightX)
            {
                scanLine.SwapLeftRight();
            }
            // 开始横向扫描生成像素点;
            bool usePerZ = device.PerspectiveCorrection;
            float invxLength = scanLine.InvLength;
            Vector2 uv = Vector2.zero;
            Color surfaceColor = Color.Zero;
            for (var x = leftX; x < rightX; x++)
            {
                float gradient = (float)(x - leftX) * invxLength;
                if (gradient < 0) { gradient = 0; }
                else if (gradient > 1) { gradient = 1; }
                float z = scanLine.leftZ + (scanLine.rightZ - scanLine.leftZ) * gradient;
                // 提前做ztest;
                if (!device.ZTest(x, y, z))
                {
                    continue;
                }
                float invZ = 0f;
                if (z <= MathUtil.Epsilon && z >= -MathUtil.Epsilon)
                {
                    invZ = 1f;
                }
                else
                {
                    invZ = 1f / z;
                }
                // 纹理映射;
                if (MaterialManager.enableTextures)
                {
                    if (null != tex)
                    {
                        uv = scanLine.leftUV + (scanLine.rightUV - scanLine.leftUV) * gradient;
                        // 这里使用透视修正后，离的足够近也会出问题;
                        if (usePerZ)
                        {
                            surfaceColor = tex.GetColor(uv.x * invZ, uv.y * invZ);
                        }
                        else
                        {
                            surfaceColor = tex.GetColor(uv.x, uv.y);
                        }
                    }
                }
                else
                {
                    surfaceColor = scanLine.leftColor + (scanLine.rightColor - scanLine.leftColor) * gradient;
                }
                if (LightUtil.enableLightings)
                {
                    Color diffuse = MathUtil.Interpolate(scanLine.leftDiffuse, scanLine.rightDiffuse, gradient);
                    if (usePerZ)
                    {
                        diffuse *= invZ;
                    }
                    surfaceColor = Color.Multiply(surfaceColor, diffuse);
                }
                if (null != handler)
                {
                    handler(x, y, z, surfaceColor);
                }
            }
        }
        private static void _VertexPragma(Texture tex, Vertex2D p1, Vertex2D p2, Vertex2D p3, AddPixelHandler handler)
        {
            // 检测顶点序列并进行排序;
            if(!Shape.Triangle.CheckAndSortVertexSequence(ref p1, ref p2, ref p3))
            {
                return;
            }
            // 排序后p1总在上方,p3总在最下方,也就是说扫描时是从[p1.y-p3.y]生成横向扫描线;
            // 接下来需要判断p2在p1的左面还是右面;
            float dP1P2, dP1P3 = 0;
            if (p2.y - p1.y > 0)
            {
                dP1P2 = (float)(p2.x - p1.x) / (float)(p2.y - p1.y);
            }
            else
            {
                dP1P2 = 0;      // p1和p2在同一水平线;
            }
            if (p3.y - p1.y > 0)
            {
                dP1P3 = (float)(p3.x - p1.x) / (float)(p3.y - p1.y);
            }
            else
            {
                dP1P3 = 0;
            }
            // 开始进行纵向扫描;
            if (dP1P2 > dP1P3)
            {
                // 在右侧时的情况，横向扫描线从p1-p3进入;
                // P1                                   ;
                // |\                                   ;
                // |_\p2                                ;
                // | /                                  ;
                // |/                                   ;
                // p3                                   ;
                Parallel.For(p1.y, p3.y + 1, y =>
                {
                    if (y < p2.y)
                    {
                        // 横向扫描上半部分，扫描线从p1-p2离开;
                        _PixelPragma(tex, y, p1, p3, p1, p2, handler);
                    }
                    else
                    {
                        // 横向扫描下半部分，扫描线从p2-p3离开;
                        _PixelPragma(tex, y, p1, p3, p2, p3, handler);
                    }
                });
            }
            else
            {
                // 在左侧时的情况;
                //    P1         ;
                //    /|         ;
                // p2/_|         ;
                //   \ |         ;
                //    \|         ;
                //    p3         ;
                Parallel.For(p1.y, p3.y + 1, y =>
                {
                    if (y < p2.y)
                    {
                        // 横向扫描上半部分，扫描线从p1-p2进入，从p1-p3离开;
                        _PixelPragma(tex, y, p1, p2, p1, p3, handler);
                    }
                    else
                    {
                        // 横向扫描上半部分，扫描线从p2-p3进入，从p1-p3离开;
                        _PixelPragma(tex, y, p2, p3, p1, p3, handler);
                    }
                });
            }
        }
        #endregion Pragmas
    }
}
