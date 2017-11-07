using Framework.Maths;
using System.ComponentModel;
using Views.Form.Views.Convert;

namespace Views.Form.Views.Lights
{
    public abstract class BaseLight : NoPosLight, IPosLight
    {
        #region Attributes
        [TypeConverterAttribute(typeof(MathConverter)),
            EditorAttribute(typeof(MathUITypeEditor),
            typeof(System.Drawing.Design.UITypeEditor))]
        public Vector3 Position 
        { 
            set { _position = value; } 
            get { return _position; }
        }
        #endregion Attributes

        #region Fields
        protected Vector3 _position = Vector3.zero;
        #endregion Fields
    }
}
