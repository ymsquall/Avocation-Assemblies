using System;
using System.Collections.Generic;
using UnityEngine;

namespace UIView
{
    public class UISingleViewFadePanel<T> : UIViewFadePanel
        where T : UIBaseView
    {
        static T mInstance = null;
        public static T Inst { get { return mInstance; } }
		protected virtual void Awake()
		{
			mInstance = this as T;
		}
        public void DestoryT()
        {
            GameObject.Destroy(gameObject);
        }
        protected virtual void OnEnable()
        {
            mInstance = this as T;
        }
        protected override void DestroyImpl()
        {
            mInstance = null;
        }
    }
}
