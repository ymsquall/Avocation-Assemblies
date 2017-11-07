using Framework.Maths;
using System;
using System.Collections.Generic;

namespace Views.Form.Views.Lights
{
    public static class LightUtil
    {
        #region Methods
        private static void ApplyLighting(IPosLight light, Vector3 eyeWorldPos, Vector3 worldPos, Vector3 worldNorm, ref Vertex2D pt)
        {
            var lightDir = Vector3.Normalize(light.Position - worldPos);
            // 反射角;
            float diffuse = Math.Max(0, Vector3.Dot(worldNorm, lightDir));
            diffuse = diffuse < 0f ? 0f : diffuse;
            // apply diffuse;
            pt.diffuse += Color.Multiply(light.LightColor, diffuse);
            // apply specular;
            Vector3 inViewDir = Vector3.Normalize(eyeWorldPos - worldPos);
            Vector3 helfDir = Vector3.Normalize(inViewDir + lightDir);
            float specular = 0;
            if (diffuse != 0)
            {
                //防止出现光源在物体背面产生高光的情况;
                float sp = MathUtil.Clamp(Math.Max(0, Vector3.Dot(helfDir, worldNorm)));
                specular = (float)System.Math.Pow(sp, 4);
            }
            pt.specular += Color.Multiply(light.LightColor, specular);
        }
        // 应用顶点光照 Blinn-Phone光照模型;
        public static void ApplyVertexLightings(List<IPosLight> lights,
            Vector3 eyeWorldPos, Vector3 ptWorldPos, Vector3 ptWorldNorm, ref Vertex2D pt)
        {
            if (!enableLightings)
            {
                return;
            }
            if (null == lights)
            {
                return;
            }
            int lightCount = lights.Count;
            for (int i = 0; i < lightCount; ++i)
            {
                IPosLight light = lights[i];
                if (null == light || !light.Enable)
                {
                    continue;
                }
                ApplyLighting(light, eyeWorldPos, ptWorldPos, ptWorldNorm, ref pt);
            }
            // 环境光;
            AmbientLight ambientLight = AmbientLight.Instance;
            if(null != ambientLight && ambientLight.Enable)
            {
                pt.diffuse = ambientLight.LightColor + pt.diffuse;
            }
        }
        #endregion Methods

        #region Fields
        public static bool enableLightings = true;
        #endregion Fields
    }
}
