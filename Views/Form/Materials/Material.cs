using System.ComponentModel;

namespace Views.Form.Materials
{
    public struct Material
    {
        #region Attributes

        [Browsable(false)]
        public string Name 
        { 
            set { _name = value; } 
            get { return _name; } 
        }

        [EditorAttribute(typeof(Views.Convert.TextureUITypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string TextureName 
        { 
            set { _textureName = value; } 
            get { return _textureName; } 
        }

        [TypeConverterAttribute(typeof(Views.Convert.TextureConverter)),
            EditorAttribute(typeof(Views.Convert.TextureUITypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public Texture Texture
        {
            get
            {
                return MaterialManager.Instance.GetTexture(_textureName);
            }
        }
        #endregion Attributes

        #region Fields
        public readonly static Material DefaultDiffuse = new Material() { _name = "mat_DefaultDiffuse" };
        private string _name;
        private string _textureName;
        #endregion Fields
    }
}
