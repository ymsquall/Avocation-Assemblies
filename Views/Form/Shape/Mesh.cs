using Framework.Maths;
using System.Collections.Generic;

namespace Views.Form.Shape
{
    public class Mesh : ShapeBase
    {
        #region Constructor
        private Mesh() { }
        #endregion Constructor

        #region Methods
        public static Mesh CreateCube()
        {
            Mesh result = new Mesh();
            result.Name = "mesh_cube";
            /*                         8------9   
             *     6------5            |      |     
             *    /|     /|            |  顶  |
             *   1-+---2  |     1------2------5------6------12
             *   | |   |  |     |      |      |      |      |
             *   | /7  | /4     |  正  |  右  |  背  |  左  |
             *   0-----3        0------3------4------7------13
             *                         |      |
             *                         |  底  |
             *                        10------11
             */
            Vertex p0 = new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), Vector3.back, Color.Red, new Vector2(0, 0.6666f));
            Vertex p1 = new Vertex(new Vector3(-0.5f, 0.5f, -0.5f), Vector3.back, Color.Green, new Vector2(0, 0.3333f));
            Vertex p2 = new Vertex(new Vector3(0.5f, 0.5f, -0.5f), Vector3.back, Color.Blue, new Vector2(0.25f, 0.3333f));
            Vertex p3 = new Vertex(new Vector3(0.5f, -0.5f, -0.5f), Vector3.back, Color.Red, new Vector2(0.25f, 0.6666f));
            Vertex p4 = new Vertex(new Vector3(0.5f, -0.5f, 0.5f), Vector3.right, Color.Green, new Vector2(0.5f, 0.6666f));
            Vertex p5 = new Vertex(new Vector3(0.5f, 0.5f, 0.5f), Vector3.right, Color.Blue, new Vector2(0.5f, 0.3333f));
            Vertex p6 = new Vertex(new Vector3(-0.5f, 0.5f, 0.5f), Vector3.front, Color.Red, new Vector2(1f, 0.3333f));
            Vertex p7 = new Vertex(new Vector3(-0.5f, -0.5f, 0.5f), Vector3.front, Color.Green, new Vector2(1f, 0.6666f));
            Vertex p8 = new Vertex(new Vector3(-0.5f, 0.5f, -0.5f), Vector3.up, Color.Blue, new Vector2(0.25f, 0));
            Vertex p9 = new Vertex(new Vector3(-0.5f, 0.5f, 0.5f), Vector3.up, Color.Red, new Vector2(0.5f, 0));
            Vertex p10 = new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), Vector3.down, Color.Green, new Vector2(0.25f, 1));
            Vertex p11 = new Vertex(new Vector3(-0.5f, -0.5f, 0.5f), Vector3.down, Color.Blue, new Vector2(0.5f, 1));
            Vertex p12 = new Vertex(new Vector3(-0.5f, 0.5f, -0.5f), Vector3.left, Color.Red, new Vector2(1, 0.3333f));
            Vertex p13 = new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), Vector3.left, Color.Green, new Vector2(1, 0.6666f));
            result._pointList.Add(p0);
            result._pointList.Add(p1);
            result._pointList.Add(p2);
            result._pointList.Add(p3);
            result._pointList.Add(p4);
            result._pointList.Add(p5);
            result._pointList.Add(p6);
            result._pointList.Add(p7);
            result._pointList.Add(p8);
            result._pointList.Add(p9);
            result._pointList.Add(p10);
            result._pointList.Add(p11);
            result._pointList.Add(p12);
            result._pointList.Add(p13);
            result._indexList.AddRange(new int[]{
                0, 1, 2, 0, 2, 3,       // 左;
                3, 2, 5, 3, 5, 4,       // 正;
                4, 5, 6, 4, 6, 7,       // 右;
                7, 6, 12, 7, 12, 13,    // 背;
                2, 8, 9, 2, 9, 5,       // 顶;
                10, 3, 4, 10, 4, 11,    // 底;
            });
            return result;
        }
        public static Mesh CreateHexahedron()
        {
            Mesh result = new Mesh();
            result.Name = "mesh_hexahedron";
            // tri1
            Vector3[] triangle = new Vector3[]{
                new Vector3(-1f, 0f, -1f),
                new Vector3(0f, 1f, 0f),
                new Vector3(1f, 0f, -1f),
            };
            Vector3 norm = Plane.PlaneFromPoints(triangle[0], triangle[1], triangle[2]).Normal;
            Vertex p0 = new Vertex(triangle[0], norm, Color.Red, new Vector2(0, 0));
            Vertex p1 = new Vertex(triangle[1], norm, Color.Green, new Vector2(0.125f, 1));
            Vertex p2 = new Vertex(triangle[2], norm, Color.Blue, new Vector2(0.25f, 0));
            // tri2
            triangle[0] = new Vector3(0f, 0f, 1f);
            triangle[1] = new Vector3(0f, 1f, 0f);
            triangle[2] = new Vector3(-1f, 0f, -1f);
            norm = Plane.PlaneFromPoints(triangle[0], triangle[1], triangle[2]).Normal;
            Vertex p3 = new Vertex(triangle[0], norm, Color.Blue, new Vector2(0.125f, 1));
            Vertex p4 = new Vertex(triangle[1], norm, Color.Green, new Vector2(0.25f, 0));
            Vertex p5 = new Vertex(triangle[2], norm, Color.Red, new Vector2(0.375f, 1));
            // tri3
            triangle[0] = new Vector3(1f, 0f, -1f);
            triangle[1] = new Vector3(0f, 1f, 0f);
            triangle[2] = new Vector3(0f, 0f, 1f);
            norm = Plane.PlaneFromPoints(triangle[0], triangle[1], triangle[2]).Normal;
            Vertex p6 = new Vertex(triangle[0], norm, Color.Blue, new Vector2(0.25f, 0));
            Vertex p7 = new Vertex(triangle[1], norm, Color.Green, new Vector2(0.375f, 1));
            Vertex p8 = new Vertex(triangle[2], norm, Color.Blue, new Vector2(0.5f, 0));
            // tri4
            triangle[0] = new Vector3(1f, 0f, -1f);
            triangle[1] = new Vector3(0, -1f, 0);
            triangle[2] = new Vector3(-1f, 0f, -1f);
            norm = Plane.PlaneFromPoints(triangle[0], triangle[1], triangle[2]).Normal;
            Vertex p9 = new Vertex(triangle[0], norm, Color.Blue, new Vector2(0.375f, 1));
            Vertex p10 = new Vertex(triangle[1], norm, Color.Green, new Vector2(0.5f, 0));
            Vertex p11 = new Vertex(triangle[2], norm, Color.Red, new Vector2(0.625f, 1));
            // tri5
            triangle[0] = new Vector3(-1f, 0f, -1f);
            triangle[1] = new Vector3(0, -1f, 0);
            triangle[2] = new Vector3(0, 0f, 1f);
            norm = Plane.PlaneFromPoints(triangle[0], triangle[1], triangle[2]).Normal;
            Vertex p12 = new Vertex(triangle[0], norm, Color.Red, new Vector2(0.5f, 0));
            Vertex p13 = new Vertex(triangle[1], norm, Color.Green, new Vector2(0.625f, 1));
            Vertex p14 = new Vertex(triangle[2], norm, Color.Blue, new Vector2(1f, 0));
            // tri6
            triangle[0] = new Vector3(0, 0f, 1f);
            triangle[1] = new Vector3(0, -1f, 0);
            triangle[2] = new Vector3(1f, 0f, -1f);
            norm = Plane.PlaneFromPoints(triangle[0], triangle[1], triangle[2]).Normal;
            Vertex p15 = new Vertex(triangle[0], norm, Color.Blue, new Vector2(0.625f, 1));
            Vertex p16 = new Vertex(triangle[1], norm, Color.Green, new Vector2(1f, 0));
            Vertex p17 = new Vertex(triangle[2], norm, Color.Blue, new Vector2(0.875f, 1));
            result._pointList.Add(p0);
            result._pointList.Add(p1);
            result._pointList.Add(p2);
            result._pointList.Add(p3);
            result._pointList.Add(p4);
            result._pointList.Add(p5);
            result._pointList.Add(p6);
            result._pointList.Add(p7);
            result._pointList.Add(p8);
            result._pointList.Add(p9);
            result._pointList.Add(p10);
            result._pointList.Add(p11);
            result._pointList.Add(p12);
            result._pointList.Add(p13);
            result._pointList.Add(p14);
            result._pointList.Add(p15);
            result._pointList.Add(p16);
            result._pointList.Add(p17);
            result._indexList.AddRange(new int[]{
                0, 1, 2, 3, 4, 5,       
                6, 7, 8, 9, 10, 11,       
                12, 13, 14, 15, 16, 17,        
            });
            return result;
        }
        //protected override void _CollectMesh(Matrix4 transMat, int w, int h, RenderType rt, List<IPosLight> lights, AddPixelHandler handler)
        //{
        //    int indexCount = _indexList.Count - 2;
        //    for (int i = 0; i < indexCount; i += 3)
        //    {
        //        int idx1 = _indexList[i];
        //        int idx2 = _indexList[i + 1];
        //        int idx3 = _indexList[i + 2];
        //        GraphPragma.DrawTriangle(transMat, 
        //            _pointList[idx1], _pointList[idx2], _pointList[idx3],
        //            MaterialName, w, h, rt, lights, handler);
        //    }
        //}
        #endregion Methods

        #region Fields
        private List<int> _indexList = new List<int>();
        #endregion Fields
    }
}
