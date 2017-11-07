using Framework.Maths;
using System;

namespace Framework.Tools
{
    public class TwoData<T1, T2>
    {
        T1 first;
        T2 second;
        public TwoData(T1 f, T2 s)
        {
            first = f;
            second = s;
        }
        public T1 First { set { first = value; } get { return first; } }
        public T2 Second { set { second = value; } get { return second; } }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            TwoData<T1, T2> oth = obj as TwoData<T1, T2>;
            if (oth == null)
                return false;
            if (!first.Equals(oth.first))
                return false;
            if (!second.Equals(oth.second))
                return false;
            return true;
        }
        public override int GetHashCode()
        {
            int hashCode = 0;
            hashCode ^= first.GetHashCode();
            hashCode ^= second.GetHashCode();
            return hashCode;
        }
    }
    public class ThreeData<T1, T2, T3>
    {
        T1 begin;
        T2 middle;
        T3 end;
        public ThreeData(T1 b, T2 m, T3 e)
        {
            begin = b;
            middle = m;
            end = e;
        }
        public T1 Bgein { set { begin = value; } get { return begin; } }
        public T2 Middle { set { middle = value; } get { return middle; } }
        public T3 End { set { end = value; } get { return end; } }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            ThreeData<T1, T2, T3> oth = obj as ThreeData<T1, T2, T3>;
            if (oth == null)
                return false;
            if (!begin.Equals(oth.begin))
                return false;
            if (!middle.Equals(oth.middle))
                return false;
            if (!end.Equals(oth.end))
                return false;
            return true;
        }
        public override int GetHashCode()
        {
            int hashCode = 0;
            hashCode ^= begin.GetHashCode();
            hashCode ^= middle.GetHashCode();
            hashCode ^= end.GetHashCode();
            return hashCode;
        }
    }

    public enum WrapMode
    {
        Default = 0,
        Once = 1,
        Clamp = 1,
        Loop = 2,
        PingPong = 4,
        ClampForever = 8
    }
    public class WrapUtils
    {
        public static float WrapValue(float v, float start, float end, WrapMode wMode)
        {
            switch (wMode)
            {
                case WrapMode.Clamp:
                case WrapMode.ClampForever:
                    return MathUtil.Clamp(v, start, end);
                case WrapMode.Default:
                case WrapMode.Loop:
                    return MathUtil.Repeat(v, end - start) + start;
                case WrapMode.PingPong:
                    return MathUtil.PingPong(v, end - start) + start;
                default:
                    return v;
            }
        }
    }
}
