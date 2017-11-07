using UnityEngine;
#if UNITY_IOS
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Networking.TcpIPNetworking;
using Platforms.PlatformSDK;
using Logic.BasicLogic;
using UIView.Popup;
using Shared;
#if SDK_WX
    using SDK_WX;
#elif SDK_91
    using SDK_91;
#elif SDK_PP
    using SDK_PP;
#elif SDK_HM
    using SDK_HM;
#elif SDK_TBT
    using SDK_TBT;
#elif SDK_AB
    using SDK_AB;
#endif

public class PlatformSDKInterface : MonoBehaviour
{
	bool mFirstLogin = true;
	static bool mLoginLocked = false;
	static bool mLoginSender = false;
	static float mLoginLockedStartTime = 0;
	static PlatformSDKInterface Inst = null;
	
	public static bool LoginSender
	{
		set{ mLoginSender = value;}
		get{ return mLoginSender;}
	}
	public static bool LoginLocked
	{
		set
		{
#if !SDK_HM
			if(value)
				mLoginLockedStartTime = Time.realtimeSinceStartup;
			Debug.Log("******SetLoginLocked=" + value.ToString() + ", time=" + mLoginLockedStartTime.ToString());
#endif
			mLoginLocked = value;
		}
		get
		{
#if !SDK_HM
			if(mLoginLocked)
			{
				float time = Time.realtimeSinceStartup;
				if((time - mLoginLockedStartTime) > 5f)
				{
					mLoginLocked = false;
					Debug.Log("******GetLoginLocked=" + mLoginLocked.ToString() + ", time=" + time.ToString());
				}
			}
#endif
			return mLoginLocked;
		}
	}
	void Awake()
	{
		Inst = this;
#if SDK_AB
		Entry.SetUnityReceiver(transform.name);
#endif
	}
	void OnDestroy()
	{
		Inst = null;
	}
#if SDK_91
#region 用C对SDK-API进行封装
	[DllImport("__Internal")]
	private static extern void CSetUnityReceiver(string gbName);
	
	[DllImport("__Internal")]
	private static extern void CSDKInit(int appId,string appKey,bool isdebug);
	
	[DllImport("__Internal")]
	private static extern void CGameResLoaded();
	
	[DllImport("__Internal")]
	private static extern void CSDKLogin();

	[DllImport("__Internal")]
	private static extern void CSDKSelAccount();
	
	[DllImport("__Internal")]
	private static extern void CSDKLoginGuest();
	
	[DllImport("__Internal")]
	private static extern void CSDKShowToolBar(int point);
	
	[DllImport("__Internal")]
	private static extern void CSDKHideToolBar();
	
	[DllImport("__Internal")]
	private static extern bool CGetIsLogined();
	
	[DllImport("__Internal")]
	private static extern void CSDKPause();
	
	[DllImport("__Internal")]
	private static extern int CGetCurrentLoginState();
	
	[DllImport("__Internal")]
	private static extern string CGetLoginUin();
	
	[DllImport("__Internal")]
	private static extern string CGetSessionId();
	
	[DllImport("__Internal")]
	private static extern string CGetNickName();
	
	[DllImport("__Internal")]
	private static extern int CSDKLogout(int param);
	
	[DllImport("__Internal")]
	private static extern int CSDKUniPay(string cooOrderSerial,string productId,string productName,float productPrice,int productCount,string payDescription);
	
	[DllImport("__Internal")]
	private static extern int CSDKUniPayAsyn(string cooOrderSerial,string productId,string productName,float productPrice,int productCount,string payDescription);
	
	[DllImport("__Internal")]
	private static extern int CSDKUniPayForCoin(string cooOrderSerial,int needPayCoins,string payDescription);
#endregion￼
#endif

