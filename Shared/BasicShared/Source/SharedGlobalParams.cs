using Framework.Tools;
using System;
using UnityEngine;

namespace Shared
{
    public enum LayerDefine : int
    {
        Nothing             = 0x00000000,
        Default             = 0,
        TransparentFX       = 1,
        IgnoreRaycast       = 2,
        InnerEmpty3         = 3,
        Water               = 4,
        UI                  = 5,
        InnerEmpty6         = 6,
        InnerEmpty7         = 7,
        CharacterController = 8,
        Ground              = 9,
        CharacterModel      = 10,
        WallCube            = 11,
        TransPoint          = 12,
        HideWeapon          = 13,
        BindPoint           = 14,
        Empty15             = 15,
        Empty16             = 16,
        Empty17             = 17,
        Empty18             = 18,
        Empty19             = 19,
        Empty20             = 20,
        Empty21             = 21,
        Empty22             = 22,
        Empty23             = 23,
        Empty24             = 24,
        Empty25             = 25,
        Empty26             = 26,
        RenderTargetUI      = 27,
        CreateChara1        = 28,
        CreateChara2        = 29,
        CreateChara3        = 30,
        NGUI                = 31,
        EveryThing          = unchecked((int)0xffffffff)
    }
    public class SharedGlobalParams : AutoSingleT<SharedGlobalParams>
    {
        public const float DefaultMainCameraOrthographicSize = 3.7f;
        public const float CameraMaxAspect = 1.7558f;
        public const float DefaultViewPortHeight = 640f;
        public const float SceneNGUIScale = 0.0058f;
        public const float DefaultPlaneU3DSize = 10.0f;
        public const float DefaultCubeU3DSize = 1.0f;
        public const float DefaultQuadU3DSize = 1.0f;
        public const int PlayerJumpAttackID = 3;
        public const int CharacterHeaderPanelDepth = -1;
        public const byte PlayerForceGuideLimitLv = 7;
        public const byte PlayerGuideLimitLv = 15;
        public static string UpdateHttpUrl = "http://123.206.26.146/";
        public static string UdpTesterAddr = "192.168.1.101";
        public static ushort UdpTesterPort = 60001;
        public static string LoginHost = "192.168.1.101";
        public static int LoginPort = 20001;
        public static string BundleVersion = "1.0.0.1";
        public static int AssetsVersion = 1;
        public static string PrivacyPolicyUrl = "";
        public static Quaternion LookRotationLeft = Quaternion.LookRotation(Vector3.left);
        public static Quaternion LookRotationRight = Quaternion.LookRotation(Vector3.right);
        public static Quaternion IdentityQuat = Quaternion.identity;
        public static Quaternion QuayRoatY180 = Quaternion.Euler(0, 180, 0);
        public static Quaternion QuayRoatY90 = Quaternion.Euler(0, 90, 0);
        public static Quaternion QuayRoatZ90 = Quaternion.Euler(0, 0, 90);
        public static Quaternion QuayRoatZ270 = Quaternion.Euler(0, 0, 270);
        public static Quaternion QuayRoatZ180 = Quaternion.Euler(0, 0, 180);
        public static Vector3 RightDir = Vector3.right;
        public static float PixelUnit2U3D(int p)
        {
            return (float)p / 100;
        }
        public static float PixelUnit2U3D(float p)
        {
            return p / 100;
        }
        public static double PixelUnit2U3D(double p)
        {
            return p / 100;
        }
        public static float PixelSpeed2U3D_MS(double s)
        {
            return (float)((s * 1000) / 100);
        }
        public static float U3DUnit2Pixel(float m)
        {
            float pixel = m * 100;
            float aaa = pixel - (float)(int)pixel;
            if (Mathf.Abs(aaa) < 0.001f)
                pixel = (float)(int)pixel;
            if (Mathf.Abs(aaa) > 0.999f)
                pixel = (float)(int)(pixel + 0.001f);
            return pixel;
        }
        public static double U3DUnit2Pixel(double m)
        {
            double r = m * 100;
            Int64 t = (Int64)(r * 1000.0);
            r = t * 0.001;
            return r * 100;
        }

        Vector2 mDefaultDesignResolution = new Vector2(960.0f, 640.0f);
        Vector2 mLoadingPanelSize = new Vector2(500.0f, 56.0f);
        public float UnityUnitScale { get { return 0.01f; } }
        public float ControllerDetph { get { return -1f; } }
        public float DesignWidth
        {
            get { return mDefaultDesignResolution.x; }
        }
        public float DesignHeight
        {
            get { return mDefaultDesignResolution.y; }
        }
        public float LoadingPanelWidth
        {
            get { return mLoadingPanelSize.x; }
        }
        public float LoadingPanelHeight
        {
            get { return mLoadingPanelSize.y; }
        }

        public string MainCameraPublicName { get { return "MainCamera"; } }
        public string Map2DBoundsLeftName { get { return "Map2DAABBLeft"; } }
        public string Map2DBoundsTopName { get { return "Map2DAABBTop"; } }
        public string Map2DBoundsRightName { get { return "Map2DAABBRight"; } }
        public string Map2DBoundsBottomName { get { return "Map2DAABBBottom"; } }
        public string NGUIMoveStackName { get { return "Slider(方向按钮)"; } }
        public string NGUIAttackBtnName { get { return "Image Button(普通攻击)"; } }
        public string NGUIJumpBtnName { get { return "Image Button(跳)"; } }
        public string NGUISkill1BtnName { get { return "Image Button(技1)"; } }
        public string NGUISkill2BtnName { get { return "Image Button(技2)"; } }
        public string NGUISkill3BtnName { get { return "Image Button(技3)"; } }
    }
}
