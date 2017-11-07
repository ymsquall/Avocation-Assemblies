using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Views.Form.Materials;
using Framework.Logger;

namespace Views.Form.Views.Convert
{
    public class TextureUITypeEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
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
                    if (context.PropertyDescriptor.Name == "TextureName")
                    {
                        Material mat = (Material)value;
                        IDictionaryEnumerator texs = MaterialManager.Instance.Textures;
                        List<string> list = new List<string>();
                        string first = string.Empty;
                        while (texs.MoveNext())
                        {
                            DictionaryEntry entry = (DictionaryEntry)texs.Current;
                            Texture tex = entry.Value as Texture;
                            if (null == tex)
                                continue;
                            if(string.IsNullOrEmpty(first))
                            {
                                first = tex.Name; 
                            }
                            Label lbl = new Label();
                            lbl.Text = tex.Name;
                            edSvc.DropDownControl(lbl);
                            list.Add(tex.Name);
                        }
                        return first;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSys.Error("TextureUITypeEditor", "TextureUITypeEditor Error : " + ex.Message);
                return value;
            }
            return value;
        }

    }
}
