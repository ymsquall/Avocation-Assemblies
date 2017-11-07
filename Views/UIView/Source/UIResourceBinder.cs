using Framework.Tools;
using System;
using System.Collections;
using System.Collections.Generic;

namespace UIView
{
    public class UIResourceBinder : SingletonMBT<UIResourceBinder>
    {
        public const string HeroImaggePrefix_friend_head = "_friend_head";
        public const string HeroImaggePrefix_hero = "_hero";
        public const string HeroImaggePrefix_hero_head = "_hero_head";
        public const string HeroImaggePrefix_loading = "_loading";
        public const string HeroImaggePrefix_player_list = "_player_list";

        public UIViewFadePanel[] UIList = null;
        public UIAtlas[] UIAtlasList = null;
        //public UIFont[] UIFontList = null;
        static Dictionary<Type, UIViewFadePanel> mUITypeMapper = new Dictionary<Type, UIViewFadePanel>();
        static Dictionary<string, UIAtlas> mUIAtlasMapper = new Dictionary<string, UIAtlas>();
        //static Dictionary<string, UIFont> mUIFontMapper = new Dictionary<string, UIFont>();

        public static T ShowView<T>() where T : UIViewFadePanel
        {
            T view = FindView<T>();
            if(null != view)
            {
                if (view.gameObject.activeSelf && view.FadeIned)
                    return view;
                view.gameObject.SetActive(true);
                view.FadeIn();
            }
            return view;
        }
        public static T CloseView<T>() where T : UIViewFadePanel
        {
            T view = FindView<T>();
            if (null != view)
            {
                if(view.gameObject.activeSelf)
                    view.FadeOut();
            }
            return view;
        }
        public static void CloseView(UIViewFadePanel view)
        {
            if (null != view)
            {
                if (view.gameObject.activeSelf)
                    view.FadeOut();
            }
        }
        public static T FindView<T>() where T : UIViewFadePanel
        {
            Type t = typeof(T);
            if (!mUITypeMapper.ContainsKey(t))
                return null;
            return mUITypeMapper[t] as T;
        }
        public static UIAtlas FindAtlas(string n)
        {
            if (!mUIAtlasMapper.ContainsKey(n))
                return null;
            return mUIAtlasMapper[n];
        }
        protected override void Awake()
        {
            base.Awake();
            mUITypeMapper.Clear();
            for (int i = 0; i < UIList.Length; ++i)
            {
                UIViewFadePanel view = UIList[i];
                Type t = view.GetType();
                if (mUITypeMapper.ContainsKey(t))
                    continue;
                mUITypeMapper.Add(t, view);
            }
            UIList = null;
            //
            mUIAtlasMapper.Clear();
            for (int i = 0; i < UIAtlasList.Length; ++i)
            {
                UIAtlas atlas = UIAtlasList[i];
                string n = atlas.name;
                if (mUIAtlasMapper.ContainsKey(n))
                    continue;
                mUIAtlasMapper.Add(n, atlas);
            }
            UIAtlasList = null;
            //
            //mUIFontMapper.Clear();
            //for (int i = 0; i < UIFontList.Length; ++i)
            //{
            //    UIFont font = UIFontList[i];
            //    string n = font.name;
            //    if (mUIFontMapper.ContainsKey(n))
            //        continue;
            //    mUIFontMapper.Add(n, font);
            //}
            //UIFontList = null;
        }

        public IEnumerator OnBackToLogin()
        {
            foreach(var view in mUITypeMapper.Values)
            {
                if(view.gameObject.activeSelf)
                {
                    if (!view.InFadeOut)
                        view.FadeOut();
                    yield return 0;
                }
            }
        }
    }
}
