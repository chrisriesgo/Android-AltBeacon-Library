using System;
using System.Text;

namespace AndroidAltBeaconLibrary.UnitTests
{
	public class TestBase
	{
		public static byte[] HexStringToByteArray(String s) 
		{
			int len = s.Length;
			var data = new byte[len / 2];
			for (int i = 0; i < len; i += 2) 
			{
				data[i / 2] = (byte)((Convert.ToByte(s.Substring(i, 1), 16) << 4) + (Convert.ToByte(s.Substring(i + 1, 1), 16)));
			}
			return data;
    	}
    	
    	public static String ByteArrayToHexString(byte[] bytes) 
    	{
	        var sb = new StringBuilder();
	        for (int i = 0; i < bytes.Length; i++) {
	            sb.AppendFormat("{0:x2}", bytes[i]);
	        }
	        return sb.ToString();
	    }
	}
}