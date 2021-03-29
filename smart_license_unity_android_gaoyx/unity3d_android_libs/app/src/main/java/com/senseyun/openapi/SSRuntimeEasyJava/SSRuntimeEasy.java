package com.senseyun.openapi.SSRuntimeEasyJava;

import android.content.Context;

public class SSRuntimeEasy {
	static{
		try{
			System.loadLibrary("SSRuntimeEasyJava");
		}catch(UnsatisfiedLinkError e){
			System.err.println("Native code library failed to load.\n" + e);
			System.exit(1);
		}
	}

	// Android 环境初始化接口
	public final static native long SlmInitAndroidEnv(Context AppContext);
	public final static native long SlmUninitAndroidEnv();

	// Contorl API 接口
	public final static native long SlmCtrlOnlineBindLicenseKey(String LicenseKey);
	public final static native long SlmCtrlOnlineRefreshLicenseKey(String desc, String licenseKey);

	public final static native long SlmCtrlOfflineBindGetC2D(String c2dFilePath);
	public final static native long SlmCtrlOfflineBindD2C(String d2cFilePath);

	/**
	 * 获取 SlmCtrl 接口执行最后一次错误码
	 * @return 返回接口内部执行错误码，用于判断接口是否执行成功。
	 * @remark 所有 SlmCtrl 前缀的接口返回值类型不是 long 时，需要调用 SlmCtrlGetLastError 获取内部执行错误码。
	 */
	public final static native long SlmCtrlGetLastError();

	/**
	 * 获取已绑定软锁的描述信息
	 * 用于获取许可ID、查询许可详细内容、刷新授权码许可等接口
	 * @return 返回已绑定的软锁描述信息
	 */
	public final static native String SlmCtrlGetOfflineDesc();
	public final static native String SlmCtrlGetLicenseID(String desc);
	public final static native String SlmCtrlReadBriefLicenseContext(String desc);

	// Runtime API 正式接口
	public final static native long SlmGetLastError();
	
	public final static native long SlmInitEasy(String APIPsd);
	public final static native long SlmCleanupEasy();
	public final static native long SlmLoginEasy(Object Param, long FmtType);
	public final static native long SlmLogoutEasy(long Handle);

	/**
	 * 返回查找许可的设备描述（JSON数组字符串）
	 * @param LicenseID
	 * @param FmtType
	 * @return
	 */
	public final static native Object SlmFindLicenseEasy(long LicenseID, long FmtType);
	public final static native Object SlmGetInfoEasy(long Handle, long InfoType, long FmtType);
	public final static native long SlmKeepAliveEasy(long Handle);
	public final static native long SlmCheckModuleEasy(long Handle, long Mod);
	public final static native long SlmEncryptEasy(long Handle, byte[] InBuffer, byte[] OutBuffer, long Len);
	public final static native long SlmDecryptEasy(long Handle, byte[] InBuffer, byte[] OutBuffer, long Len);
	public final static native long SlmUserDataGetsizeEasy(long Handle, long DataType);
	public final static native long SlmUserDataReadEasy(long Handle, long DataType, byte[] ReadBuffer, long Offset, long Len);
	public final static native long SlmUserDataWriteEasy(long Handle, byte[] WriteBuffer, long Offset, long Len);
	public final static native long SlmPubDataGetsizeEasy(long Handle, long LicenseID);
	public final static native long SlmPubDataReadEasy(long Handle, long LicenseID, byte[] ReadBuffer, long Offset, long Len);
	public final static native long SlmGetVersionEasy(long[] APIVersion, long[] SSVersion);
	public final static native String SlmErrorFormatEasy(long ErrorCode, long Language);
//	public final static native Object SlmEnumDeviceEasy();	// TODO 暂不支持
	public final static native void SlmFreeEasy(Object obj);
	public final static native long SlmGetDeveloperIDEasy(String[] DeveloperID);
//	public final static native Object SlmEnumLicenseIDEasy(String DeviceInfo);	// TODO 暂不支持
//	public final static native Object SlmGetLicenseInfoEasy(String DeviceInfo, long LicenseID);	// TODO 暂不支持
	
}
