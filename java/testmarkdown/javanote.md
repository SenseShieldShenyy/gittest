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
     
       > 如果能够预估使用空间的大小，最好还是使用构造函数提前申请所需要的内存空间，以提高性能
     
   - StringBuilder 

4. 时间相关类

   > 看Java 文档： 带有Deprecated 关键字代表再将来的版本可能会废弃

   - java.util.Date(Deprecated)
     - getTime(),返回来自 1970.1.1以来的毫秒数,这个毫秒数使用long已经能够足够表示了

   - java.sql.Date （和数据库对应的时间类）
   - Calendar 是目前最常用的，但是是抽象类
     - Calendar gc = Calendar.getInstance();
     - Calendaer gc = new GregorianCalender();
     - 简单的工厂模式（老师并未介绍）

5. 格式化类

   - java.text包java.text.Format 的子类

     - NumberFormat：数字格式化，抽象类
       - DecimalFormat   DecimalFormat df1 = new DecimalFormat("#.00");
       - 主要介绍的是 数字格式化df 如何new出来，# 和 0 的区别是什么。两者对于整数部分的处理是一样的，对于小数部分，#是最多能有多少位，0 是有且只有多少位。

     - MessageFormat: 日期/时间格式化，抽象类
       - SimpleDateFormat

     - Java.time.format 包
       - DateTimeFormatter

###   Java异常和异常处理

1. java 异常分类

   ![1613782208030](G:\code\java\testmarkdown\javanote\1613782208030.png)

    ![1613782254902](G:\code\java\testmarkdown\javanote\1613782254902.png)

   ![1613782362137](C:\Users\shenyy\AppData\Roaming\Typora\typora-user-images\1613782362137.png)

2. 异常处理

   - 允许用户及时保存结果
   - 抓住异常分析异常内容
   - 控制程序返回安全状态

   ![1613782655382](G:\code\java\testmarkdown\javanote\1613782655382.png)

   - try：正常业务代码
   - catch： 出现错误之后执行的代码
   - finally：一定会执行的代码
   - 规则 try 是必须执行的，catch和finally 必须有一个

   ![1613782869682](G:\code\java\testmarkdown\javanote\1613782869682.png)

3. 自定义异常

   - 实例：

     - super关键字：是一个用于直接引用父类对象的引用变量，当子类的示例被创建时，被引用变量引用的父类实列也会被创建。
       - super关键字的用途：
         - 用于引用父类实列的变量或者字段 super.coloer
         - 用于调用父类的方法（应该在子类重写了父类的方法时使用super）super.eat()
         - 用于调用父类的构造函数super()

     - 自定义异常若是继承自exception ,程序遇到此类异常需要使用try  catch进行处理。

###  java数据结构

1. 数组：

   - 数组是一个可以存放多个数据的容器
     - 数据是同一个类型
     - 所有数据都是线性规则排列
     - 可以通过位置索引快速定位访问数据
     - 需要明确容器的长度
   - Java数组定义和初始化
     - int a []; a 还没有进行new操作，实际上是null，也不知道内存的位置
     - int [] c = new int[2]; c有两个元素，都是0
     - int d [] =new int []{0,1,2}  //d有三个元素，同时进行定义和初始化
     - int e[] = {1,3,4}  //同时进行定义和初始化
   - 数组索引
     - 数组的length属性标识数组的长度
     - 索引 从  0  到 length -1
     - 数组不能越界访问，否则报异常 ArrayIndexOutOfBoundsException异常
   - 数组遍历：
     - 需要索引 for(int i = 0 ; i  < d.length; i++)
     - 不需要索引 for(int e : d){System.out.println(e)}

   - 多维数组
     - 是数组的数组
     - 储存是按照行储存的原则
     - 规则数据 int a [] [] = new int [2] [3]
     - 不规则数组 int b [] []; b = new int [3] []; b[0] = new int [3] ;  b[1] = new int [4] ; b[2] = new int [5];

2. JCF

   - 容器：能够存放数据的空间结构
     - 数组/多维数据  ，只能线性存放
     - 列表/散列集/树……
   - 容器框架：为表示和操作容器而规定的一种标准体系结构
     - 对外的接口：容器中所能存放的抽象数据类型
     - 接口的实现： 可复用的数据结构
     - 算法：对数据的查找和排序
   - 容器框架优点：提高储存效率，避免程序员重复劳动
   - C++的STL，Java的JCF
   - JCF集合接口Collection
     - add,contains,remove,size
     - iterator
   - JCF迭代器接口 Iterator
     - hasNext
     - next
     - remove
   - JCF主要数据结构实现类
     - 列表（List,ArrayList,LinkedList）
     - 集合（Set, HashSet,TreeSet,LinkedHashSet）
     - 映射（Map,HashMap,TreeMap,LinkedHashMap）
   - JCF 主要算法类
     - Arrays：对数组进行查找或者排序等操作
     - Collection：对Collection 及其子类进行排序和查找操作

3. 列表List

   - List : 列表
     - 有序的Collection
     - 允许重复的元素
     - {1，2，4，{5，2}，1，3}
   - List 主要实现
     - ArrayList （非同步）
     - LinkedList（非同步）
     - Vector （同步）
   - ArrayList:
     - 以数组形式实现的列表，不支持同步
     - 利用索引位置可以快速定位访问
     - 不适合指定位置的插入，删除操作(可能有性能上的损耗，但是可以这么做)
     - 适合变动不大，用于查询的数据
     - 与 JAVA 的数组相比其容量可以动态调整
     - ArrayList 在元素填满容器时，会自动扩充容器大小的50%

> Integer 将int 和 object 融合在一起，Integer 对象包含了一个int 字段。Integer对象提供了关于int 处理的很多方法，列如 int 和String的相互转换。   

