using System;
using AltBeaconOrg.BoundBeacon.Utils;
using Java.Util;
using NUnit.Framework;

namespace AndroidAltBeaconLibrary.UnitTests
{
	[TestFixture]
	public class UrlBeaconUrlCompressorTest : TestBase
	{
		static char[] hexArray = "0123456789ABCDEF".ToCharArray();

	    /**
	     * URLs to test:
	     * <p/>
	     * http://www.radiusnetworks.com
	     * https://www.radiusnetworks.com
	     * http://radiusnetworks.com
	     * https://radiusnetworks.com
	     * https://radiusnetworks.com/
	     * https://radiusnetworks.com/v1/index.html
	     * https://api.v1.radiusnetworks.com
	     * https://www.api.v1.radiusnetworks.com
	     */
	    [Test]
	    public void testCompressURL() {
	        String testURL = "http://www.radiusnetworks.com";
	        byte[] expectedBytes = {0x00, 
	        	Convert.ToByte('r'), 
	        	Convert.ToByte('a'), 
	        	Convert.ToByte('d'), 
	        	Convert.ToByte('i'), 
	        	Convert.ToByte('u'), 
	        	Convert.ToByte('s'), 
	        	Convert.ToByte('n'), 
	        	Convert.ToByte('e'), 
	        	Convert.ToByte('t'), 
	        	Convert.ToByte('w'), 
	        	Convert.ToByte('o'), 
	        	Convert.ToByte('r'), 
	        	Convert.ToByte('k'), 
	        	Convert.ToByte('s'), 
	        	0x07};
	        Assert.True(Arrays.Equals(expectedBytes, UrlBeaconUrlCompressor.Compress(testURL)));
	    }
	
	    [Test]
	    public void testCompressHttpsURL() {
	        String testURL = "https://www.radiusnetworks.com";
	        byte[] expectedBytes = {0x01, 
	        Convert.ToByte('r'), 
	        	Convert.ToByte('a'), 
	        	Convert.ToByte('d'), 
	        	Convert.ToByte('i'), 
	        	Convert.ToByte('u'), 
	        	Convert.ToByte('s'), 
	        	Convert.ToByte('n'), 
	        	Convert.ToByte('e'), 
	        	Convert.ToByte('t'), 
	        	Convert.ToByte('w'), 
	        	Convert.ToByte('o'), 
	        	Convert.ToByte('r'), 
	        	Convert.ToByte('k'), 
	        	Convert.ToByte('s'),
	        	0x07};
	        Assert.True(Arrays.Equals(expectedBytes, UrlBeaconUrlCompressor.Compress(testURL)));
	    }
	
	    [Test]
	    public void testCompressWithTrailingSlash() {
	        String testURL = "http://google.com/123";
	        byte[] expectedBytes = {0x02, 
	        	Convert.ToByte('g'), 
	        	Convert.ToByte('o'), 
	        	Convert.ToByte('o'), 
	        	Convert.ToByte('g'), 
	        	Convert.ToByte('l'), 
	        	Convert.ToByte('e'), 
	        	0x00, 
	        	Convert.ToByte('1'), 
	        	Convert.ToByte('2'), 
	        	Convert.ToByte('3')};
	        Assert.True(Arrays.Equals(expectedBytes, UrlBeaconUrlCompressor.Compress(testURL)));
	    }
	
	    [Test]
	    public void testCompressWithoutTLD() {
	        String testURL = "http://xxx";
	        byte[] expectedBytes = {0x02, 
	        	Convert.ToByte('x'), 
	        	Convert.ToByte('x'), 
	        	Convert.ToByte('x')};
	        Assert.True(Arrays.Equals(expectedBytes, UrlBeaconUrlCompressor.Compress(testURL)));
	    }
	
	    [Test]
	    public void testCompressWithSubdomains() {
	        String testURL = "http://www.forums.google.com";
	        byte[] expectedBytes = {0x00, 
	        	Convert.ToByte('f'), 
	        	Convert.ToByte('o'), 
	        	Convert.ToByte('r'), 
	        	Convert.ToByte('u'), 
	        	Convert.ToByte('m'), 
	        	Convert.ToByte('s'), 
	        	Convert.ToByte('.'),
	        	Convert.ToByte('g'), 
	        	Convert.ToByte('o'), 
	        	Convert.ToByte('o'), 
	        	Convert.ToByte('g'), 
	        	Convert.ToByte('l'), 
	        	Convert.ToByte('e'),
	        0x07};
	        Assert.True(Arrays.Equals(expectedBytes, UrlBeaconUrlCompressor.Compress(testURL)));
	    }
	
