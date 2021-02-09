1、 SlmRuntimeCSharp  是整一个项目工程
2、 testfile          测试往加密所内写入文件的文件
3、 工程编译后生成的可执行程序是one-folder 的形式，运行时需要依赖同目录下的动态库，和对应 x86 x64下的动态库,由于深思的不同开发商之间有库隔离，故使用者需要替换为自己的开发商库。
	开发商库目录位置示例： C:\Program Files (x86)\senseshield\sdk\API\C#\Sample\SlmRuntimeCSharp\bin\Debug
4、 program.cs 中有证书链需要修改 demo 中的证书链为笔者测试锁的证书，使用者需要根据自己的锁修改对应证书链
5、 程序为demo测试程序，界面上所有功能都已经实现，在本地能够测试成功，程序需要在装有 ，net4  的机器上才可以正常运行。xp上亲测有效