using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Java.IO;

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
	    
	    // utilty methods for testing serialization	
	    protected byte[] ConvertToBytes(Java.Lang.Object obj)
	    {
	     //   var bf = new BinaryFormatter();
		    //using (var ms = new MemoryStream())
		    //{
		    //    bf.Serialize(ms, obj);
		    //    return ms.ToArray();
		    //}
		    using(var bos = new MemoryStream())
		    using(var oos = new Java.IO.ObjectOutputStream(bos))
		    {
		    	oos.WriteObject(obj);
		    	return bos.ToArray();
		    }
	    }
	    
	    protected Java.Lang.Object ConvertFromBytes(byte[] bytes)
	    {
	        using (var ms = new MemoryStream())
	        using (var ois = new ObjectInputStream(ms))
		    {
				//var binForm = new BinaryFormatter();
				//memStream.Write(bytes, 0, bytes.Length);
				//memStream.Seek(0, SeekOrigin.Begin);
				//var obj = binForm.Deserialize(memStream);
				//return obj;
				return ois.ReadObject();
		    }
	    }
    }
}