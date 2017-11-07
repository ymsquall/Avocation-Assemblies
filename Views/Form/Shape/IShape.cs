using Framework.Maths;
using System.Collections.Generic;
using Views.Form.Views;
using Views.Form.Views.Lights;

namespace Views.Form.Shape
{
    public interface IShape
    {
        Matrix4 BuildWorldMatrix();
        void CollectVertex(Matrix4 transMat, int w, int h, RenderType rt, List<IPosLight> lights, AddPixelHandler handler);

        string Name { set; get; }
        bool Enabled { set; get; }
        string MaterialName { set; get; }
        bool Focus { set; get; }
        Vector3 Origin { set; get; }
        Vector3 Rotation { set; get; }
        Vector3 Scale { set; get; }
        Vector3 RotaAxis { set; get; }
        Vertex this[int i] { set; get; }
        IEnumerator<Vertex> Points { get; }
        Matrix4 WorldMatrix { set; get; }
    }
}
