#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Platforms.PlatformSDK
{
    public enum IOSPlatform
    {
		SDK_None = 0,
        SDK_WX,
        SDK_91,
		SDK_PP,
        SDK_HM,
		SDK_TBT,
		SDK_AB,
        max
    }

    public enum AndroidPlatform
    {
		SDK_None = 0,
        SDK_WX,
        SDK_91,
		SDK_PP,
        max
    }
    public enum WP_8_1Platform
    {
        SDK_None = 0,
        SDK_AB,
        max
    }
}
#if !SDK_WX && !SDK_91 && !SDK_PP && !SDK_HM && !SDK_TBT && !SDK_AB
public class SDKConst
{
	public const int appid = 0;
	public const string appKey = "";
	public const int type = 0;
	#if UNITY_IOS && UNITY_EDITOR
		public const iOSTargetOSVersion IOS_TARGET_VER = iOSTargetOSVersion.iOS_4_3;
		public const string SplashImagePath = "";
		public const bool allowedAutorotateToLandscapeLeft = true;
		public const bool allowedAutorotateToLandscapeRight = true;
		public const bool allowedAutorotateToPortrait = false;
		public const bool allowedAutorotateToPortraitUpsideDown = false;
	#endif
#if UNITY_EDITOR
	public const string CODE_SIGN_IDENTITY = "";
	public const string PROVISIONING_PROFILE = "";
	public const string bundle = "";
	public const string PlistAdd = "";
	public const string rotaReplace = @"<string>UIInterfaceOrientationLandscapeLeft</string>";
	public static void EditorCode(string path){}
#endif
}
#endif