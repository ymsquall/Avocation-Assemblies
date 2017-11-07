using Framework.Maths;
using Views.Form.Materials;

namespace Views.Form.Shape
{
    public struct HorizonScanLine
    {
        #region Methods
        // 插值计算横向扫描线的两个端点;
        public static bool InterpolateWithVertex2D(float y, Vertex2D pa, Vertex2D pb, Vertex2D pc, Vertex2D pd, bool enableLightings, ref HorizonScanLine scanLine)
        {
            // 扫描线从pa-pb进入，从pc-pd离开;
            // 如果pa和pb在同一水平线上,则扫描线起始点为pb.x;
            var gradient1 = pa.y != pb.y ? (float)(y - pa.y) / (float)(pb.y - pa.y) : 1;
            // 如果pc和pd在同一水平线上，则扫描线的结束点为pd.x;
            var gradient2 = pc.y != pd.y ? (float)(y - pc.y) / (float)(pd.y - pc.y) : 1;
            // 插值计算扫描线的端点x坐标;
            scanLine.left = MathUtil.Interpolate(pa.x, pb.x, gradient1);
            scanLine.right = MathUtil.Interpolate(pc.x, pd.x, gradient2);
            if (scanLine.left == scanLine.right)
            {
                // 扫描线端点x坐标重合;
                return false;
            }
            // 深度插值;
            scanLine.leftZ = MathUtil.Interpolate(pa.depth, pb.depth, gradient1);
            scanLine.rightZ = MathUtil.Interpolate(pc.depth, pd.depth, gradient2);
            // 光照插值;
            if (enableLightings)
            {
                Color paLightColor, pbLightColor, pcLightColor, pdLightColor;
                paLightColor = pa.diffuse + pa.specular;
                pbLightColor = pb.diffuse + pb.specular;
                pcLightColor = pc.diffuse + pc.specular;
                pdLightColor = pd.diffuse + pd.specular;
                scanLine.leftDiffuse = MathUtil.Interpolate(paLightColor, pbLightColor, gradient1);
                scanLine.rightDiffuse = MathUtil.Interpolate(pcLightColor, pdLightColor, gradient2);
            }
            // 纹理坐标插值;
            scanLine.leftUV = MathUtil.Interpolate(pa.uv0, pb.uv0, gradient1);
            scanLine.rightUV = MathUtil.Interpolate(pc.uv0, pd.uv0, gradient2);
            // 顶点颜色插值;
            if (!MaterialManager.enableTextures)
            {
                // 只有在禁用贴图时才需要插值顶点颜色;
                scanLine.leftColor = MathUtil.Interpolate(pa.color, pb.color, gradient1);
                scanLine.rightColor = MathUtil.Interpolate(pc.color, pd.color, gradient2);
            }
            return true;
        }
        // 交换起点和终点;
        public void SwapLeftRight()
        {
            // x
            int tmp = left; left = right; right = tmp;
            // z;
            float ftmp = leftZ; leftZ = rightZ; rightZ = ftmp;
            // light;
            Color ctmp = leftDiffuse; leftDiffuse = rightDiffuse; rightDiffuse = ctmp;
            // color;
            ctmp = leftColor; leftColor = rightColor; rightColor = ctmp;
            // tex;
            Vector2 v2tmp = leftUV; leftUV = rightUV; rightUV = v2tmp;
        }
        #endregion Methods

        #region Attributes
        public int Length
        {
            get
            {
                return right - left;
            }
        }
        public float InvLength
        {
            get
            {
                float result = right - left;
                if(result <= MathUtil.Epsilon && result >= -MathUtil.Epsilon)
                {
                    return 1f;
                }
                return 1f / result;
            }
        }
        #endregion Attributes

        #region Fields
        public int left, right;         // 起点终点的x坐标
        public float leftZ, rightZ;
        public Vector2 leftUV, rightUV;
        public Color leftColor, rightColor, leftDiffuse, rightDiffuse;
        #endregion Fields
    }
}
