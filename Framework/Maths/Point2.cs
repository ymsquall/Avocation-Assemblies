using System;

namespace Framework.Maths
{
    public partial struct Point2
    {
        #region Constructor
        public Point2(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
        public Point2(double _x, double _y)
        {
            x = (int)_x;
            y = (int)_y;
        }
        public Point2(Point2 oth)
        {
            x = oth.X;
            y = oth.Y;
        }
        #endregion Constructor

        #region Attributes
        public float Length
        {
            get { return (float)Math.Sqrt(SqrLength); }
        }
        public float SqrLength
        {
            get { return x * x + y * y; }
        }
        public int X
        { 
            set { x = value; } 
            get { return x; }
        }
        public int Y
        { 
            set { y = value; } 
            get { return y; }
        }
        #endregion Attributes

        #region Fields
        public readonly static Point2 Zero = new Point2(0, 0);
        public readonly static Point2 One = new Point2(1, 1);
        public int x, y;
        #endregion Fields
    }
}