	public delegate void SDKEventHandler(bool succ);
	static SDKEventHandler OnInited = null;
#if SDK_DEBUG
    public static bool mDebug = true;// TODO: 设置调试模式：true为开启，fasle为关闭
#else
	public static bool mDebug = false;// TODO: 设置调试模式：true为开启，fasle为关闭
#endif
    void Start () 
    {
		//Debug.Log(this.transform.name);
#if SDK_91
		CSetUnityReceiver(this.transform.name);	
#elif SDK_HM
		Entry.SetUnityReceiver(transform.name);
		Entry.ZHPayInit(SDKConst.appid, 1);
#elif SDK_TBT
		SDK.SetUnityReceiver(transform.name);
#endif
    }

#region SDK - 回调
#if SDK_91
    void InitResult()
    {
	    Debug.Log("回调：初始化完成");
		if (null != OnInited)
			OnInited (true);
	    //LsNdLogin();
    }

    void LoginResultSuccess(string uin)
    {
		string sid = GetSessionId ();
		Debug.Log("回调：账号登陆成功:"+ uin + ", SessionId = " + sid + ", sdk = " + SDKConst.type.ToString());
		OnSDKLogin (uin, sid);
    }

    void LoginResultChangeOk(string uin)
	{
		string sid = GetSessionId ();
		Debug.Log("回调：切换账号成功:"+ uin + ", SessionId = " + sid + ", sdk = " + SDKConst.type.ToString());
		OnSDKLogin (uin, sid);
	}

	void LoginResultFail(string error)
    {
		Debug.Log("回调：账号登陆失败-" + error);
		SDKLogout (1);
		if (null != InSceneLogicModel.Inst)
			InSceneLogicModel.Inst.BackToLoginScene (弹框按钮.确认);
		else if(null != LoginLogicModel.Inst)
			LoginLogicModel.Inst.ChangeState(LoginLogicModel.状态类型.登陆);
		LoginLocked = false;
    }

    void PauseExit()
    {
		Debug.Log("回调：暂停页-从桌面回游戏");
		Time.timeScale = 1;
	}

    void PayResultUserCancel(string result)
    {
		Debug.Log("回调：用户取消支付"+result);
    }

    void PayResultNetworkFail(string result)
    {
		Debug.Log("回调：支付时网络错误"+result);
    }

    void PayResultServerReturnError(string result)
    {
		Debug.Log("回调：支付时服务器发生错误"+result);
    }

    void PayResultOrderSerialSubmitted(string result)
    {
	    //使用异步支付或代币充值支付方式的需要处理
		Debug.Log("回调:订单已经提交，此时应当对游戏服务器进行轮询支付结果"+result);
    }

    void PayResultOtherError(string result)
    {
		Debug.Log("回调：支付时其他未知错误"+result);
    }

    void PayResultSuccess(string result)
    {
		Debug.Log("回调：支付成功，应做发货处理"+result);
    }

