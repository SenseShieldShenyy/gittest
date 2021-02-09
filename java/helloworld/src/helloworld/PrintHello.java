package helloworld;

public class PrintHello {
	public static void main(String[] args)
	{
		System.out.print("hello world");
		System.out.println("hello world");
		System.out.print("hello world");
		for (int i = 0 ;i < args.length;i++)
		{
			System.out.println(args[i]);
		}
	}

}
