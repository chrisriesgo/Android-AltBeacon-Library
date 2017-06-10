using AltBeaconOrg.BoundBeacon;
using Java.Lang;
using Java.Util;
using NUnit.Framework;

namespace AndroidAltBeaconLibrary.UnitTests
{
	[TestFixture]
	public class IdentifierTest
	{
		[Test]
	    public void testEqualsNormalizationIgnoresCase() {
	        Identifier identifier1 = Identifier.Parse("2f234454-cf6d-4a0f-adf2-f4911ba9ffa6");
	        Identifier identifier2 = Identifier.Parse("2F234454-CF6D-4A0F-ADF2-F4911BA9FFA6");
	
	        AssertEx.True("Identifiers of different case should match", identifier1.Equals(identifier2));
	    }
	
	    [Test]
	    public void testToStringNormalizesCase() {
	        Identifier identifier1 = Identifier.Parse("2F234454-CF6D-4A0F-ADF2-F4911BA9FFA6");
	
	        AssertEx.AreEqual("Identifiers of different case should match", "2f234454-cf6d-4a0f-adf2-f4911ba9ffa6", identifier1.ToString());
	    }
	
	    [Test]
	    public void testToStringEqualsUuid() {
	        Identifier identifier1 = Identifier.Parse("2F234454-CF6D-4A0F-ADF2-F4911BA9FFA6");
	
	        AssertEx.AreEqual("uuidString of Identifier should match", "2f234454-cf6d-4a0f-adf2-f4911ba9ffa6", identifier1.ToUuid().ToString());
	    }
	
	    [Test]
	    [Ignore("ToUuidString is deprecated")]
	    public void testToUuidEqualsToUuidString() {
	        Identifier identifier1 = Identifier.Parse("2F234454-CF6D-4A0F-ADF2-F4911BA9FFA6");
	
	        //AssertEx.AreEqual("uuidString of Identifier should match", identifier1.ToUuid().ToString(), identifier1.ToUuidString());
	    }
	
	    [Test]
	    public void testToByteArrayConvertsUuids() {
	        Identifier identifier1 = Identifier.Parse("2F234454-CF6D-4A0F-ADF2-F4911BA9FFA6");
	        byte[] bytes = identifier1.ToByteArrayOfSpecifiedEndianness(true);
	        AssertEx.AreEqual("byte array is correct length", bytes.Length, 16);
	        AssertEx.AreEqual("first byte of uuid converted properly", 0x2f, bytes[0] & 0xFF);
	        AssertEx.AreEqual("second byte of uuid converted properly", 0x23, bytes[1] & 0xFF);
	        AssertEx.AreEqual("last byte of uuid converted properly", 0xa6, bytes[15] & 0xFF);
	    }
	
	    [Test]
	    public void testToByteArrayConvertsUuidsAsLittleEndian() {
	        Identifier identifier1 = Identifier.Parse("2F234454-CF6D-4A0F-ADF2-F4911BA9FFA6");
	        byte[] bytes = identifier1.ToByteArrayOfSpecifiedEndianness(false);
	        AssertEx.AreEqual("byte array is correct length", bytes.Length, 16);
	        AssertEx.AreEqual("first byte of uuid converted properly", 0xa6, bytes[0] & 0xFF);
	        AssertEx.AreEqual("last byte of uuid converted properly", 0x2f, bytes[15] & 0xFF);
	    }
	