    void SessionInvalid(string result)
    {
		Debug.Log("回调：Session会话失效");
    }
	
#elif SDK_PP
	// 用户登录回调
	// 该回调返回字符串型 token_key,只有登录成功有效
	void U3D_loginCallBack(string token_key)
	{
		Debug.Log ("回调：账号登陆成功:" + token_key);
		OnSDKLogin ("", token_key);
		Bonjour.LoginViewShowing = false;
	}
	// 用户登出回调
	// 无返回
	void U3D_logOffCallBack()
	{
		Debug.Log ("回调：账号登出:");
		Bonjour.LoginViewShowing = false;
		if (null != InSceneLogicModel.Inst)
			InSceneLogicModel.Inst.BackToLoginScene (弹框按钮.确认);
		else if(null != LoginLogicModel.Inst)
			LoginLogicModel.Inst.ChangeState(LoginLogicModel.状态类型.登陆);
		LoginLocked = false;
	}
	// 关闭 SDK 客户端页面回调
	// 返回客户端页面编号
	void U3D_closePageViewCallBack(string s)
	{
		Debug.Log ("回调：返回客户端页面编号:" + s);
		int c = -1;
		if(int.TryParse(s, out c))
		{
			Common.PPPageCode code = (Common.PPPageCode)c;
			switch (code)
			{
			case Common.PPPageCode.PPLoginViewPageCode:
				if(GetCurrentLoginState() == 0)
					LoginLocked = false;
				Bonjour.LoginViewShowing = false;
				break;
			case Common.PPPageCode.PPRegisterViewPageCode:
				if(GetCurrentLoginState() == 0)
					LoginLocked = false;
				break;
			}
		}
	}
	// 关闭 web 页面回调
	// 返回 web 页面编号
	void U3D_closeWebViewCallBack(string s)
	{
		Debug.Log ("回调：返回 web 页面编号:" + s);
	}
	// 购买道具回调
	// 返回错误编号,0 为成功。只有账号 PP 币余额大于 商品价格时会有回调。
	void U3D_payResultCallBack(string s)
	{
		Debug.Log ("回调：返回购买道具编号:" + s);
		int c = -1;
		if(int.TryParse(s, out c))
		{
			Common.PPPayResultCode code = (Common.PPPayResultCode)c;
			switch(code)
			{
			case Common.PPPayResultCode.PPPayResultCodeCommunicationFail:
			case Common.PPPayResultCode.PPPayResultCodeUntreatedBillNo:
				{
					PopupDialogView.Popup (弹框类型.错误提示, Text.SharedShowStrings.充值提醒.ToString (),
				                       Text.SharedShowStrings.购买商品请求处理成功.ToString () + "!" +
				                       Text.SharedShowStrings.如果长时间未收到商品请联系PP充值客服.ToString () + "。" + 
				                       Text.SharedShowStrings.电话.ToString () + ":020-38276673");
				}
				break;
			}
		}
	}
	// 获取当前 UserName
	// 返回当前用户名
	// (1.3.8 之后被禁止)
	//U3D_currentUserName
	// 游戏更新检测回调
	// 无返回
	void U3D_ppVerifyingUpdatePassCallBack()
	{
		Debug.Log ("回调：游戏更新检测完成");
		if (null != OnInited)
			OnInited (true);
	}
	// 获取当前用户登录状态
	// (1.3.8 之后启动)0:未登录,1:登录
	void U3D_LoginState(int p)
	{
		Debug.Log ("回调：获取当前用户登录状态:" + p.ToString());
	}
#elif SDK_HM
	bool FirstShowSDKView = true;
	static string HMUserID = "";
	static string HMValidateToken = "";
	void ZHPayLoginSuccess(string result)
	{
		// TODO: 回调：账号登陆成功
		Dictionary<string,string> userInfo = Entry.SplitStringToDic(result);
		HMUserID = userInfo["userId"];                         //用户ID，唯一标识
		HMValidateToken = userInfo["validateToken"];   //登陆验证token，可用于服务器登录有效性验证，详见文档
		string userName = userInfo["userName"];              //用户名，1.3.0版将弃用，首次接入的开发者不要使用，老开发者可继续使用
		Debug.Log("ZHPay回调：账号登陆成功ID:"+HMUserID);
		OnSDKLogin (HMUserID, HMValidateToken);
	}
	void ZHPayLoginCancel()
	{
		// TODO: 回调：登录取消 
		Debug.Log ("ZHPay回调：登录取消");
		LoginLocked = false;
	}
	void ZHPayDidLogout()
	{
		// TODO: 回调：账号注销成功，此时需要将游戏退出，并切换到登录前画面
		Debug.Log ("ZHPay回调：注销成功");
		LoginLogicModel loginLogic = LoginLogicModel.Inst;
		if (null != loginLogic)
		{
			if(loginLogic.NowState != LoginLogicModel.状态类型.登陆)
				loginLogic.ChangeState(LoginLogicModel.状态类型.登陆);
			return;
		}
		if (null != InSceneLogicModel.Inst)
			InSceneLogicModel.Inst.BackToLoginScene (弹框按钮.确认);
		LoginLocked = false;
	}
	void ZHPayViewIn()
	{
		// TODO: 回调：SDK界面出现
		Debug.Log ("ZHPay回调：SDK界面出现");
		if (FirstShowSDKView)
		{
			LoginLogicModel.Inst.OnSDKInited(true);
			FirstShowSDKView = false;
		}
	}
	void ZHPayViewOut()
	{
		// TODO: 回调：SDK界面退出
		Debug.Log ("ZHPay回调：SDK界面退出");
	}
	void ZHPayCheckUpdateFinish(string result)
	{
		//TODO: 回调：检查更新回调
		Dictionary<string,string> resultInfo = Entry.SplitStringToDic(result);
		bool isSuccess = (resultInfo["isSuccess"]=="1");
		bool needUpdate = (resultInfo["needUpdate"]=="1");
		bool isForce = (resultInfo["isForce"]=="1");
		/*在此可添加您的处理*/
		Debug.Log ("ZHPay回调：检查更新完毕，是否请求成功:" + isSuccess + " 是否需要更新:" + needUpdate + " 是否强制更新:" + isForce);
	}
	void ZHPayResultSuccessWithOrder(string result)
	{
		// TODO: 回调：订单支付成功
		Dictionary<string,string> orderInfo = Entry.SplitStringToDic(result);
		string orderId = orderInfo["orderId"];
		string productName = orderInfo["productName"];
		string productDescription = orderInfo["gameName"];
		string productPrice = orderInfo["productPrice"];
		string userParam = orderInfo["userParam"];
		/*在此可处理您的订单*/
		
		
		Debug.Log ("ZHPay回调：订单支付成功："+result);
	}
	internal void ZHPayResultFailedWithOrder(string result)
	{
		// TODO: 回调：订单支付失败
		Dictionary<string,string> orderResult = Entry.SplitStringToDic(result);
		string errorCode = orderResult["errorCode"];
		string orderId = orderResult["orderId"];
		string productName = orderResult["productName"];
		string productDescription = orderResult["gameName"];
		string productPrice = orderResult["productPrice"];
		string userParam = orderResult["userParam"];
		// errorCode："0":余额不足  "1":订单创建错误  "2":重复提交订单,请更换订单号  "3":网络不通畅（可能已购买成功但客户端已超时,建议去自己服务器进行订单查询）  "4":服务器错误  "5":其它错误
		/*在此可处理您的订单*/
		
		Debug.Log ("ZHPay回调：订单支付失败："+result);
	}
	internal void ZHPayResultCancelWithOrder(string result)
	{
		// TODO: 回调：用户中途取消支付
		Dictionary<string,string> orderInfo = Entry.SplitStringToDic(result);
		string orderId = orderInfo["orderId"];
		string productName = orderInfo["productName"];
		string productDescription = orderInfo["gameName"];
		string productPrice = orderInfo["productPrice"];
		string userParam = orderInfo["userParam"];
		/*在此可处理您的订单*/
		
		
		Debug.Log ("ZHPay回调：用户中途取消支付："+result);
	}
	internal void ZHCheckOrderFinishedWithOrder(string result)
	{
		// TODO: 回调：查询订单完毕
		Dictionary<string,string> orderResult = Entry.SplitStringToDic(result);
		string status = orderResult["status"];
		string orderId = orderResult["orderId"];
		string money = orderResult["money"];
		// status：0:待支付  1:已支付  2:过期失效（未曾支付） 3: 订单不存在（或未完成支付流程）  4:支付失败
		/*在此可处理您的订单*/
		
		Debug.Log ("ZHPay回调：查询订单完毕："+result);
	}
	internal void ZHCheckOrderDidFailed(string orderId)
	{
		// TODO: 回调：查询订单失败
		Debug.Log ("ZHPay回调：查询订单失败，订单号："+orderId);
	}
#elif SDK_AB
	static string SDKUserName = "";
	void InitResultOK()
	{
		Debug.Log("回调：初始化完成");
		if (null != OnInited)
			OnInited (true);
	}
	void InitResultFailed()
	{
		Debug.Log("回调：初始化Failed!");
		if (null != OnInited)
			OnInited (false);
	}
	void LoginResultOK(string userName)
	{
		SDKUserName = userName;
		OnSDKLogin (SDKUserName, "");
	}
	void LoginResultFailed()
	{
		LoginLocked = false;
	}
	void LoginResultCancel()
	{
		LoginLocked = false;
	}
	void PayResultOK(string resultInfo)
	{
	}
	void PayResultOK_SignFailed(string resultInfo)
	{
	}
	void PayResultUserCancel(string resultInfo)
	{
	}
	void PayResultFailed(string resultInfo)
	{
	}
#elif SDK_TBT
	static bool isUseOldMode = true;
	static string lastOrderNumber = "";
	void DebugLog(string msg)
	{
		Debug.Log ("[SDK_TBT]:" + msg);
	}
	void OnSDKInited()
	{
		DebugLog("回调：初始化完成");
		if (null != OnInited)
			OnInited (true);
		SDKShowToolBar ();
	}
	void OnLogined(string uin)
	{
		string sid = SDK.TBSessionID();
		DebugLog("回调：账号登陆成功:"+ uin + ", SessionId = " + sid + ", sdk = " + SDKConst.type.ToString());
		OnSDKLogin (uin, sid);
	}
	void OnLogouted()
	{
		DebugLog ("DidLogout");
		if (null != InSceneLogicModel.Inst)
			InSceneLogicModel.Inst.BackToLoginScene (弹框按钮.确认);
		else if(null != LoginLogicModel.Inst)
			LoginLogicModel.Inst.ChangeState(LoginLogicModel.状态类型.登陆);
		LoginLocked = false;
	}
	void OnLeavedDefaultPage()
	{
		DebugLog ("OnLeavedDefaultPage");
	}
	void OnLeavedLoginPage()
	{
		DebugLog ("OnLeavedLoginPage");
		if(GetCurrentLoginState() == 0)
			LoginLocked = false;
	}
	void OnLeavedUserCenter()
	{
		DebugLog ("OnLeavedUserCenter");
	}
	void OnLeavedUserPaying(string order)
	{
		DebugLog ("OnLeavedUserPaying");
	}
	void OnCheckOrder_WaitingForPay(string order)
	{
		DebugLog ("OnCheckOrder_WaitingForPay:" + order);
	}
	void OnCheckOrder_Paying(string order)
	{
		DebugLog ("OnCheckOrder_Paying:" + order);
	}
	void OnCheckOrder_Failed(string order)
	{
		DebugLog ("OnCheckOrder_Failed:" + order);
	}
	void OnCheckOrder_Success(string order)
	{
		DebugLog ("OnCheckOrder_Success:" + order);
	}
	void OnCheckOrderFailed(string order)
	{
		DebugLog ("CheckOrderFailed:" + order);
	}
	void OnBuyGoodsSuccessed(string order)
	{
		DebugLog ("OnBuyGoodsSuccessed:" + order);
	}
	void OnBuyGoods_BalanceNotEnough(string order)
	{
		DebugLog ("OnBuyGoods_BalanceNotEnough:" + order);
	}
	void OnBuyGoods_ServerError(string order)
	{
		DebugLog ("OnBuyGoods_ServerError:" + order);
	}
	void OnBuyGoods_OrderEmpty(string order)
	{
		DebugLog ("OnBuyGoods_OrderEmpty:" + order);
	}
	void OnBuyGoods_NetworkError(string order)
	{
		DebugLog ("OnBuyGoods_NetworkError:" + order);
	}
	void OnBuyGoods_OtherError(string order)
	{
		DebugLog ("OnBuyGoods_OtherError:" + order);
	}
	void OnEnterRechargeWebView(string order)
	{
		DebugLog ("OnEnterRechargeWebView:" + order);
	}
	void OnBuyGoodsCancelByUser(string order)
	{
		DebugLog ("OnBuyGoodsCancelByUser:" + order);
	}
#endif
#endregion

#region SDK-API
	public static void SDKInit(SDKEventHandler callback, string callObjName)
	{
		OnInited = callback;
#if SDK_91
		// TODO: API：SDK初始化
		CSDKInit(SDKConst.appid,SDKConst.appKey,mDebug);
#elif SDK_PP
		Bonjour.initSDK(SDKConst.appid,SDKConst.appKey, true, 100, true, true, true, "", true, true, false, false, callObjName);
#elif SDK_HM
#elif SDK_AB
		Entry.initSDK(SDKConst.appid.ToString(), SDKConst.appKey, 1);
#elif SDK_TBT
		SDK.TBSetUseOldLoadingMode(isUseOldMode);
		isUseOldMode = !isUseOldMode;
		SDK.TBInit(SDKConst.appid);
#endif
    }
	public static void SDKLogin()
    {
#if SDK_HM
		if (GetCurrentLoginState () != 0)
		{
			if(!TcpIPNetwork.Inst.Connected || !LoginSender)
				OnSDKLogin (HMUserID, HMValidateToken);
		}
#endif
		if(LoginLocked)
		{
			Debug.LogWarning("SDKLogin:LoginLocked");
			return;
		}
		Debug.LogWarning("******SDKLogin:LoginNotLocked");
		TcpIPNetwork.Inst.Close (false);
		if (GetCurrentLoginState () != 0)
			SDKLogout (1);
#if SDK_91
		// TODO: API：账号登陆
		if(Inst.mFirstLogin)
		{
			CSDKLogin();
			Inst.mFirstLogin = false;
		}
		else
		{
			CSDKSelAccount();
		}
#elif SDK_PP
		//if(!Bonjour.LoginViewShowing)
		{
			Debug.Log("*****SDKLogin.SDKLogin");
			Bonjour.showLoginView();
			Bonjour.LoginViewShowing = true;
		}
#elif SDK_HM
		Entry.ZHPayStartLogin();
#elif SDK_AB
		Entry.showLoginView();
#elif SDK_TBT
		SDK.TBLogin(0);
#endif
		TcpIPNetwork.Inst.ConnectServer (SharedGlobalParams.LoginHost, SharedGlobalParams.LoginPort);
		LoginLocked = true;
    }
#if SDK_91
	public static void SDKLoginGuest()
    {
		// TODO: API：游客登陆
		CSDKLoginGuest();
    }

