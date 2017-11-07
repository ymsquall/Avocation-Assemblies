
namespace Scripting.UniLua
{
#if !UNITY_3 && !UNITY_4 && !UNITY_5
	using System.Diagnostics;
#endif

	internal class LuaOSLib
	{
		public const string LIB_NAME = "os";

		public static int OpenLib( ILuaState lua )
		{
			NameFuncPair[] define = new NameFuncPair[]
			{
#if !UNITY_3 && !UNITY_4 && !UNITY_5 && !UNITY_WEBPLAYER
				new NameFuncPair("clock", 	OS_Clock),
#endif
			};

			lua.L_NewLib( define );
			return 1;
		}

#if !UNITY_WEBPLAYER
		private static int OS_Clock( ILuaState lua )
        {
#if !UNITY_3 && !UNITY_4 && !UNITY_5
			lua.PushNumber( Process.GetCurrentProcess().TotalProcessorTime.TotalSeconds );
#endif
			return 1;
		}
#endif
        }
    }