	    [Test]
	    public void testCompressWithSubdomainsWithTrailingSlash() {
	        String testURL = "http://www.forums.google.com/";
	        byte[] expectedBytes = {0x00, 
	        	Convert.ToByte('f'), 
	        	Convert.ToByte('o'), 
	        	Convert.ToByte('r'), 
	        	Convert.ToByte('u'), 
	        	Convert.ToByte('m'), 
	        	Convert.ToByte('s'), 
	        	Convert.ToByte('.'),
	        	Convert.ToByte('g'), 
	        	Convert.ToByte('o'), 
	        	Convert.ToByte('o'), 
	        	Convert.ToByte('g'), 
	        	Convert.ToByte('l'), 
	        	Convert.ToByte('e'), 
	        	0x00};
	        Assert.True(Arrays.Equals(expectedBytes, UrlBeaconUrlCompressor.Compress(testURL)));
	    }
	
	    [Test]
	    public void testCompressWithMoreSubdomains() {
	        String testURL = "http://www.forums.developer.google.com/123";
	        byte[] expectedBytes = {0x00, 
	        	Convert.ToByte('f'), 
	        	Convert.ToByte('o'), 
	        	Convert.ToByte('r'), 
	        	Convert.ToByte('u'), 
	        	Convert.ToByte('m'), 
	        	Convert.ToByte('s'), 
	        	Convert.ToByte('.'),
	        	Convert.ToByte('d'), 
	        	Convert.ToByte('e'), 
	        	Convert.ToByte('v'), 
	        	Convert.ToByte('e'), 
	        	Convert.ToByte('l'), 
	        	Convert.ToByte('o'),
	        	Convert.ToByte('p'), 
	        	Convert.ToByte('e'), 
	        	Convert.ToByte('r'),  
	        	Convert.ToByte('.'),
	        	Convert.ToByte('g'), 
	        	Convert.ToByte('o'), 
	        	Convert.ToByte('o'), 
	        	Convert.ToByte('g'), 
	        	Convert.ToByte('l'), 
	        	Convert.ToByte('e'),
	        	0x00, 
	        	Convert.ToByte('1'), 
	        	Convert.ToByte('2'), 
	        	Convert.ToByte('3')};
	        Assert.True(Arrays.Equals(expectedBytes, UrlBeaconUrlCompressor.Compress(testURL)));
	    }
	
	    [Test]
	    public void testCompressWithSubdomainsAndSlashesInPath() {
	        String testURL = "http://www.forums.google.com/123/456";
	        byte[] expectedBytes = {0x00, (byte)'f', (byte)'o', (byte)'r', (byte)'u', (byte)'m', (byte)'s', (byte)'.', (byte)'g', (byte)'o', (byte)'o', (byte)'g', (byte)'l', (byte)'e', 0x00, (byte)'1', (byte)'2', (byte)'3', (byte)'/', (byte)'4', (byte)'5', (byte)'6'};
	        Assert.True(Arrays.Equals(expectedBytes, UrlBeaconUrlCompressor.Compress(testURL)));
	    }
	
	    [Test]
	    public void testCompressWithDotCaTLD() {
	        String testURL = "http://google.ca";
	        byte[] expectedBytes = {0x02, (byte)'g', (byte)'o', (byte)'o', (byte)'g', (byte)'l', (byte)'e', (byte)'.', (byte)'c', (byte)'a'};
	        Assert.True(Arrays.Equals(expectedBytes, UrlBeaconUrlCompressor.Compress(testURL)));
	    }
	
	    [Test]
	    public void testCompressWithDotInfoTLD() {
	        String testURL = "http://google.info";
	        byte[] expectedBytes = {0x02, (byte)'g', (byte)'o', (byte)'o', (byte)'g', (byte)'l', (byte)'e', 0x0b};
	        Assert.True(Arrays.Equals(expectedBytes, UrlBeaconUrlCompressor.Compress(testURL)));
	    }
	
	    [Test]
	    public void testCompressWithDotCaTLDWithSlash() {
	        String testURL = "http://google.ca/";
	        byte[] expectedBytes = {0x02, (byte)'g', (byte)'o', (byte)'o', (byte)'g', (byte)'l', (byte)'e', (byte)'.', (byte)'c', (byte)'a', (byte)'/'};
	        Assert.True(Arrays.Equals(expectedBytes, UrlBeaconUrlCompressor.Compress(testURL)));
	    }
	
