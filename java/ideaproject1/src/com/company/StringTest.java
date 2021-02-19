package com.company;

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
    }
}
