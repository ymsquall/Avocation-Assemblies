#if UNITY_WP_8_1

namespace Type_WP_8_1
{
    public class Type
    {
        public static System.Reflection.MethodInfo GetMethod(System.Type t, string n, System.Type[] tps = null)
        {
            System.Reflection.MethodInfo ret = null;
#if NETFX_CORE
			try
			{
				ret = System.Reflection.RuntimeReflectionExtensions.GetRuntimeMethod(t, n, tps);
			}
			catch (System.Exception ex)
			{
				UnityEngine.Debug.LogError("Failed to bind " + t + "." + n + "\n" +  ex.Message);
				return ret;
			}
#else // NETFX_CORE
            for (ret = null; t != null; )
            {
                try
                {
                    ret = t.GetMethod(n, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
                    if (ret != null) break;
                }
                catch (System.Exception) { }
#if UNITY_WP8 || UNITY_WP_8_1
                // For some odd reason Type.GetMethod(name, bindingFlags) doesn't seem to work on WP8...
                try
                {
                    ret = t.GetMethod(n);
                    if (ret != null) break;
                }
                catch (System.Exception) { }
#endif
                t = t.BaseType;
            }
#endif // NETFX_CORE
            return ret;
        }

        public static System.Reflection.FieldInfo GetField(System.Type t, string n)
        {
            System.Reflection.FieldInfo ret = null;
#if NETFX_CORE
			try
			{
				ret = System.Reflection.RuntimeReflectionExtensions.GetRuntimeField(t, n);
			}
			catch (System.Exception ex)
			{
				UnityEngine.Debug.LogError("Failed to bind " + t + "." + n + "\n" +  ex.Message);
				return ret;
			}
#else // NETFX_CORE
            for (ret = null; t != null; )
            {
                try
                {
                    ret = t.GetField(n, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
                    if (ret != null) break;
                }
                catch (System.Exception) { }
#if UNITY_WP8 || UNITY_WP_8_1
                // For some odd reason Type.GetMethod(name, bindingFlags) doesn't seem to work on WP8...
                try
                {
                    ret = t.GetField(n);
                    if (ret != null) break;
                }
                catch (System.Exception) { }
#endif
                t = t.BaseType;
            }
#endif // NETFX_CORE
            return ret;
        }

        public static System.Reflection.PropertyInfo GetProperty(System.Type t, string n)
        {
            System.Reflection.PropertyInfo ret = null;
#if NETFX_CORE
			try
			{
                ret = System.Reflection.RuntimeReflectionExtensions.GetRuntimeProperty(t, n);
			}
			catch (System.Exception ex)
			{
				UnityEngine.Debug.LogError("Failed to bind " + t + "." + n + "\n" +  ex.Message);
				return ret;
			}
#else // NETFX_CORE
            for (ret = null; t != null; )
            {
                try
                {
                    ret = t.GetProperty(n, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
                    if (ret != null) break;
                }
                catch (System.Exception) { }
#if UNITY_WP8 || UNITY_WP_8_1
                // For some odd reason Type.GetMethod(name, bindingFlags) doesn't seem to work on WP8...
                try
                {
                    ret = t.GetProperty(n);
                    if (ret != null) break;
                }
                catch (System.Exception) { }
#endif
                t = t.BaseType;
            }
#endif // NETFX_CORE
            return ret;
        }

        public static bool IsAssignableFrom(System.Type to, System.Type from)
        {
            bool ret = false;
#if NETFX_CORE
            try
            {
                System.Reflection.TypeInfo ti1 = System.Reflection.IntrospectionExtensions.GetTypeInfo(to);
                System.Reflection.TypeInfo ti2 = System.Reflection.IntrospectionExtensions.GetTypeInfo(from);
                ret = ti1.IsAssignableFrom(ti2);
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError("Failed to get typeinfo" + to + "<=>" + from + "\n" +  ex.Message);
                return ret;
            }
#else // NETFX_CORE
            try
            {
                ret = to.IsAssignableFrom(from);
            }
            catch (System.Exception) { }
#if UNITY_WP8 || UNITY_WP_8_1
            // For some odd reason Type.GetMethod(name, bindingFlags) doesn't seem to work on WP8...
            try
            {
                ret = to.IsAssignableFrom(from);
            }
            catch (System.Exception) { }
#endif
#endif // NETFX_CORE
            return ret;
//#if NETFX_CORE
//            if (to.GetTypeInfo().IsAssignableFrom(from.GetTypeInfo()))
//                return true;
//#else
//            if (to.IsAssignableFrom(from))
//                return true;
//            return false;
//#endif
        }
    }
}
#endif