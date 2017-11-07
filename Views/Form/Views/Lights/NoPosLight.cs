using Framework.Maths;
using System.ComponentModel;
using Views.Form.Views.Convert;

namespace Views.Form.Views.Lights
{
    public abstract class NoPosLight : ILight
    {
        #region Constructor
        public NoPosLight()
        {
            Enable = true;
        }
        #endregion Constructor

        #region Attributes
        [Browsable(false)]
        public string Name
        { 
            set { _name = value; } 
            get { return _name; }
        }
        [TypeConverterAttribute(typeof(MathConverter)),
            EditorAttribute(typeof(MathUITypeEditor),
            typeof(System.Drawing.Design.UITypeEditor))]
        public Color LightColor 
        { 
            set { _lightColor = value; } 
            get { return _lightColor; } 
        }
        public bool Enable { set; get; }
        #endregion Attributes

        #region Fields
        protected string _name = string.Empty;
        protected Color _lightColor = Color.White;
        #endregion Fields
    }
}
