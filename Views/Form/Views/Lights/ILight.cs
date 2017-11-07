using Framework.Maths;

namespace Views.Form.Views.Lights
{
    public interface ILight
    {
        string Name { set; get; }
        Color LightColor { set; get; }  // a为强度;
        bool Enable { set; get; }
    }
    public interface IPosLight : ILight
    {
        Vector3 Position { set; get; }
    }
}
