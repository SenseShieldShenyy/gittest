### 一. 概述

因为`Unity3d`与`Android`的交互可以通过`jar`包或`aar`包的方式。这里使用的是jar包的方式。如果需要其它调用方式可以通过网络搜索相关的文章。两种方式对于`Android`中的`SmartLicense `并没有影响或区别。 只要能够通过`Untiy3d`中 `JavaAndroidJavaClass `和 `AndroidJavaObject `两个类对象获取当前Activity的实例都是可以正常调用到`SmartLicense`的。

此Demo中并未涉及使用到通过`Unity3d`调用`Android`中`APK`资源的部分。

**在调用时，建议将SDK的接口编写为java函数， 通过`Unity3d`中的`Start()`或`OnGUI()`事件函数对SDK进行初始化或授权检测。**



**测试环境:**

Android: 5.1

Unity3d: 2019.1.08a

**Demo逻辑:**

1. 在Unity3d中调用Unity3d接口获取Android端Activty实例托管对象

2. 在Unity3d中调用Activity实例函数初始化SDK底层库(initSmartLicense函数)

3. 在Unity3d中调用Activity实例函数检查是否已经激活授权（findlicense函数）

4. 如果没有查找到对应函数 ，调用Activity中的绑定函数使用 bindSmartLicense 进行绑定

5. 在unity中 调用Activity实例的测试函数 （testRuntimeAPI函数）



### 二、`Unity3d`调用`Android SmartLicense`流程

#### 2.1 Android 端编译jar包

1. 打开`项目/app/build.gradle` 将 

   - `apply plugin: 'com.android.application'` 修改为 `apply plugin: 'com.android.library'`

   - 移除`defaultConfig`中的 `applicationId` 参数

     ![image-20200708114416669](Unity3d_Android_SmartLicense_Demo%E4%BD%BF%E7%94%A8%E8%AF%B4%E6%98%8E.assets/image-20200708114416669.png)
     
     

2. 将下面一段代码添加到`app/build.gradle`的尾部

   ```gradle
   task clearJar(type: Delete) {
       delete 'build/libs/unitylibs.jar' //jar包的名字，随便命名 
   }
   task makeJar(type:org.gradle.api.tasks.bundling.Jar) {
       //指定生成的jar名 
       baseName 'unitylibs'
       //从哪里打包class文件 
       from('build/intermediates/javac/debug/classes/')
       from('libs')
       //打包到jar后的目录结构 
       into('lib')
       //去掉不需要打包的目录和文件 
       exclude('test/', '**/BuildConfig.class', 'libs/classes.jar')
   }
   
   makeJar.dependsOn(clearJar, build)
   ```
   
3. 将`Unity3d`中的`classes.jar`放到`android项目/app/libs`目录中. `classes.jar`一般可以在下面的`Unity3d`安装目录中找到，分为`mono`和`IL2CPP`两个版本:

   **Mac:**

   */Applications/Unity/Unity.app/Contents/PlaybackEngines/AndroidPlayer/src/com/unity3d/player*

   **Windows:**

   *C:\Program Files\Unity\Editor\Data\PlaybackEngines\AndroidPlayer\src\com\unity3d\player* 

   ![image-20200708143444762](Unity3d_Android_SmartLicense_Demo%E4%BD%BF%E7%94%A8%E8%AF%B4%E6%98%8E.assets/image-20200708143444762.png)

4. 修改创建的空项目中的`MainActivity.java`的继承关系从`extends AppCompatActivity`改为` extends UnityPlayActivity`。

   **因为Untiy3d APK在程序启动后会接管Activity，所以在`MainActivity`的`onCreate()`事件函数中`setContentView()` 初始化界面的函数要去掉。**

   首次测试建议先使用Demo中的`MainActivity.java`文件进行代替。 待环境测试通过后在使用自己实现的代码文件。

5. 拷贝java文件和库文件. 将`android SmartLicense SDK`中的 

   - `jni_class` 目录下的所有 `*.java` 文件到工程指定包名 `com.senseyun.openapi.SSRuntimeEasyJava` 目录下，即拷贝至 `app/src/main/java/com/senseyun/openapi/SSRuntimeEasyJava` 目录。

   -  `lib/armeabi-v7a/libSSRuntimeEasyJava.so` 文件到项目 `app/libs/armeabi-v7a` 目录。

     ![image-20200708164503356](Unity3d_Android_SmartLicense_Demo%E4%BD%BF%E7%94%A8%E8%AF%B4%E6%98%8E.assets/image-20200708164503356.png)

