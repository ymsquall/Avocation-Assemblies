namespace Framework.Maths.ColorSpace
{
    /* 颜色空间系列1: RGB和CIEXYZ颜色空间的转换及相关优化
    颜色空间系列代码下载链接：http://files.cnblogs.com/Imageshop/ImageInfo.rar （同文章同步更新）
    在颜色感知的研究中，CIE 1931 XYZ 色彩空间（也叫做 CIE 1931 色彩空间）是其中一个最先采用数学方式来定义的色彩空间，它由国际照明委员会（CIE）于1931年创立。CIE XYZ 色彩空间是从 1920 年代后期 W.David Wright (Wright 1928) 和 John Guild(Guild 1931) 做的一系列实验中得出的。他们的实验结果合并到了 CIE RGB 色彩空间的规定中，CIE XYZ 色彩空间再从它得出。
    更过具体的关于XYZ空间的理论解释可见：点击打开链接
    本文的重点是如何优化这个RGB<->XYZ相互转换的过程。
    从相关的文献包括OpenCv的文档中可找到两者的理论转换算式如下：
    [X]   [ 0.412453    0.357580    0.180423][R]
    [Y] = [ 0.212671    0.715160    0.072169][G]       (1)
    [Z]   [ 0.019334    0.119193    0.950227][B]

    [R]   [ 3.240479   -1.537150   -0.498535][X]
    [G] = [-0.969256    1.875992    0.041556][Y]       (2)
    [B]   [ 0.055648   -0.204043    1.057311][Z]
    仔细观察式（1），其中 X = 0.412453 * R + 0.412453 * G + 0.412453B ; 各系数相加之和为0.950456，非常接近于1，我们知道R/G/B的取值范围为[0, 255]，如果系数和等于1，则X的取值范围也必然在[0, 255] 之间，因此我们可以考虑等比修改各系数，使其之和等于1，这样就做到了XYZ和RGB在同等范围的映射，因此第一行的系数应分别修改为[0.412453    0.357580    0.180423]  / [0.950456] = [0.433953    0.376219    0.189828]。
    式（1）的第二行，三个系数之和恰为1，因此无需修正。
    式（1）的第三行，三个系数之和为1.088754，修正算式为[0.019334    0.119193    0.950227]  /  [1.088754] = [0.017758    0.109477    0.872765]
    由于式（1）的变化，式（2）必须做相应的调整，考虑式（1）关于X的各分量都除以了 0.950456，因此，只需在式2的对应分量上乘以  0.950456即可，同理，关于Z的各分量由于都除以了1.088754，式（2）各分量必须对应乘以1.088754。得到最终的变换式(3)(4)。

    [X]   [0.433953   0.376219   0.189828][R]
    [Y] = [0.212671   0.715160   0.072169][G]       （3）
    [Z]   [0.017758   0.109477   0.872765][B]

    [R]   [ 3.0799327  -1.537150  -0.542782 ][X]
    [G] = [-0.921235    1.875992   0.0452442][Y]     （4）
    [B]   [ 0.0528909  -0.204043   1.1511515][Z]

    如果有朋友查阅过OpenCv的RGB到LAB空间的转换，就可以发现Cv就是用的上述矩阵先将RGB转到XYZ，再由XYZ转为LAB的。
    由以上数式可以看出RGB和XYZ颜色空间的转换时线性的，因此，两个系数矩阵之间的成绩必为一个E矩阵（对角线为1，其他元素都为0），读者可以用matlab测试下。
    由于各小数的存在，理论上说，RGB颜色空间的颜色对应的XYZ分量的数值一般都为浮点数，之前说过经过调整系数矩阵后其有效范围在[0, 255] 之间，这和RGB的范围是一致的，因此我们更感兴趣的可能是用整数表示XYZ的值，此时，如果先用上述计算式计算，最后在用（int) 强制取整，则效率很低下，因此，很有必要做点的优化。
    优化的原理基本就是用整数的乘除法来替代浮点运算，比如，对各系数乘以一个很大的数，计算出结果在整除这个数，则得到的数字和之前的浮点算式取整结果是一致的。
    如何取放大系数，也有着一定的讲究，比如0.433953 ，很多朋友的第一反应应该是乘以1000000得到433953 ，不错，这是个很好的优化，却不是最好的，因为最后的整除1000000相对来说也是个慢的过程，如果我们能够整除一个2的N次幂数，则可以用整数的移位来代替整除。众所周知，移位的速度非常快。
    那这个N如何取呢，比方说取1可行吗，分析下马上得到的结果是绝对不行，因为很多系数乘以2再取整就变为0了。我对这个N的取值建议是在保证整个算式的每个部分的计算结果不超过int（对于64位CPU，则是long类型）类型的最大范围时，N越大越好。像我们这种情况，由于RGB的取值范围是[255]，因此N的取值最大只能是23。
    假定我们取N的值为20，则RGB转XYZ的算式可以写为如下：

    X = (Blue * 199049 + Green * 394494 + Red * 455033 + 524288) >> 20; // 这些系数是按照RGBLAB类里的labXr_32f放大2^20后得到的  
    Y = (Blue * 75675  + Green * 749900 + Red * 223002 + 524288) >> 20;  
    Z = (Blue * 915161 + Green * 114795 + Red * 18621  + 524288) >> 20;  //  这里无需验证结果是否在[0,255]之间，必然在。

    注意算式中的524288，这个值等于(2^20)/2，加上他的作用是使整个算式能够做到四舍五入。
    另外，还要注意各系数小数点后数字的累积，那X一行来说事，0.433953  * 2^20  =  455032.700928，我们取值455033 ，0.376219    * 2^20= 394494.214144 ，则取值394494 ，那么最后一个系数其实可以不用计算，直接拿 2^20-455033 -394494 =199049 。
    对应的XYZ转RGB空间算式为：

    Blue =  (X *  55460   - Y * 213955  + Z * 1207070) >> 20;  
    Green = (X * -965985  + Y * 1967119 + Z * 47442  ) >> 20;    // x * -965985 和 -x * 965985 在反汇编后是不一样的，后者多了个neg指令  
    Red =   (X *  3229543 - Y * 1611819 - Z * 569148 ) >> 20;  
    if (Red > 255) Red = 255; else if (Red < 0) Red = 0;      // 这里需要判断，因为RGB空间所有的颜色转换到XYZ后，并不是填充满了0-255的范围的，反转过去就会存在一些溢出的点。
    if (Green > 255) Green = 255; else if (Green < 0) Green = 0;  // 编译后比三目运算符的效率高  
    if (Blue > 255) Blue = 255; else if (Blue < 0) Blue = 0;  

    正如代码中的注释一样，XYZ-RGB的转换必须判断转换的颜色是否在有效范围内。
    另外对上述算式提一点点优化方面的是事情：

    Green = (X * -965985 + Y * 1967119 + Z * 47442) >> 20;       // x * -965985 和 -x * 965985 在反汇编后是不一样的，后者多了个neg指令  
    00000048  imul        ebx,edi,0FFF1429Fh   
    0000004e  imul        eax,dword ptr [ebp-10h],1E040Fh   
    00000055  add         ebx,eax   
    00000057  imul        eax,dword ptr [ebp-14h],0B952h   
    0000005e  add         ebx,eax   
    00000060  sar         ebx,14h
    另外一种写法：
    Green = (-X * 965985 + Y * 1967119 + Z * 47442) >> 20;       // x * -965985 和 -x * 965985 在反汇编后是不一样的，后者多了个neg指令  
    00000048  mov         ebx,edi   
    0000004a  neg         ebx   
    0000004c  imul        ebx,ebx,0EBD61h   
    00000052  imul        eax,dword ptr [ebp-10h],1E040Fh   
    00000059  add         ebx,eax   
    0000005b  imul        eax,dword ptr [ebp-14h],0B952h   
    00000062  add         ebx,eax   
    00000064  sar         ebx,14h
    */

    public unsafe partial struct CIEXYZ
    {
        #region Constructor
        // =524288，这个值等于(2^20)/2，加上他的作用是使整个算式能够做到四舍五入。
        private const int Coefficient = (int)((2 ^ 20) * 0.5);
        public CIEXYZ(Color color)
        {
            x = y = z = 0;
            alpha = color.a;
            CovnertFrom(color);
        }
        #endregion Constructor

        #region Methods
        private void CovnertFrom(Color color)
        {
            int from = color.ToRgb();
            int to = 0;
            RGBXYZ.ToXYZ((byte*)&from, (byte*)&to);
            FromXyz(to);
        }
        private Color CovnertTo()
        {
            int from = ToXyz();
            int to = 0;
            RGBXYZ.ToRGB((byte*)&from, (byte*)&to);
            Color ret = Color.FromRgb(to);
            ret.a = alpha;
            return ret;
        }
        public static CIEXYZ FromColor(Color color)
        {
            CIEXYZ ret = new CIEXYZ();
            ret.CovnertFrom(color);
            ret.alpha = color.a;
            return ret;
        }
        public static Color ToColor(CIEXYZ xyz)
        {
            Color ret = xyz.CovnertTo();
            ret.a = xyz.alpha;
            return ret;
        }
        public void FromXyz(int xyz)
        {
            x = 0xFF & xyz;
            y = 0xFF00 & xyz;
            y >>= 8;
            z = 0xFF0000 & xyz;
            z >>= 16;
        }
        public int ToXyz()
        {
            return (int)(((uint)x << 16) | (ushort)(((ushort)y << 8) | z));
        }
        #endregion Methods

        #region Attributes
        public int X { set { x = value; } get { return x; } }
        public int Y { set { y = value; } get { return y; } }
        public int Z { set { z = value; } get { return z; } }
        #endregion Attributes

        #region Fields
        public int x, y, z;
        public float alpha;
        #endregion Fields
    }
}
