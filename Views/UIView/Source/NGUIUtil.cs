using System.Collections.Generic;
using UnityEngine;

namespace UIView
{
    public class UILabelEffect
    {
        public static readonly UILabelEffect 公用文字描边 = new UILabelEffect(UILabel.Effect.Outline, 1, 1, new Color(0.02f, 0.04f, 0.21f, 0.62f));
        public static readonly UILabelEffect 活动副本名描边 = new UILabelEffect(UILabel.Effect.Outline, 1, 1, new Color(0, 0.18f, 0.17f, 0.7f));
        public static readonly UILabelEffect 公用文字阴影 = new UILabelEffect(UILabel.Effect.Shadow, 1, 1, new Color(0.02f, 0.04f, 0.21f, 1f));
        public UILabelEffect() { }
        public UILabelEffect(UILabel.Effect t, float x, float y)
        {
            type = t;
            dist.x = x;
            dist.y = y;
        }
        public UILabelEffect(UILabel.Effect t, float x, float y, Color c)
        {
            type = t;
            dist.x = x;
            dist.y = y;
            color = c;
        }
        public void FillLabel(params UILabel[] lbls)
        {
            if(null != lbls)
            {
                for(int i = 0; i < lbls.Length; ++ i)
                {
                    lbls[i].effectStyle = type;
                    lbls[i].effectDistance = dist;
                    lbls[i].effectColor = color;
                }
            }
        }
        public UILabel.Effect type = UILabel.Effect.None;
        public Vector2 dist = Vector2.zero;
        public Color color = Color.black;
    }
    public static class NGUIUtil
    {
        public const int DefaultFontSize = 16;
#if USE_OLD_UILOAD && UNITY_EDITOR
        public static Dictionary<byte, GameObject> UIResList = new Dictionary<byte, GameObject>();
        public const string UIViewPrefabPath = "Assets/Development/UIView/";
        public const string UIViewDialogPath = "Assets/Development/UIView/PopupDialog/";
        public const string UIViewTemplayePath = "Assets/Development/UIView/Templates/";
        public static Dictionary<byte, UIAtlas> PublicAtlasList = new Dictionary<byte, UIAtlas>();
        public static Dictionary<byte, UIFont> PublicFontList = new Dictionary<byte, UIFont>();
        public static Dictionary<byte, GameObject> TemplateDictSliders = new Dictionary<byte, GameObject>();
#endif
        public static Dictionary<string, string> NumberImageStringList = null;
        
#if USE_OLD_UILOAD && UNITY_EDITOR
        public static UnityEngine.GameObject FindUIResTemplate(UIHlp.UI资源模板 wen)
        {
            if (!UIResList.ContainsKey((byte)wen))
            {
                NGUI2DRootPanel uiRoot = NGUI2DRootPanel.Inst;
                string tempName = UIViewTemplayePath + wen.ToString();
                UnityEngine.Object assetObject = UnityEngine.Resources.Load(tempName);
                if (null != assetObject)
                {
                    UnityEngine.GameObject newObj = NGUITools.AddChild(uiRoot.gameObject, assetObject as UnityEngine.GameObject);
                    assetObject = null;
                    UnityEngine.Resources.UnloadUnusedAssets();
                    UIResList.Add((byte)wen, newObj);
                    return newObj;
                }
                else
                    return null;
            }
            return UIResList[(byte)wen];
        }
        public static UIAtlas FindAtlas(UIHlp.UI图包类型 at)
        {
            UIAtlas atlas = null;
            if (!PublicAtlasList.ContainsKey((byte)at))
            {
                GameObject atlasObj = FindUIResTemplate(UIHlp.UI资源模板.图集模板);
                if (null != atlasObj)
                {
                    if (null == atlasObj.GetComponent<TemplateAtlas>())
                        atlasObj.AddComponent<TemplateAtlas>();
                    Transform trans = atlasObj.transform.FindChild(at.ToString());
                    if (null == trans)
                    {
                        Debug.LogError("找不到名为" + at.ToString() + "的图包!!!");
                        return null;
                    }
                    atlas = trans.GetComponent<UIAtlas>();
                    PublicAtlasList.Add((byte)at, atlas);
                }
            }
            else
            {
                atlas = PublicAtlasList[(byte)at];
                if (null == atlas)
                {
                    PublicAtlasList.Remove((byte)at);
                    return FindAtlas(at);
                }
            }
            return atlas;
        }
        public static UIFont FindFont(UIHlp.动态字体类型 ft)
        {
            UIFont font = null;
            if (!PublicFontList.ContainsKey((byte)ft))
            {
                GameObject fontsObj = FindUIResTemplate(UIHlp.UI资源模板.字体集模板);
                if (null != fontsObj)
                {
                    if (null == fontsObj.GetComponent<TemplateFonts>())
                        fontsObj.AddComponent<TemplateFonts>();
                    Transform trans = fontsObj.transform.FindChild(ft.ToString());
                    if (null != trans)
                    {
                        font = trans.GetComponent<UIFont>();
                        PublicFontList.Add((byte)ft, font);
                    }
                }
            }
            else
            {
                font = PublicFontList[(byte)ft];
                if (null == font)
                {
                    PublicFontList.Remove((byte)ft);
                    return FindFont(ft);
                }
            }
            return font;
        }
        public static UISlider FindSlider(进度条类型 t, GameObject parent)
        {
            GameObject prefab = null;
            if (!TemplateDictSliders.ContainsKey((byte)t))
            {
                GameObject slidersObj = FindUIResTemplate(UIHlp.UI资源模板.进度条集模板);
                if (null != slidersObj)
                {
                    if (null == slidersObj.GetComponent<TemplateDictSliders>())
                        slidersObj.AddComponent<TemplateDictSliders>();
                    Transform trans = slidersObj.transform.FindChild(t.ToString());
                    prefab = trans.gameObject;
                    TemplateDictSliders.Add((byte)t, prefab);
                }
            }
            else
            {
                prefab = TemplateDictSliders[(byte)t];
                if (null == prefab)
                {
                    TemplateDictSliders.Remove((byte)t);
                    return FindSlider(t, parent);
                }
            }
            GameObject slider = NGUITools.AddChild(parent, prefab);
            return slider.GetComponent<UISlider>();
        }
#else
   //     public static UIFont FindFont(UIHlp.动态字体类型 t)
   //     {
   //         if (null != UIResourceManager.Inst)
   //             return UIResourceManager.Inst[t];
			//return null;
   //     }
#endif
        public static string Number2ImageName(char c, string format)
        {
            if(null == NumberImageStringList)
            {
                NumberImageStringList = new Dictionary<string, string>(){
                        //{ ".", "dian" },
                    };
            }
            int num;
            if (int.TryParse(c.ToString(), out num))
                return string.Format("{0}{1}", format, num.ToString());
            if (NumberImageStringList.ContainsKey(c.ToString()))
                return string.Format("{0}{1}", format, NumberImageStringList[c.ToString()]);
            return string.Format("{0}{1}", format, c.ToString());
        }
        public static Vector3 U3DWorldPos2NGUIWorldPos(Vector3 u3dp)
        {
            if (null == NGUI2DRootPanel.Inst || null == Camera.main || null == NGUI2DRootPanel.Inst.mUICamera)
                return Vector3.zero;
            Vector3 pos2D = Camera.main.WorldToScreenPoint(u3dp);
            pos2D.z = 0;
            return NGUI2DRootPanel.Inst.mUICamera.ScreenToWorldPoint(pos2D);
        }
        public static Vector3 U3DWorldPos2NGUIViewPortPoint(Vector3 u3dp)
        {
            if (null == NGUI2DRootPanel.Inst || null == Camera.main || null == NGUI2DRootPanel.Inst.mUICamera)
                return Vector3.zero;
            Vector3 pos2D = Camera.main.WorldToScreenPoint(u3dp);
            pos2D.z = 0;
            return NGUI2DRootPanel.Inst.mUICamera.ScreenToViewportPoint(pos2D);
        }
        public static Vector3 NGUIViewPortPos2NGUIWorldPoint(Vector3 pt)
        {
            if (null == NGUI2DRootPanel.Inst || null == NGUI2DRootPanel.Inst.mUICamera)
                return Vector3.zero;
            return NGUI2DRootPanel.Inst.mUICamera.ViewportToWorldPoint(pt);
        }

