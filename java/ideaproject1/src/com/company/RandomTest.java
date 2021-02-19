package com.company;

import java.util.Random;

public class RandomTest {
    public static void main(String[] arg)
    {
        Random rd = new Random();
        System.out.println(rd.nextBoolean());
        System.out.println(rd.nextDouble());
        System.out.println(rd.nextInt(100));//获得 0 ~ 100之间的随机数
        //生成一个范围内的随机数
        System.out.println(Math.round(Math.random() * 10)); //获得 0~10之间的数
        // 返回一个随机数组
        //rd.ints();
        int [] arr = rd.ints(10).toArray();
        for (int i = 0; i < arr.length;i++)
        {
            System.out.println(arr[i]);
        }
        arr = rd.ints(5,10,100).toArray();
        for (int i = 0 ; i < arr.length; i++)
        {
            System.out.println(arr[i]);
        }
    }
}