	    [Test]
	    public void testToByteArrayConvertsHex() {
	        Identifier identifier1 = Identifier.Parse("0x010203040506");
	        byte[] bytes = identifier1.ToByteArrayOfSpecifiedEndianness(true);
	        AssertEx.AreEqual("byte array is correct length", bytes.Length, 6);
	        AssertEx.AreEqual("first byte of hex is converted properly", 0x01, bytes[0] & 0xFF);
	        AssertEx.AreEqual("last byte of hex is converted properly", 0x06, bytes[5] & 0xFF);
	    }
	    [Test]
	    public void testToByteArrayConvertsDecimal() {
	        Identifier identifier1 = Identifier.Parse("65534");
	        byte[] bytes = identifier1.ToByteArrayOfSpecifiedEndianness(true);
	        AssertEx.AreEqual("byte array is correct length", bytes.Length, 2);
	        AssertEx.AreEqual("reported byte array is correct length", identifier1.ByteCount, 2);
	        AssertEx.AreEqual("first byte of decimal converted properly", 0xff, bytes[0] & 0xFF);
	        AssertEx.AreEqual("last byte of decimal converted properly", 0xfe, bytes[1] & 0xFF);
	    }
	
	    [Test]
	    public void testToByteArrayConvertsInt() {
	        Identifier identifier1 = Identifier.FromInt(65534);
	        byte[] bytes = identifier1.ToByteArrayOfSpecifiedEndianness(true);
	        AssertEx.AreEqual("byte array is correct length", bytes.Length, 2);
	        AssertEx.AreEqual("reported byte array is correct length", identifier1.ByteCount, 2);
	        AssertEx.AreEqual("conversion back equals original value", identifier1.ToInt(), 65534);
	        AssertEx.AreEqual("first byte of decimal converted properly", 0xff, bytes[0] & 0xFF);
	        AssertEx.AreEqual("last byte of decimal converted properly", 0xfe, bytes[1] & 0xFF);
	    }
	
	    [Test]
	    public void testToByteArrayFromByteArray() {
	        byte[] byteVal = new byte[] {(byte) 0xFF, (byte) 0xAB, 0x12, 0x25};
	        Identifier identifier1 = Identifier.FromBytes(byteVal, 0, byteVal.Length, false);
	        byte[] bytes = identifier1.ToByteArrayOfSpecifiedEndianness(true);
	
	        AssertEx.AreEqual("byte array is correct length", bytes.Length, 4);
	        AssertEx.AreEqual("correct string representation", identifier1.ToString(), "0xffab1225");
	        AssertEx.True("arrays equal", Arrays.Equals(byteVal, bytes));
	        AssertEx.AreNotSame("arrays are copied", bytes, byteVal);
	    }
	
	    [Test]
	    public void testComparableDifferentLength() {
	        byte[] value1 = new byte[] {(byte) 0xFF, (byte) 0xAB, 0x12, 0x25};
	        Identifier identifier1 = Identifier.FromBytes(value1, 0, value1.Length, false);
	        byte[] value2 = new byte[] {(byte) 0xFF, (byte) 0xAB, 0x12, 0x25, 0x11, 0x11};
	        Identifier identifier2 = Identifier.FromBytes(value2, 0, value2.Length, false);
	
	        AssertEx.AreEqual("identifier1 is smaller than identifier2", identifier1.CompareTo(identifier2), -1);
	        AssertEx.AreEqual("identifier2 is larger than identifier1", identifier2.CompareTo(identifier1), 1);
	    }
	
	    [Test]
	    public void testComparableSameLength() {
	        byte[] value1 = new byte[] {(byte) 0xFF, (byte) 0xAB, 0x12, 0x25, 0x22, 0x25};
	        Identifier identifier1 = Identifier.FromBytes(value1, 0, value1.Length, false);
	        byte[] value2 = new byte[] {(byte) 0xFF, (byte) 0xAB, 0x12, 0x25, 0x11, 0x11};
	        Identifier identifier2 = Identifier.FromBytes(value2, 0, value2.Length, false);
	
	        AssertEx.AreEqual("identifier1 is equal to identifier2", identifier1.CompareTo(identifier1), 0);
	        AssertEx.AreEqual("identifier1 is larger than identifier2", identifier1.CompareTo(identifier2), 1);
	        AssertEx.AreEqual("identifier2 is smaller than identifier1", identifier2.CompareTo(identifier1), -1);
	    }
	