	public static bool GetIsLogined()
	{
	    // TODO: API：判断玩家是否登陆状态
		return CGetIsLogined();
	}
#endif
	static void OnSDKLogin(string uni, string token)
	{
		if (null != LoginLogicModel.Inst)
		{
			if(!TcpIPNetwork.Inst.Connected)
				TcpIPNetwork.Inst.ConnectServer (SharedGlobalParams.LoginHost, SharedGlobalParams.LoginPort);
			//TcpIPNetwork.Inst.SendMessage (C2S_LoginSDK.Create (uni, token, SDKConst.type));
			Debug.Log("connect to " + SharedGlobalParams.LoginHost + ":" + SharedGlobalParams.LoginPort.ToString() + 
			          "Logining[" + uni + "-" + token + "]");
			LoginSender = true;
			//TcpIPNetwork.Inst.SendMessage (C2S_Login.Create ("ym2000", "111111"));
		}
	}
	public static void ShowUserCenter()
	{
#if SDK_PP
		Bonjour.showCenterView();
#elif SDK_HM
		Entry.ZHPayShowUserCenter();
#elif SDK_TBT
		SDK.TBUserCenter(0);
#endif
	}
	public static void ShowAppCenter()
	{
#if SDK_TBT
		SDK.TBGameRecommend(0);
#endif
	}
	public static void SDKShowToolBar()
	{
#if SDK_91
		// TODO: API：显示工具条: point=1,2,3,4,5,6,分别表示：左上、右上、左中、右中、左下、右下 
		CSDKShowToolBar(1);
#elif SDK_TBT
		// point=1,2,3,4,5,6,分别表示：左上、右上、左中、右中、左下、右下 
		SDK.TBShowToolBar(3, true);
#endif
	}
	