        public static BoxCollider UpdateWidgetCollider(GameObject obj, bool ret = false)
        {
            BoxCollider box = obj.GetComponent<Collider>() as BoxCollider;
            if (null == box)
            {
                NGUITools.AddWidgetCollider(obj);
                if (ret)
                    box = obj.GetComponent<Collider>() as BoxCollider;
            }
            else
                NGUITools.UpdateWidgetCollider(box, false);
            return box;
        }
        public static void UpdateWidgetCollider(Component com, out BoxCollider box, out BoxCollider2D box2D, bool ret = false)
        {
            box = null;
            box2D = null;
            UICamera ui = UICamera.FindCameraForLayer(com.gameObject.layer);
            if (ui != null && (ui.eventType == UICamera.EventType.World_2D || ui.eventType == UICamera.EventType.UI_2D))
            {
                box2D = com.GetComponent<BoxCollider2D>();
                if (null == box2D)
                {
                    NGUITools.AddWidgetCollider(com.gameObject);
                    if (ret)
                        box2D = com.GetComponent<BoxCollider2D>();
                }
                else
                    NGUITools.UpdateWidgetCollider(box2D.gameObject, false);
            }
            else
            {
                box = com.GetComponent<BoxCollider>();
                if (null == box)
                {
                    NGUITools.AddWidgetCollider(com.gameObject);
                    if (ret)
                        box = com.GetComponent<BoxCollider>();
                }
                else
                    NGUITools.UpdateWidgetCollider(box.gameObject, false);
            }
        }
    }
}
