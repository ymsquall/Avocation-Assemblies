using Framework.Maths;
using System.Collections.Generic;
using Views.Form.Views;
using Views.Form.Views.Lights;

namespace Views.Form.Shape
{
    public class Grids : ShapeBase
    {
        #region Constructor
        private Grids() { }
        #endregion Constructor

        #region Methods
        public static Grids CreateStandardGrids(int row, int col)
        {
            Grids result = new Grids();
            result.Name = "mesh_standardgrids";
            float leftX = -(float)col * 0.5f;
            float nearZ = -(float)row * 0.5f;

            for (int i = 0; i < row; ++i)
            {
                Vertex p0 = new Vertex(new Vector3(leftX, 0f, nearZ + (float)i), Vector3.up, Color.Gray);
                Vertex p1 = new Vertex(new Vector3(-leftX, 0f, nearZ + (float)i), Vector3.up, Color.Gray);
                result._pointList.Add(p0);
                result._pointList.Add(p1);
            }
            for (int i = 0; i < col; ++i)
            {
                Vertex p0 = new Vertex(new Vector3(leftX + (float)i, 0f, nearZ), Vector3.up, Color.Gray);
                Vertex p1 = new Vertex(new Vector3(leftX + (float)i, 0f, -nearZ), Vector3.up, Color.Gray);
                result._pointList.Add(p0);
                result._pointList.Add(p1);
            }
            return result;
        }
        protected override void _CollectMesh(Matrix4 transMat, int w, int h, RenderType rt, List<IPosLight> lights, AddPixelHandler handler)
        {
            if (_pointList.Count % 2 != 0)
            {
                return;
            }
            Camera camera = Camera.Default;
            if (null == camera)
            {
                return;
            }
            int pointCount = _pointList.Count - 1;
            for (int i = 0; i < pointCount; i += 2)
            {
                Vertex v0 = _pointList[i];
                Vertex v1 = _pointList[i + 1];
                Vertex2D p0 = GraphPragma.BuildVertex2D(v0, transMat, camera.Pos, lights, w, h);
                Vertex2D p1 = GraphPragma.BuildVertex2D(v1, transMat, camera.Pos, lights, w, h);
                Line.DrawLine(p0, p1, handler, DrawLineType.Bresenham, SoftDevice.Default.ZTest);
            }
        }
        #endregion Methods
    }
}