	public static void SDKHideToolBar()
	{
#if SDK_91
		// TODO: API：隐藏工具条
		CSDKHideToolBar();
#elif SDK_TBT
		SDK.TBHideToolBar();
#endif
	}
	public static void SDKPause()
	{
#if SDK_91
		// TODO: API：show pause page
		CSDKPause();
#endif
	}
	public static int GetCurrentLoginState()
	{
#if SDK_91
	    // TODO: API：返回账号状态：0表示未登陆、1表示游客登陆、2表示普通账号登陆
		return CGetCurrentLoginState();
#elif SDK_PP
		return Bonjour.PPLoginState();
#elif SDK_HM
		return Entry.ZHPayIsLogined() ? 1 : 0;
#elif SDK_TBT
		return SDK.TBIsLogined() ? 1 : 0;
#endif
		return 0;
    }
	public static int SDKLogout(int param)
	{
		int ret = 0;
#if SDK_91
		// TODO: API：注销,返回错误码,param: 0,表示注销；1，表示注销，并清除自动登录
		ret = CSDKLogout(param);
#elif SDK_PP
		Bonjour.PPlogout();
#elif SDK_HM
		Entry.ZHPayStartLogout ();
#elif SDK_TBT
		SDK.TBLogout();
#endif
		return ret;
	}
#if SDK_91
	public static string GetLoginUin()
	{
	    // TODO: API：获取账号主键ID
	    return CGetLoginUin();
    }

