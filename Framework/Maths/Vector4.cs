namespace Framework.Maths
{
    public partial struct Vector4
    {
        #region Constructor
        public Vector4(float _x, float _y, float _z, float _w)
        {
            x = _x;
            y = _y;
            z = _z;
            w = _w;
        }
        public Vector4(Vector4 oth)
        {
            x = oth.x;
            y = oth.y;
            z = oth.z;
            w = oth.w;
        }
        public Vector4(Vector3 oth)
        {
            x = oth.x;
            y = oth.y;
            z = oth.z;
            w = 1;
        }
        public Vector4(float scaler)
        {
            x = scaler;
            y = scaler;
            z = scaler;
            w = scaler;
        }
        #endregion Constructor

        #region Methods
        public float Dot(Vector4 vec)
	    {
		    return x * vec.x + y * vec.y + z * vec.z + w * vec.w;
	    }
	    public Vector3 XYZ()
	    {
            return new Vector3(x, y, z);
        }
        #endregion Methods

        #region Fields
        public float x, y, z, w;
        #endregion Fields
    }
}
