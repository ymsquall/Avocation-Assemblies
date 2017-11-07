using Framework.Maths;

namespace Views.Form.Views.Lights
{
    // 全局环境光，只能有一个;
    public class AmbientLight : NoPosLight
    {
        #region Constructor
        private AmbientLight() 
        {
            _name = "light_globalAmbient";
            _lightColor = Color.FromArgb(1f, 0.2f, 0.2f, 0.2f);
        }
        #endregion Constructor

        #region Attributes
        public static AmbientLight Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new AmbientLight();
                }
                return _instance;
            }
        }
        #endregion Attributes

        #region Fields
        private static AmbientLight _instance = null;
        #endregion Fields
    }
}
