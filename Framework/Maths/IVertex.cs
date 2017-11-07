namespace Framework.Maths
{
    public interface IVertex
    {
        Vector3 Position { set; get; }
        Vector3 Normal { set; get; }
        Vector2 UV0 { set; get; }
        Color Color { set; get; }
    }
}