	public static string GetSessionId()
    {
	    // TODO: API：获取会话ID
		return  CGetSessionId();
    }

	public static string GetNickName()
    {
	    // TODO: API：获取昵称
		return  CGetNickName();
	}
	public static int SDKUniPay(string cooOrderSerial,string productId,string productName,float productPrice,int productCount,string payDescription)
    {
	    // TODO: API：同步支付（订单号，道具ID，道具名，价格，数量，分区：不超过20个英文或数字的字符串）
		return CSDKUniPay(cooOrderSerial,productId,productName,productPrice,productCount,payDescription);
    }

	public static int SDKUniPayForCoin(string cooOrderSerial,int needPayCoins,string payDescription)
    {
	    // TODO: API：代币充值（订单号，代币数量，分区：不超过20个英文或数字的字符串）
		return CSDKUniPayForCoin(cooOrderSerial,needPayCoins,payDescription);
	}
#endif
	public static int SDKUniPayAsyn(string cooOrderSerial,int productId,string productName,float productPrice,int productCount, int zoneID, string roleID)
	{
		int ret = 0;
#if SDK_91
		// TODO: API：异步支付（订单号，道具ID，道具名，价格，数量，分区：不超过20个英文或数字的字符串）
		ret = CSDKUniPayAsyn(cooOrderSerial,roleID,productName,productPrice,productCount,zoneID.ToString());
#elif SDK_PP
		string userID = SharedLoginGameKeys.Inst.UserID;
		Bonjour.exchangeGoods(productPrice, cooOrderSerial, productName, userID + "," + roleID, zoneID);
#elif SDK_HM
		Entry.ZHPayStartOrder(cooOrderSerial, productName, Text.SharedShowStrings.斗战西游.ToString(), (int)productPrice, Entry.ZHPayGetUserId() + "," + roleID);
#elif SDK_AB
		Entry.checkoutWithOrder(cooOrderSerial, productId, (int)productPrice, SDKUserName + "," + roleID, "");
#elif SDK_TBT
		lastOrderNumber = cooOrderSerial;
		SDK.TBPayRMB((int)productPrice, cooOrderSerial, SDK.TBUserID() + "," + roleID);
#endif
		return ret;
	}
#endregion
	public static void GameResLoaded()
	{
#if SDK_91
		CGameResLoaded();
#endif
	}
	public static void BackToLogin()
	{
		TcpIPNetwork.Inst.Close (false);
		if (GetCurrentLoginState () != 0)
			SDKLogout (1);
	}
}
#else
public class PlatformSDKInterface : MonoBehaviour
    {
		public static bool LoginLocked = false;
		public static bool LoginSender = false;
        public static void SDKShowToolBar() { }
        public static void SDKHideToolBar() { }
        public static void ShowUserCenter() { }
    }
#endif