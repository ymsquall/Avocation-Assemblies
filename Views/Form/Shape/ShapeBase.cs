using System.Collections.Generic;
using System.ComponentModel;
using Views.Form.Views.Convert;
using Framework.Maths;
using Views.Form.Views;
using Views.Form.Views.Lights;

namespace Views.Form.Shape
{
    public delegate bool ZTestHandler(int x, int y, float depth);
    public delegate void AddPixelHandler(int x, int y, float zBuff, Color c);
    public class ShapeBase : IShape
    {
        #region Methods
        protected bool _HandleOnePoint(Matrix4 transformMat, int w, int h, AddPixelHandler handler)
        {
            int pointCount = _pointList.Count;
            if (pointCount < 2)
            {
                // 只有一个点就画一个点;
                for (int i = 0; i < pointCount; ++i)
                {
                    Vertex vertex = _pointList[i];
                    // 将3d坐标投影到2d空间;
                    Vertex2D point = GraphPragma.ProjectToScreen(vertex.Position, transformMat, (float)w, (float)h);
                    point.color = vertex.Color;
                    // 添加像素点;
                    if (null != handler)
                    {
                        handler(point.x, point.y, 0, vertex.Color);
                    }
                }
                return true;
            }
            return false;
        }
        public Matrix4 BuildWorldMatrix()
        {
            _worldMatrix = Matrix4.MakeScale(Scale) *
                Matrix4.MakeRotationPYR(Rotation.x, Rotation.y, Rotation.z) *
                Matrix4.MakeTrans(Origin);
            int pointCount = _pointList.Count;
            for (int i = 0; i < pointCount; ++i)
            {
                Vertex vertex = _pointList[i];
                vertex.TransliteToWorld(_worldMatrix);
                _pointList[i] = vertex;
            }
            return _worldMatrix;
        }
        public void CollectVertex(Matrix4 transMat, int w, int h, RenderType rt, List<IPosLight> lights, AddPixelHandler handler)
        {
            if (_HandleOnePoint(transMat, w, h, handler))
            {
                return;
            }
            // 多于一个点就可以连线了;
            _CollectMesh(transMat, w, h, rt, lights, handler);
        }
        protected virtual void _CollectMesh(Matrix4 transMat, int w, int h, RenderType rt, List<IPosLight> lights, AddPixelHandler handler)
        {
            Camera camera = Camera.Default;
            if(null == camera)
            {
                return;
            }
            Vertex vertex1, vertex2;
            Vertex2D point1, point2;
            int pointCount = _pointList.Count - 1;
            for (int i = 0; i < pointCount; ++i)
            {
                vertex1 = _pointList[i];
                vertex2 = _pointList[i + 1];
                // 将3d坐标投影到2d空间;
                point1 = GraphPragma.BuildVertex2D(vertex1, transMat, camera.Pos, lights, w, h);
                point2 = GraphPragma.BuildVertex2D(vertex2, transMat, camera.Pos, lights, w, h);
                // 计算线段;
                Line.DrawLine(point1, point2, handler, DrawLineType.Bresenham, SoftDevice.Default.ZTest);
            }
            vertex1 = _pointList[_pointList.Count - 1];
            vertex2 = _pointList[0];
            point1 = GraphPragma.BuildVertex2D(vertex1, transMat, camera.Pos, lights, w, h);
            point2 = GraphPragma.BuildVertex2D(vertex2, transMat, camera.Pos, lights, w, h);
            Line.DrawLine(point1, point2, handler, DrawLineType.Bresenham, SoftDevice.Default.ZTest);
        }
        #endregion Methods

        #region Attributes
        [Browsable(false)]
        public string Name { set; get; }
        public bool Enabled
        { 
            set { _enabled = value; } 
            get { return _enabled; } 
        }
        public bool Focus { set; get; }
        public string MaterialName 
        { 
            set { _materialName = value; } 
            get { return _materialName; } 
        }

        [TypeConverterAttribute(typeof(MathConverter)),
            EditorAttribute(typeof(MathUITypeEditor),
            typeof(System.Drawing.Design.UITypeEditor))]
        public Vector3 Origin 
        { 
            set { _origin = value; } 
            get { return _origin; }
        }

        [TypeConverterAttribute(typeof(MathConverter)),
            EditorAttribute(typeof(MathUITypeEditor),
            typeof(System.Drawing.Design.UITypeEditor))]
        public Vector3 Rotation
        { 
            set { _rotation = value; } 
            get { return _rotation; }
        }

        [TypeConverterAttribute(typeof(MathConverter)),
            EditorAttribute(typeof(MathUITypeEditor),
            typeof(System.Drawing.Design.UITypeEditor))]
        public Vector3 Scale
        { 
            set { _scale = value; } 
            get { return _scale; }
        }
        [TypeConverterAttribute(typeof(MathConverter)),
            EditorAttribute(typeof(MathUITypeEditor),
            typeof(System.Drawing.Design.UITypeEditor))]
        public Vector3 RotaAxis 
        {
            set { _rotaAxis = value; }
            get { return _rotaAxis; }
        }
        public Vertex this[int i]
        {
            set
            {
                if (null == _pointList)
                {
                    return;
                }
                if (i < 0 || i >= _pointList.Count)
                {
                    return;
                }
                _pointList[i] = value;
            }
            get
            {
                if (i < 0 || i >= _pointList.Count)
                {
                    return Vertex.Origin;
                }
                return _pointList[i];
            }
        }
        [Browsable(false)]
        public IEnumerator<Vertex> Points
        {
            get
            {
                return _pointList.GetEnumerator();
            }
        }
        [Browsable(false)]
        public Matrix4 WorldMatrix 
        { 
            set { _worldMatrix = value; } 
            get { return _worldMatrix; }
        }
        #endregion Attributes

        #region Fields
        protected List<Vertex> _pointList = new List<Vertex>();
        protected Vector3 _origin = Vector3.zero;
        protected Vector3 _rotation = Vector3.zero;
        protected Vector3 _scale = Vector3.one;
        protected Vector3 _rotaAxis = Vector3.zero;
        protected bool _enabled = true;
        protected string _materialName = string.Empty;
        protected Matrix4 _worldMatrix;
        #endregion Fields
    }
}
