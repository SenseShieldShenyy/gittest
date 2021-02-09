package Test;

import java.util.UUID;

public class Test {
		String ttp = String.valueOf(System.currentTimeMillis());
			byte[] timeStamp = ttp.getBytes();
	String uuidStr = UUID.randomUUID().toString();
	byte[] uuid = uuidStr.getBytes("UTF-8");
	byte[] resourceURI = "/v2/sv/addUser".getBytes("UTF-8");

}