	    [Test]
	    public void testCompressWithDotCoTLD() {
	        String testURL = "http://google.co";
	        byte[] expectedBytes = {0x02, (byte)'g', (byte)'o', (byte)'o', (byte)'g', (byte)'l', (byte)'e', (byte)'.', (byte)'c', (byte)'o'};
	        String hexBytes = bytesToHex(UrlBeaconUrlCompressor.Compress(testURL));
	        Assert.True(Arrays.Equals(expectedBytes, UrlBeaconUrlCompressor.Compress(testURL)));
	    }
	
	    [Test]
	    public void testCompressWithShortenedURLContainingCaps() {
	        String testURL = "http://goo.gl/C2HC48";
	        byte[] expectedBytes = {0x02, (byte)'g', (byte)'o', (byte)'o', (byte)'.', (byte)'g', (byte)'l', (byte)'/', (byte)'C', (byte)'2', (byte)'H', (byte)'C', (byte)'4', (byte)'8'};
	        String hexBytes = bytesToHex(UrlBeaconUrlCompressor.Compress(testURL));
	        Assert.True(Arrays.Equals(expectedBytes, UrlBeaconUrlCompressor.Compress(testURL)));
	    }
	
	    [Test]
	    public void testCompressWithSchemeInCaps() {
	        String testURL = "HTTP://goo.gl/C2HC48";
	        byte[] expectedBytes = {0x02, (byte)'g', (byte)'o', (byte)'o', (byte)'.', (byte)'g', (byte)'l', (byte)'/', (byte)'C', (byte)'2', (byte)'H', (byte)'C', (byte)'4', (byte)'8'};
	        String hexBytes = bytesToHex(UrlBeaconUrlCompressor.Compress(testURL));
	        Assert.True(Arrays.Equals(expectedBytes, UrlBeaconUrlCompressor.Compress(testURL)));
	    }
	
	    [Test]
	    public void testCompressWithDomainInCaps() {
	        String testURL = "http://GOO.GL/C2HC48";
	        byte[] expectedBytes = {0x02, (byte)'g', (byte)'o', (byte)'o', (byte)'.', (byte)'g', (byte)'l', (byte)'/', (byte)'C', (byte)'2', (byte)'H', (byte)'C', (byte)'4', (byte)'8'};
	        String hexBytes = bytesToHex(UrlBeaconUrlCompressor.Compress(testURL));
	        Assert.True(Arrays.Equals(expectedBytes, UrlBeaconUrlCompressor.Compress(testURL)));
	    }
	
	    [Test]
	    public void testCompressHttpsAndWWWInCaps() {
	        String testURL = "HTTPS://WWW.radiusnetworks.com";
	        byte[] expectedBytes = {0x01, (byte)'r', (byte)'a', (byte)'d', (byte)'i', (byte)'u', (byte)'s', (byte)'n', (byte)'e', (byte)'t', (byte)'w', (byte)'o', (byte)'r', (byte)'k', (byte)'s', 0x07};
	        Assert.True(Arrays.Equals(expectedBytes, UrlBeaconUrlCompressor.Compress(testURL)));
	    }
	
	    [Test]
	    public void testCompressEntireURLInCaps() {
	        String testURL = "HTTPS://WWW.RADIUSNETWORKS.COM";
	        byte[] expectedBytes = {0x01, (byte)'r', (byte)'a', (byte)'d', (byte)'i', (byte)'u', (byte)'s', (byte)'n', (byte)'e', (byte)'t', (byte)'w', (byte)'o', (byte)'r', (byte)'k', (byte)'s', 0x07};
	        Assert.True(Arrays.Equals(expectedBytes, UrlBeaconUrlCompressor.Compress(testURL)));
	    }
	
	    [Test]
	    public void testCompressEntireURLInCapsWithPath() {
	        String testURL = "HTTPS://WWW.RADIUSNETWORKS.COM/C2HC48";
	        byte[] expectedBytes = {0x01, (byte)'r', (byte)'a', (byte)'d', (byte)'i', (byte)'u', (byte)'s', (byte)'n', (byte)'e', (byte)'t', (byte)'w', (byte)'o', (byte)'r', (byte)'k', (byte)'s', 0x00, (byte)'C', (byte)'2', (byte)'H', (byte)'C', (byte)'4', (byte)'8'};
	        Assert.True(Arrays.Equals(expectedBytes, UrlBeaconUrlCompressor.Compress(testURL)));
	    }
	
	    [Test]
	    public void testDecompressWithDotCoTLD() {
	        String testURL = "http://google.co";
	        byte[] testBytes = {0x02, (byte)'g', (byte)'o', (byte)'o', (byte)'g', (byte)'l', (byte)'e', (byte)'.', (byte)'c', (byte)'o'};
	        Assert.AreEqual(testURL, UrlBeaconUrlCompressor.Uncompress(testBytes));
	    }
	
