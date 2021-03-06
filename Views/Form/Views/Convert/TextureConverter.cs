﻿using System.ComponentModel;
using System.Globalization;
using Views.Form.Materials;

namespace Views.Form.Views.Convert
{
    public class TextureConverter : ExpandableObjectConverter
    {
        // 如果 destinationType 参数与使用此类型转换器的类（示例中的 SpellingOptions 类）的类型相同，则覆盖 CanConvertTo 方法并返回 true ；否则返回基类 CanConvertTo 方法的值。 
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                           System.Type destinationType)
        {
            if (destinationType == typeof(Texture))
                return true;
            return base.CanConvertTo(context, destinationType);
        }
        // 覆盖 ConvertTo 方法，并确保 destinationType 参数是一个 String ，并且值的类型与使用此类型转换器的类（示例中的 SpellingOptions 类）相同。
        // 如果其中任一情况为 false ，都将返回基类 ConvertTo 方法的值；否则，返回值对象的字符串表示。
        // 字符串表示需要使用唯一分隔符将类的每个属性隔开。
        // 由于整个字符串都将显示在 PropertyGrid 中，因此需要选择一个不会影响可读性的分隔符，逗号的效果通常比较好。 
        public override object ConvertTo(ITypeDescriptorContext context, 
            CultureInfo culture, object value, System.Type destinationType)
        {
            return base.ConvertTo(context, culture, value, destinationType);
        }
        // 通过指定类型转换器可以从字符串进行转换，您可以启用网格中对象字符串表示的编辑。
        // 要执行此操作，首先需要覆盖 CanConvertFrom 方法并返回 true （如果源 Type 参数为 String 类型）；否则，返回基类 CanConvertFrom 方法的值。
        public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)   
        {  
             if (sourceType == typeof(string))  
                 return true;  
             return base.CanConvertFrom(context, sourceType);  
        }
        // 要启用对象基类的编辑，同样需要覆盖 ConvertFrom 方法并确保值参数是一个 String 。
        // 如果不是 String ，将返回基类 ConvertFrom 方法的值；否则，返回基于值参数的类（示例中的 SpellingOptions 类）的新实例。
        // 您需要根据值参数解析类的每个属性的值。了解在 ConvertTo 方法中创建的分隔字符串的格式将有助于您的解析。 
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return base.ConvertFrom(context, culture, value);
        }
        // 覆盖 GetStandardValuesSupported 方法并返回 true ，表示此对象支持可以从列表中选取的一组标准值。
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }  
        // 覆盖 GetStandardValues 方法并返回填充了标准值的 StandardValuesCollection 。
        // 创建 StandardValuesCollection 的方法之一是在构造函数中提供一个值数组。
        // 对于选项窗口应用程序，您可以使用填充了建议的默认文件名的 String 数组。   
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return base.GetStandardValues(context);
        }
        //// 如果希望用户能够键入下拉列表中没有包含的值，请覆盖 GetStandardValuesExclusive 方法并返回 false 。
        //// 这从根本上将下拉列表样式变成了组合框样式。
        //public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        //{
        //    return false;
        //}
    }
}
