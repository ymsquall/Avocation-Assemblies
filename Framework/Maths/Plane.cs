using System;

namespace Framework.Maths
{
    public partial struct Plane
    {
        // ax+by+cz+d=0 in plane;
        // ax+by+cz+d<0 outside;
        // ax+by+cz+d>0 inside;
        #region Constructor
        public Plane(Plane plane)
        {
            a = plane.a; b = plane.b; c = plane.c; d = plane.d;
        }
        public Plane(float _a, float _b, float _c, float _d)
        {
            a = _a; b = _b; c = _c; d = _d;
        }
        #endregion Constructor

        #region Methods
        public static Plane PlaneFromPoints(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            Vector3 vec21 = p1 - p2;
            Vector3 vec31 = p1 - p3;
            Vector3 norm = Vector3.Normalize(Vector3.Cross(vec21, vec31));
            Vector4 vecDot = PlaneDotNormal(norm, p1);

            return new Plane(norm.x, norm.y, norm.z, -vecDot.w);
        }
        public static Vector4 PlaneDotNormal(Vector3 p, Vector3 v)
        {
            return new Vector4(
                p.x * v.x + p.y * v.y + p.z * v.z,
                p.x * v.x + p.y * v.y + p.z * v.z,
                p.x * v.x + p.y * v.y + p.z * v.z,
                p.x * v.x + p.y * v.y + p.z * v.z);
        }
        public static Vector4 PlaneDotCoord(Vector4 p, Vector3 v)
        {
            return new Vector4(
                p.x * v.x + p.y * v.y + p.z * v.z + p.w * 1f,
                p.x * v.x + p.y * v.y + p.z * v.z + p.w * 1f,
                p.x * v.x + p.y * v.y + p.z * v.z + p.w * 1f,
                p.x * v.x + p.y * v.y + p.z * v.z + p.w * 1f);
        }
        public static Plane PlaneNormalize(Plane plane)
        {
            float lenSqrt = plane.a * plane.a + plane.b * plane.b + plane.c * plane.c;
            lenSqrt = (float)Math.Sqrt(lenSqrt);
            float rl = 1.0f / MathUtil.NotBeZero(lenSqrt);
            return new Plane(plane.a * rl, plane.b * rl, plane.c * rl, plane.d * rl);
        }
        //C#直线与平面的交点坐标计算函数;
        public Vector3 IntersectLine(Vector3 p1, Vector3 p2)
        {
            Vector3 dir = Vector3.Normalize(p2 - p1);
            Vector3 ret = p2;
            float fac = (a * dir.x + b * dir.y + c * dir.z);
            if (MathUtil.IsZero(fac))    
            {
                // 直线与平面平行 ;
                return ret;
            }
            // 系数比值 t=-(Apx+Bpy+Cpz+D)/(A*dx+B*dy+C*dz);
            float t = -(a * p1.x + b * p1.y + c * p1.z + d) / MathUtil.NotBeZero(fac);
            ret = p1 + dir * t;
            return ret;
        }
        public bool IsPointOutside(Vector3 p)
        {
            return IsPointOutside(p.x, p.y, p.z);
        }
        public bool IsPointOutside(float x, float y, float z)
        {
            // 点在平面外侧(法线方向相反侧);
            return a * x + b * y + c * z + d < 0f;
        }
        // 返回点到平面的距离;
        public float Point2PlaneDist(Vector3 p)
        {
            return Point2PlaneDist(p.x, p.y, p.z);
        }
        public float Point2PlaneDist(float x, float y, float z)
        {
            float length = (float)Math.Sqrt(a * a + b * b + c * c);
            return (a * x + b * y + c * z + d) / MathUtil.NotBeZero(length);
        }
        // 点在盒子内;
        public static bool PointInBox(Plane[] planes, Vector3 p)
        {
            if (null == planes)
            {
                return false;
            }
            int planeLength = planes.Length;
            for (int i = 0; i < planeLength; ++i)
            {
                if (planes[i].IsPointOutside(p))
                {
                    // 点在任意平面外侧，则这个点一定在盒子外侧;
                    return false;
                }
            }
            // 在盒子内侧或落在墙上;
            return true;
        }
        public static bool GeometryInBox(Plane[] planes, Vector3[] points, ref bool completelyInSide/*完全在内部*/)
        {
            if (null == planes)
            {
                return false;
            }
            int insidePointNum = 0;
            int planeLength = planes.Length;
            for (int i = 0; i < planeLength; i++)
            {
                int j = points.Length;
                bool allPtInPlane = true;
                Plane plane = planes[i];
                Vector3 planePos = new Vector3(plane.a, plane.b, plane.c) * plane.d;
                // 分别判断各个点是否在盒子内部;
                int pointLength = points.Length;
                for (int p = 0; p < pointLength; ++p)
                {
                    Vector3 pt = points[p];
                    if (plane.IsPointOutside(pt))
                    {
                        allPtInPlane = false;
                        j--;
                    }
                }
                // 无交集,说明整个几何体在盒子外部（整个裁剪掉，不参与绘制）;
                if (0 == j)
                {
                    return false;
                }
                // 记录在内部的点的数量;
                if (allPtInPlane)
                {
                    ++insidePointNum;
                }
            }
            // 如果所有点都在盒子内，则说明整个几何体在盒子内（不需要裁剪，直接绘制）;
            completelyInSide = insidePointNum == planes.Length;
            return true;
        }
        // 线段是否在盒子内，是否与墙壁相交;
        public static bool LineInBox(Plane[] planes, Vector3 p1, Vector3 p2, ref bool completelyInSide)
        {
            return GeometryInBox(planes, new Vector3[] { p1, p2 }, ref completelyInSide);
        }
        // 三角形是否在盒子内，是否与墙壁相交;
        public static bool TriangleInBox(Plane[] planes, Vector3 p1, Vector3 p2, Vector3 p3, ref bool completelyInSide)
        {
            return GeometryInBox(planes, new Vector3[] { p1, p2, p3 }, ref completelyInSide);
        }
        // 盒子是否在盒子内，是否与墙壁相交,一般用作包围盒相交检测;
        public static bool CubeInBox(Plane[] planes, Vector3 p, float size, ref bool completelyInSide)
        {
            float xMax = p.x + size;
            float xMin = p.x - size;
            float yMax = p.y + size;
            float yMin = p.y - size;
            float zMax = p.z + size;
            float zMin = p.z - size;

            _testInBoxCube[0].x = xMin;
            _testInBoxCube[0].y = yMin;
            _testInBoxCube[0].z = zMin;

            _testInBoxCube[1].x = xMax;
            _testInBoxCube[1].y = yMin;
            _testInBoxCube[1].z = zMin;

            _testInBoxCube[2].x = xMin;
            _testInBoxCube[2].y = yMax;
            _testInBoxCube[2].z = zMin;

            _testInBoxCube[3].x = xMax;
            _testInBoxCube[3].y = yMax;
            _testInBoxCube[3].z = zMin;

            _testInBoxCube[4].x = xMin;
            _testInBoxCube[4].y = yMin;
            _testInBoxCube[4].z = zMax;

            _testInBoxCube[5].x = xMax;
            _testInBoxCube[5].y = yMin;
            _testInBoxCube[5].z = zMax;

            _testInBoxCube[6].x = xMin;
            _testInBoxCube[6].y = yMin;
            _testInBoxCube[6].z = zMax;

            _testInBoxCube[7].x = xMax;
            _testInBoxCube[7].y = yMax;
            _testInBoxCube[7].z = zMax;

            return GeometryInBox(planes, _testInBoxCube, ref completelyInSide);
        }
        #endregion Methods

        #region Attributes
        public Vector3 Normal
        {
            get { return Vector3.Normalize(new Vector3(a, b, c)); }
        }
        #endregion Attributes

        #region Fields
        private static Vector3[] _testInBoxCube = new Vector3[8];
        public float a, b, c, d;
        #endregion Fields
    }
}