6. 在`Android Studio`终端中执行`gradlew makeJar`命令， 生成Jar包。生成的jar包存储在`app/build/libs`目录中。Demo中名称为`unitylibs.jar`

   ![image-20200708144637259](Unity3d_Android_SmartLicense_Demo%E4%BD%BF%E7%94%A8%E8%AF%B4%E6%98%8E.assets/image-20200708144637259.png)

7. 初期测试建议使用Demo中的代码直接测试。完整的流程测试通过后再调用SDK实现相应的功能。另外在使用SDK时需要了解下面的接口调用顺序.

   ```
   *   使用许可调用顺序：
   *       第一步，调用SlmInitEasy函数进行全局初始化
   			  调用不依赖许可句柄参数的通用函数（可选）
   *       第二步，调用SlmLoginEasy函数登录许可（硬件许可，网络许可，云许可，软许可）
   *       第三步，调用依赖许可句柄参数的函数
   *       第四步，调用SlmLogoutEasy函数，登出当前许可
   *       第五步，程序退出时，调用SlmCleanupEasy函数反初始化（可选调用）
   ```

#### 2.2 Unity端设置

1. 将Android中生成的以下项目文件复制到`Assets/Plugin/Android`目录下（如果目录不存在可自行创建）

   - `app/build/libs/unitylibs.jar`文件
   - `app/src/main/com/example/unity_android_libs/AndroidManifest.xml`文件

   ![image-20200709115057097](Unity3d_Android_SmartLicense_Demo%E4%BD%BF%E7%94%A8%E8%AF%B4%E6%98%8E.assets/image-20200709115057097.png)

   - Unity3d_android_sl Demo 下的`Plogins/Scripts`复制到项目目录中，此目录下的slm文件夹是一些定义和错误码:

     ![image-20200710120409466](Unity3d_Android_SmartLicense_Demo%E4%BD%BF%E7%94%A8%E8%AF%B4%E6%98%8E.assets/image-20200710120409466.png)

2. 在Assets 创建脚本，并编写相应的逻辑。初期可直接使用Demo中的`Test.cs`文件。

   Test.cs 文件在使用前有两种变量需要进行修复。

   - `Start()`函数中需要预先初始化好`开发商密码`, 此密码可以在

     [开发商云平台]:https://developer.lm.virbox.com/login.html?code=598c8d1f06

     的 **开发者信息**  中找到。

     ![image-20200710112532192](Unity3d_Android_SmartLicense_Demo%E4%BD%BF%E7%94%A8%E8%AF%B4%E6%98%8E.assets/image-20200710112532192.png)

   - `OnClick()`函数中需要预先将创建好的用于测试的授权码填写在此处

     ![image-20200710112639347](Unity3d_Android_SmartLicense_Demo%E4%BD%BF%E7%94%A8%E8%AF%B4%E6%98%8E.assets/image-20200710112639347.png)

3. 在`Unity3d`中的`Build Setting`界面中切换为Android平台， 并且将`Package Name`设置与Android项目结构中的包名相同。

   ![image-20200709142058304](Unity3d_Android_SmartLicense_Demo%E4%BD%BF%E7%94%A8%E8%AF%B4%E6%98%8E.assets/image-20200709142058304.png)

4. 生成APK并安装测试

     

### 三. 启动后效果

![Demo](Unity3d_Android_SmartLicense_Demo%E4%BD%BF%E7%94%A8%E8%AF%B4%E6%98%8E.assets/Demo.png)



### 四. 常见问题

1. Unity3d生成APK时提示资源文件没有找到

   ![20200706115948137](Unity3d_Android_SmartLicense_Demo%E4%BD%BF%E7%94%A8%E8%AF%B4%E6%98%8E.assets/20200706115948137.png)

   此问题需要去掉`AndroidManifest.xml`文件中对资源文件的引用

   ![image-20200710104824566](Unity3d_Android_SmartLicense_Demo%E4%BD%BF%E7%94%A8%E8%AF%B4%E6%98%8E.assets/image-20200710104824566.png)

   或者，学习在Unity3d中使用Android资源的相关方法（本文档未提供）

2. APK运行时提示方法签名未找到

   ![image-20200710110143261](Unity3d_Android_SmartLicense_Demo%E4%BD%BF%E7%94%A8%E8%AF%B4%E6%98%8E.assets/image-20200710110143261.png)

   该错误可能有两个原因：

   1. 在`Untiy3d项目/Plugin/Android`目录下没有存放对应的`AndroidManifest.xml`文件
   2. 在Unity3d中调用Activity实例时，函数名称或函数返回值类型不对。