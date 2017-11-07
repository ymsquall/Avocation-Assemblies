using System.Collections;

namespace Views.Form.Materials
{
    public class MaterialManager
    {
        #region Methods
        public Texture GetTexture(Material mat)
        {
            return GetTexture(mat.TextureName);
        }
        public Texture GetTexture(string texName)
        {
            if (!string.IsNullOrEmpty(texName))
            {
                Texture tex = _textureTable[texName] as Texture;
                if (null != tex)
                {
                    return tex;
                }
            }
            return Texture.WhiteBlock;
        }
        public void AddMaterial(Material mat)
        {
            if (string.IsNullOrEmpty(mat.Name))
            {
                return;
            }
            _materialTable[mat.Name] = mat;
        }
        public void AddTexture(Texture tex)
        {
            if (null == tex || string.IsNullOrEmpty(tex.Name))
            {
                return;
            }
            _textureTable[tex.Name] = tex;
        }
        #endregion Methods

        #region Attributes
        public static MaterialManager Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new MaterialManager();
                }
                return _instance;
            }
        }
        public Material this[string name]
        {
            get
            {
                if (!string.IsNullOrEmpty(name))
                {
                    // 这里不先检查有无key直接下标访问的话会报异常;
                    object value = _materialTable[name];
                    if (null != value)
                    {
                        return (Material)_materialTable[name];
                    }
                }
                return Material.DefaultDiffuse;
            }
        }
        public IDictionaryEnumerator Materials
        {
            get { return _materialTable.GetEnumerator(); }
        }
        public IDictionaryEnumerator Textures
        {
            get { return _textureTable.GetEnumerator(); }
        }
        #endregion Attributes

        #region Fields
        private static MaterialManager _instance = null;
        public static bool enableTextures = true;
        private Hashtable _materialTable = new Hashtable();
        private Hashtable _textureTable = new Hashtable();
        #endregion Fields
    }
}