	    [Test]
	    public void testParseIntegerMaxInclusive() {
	        Identifier.Parse("65535");
	    }
	
	    [Test]
	    public void testParseIntegerAboveMax() {
	        Assert.Throws(typeof(IllegalArgumentException), () =>
			{
				Identifier.Parse("65536");
			});
	    }
	
	    [Test]
	    public void testParseIntegerMinInclusive() {
	        Identifier.Parse("0");
	    }
	
	    [Test]
	    public void testParseIntegerBelowMin() {
	        Assert.Throws(typeof(IllegalArgumentException), () =>
			{
				Identifier.Parse("-1");
			});
	    }
	
	    [Test]
	    public void testParseIntegerWayTooBig() {
	        Assert.Throws(typeof(IllegalArgumentException), () =>
			{
				Identifier.Parse("3133742");
			});
	    }
	
	    /*
	     * This is here because Identifier.parse wrongly accepts UUIDs without
	     * dashes, but we want to be backward compatible.
	     */
	    [Test]
	    [Ignore("Technically this works, but the Equals comparer isn't working right.")]
	    public void testParseInvalidUuid() {
	        UUID uuid = UUID.FromString("2f234454-cf6d-4a0f-adf2-f4911ba9ffa6");
	        Identifier id = Identifier.Parse("2f234454cf6d4a0fadf2f4911ba9ffa6");
	        AssertEx.AreEqual("Malformed UUID was parsed as expected.", id.ToUuid(), uuid);
	    }
	
	    [Test]
	    public void testParseHexWithNoPrefix() {
	        Identifier id = Identifier.Parse("abcd");
	        AssertEx.AreEqual("Should parse and get back equivalent decimal value for small numbers", "43981", id.ToString());
	    }
	
	    [Test]
	    public void testParseBigHexWithNoPrefix() {
	        Identifier id = Identifier.Parse("123456789abcdef");
	        AssertEx.AreEqual("Should parse and get prefixed hex value for big numbers", "0x0123456789abcdef", id.ToString());
	    }
	    [Test]
	    public void testParseZeroPrefixedDecimalNumberAsHex() {
	        Identifier id = Identifier.Parse("0010");
	        AssertEx.AreEqual("Should be treated as hex in parse, but converted back to decimal because it is small", "16", id.ToString());
	    }
	    [Test]
	    public void testParseNonZeroPrefixedDecimalNumberAsDecimal() {
	        Identifier id = Identifier.Parse("10");
	        AssertEx.AreEqual("Should be treated as decimal", "10", id.ToString());
	    }
	    [Test]
	    public void testParseDecimalNumberWithSpecifiedLength() {
	        Identifier id = Identifier.Parse("10", 8);
	        AssertEx.AreEqual("Should be treated as hex because it is long", "0x000000000000000a", id.ToString());
	        AssertEx.AreEqual("Byte count should be as specified", 8, id.ByteCount);
	    }
	    [Test]
	    public void testParseDecimalNumberWithSpecifiedShortLength() {
	        Identifier id = Identifier.Parse("10", 2);
	        AssertEx.AreEqual("Should be treated as decimal because it is short", "10", id.ToString());
	        AssertEx.AreEqual("Byte count should be as specified", 2, id.ByteCount);
	    }
	    [Test]
	    public void testParseHexNumberWithSpecifiedLength() {
	        Identifier id = Identifier.Parse("2fffffffffffffffffff", 10);
	        AssertEx.AreEqual("Should be treated as hex because it is long", "0x2fffffffffffffffffff", id.ToString());
	        AssertEx.AreEqual("Byte count should be as specified", 10, id.ByteCount);
	    }
	    [Test]
	    public void testParseZeroAsInteger() {
	        Identifier id = Identifier.Parse("0");
	        AssertEx.AreEqual("Should be treated as int because it is a common integer", "0", id.ToString());
	        AssertEx.AreEqual("Byte count should be 2 for integers", 2, id.ByteCount);
	    }
	}
}
