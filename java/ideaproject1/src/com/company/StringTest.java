package com.company;

import java.util.Calendar;

public class StringTest {
    public static void main(String[] args)
    {
        String str1  = "123";
        String str2  = str1;
        str2 += "4";
        System.out.println(str1);

        StringBuffer sb1 = new StringBuffer("123");
        System.out.println(sb1);
        StringBuffer sb2 = sb1;
        sb2.append('4');
        System.out.println(sb1);

        char c1 = 97;
        System.out.println(c1);

        //测试时间比较
        int n = 8000;
        Calendar t1 = Calendar.getInstance();
        String a = new String();
        for (int i = 0; i < n; i++)
        {
            a = a + i + ",";
        }
        System.out.println(Calendar.getInstance().getTimeInMillis() - t1.getTimeInMillis());

        Calendar t2 = Calendar.getInstance();
        StringBuffer sbf = new StringBuffer();
        for (int i = 0; i < n; i++)
        {
           sbf.append(i);
           sbf.append(",");
        }
        System.out.println(Calendar.getInstance().getTimeInMillis() - t2.getTimeInMillis());

        Calendar t3 = Calendar.getInstance();
        StringBuilder sbd = new StringBuilder();
        for (int i = 0; i < n; i++)
        {
            sbd.append(i);
            sbd.append(",");
        }
        System.out.println(Calendar.getInstance().getTimeInMillis() - t3.getTimeInMillis());
        //如何分配内存   length 是实际长度  capacity 存储空间
        StringBuffer sb3 = new StringBuffer();
        System.out.println("length :"+sb3.length());
        System.out.println("capacity : "+ sb3.capacity());

        StringBuffer sb4 = new StringBuffer("123");
        sb4.append("456");
        System.out.println("length :"+sb4.length());
        System.out.println("capacity : "+ sb4.capacity());

        sb4.append("7890123456789");
        System.out.println("length :"+sb4.length());
        System.out.println("capacity : "+ sb4.capacity());

        sb4.append("01");
        System.out.println("length :"+sb4.length());
        System.out.println("capacity : "+ sb4.capacity());
    }
}
