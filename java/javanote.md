#   遇到的问题

1.  无效的发行版本（使用IDEA 导入eclipse的时候遇到问题）

   解决办法：参考好心人的博客：https://www.cnblogs.com/chenyanbin/p/12168522.html

2. 如何修增加IDEA的java配置

   参考官方文档：https://www.jetbrains.com/help/idea/run-debug-configuration.html#create-permanent

3. javac -version 和 java -version 的版本不一样

   参考好心人的博客：https://www.cnblogs.com/shined/p/4492615.html

# 知识点

###  第七章 package import 和class path

2. jar 文件的导出和导入
   - jar文件，是java特有的一种文件格式，用于可执行文件的传播。
   
   - 实际上是一组class文件的额压缩包，本质上和c++的dll是类似的，本质上是一个压缩包。
   
   - class 是由.java编译过来的字节码，能够一定程度的保护只是产权，针对性的由Java的混淆器，编译过的Java是经过混淆的
   
     1. 命令行解释：
   
        - 编译类需要类的完整全路径（带.java扩展名）
   
        - 运行命令行：运行类需要完整类名（包名+类名  不带.class扩展） Java     - classpath  .;c:\ temp  cn.com.test.Man
   
        - java  : %JAVA_HOME%bin  
   
        - -classpath  缩写： cp
   
        - .;c:\temp   :  子路径1（当前目录    .   ）   按照；隔开   子路径2 （c:\temp）
   
          > 在某一个子路径找到所需要的类之后，后续的子路径不再寻找，classpath 在所有的子路径中优先级最高
          >
          > 在所有子路径中都找不到所需要的类： NoClassDefException
   
        2. 编译和运行规则
   
           - 编译一个类：需要Java 文件的全路径和扩展名
   
           - 运行一个类，需要类的全名称（非文件路径），无需写扩展名。
   
           - 编译类需要给处这个类所依赖的类（包括依赖的类再以来的其他所有类的所在路径）
   
           - 运行类比编译类多当前运行类的所在路径。
   
           - classpath 参数可以包含jar包，如果路径内有空格，  classpath 参数需要整体添加双引号
   
           - -java -classpath ".;c:\test.jar;c:\temp;c\a bc"   cn.com.test.Man
   
             ![1613696685699](C:\Users\shenyy\AppData\Roaming\Typora\typora-user-images\1613696685699.png)
   
   3. Java 访问权限
   
      - Java 一般可以  所有的变量都用private ，所有的方法都用public
      - Java访问权限有四种：
        - private ：私有的，自能本类访问
        - default ：（通常忽略不写，写了就报错）默认一个包内可以访问
        - protected：同一个包，子类都可以访问
        - public：公开的，所有类都可以访问
   
      - 使用范围
        - 四种都可以用于修饰成员变量，成员方法，构造函数
        - default 和 public 可以修饰类![1613697642229](C:\Users\shenyy\AppData\Roaming\Typora\typora-user-images\1613697642229.png)

###  Java常用类

1. 类库概述
   - Java类库
     - 包名用Java 开头的额都是Java 的核心包 
     - 包名用javax 开始的包时Java的扩展包（Java Extension Package）

![1613699581759](C:\Users\shenyy\AppData\Roaming\Typora\typora-user-images\1613699581759.png)

> 了解核心类，其他的查询文档就好了

2. 数学相关类

   - Java数字类
     - 整数类  Short 、Int、 Long
     - 浮点数 Float， Double
     - 大数类 BigInterger(大整数),BigDecimal（大浮点数）
     - 随机数类 Random
     - 工具类 Math

   - 整数类型：
     - short ：16位，2个字节 -32678 ~ 32678（-2^15 ~ 2^15-1 ） ，默认为0  
     - int: 32位 ，4个字节  (-2^31 ~ 2^31-1) 默认为0
     - long 64位， 8个字节，有符号的以二进制补码的形式表示整数  （-2^63 ~ 2^63-1） 默认值为  0L

   - 浮点数类型：
     - flaot : 32 位  4 个字节 可以精确到 6~7位。不建议用于精确表示
     - double: 64位 8 个字节，可以精确到 15~16位。不建议在精确的场景使用float 和double，如果需要精确使用，需要用到BigDecimal

   - 大数字类
     - BigIteger : 支持无限大的整数类
     - BigDecimal 支持无限大的小数运算，使用时需要注意精度和截断（除法计算的时候可能会进行无限循环，使用的时候可以启用截断，和四舍五入） ,尽量使用字符串对BigDecimal进行赋值，使用字面值复制会有误差

   - Random 随机数类：
     - Random rd = new Random()
     - rd.ints  rd.nextInts

3. 字符串相关类
   - String （使用最多的类）
   - StringBuffer
     - 初始占用的字节数是16个字节（不初始化）
     - 初始化时在原来的字节数上加上16  eg:new StringBuffer(“123”),占用字节数位19
     - 一旦length大于capacity时，会在原来的基础上加1乘以二，eg: length = 19. .append("123").capzcity = (19+1)*2;
     - 如果翻倍不够，则StringBuffer 会以原来的字符串的长度来定义capacity
     - 使用sb.trimToSize()，能够压缩空间，使得capacity = length
   - StringBuilder 