	    [Test]
	    public void testDecompressWithPath() {
	        String testURL = "http://google.com/123";
	        byte[] testBytes = {0x02, (byte)'g', (byte)'o', (byte)'o', (byte)'g', (byte)'l', (byte)'e', 0x00, (byte)'1', (byte)'2', (byte)'3'};
	        Assert.AreEqual(testURL, UrlBeaconUrlCompressor.Uncompress(testBytes));
	    }
	
	    [Test]
	    public void testUncompressHttpsURL() {
	        String testURL = "https://www.radiusnetworks.com";
	        byte[] testBytes = {0x01, (byte)'r', (byte)'a', (byte)'d', (byte)'i', (byte)'u', (byte)'s', (byte)'n', (byte)'e', (byte)'t', (byte)'w', (byte)'o', (byte)'r', (byte)'k', (byte)'s', 0x07};
	        Assert.AreEqual(testURL, UrlBeaconUrlCompressor.Uncompress(testBytes));
	    }
	
	    [Test]
	    public void testUncompressHttpsURLWithTrailingSlash() {
	        String testURL = "https://www.radiusnetworks.com/";
	        byte[] testBytes = {0x01, (byte)'r', (byte)'a', (byte)'d', (byte)'i', (byte)'u', (byte)'s', (byte)'n', (byte)'e', (byte)'t', (byte)'w', (byte)'o', (byte)'r', (byte)'k', (byte)'s', 0x00};
	        Assert.AreEqual(testURL, UrlBeaconUrlCompressor.Uncompress(testBytes));
	    }
	
	    [Test]
	    public void testUncompressWithoutTLD() {
	        String testURL = "http://xxx";
	        byte[] testBytes = {0x02, (byte)'x', (byte)'x', (byte)'x'};
	        Assert.AreEqual(testURL, UrlBeaconUrlCompressor.Uncompress(testBytes));
	    }
	
	    [Test]
	    public void testUncompressWithSubdomains() {
	        String testURL = "http://www.forums.google.com";
	        byte[] testBytes = {0x00, (byte)'f', (byte)'o', (byte)'r', (byte)'u', (byte)'m', (byte)'s', (byte)'.', (byte)'g', (byte)'o', (byte)'o', (byte)'g', (byte)'l', (byte)'e', 0x07};
	        Assert.AreEqual(testURL, UrlBeaconUrlCompressor.Uncompress(testBytes));
	    }
	
	    [Test]
	    public void testUncompressWithSubdomainsAndTrailingSlash() {
	        String testURL = "http://www.forums.google.com/";
	        byte[] testBytes = {0x00, (byte)'f', (byte)'o', (byte)'r', (byte)'u', (byte)'m', (byte)'s', (byte)'.', (byte)'g', (byte)'o', (byte)'o', (byte)'g', (byte)'l', (byte)'e', 0x00};
	        Assert.AreEqual(testURL, UrlBeaconUrlCompressor.Uncompress(testBytes));
	    }
	
	    [Test]
	    public void testUncompressWithSubdomainsAndSlashesInPath() {
	        String testURL = "http://www.forums.google.com/123/456";
	        byte[] testBytes = {0x00, (byte)'f', (byte)'o', (byte)'r', (byte)'u', (byte)'m', (byte)'s', (byte)'.', (byte)'g', (byte)'o', (byte)'o', (byte)'g', (byte)'l', (byte)'e', 0x00, (byte)'1', (byte)'2', (byte)'3', (byte)'/', (byte)'4', (byte)'5', (byte)'6'};
	        Assert.AreEqual(testURL, UrlBeaconUrlCompressor.Uncompress(testBytes));
	    }
	
	    [Test]
	    public void testUncompressWithDotInfoTLD() {
	        String testURL = "http://google.info";
	        byte[] testBytes = {0x02, (byte)'g', (byte)'o', (byte)'o', (byte)'g', (byte)'l', (byte)'e', 0x0b};
	        Assert.AreEqual(testURL, UrlBeaconUrlCompressor.Uncompress(testBytes));
	    }
	    
	    String bytesToHex(byte[] bytes) {
	        char[] hexChars = new char[bytes.Length * 2];
	        for (int j = 0; j < bytes.Length; j++) {
	            int v = bytes[j] & 0xFF;
	            hexChars[j * 2] = hexArray[v >> 4];
	            hexChars[j * 2 + 1] = hexArray[v & 0x0F];
	        }
	        return new String(hexChars);
	    }	
	}
}
