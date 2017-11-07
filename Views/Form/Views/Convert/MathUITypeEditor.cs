using System;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Framework.Maths;
using Framework.Logger;

namespace Views.Form.Views.Convert
{
    public class MathUITypeEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            if (context.Instance is Color)
            {
                return UITypeEditorEditStyle.Modal;
            }
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context,
            System.IServiceProvider provider, object value)
        {
            try
            {
                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (edSvc != null)
                {
                    if (value is Color)
                    {
                        Color result = Color.Black;
                        ColorDialog loColorForm = new ColorDialog();
                        DialogResult dr = loColorForm.ShowDialog();
                        if (dr == DialogResult.OK)
                        {
                            System.Drawing.Color loResultColor = loColorForm.Color;
                            result = loResultColor;
                        }
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSys.Popup("Exception", "PropertyGridDateItem Error : " + ex.Message);
                return value;
            }
            return value;
        }

    }
}
