package com.example.unity3d_android_libs;

import android.content.Context;
import android.os.Bundle;
import android.util.Log;

import com.senseyun.openapi.SSRuntimeEasyJava.ErrorCode;
import com.senseyun.openapi.SSRuntimeEasyJava.INFO_FORMAT_TYPE;
import com.senseyun.openapi.SSRuntimeEasyJava.INFO_TYPE;
import com.senseyun.openapi.SSRuntimeEasyJava.LIC_USER_DATA_TYPE;
import com.senseyun.openapi.SSRuntimeEasyJava.SSRuntimeEasy;
import com.senseyun.openapi.SSRuntimeEasyJava.ST_LOGIN_PARAM;
import com.unity3d.player.UnityPlayerActivity;

public class MainActivity extends UnityPlayerActivity {
    public String TAG = MainActivity.class.getName();
    /**
     * Called when the activity is first created.
     */
    Context mContext = null;

    // 全局记录初始化状态
    private static long slmInitAndroidEnvRet = -1;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        //Unity3d调用的Activity中不要使用setContentView()函数
        mContext = this;
    }

    /**
     * 初始化授权SDK
     * @param APIPassword
     * @return
     */
    public String initSmartLicense(String APIPassword) {
        long ret = 0;
        String msg = "initSmartLicense  ";

        // 初始化软锁环境，需要传入当前应用的 Context
        slmInitAndroidEnvRet =
                SSRuntimeEasy.SlmInitAndroidEnv(getApplicationContext());
        if (slmInitAndroidEnvRet != ErrorCode.SS_OK) {
            Log.i(TAG, "slmInitAndroidEnvRet: " + slmInitAndroidEnvRet);
            msg += "slmInitAndroidEnvRet: " + slmInitAndroidEnvRet;
        }

        // Runtime （许可相关接口）初始化，全局只需要初始化一次即可
        ret = SSRuntimeEasy.SlmInitEasy(APIPassword);
        Log.i(TAG, "SlmInitEasy = " + ret); // 输出信息到界面
        msg = "SlmInitEasy = " + ret;
        return msg;
    }


    public String bindSmartLicense(String lic) {
        long ret = 0;
        String retMsg = "bindSmartLicense: null";
        if (slmInitAndroidEnvRet == 0) { // slmInitAndroidEnvRet 全局变量
            // 调用接口联网激活授权码
            ret = SSRuntimeEasy.SlmCtrlOnlineBindLicenseKey(lic);
            // 显示联网激活执行结果
            retMsg = "SlmBindLicenseKey licenseKey = " + lic + " r = " + ret;
        }
        else
            retMsg += "lic Error";

        return retMsg;
    }

    /**
     * 使用授权SDK中的接口查找 批量的授权ID是否已经绑定
     */
    public long findlicense(int licenseId){
        long ret = 0;

        // 查找许可（不需要登录许可）
        Object findLic = new Object();
        findLic = SSRuntimeEasy.SlmFindLicenseEasy(licenseId, INFO_FORMAT_TYPE.JSON.get());
        ret = SSRuntimeEasy.SlmGetLastError();
        Log.i(TAG, "SlmFindLicenseEasy= " + ret);
        Log.i(TAG, "FindLicense = " + findLic);
        if (ret == ErrorCode.SS_OK) {
            SSRuntimeEasy.SlmFreeEasy(findLic);
        }
        return ret;
    }

    /**
     * 对授权SDK中的常用接口进行演示测试
     * @param licenseID
     * @return
     */
    public String testRuntimeAPI(int licenseID) {
        long ret = 0;

        StringBuilder sb = new StringBuilder("testRuntimeAPI: \n");

        // 获取开发商ID（不需要登录许可）
        String[] DeveloperID = new String[1];
        ret = SSRuntimeEasy.SlmGetDeveloperIDEasy(DeveloperID);
        Log.i(TAG, "SlmGetDeveloperIDEasy= " + ret);
        Log.i(TAG, "DeveloperID = " + DeveloperID[0]);
        sb.append("SlmGetDeveloperIDEasy= " + ret);
        sb.append("\n");
        sb.append("DeveloperID = " + DeveloperID[0]);
        sb.append("\n");
        sb.append("\n");

        // 获取 API 版本（不需要登录许可）
        long[] api = new long[1];
        long[] ss = new long[1];
        ret = SSRuntimeEasy.SlmGetVersionEasy(api, ss);
        Log.i(TAG, "SlmGetVersionEasy= " + ret);
        Log.i(TAG, "API VERSION = " + api[0]);
        sb.append("SlmGetVersionEasy= " + ret);
        sb.append("\n");
        sb.append("API VERSION = " + api[0]);
        sb.append("\n");sb.append("\n");


        // 查找许可（不需要登录许可）
        Object findLic = new Object();
        findLic = SSRuntimeEasy.SlmFindLicenseEasy(licenseID, INFO_FORMAT_TYPE.JSON.get());
        ret = SSRuntimeEasy.SlmGetLastError();
        Log.i(TAG, "SlmFindLicenseEasy= " + ret);
        Log.i(TAG, "FindLicense = " + findLic);
        if (ret == ErrorCode.SS_OK) {
            SSRuntimeEasy.SlmFreeEasy(findLic);
        }
        sb.append("SlmFindLicenseEasy= " + ret);
        sb.append("\n");
        sb.append("FindLicense = " + findLic);
        sb.append("\n");sb.append("\n");

        // 许可业务接口（需要登录许可）
        long Handle = 0;
        ST_LOGIN_PARAM stLogin = new ST_LOGIN_PARAM();
        stLogin.size = stLogin.getSize();
        stLogin.license_id = licenseID;
        Handle = SSRuntimeEasy.SlmLoginEasy(stLogin,
                INFO_FORMAT_TYPE.STRUCT.get()); //login by struct
        ret = SSRuntimeEasy.SlmGetLastError();
        Log.i(TAG, "SlmLoginEasy= " + ret);
        Log.i(TAG, "LicenseID = " + licenseID);
        sb.append("SlmLoginEasy= " + ret);
        sb.append("\n");
        sb.append("LicenseID = " + licenseID);
        sb.append("\n");sb.append("\n");

        // 检查许可是否有效
        // 当许可过期时，该接口调用会立即返回错误码
        // 当前接口通常在独立线程被调用，用于运行中的应用程序许可是否到期
        ret = SSRuntimeEasy.SlmKeepAliveEasy(Handle);
        Log.i(TAG, "SlmKeepAliveEasy= " + ret);
        sb.append("SlmKeepAliveEasy= " + ret);
        sb.append("\n");

        // 获取许可信息（支持类型 INFO_TYPE）（需要登录许可）
        Object licInfo = new Object();
        licInfo = SSRuntimeEasy.SlmGetInfoEasy(Handle, INFO_TYPE.LICENSE_INFO.get(), INFO_FORMAT_TYPE.JSON.get());
        ret = SSRuntimeEasy.SlmGetLastError();
        Log.i(TAG, "SlmGetInfoEasy= " + ret);
        Log.i(TAG, "licInfo r = " + licInfo);
        if (ret == ErrorCode.SS_OK) {
            SSRuntimeEasy.SlmFreeEasy(licInfo);
        }
        sb.append("SlmGetInfoEasy= " + ret);
        sb.append("\n");
        sb.append("licInfo r = " + licInfo);
        sb.append("\n");sb.append("\n");


        // 许可加密，使用当前许可ID进行加密（需要登录许可）
        String encDataStr = "test data.......";
        Log.i(TAG, "EncData = " + encDataStr);
        sb.append("EncData = " + encDataStr);

        byte[] EncData = new byte[16];
        ret = SSRuntimeEasy.SlmEncryptEasy(Handle, encDataStr.getBytes(),
                EncData, 16);
        Log.i(TAG, "SlmEncryptEasy= " + ret);
        sb.append("SlmEncryptEasy= " + ret);
        sb.append("\n");

        byte[] DecData = new byte[16];
        ret = SSRuntimeEasy.SlmDecryptEasy(Handle, EncData, DecData, 16);
        Log.i(TAG, "SlmDecryptEasy= " + ret);
        sb.append("SlmDecryptEasy= " + ret);
        if (ret == ErrorCode.SS_OK) {
            String decResultStr = new String(DecData);
            Log.i(TAG, "DecResult = " + decResultStr);
            sb.append("SlmDecryptEasy= " + ret);

        }
        sb.append("\n");
        sb.append("\n");

        // 数据区-读取公开区数据（需要登录许可）
        byte[] ReadBuff;
        long DataSize = 0;
        DataSize = SSRuntimeEasy.SlmUserDataGetsizeEasy(Handle,
                LIC_USER_DATA_TYPE.PUB.get());
        ret = SSRuntimeEasy.SlmGetLastError();
        Log.i(TAG, "SlmUserDataGetsizeEasy[PUB]= " + ret);
        Log.i(TAG, "DataSize = " + DataSize);
        sb.append("SlmUserDataGetsizeEasy[PUB]= " + ret);
        sb.append("\n");
        sb.append("DataSize = " + DataSize);
        sb.append("\n");sb.append("\n");


        if (ret == ErrorCode.SS_OK && DataSize > 0) {
            ReadBuff = new byte[(int) DataSize];
            ret = SSRuntimeEasy.SlmUserDataReadEasy(Handle,
                    LIC_USER_DATA_TYPE.PUB.get(), ReadBuff, 0, DataSize);
            Log.i(TAG, "SlmUserDataReadEasy[PUB]= " + ret);
            sb.append("SlmUserDataReadEasy[PUB]= " + ret);
            sb.append("\n");
        }


        // 数据区-读写区写入数据（需要登录许可）
        String WriteData = "test 123...";
        ret = SSRuntimeEasy.SlmUserDataWriteEasy(Handle, WriteData.getBytes(),
                0, WriteData.length());
        Log.i(TAG, "SlmUserDataWriteEasy[RAW]= " + ret);
        Log.i(TAG, "DataSize = " + WriteData.length());
        sb.append("SlmUserDataWriteEasy[RAW]= " + ret);
        sb.append("DataSize = " + WriteData.length());
        sb.append("\n");sb.append("\n");


        // 检查许可模块是否存在（需要登录许可）
        ret = SSRuntimeEasy.SlmCheckModuleEasy(Handle, 1);
        Log.i(TAG, "SlmCheckModuleEasy[1]= " + ret);
        sb.append("SlmCheckModuleEasy[1]= " + ret);
        sb.append("\n");
        sb.append("\n");


        // 登出许可
        if (Handle != 0) {
            ret = SSRuntimeEasy.SlmLogoutEasy(Handle);
            Log.i(TAG, "SlmLogoutEasy= " + ret);
            sb.append("SlmLogoutEasy= " + ret);
            sb.append("\n");
        }


        // 清空 SlmInitEasy 申请资源（全局只需要释放一次，与 SlmInitEasy 对应）
        ret = SSRuntimeEasy.SlmCleanupEasy();
        Log.i(TAG, "SlmCleanupEasy= " + ret);
        sb.append("SlmCleanupEasy= " + ret);
        sb.append("\n");sb.append("\n");

        return sb.toString();
    }

    @Override
    protected void onDestroy() {
        // 释放 Android 环境初始化资源，与 SlmInitAndroidEnv 对应
        if (slmInitAndroidEnvRet == 0) {
            SSRuntimeEasy.SlmUninitAndroidEnv();
        }
        super.onDestroy();
    }

}