using UnityEngine;
using UnityEngine.UI;
using System;
using SLM;

public class Test : MonoBehaviour
{
    private static float xValue;
    private static float yValue;
    private static float zValue;

    private String wifiStrength;

    private AndroidJavaClass jc;
    private AndroidJavaObject jo;

    private InputField APIPassword;
    private InputField LicenseCode;
    private InputField LicenseId;

    private Button testBtn;
    void Start()
    {
        GameObject obj;

		//初始化开发商密码
        obj = GameObject.Find("APIPassword");
        APIPassword = obj.GetComponent<InputField>();
        APIPassword.text = "这里需要使用自己的开发商密码";

		//授权码字段，留空。
        obj = GameObject.Find("LicenseCode");
        LicenseCode = obj.GetComponent<InputField>();
		//这里不需要填写内容
        LicenseCode.text = "";

		//授权码中包含的许可ID号
        obj = GameObject.Find("LicenseID");
        LicenseId = obj.GetComponent<InputField>();
        LicenseId.text = "1025";

		//测试按钮， 点击测试后， 调用Android端功能函数
        obj = GameObject.Find("TestBtn");
        testBtn = obj.GetComponent<Button>();
        testBtn.onClick.AddListener(OnClick);

        Debug.Log("OnStart");
    }

    void OnGUI()
    {

    }

    void OnClick()
    {

        Debug.Log("APIPassword: " + APIPassword.text + " \n  ");
        Debug.Log("LicenseCode: " + LicenseCode.text + " \n  ");
        Debug.Log("LicenseId: " + LicenseId.text + " \n  ");

        jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

        //初始化Android端SDK（只需要调用一次）
        String initStr = jo.Call<String>("initSmartLicense", APIPassword.text);
        Debug.Log("initSmartLicense \n  " + initStr);

        //查找指定许可Id
        long ret = jo.Call<long>("findlicense", int.Parse(LicenseId.text));
        Debug.Log("findlicense result code " + ret);

		//如果许可不存在，则进行绑定
        if (ret == SSErrCode.SS_ERROR_RESPONSE || ret == SSErrCode.SS_ERROR_LIC_NOT_FOUND)
        {
            LicenseCode.text = "测试前需要先创建测试使用的授权码填写在这里";
            Debug.Log("指定测试授权码");
			//绑定授权码
            String bindRetStr = jo.Call<string>("bindSmartLicense", LicenseCode.text);
            Debug.Log("bindSmartLicense \n  " + bindRetStr);
        }
        else if (ret != SSErrCode.SS_OK)
        {
            Debug.Log("未找到指定许可: error code " + ret);
            return;
        }

        if (ret == 0)
        {
			//检查授权成功，调用Android端封装好的函数功能，这里是对接口的简单测试。
            String testRuntimeStr = jo.Call<string>("testRuntimeAPI", int.Parse(LicenseId.text));
            Debug.Log("testRuntimeAPI \n  " + testRuntimeStr);
        }
    }
}