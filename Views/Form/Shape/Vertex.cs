using Framework.Maths;
using System.ComponentModel;
using Views.Form.Views.Convert;

namespace Views.Form.Shape
{
    public struct Vertex : IVertex
    {
#region Constructor
        public Vertex(Vector3 p, Vector3 n, Color c)
        {
            position = p;
            normal = n;
            color = c;
            uv0 = Vector2.zero;
            worldPosition = Vector3.zero;
            worldNormal = Vector3.zero;
        }
        public Vertex(Vector3 p, Vector3 n, Color c, Vector2 _uv0)
        {
            position = p;
            normal = n;
            color = c;
            uv0 = _uv0;
            worldPosition = Vector3.zero;
            worldNormal = Vector3.zero;
        }
        public Vertex(Vertex oth)
        {
            position = oth.position;
            normal = oth.normal;
            color = oth.color;
            uv0 = oth.uv0;
            worldPosition = oth.worldPosition;
            worldNormal = oth.worldNormal;
        }
#endregion Constructor

#region Methods
        public void SetColor(Color c)
        {
            color = c;
        }
        public void TransliteToWorld(Matrix4 worldMat)
        {
            // 顶点信息变换到世界空间
            worldPosition = worldMat * position;
            worldNormal = worldMat * normal;
            //worldNormal = Vector3.Normalize(Matrix4.Inverse(worldMat).Transpose() * normal);
        }
#endregion Methods

#region Attributes
        [TypeConverterAttribute(typeof(MathConverter)),
            EditorAttribute(typeof(MathUITypeEditor),
            typeof(System.Drawing.Design.UITypeEditor))]
        public Vector3 Position 
        { 
            set { position = value; }
            get { return position; }
        }

        [TypeConverterAttribute(typeof(MathConverter)),
            EditorAttribute(typeof(MathUITypeEditor),
            typeof(System.Drawing.Design.UITypeEditor))]
        public Vector3 Normal
        { 
            set { normal = value; } 
            get { return normal; } 
        }
        public Color Color
        { 
            set { color = value; } 
            get { return color; } 
        }

        [TypeConverterAttribute(typeof(MathConverter)),
            EditorAttribute(typeof(MathUITypeEditor),
            typeof(System.Drawing.Design.UITypeEditor))]
        public Vector2 UV0
        { 
            set { uv0 = value; } 
            get { return uv0; } 
        }
#endregion Attributes

#region Fields
        public readonly static Vertex Origin =
            new Vertex(Vector3.zero, Vector3.up, Color.White);
        public Vector3 position;
        public Vector3 normal;
        public Color color;
        public Vector2 uv0;
        public Vector3 worldPosition;
        public Vector3 worldNormal;
#endregion Fields
    }
}
