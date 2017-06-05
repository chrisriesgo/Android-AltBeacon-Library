using System;
using AltBeaconOrg.BoundBeacon;
using NUnit.Framework;

namespace AndroidAltBeaconLibrary.UnitTests
{
	[TestFixture]
	public class BeaconParserTest : TestBase
	{
		[Test]
		public void testSetBeaconLayout() 
		{
	        var bytes = HexStringToByteArray("02011a1bffbeac2f234454cf6d4a0fadf2f4911ba9ffa600010002c509000000");
	        var parser = new BeaconParser();
	        parser.SetBeaconLayout("m:2-3=beac,i:4-19,i:20-21,i:22-23,p:24-24,d:25-25");
	
	        Assert.AreEqual(2, parser.MatchingBeaconTypeCodeStartOffset, "parser should get beacon type code start offset");
	        Assert.AreEqual(3, parser.MatchingBeaconTypeCodeEndOffset, "parser should get beacon type code end offset");
	        Assert.AreEqual(Convert.ToInt64(0xbeacL), Convert.ToInt64(parser.MatchingBeaconTypeCode), "parser should get beacon type code");
	        Assert.AreEqual(4, parser.IdentifierStartOffsets[0], "parser should get identifier start offset");
	        AssertEx.AreEqual("parser should get identifier end offset", 19, parser.IdentifierEndOffsets[0]);
	        AssertEx.AreEqual("parser should get identifier start offset", 20, parser.IdentifierStartOffsets[1]);
	        AssertEx.AreEqual("parser should get identifier end offset", 21, parser.IdentifierEndOffsets[1]);
	        AssertEx.AreEqual("parser should get identifier start offset", 22, parser.IdentifierStartOffsets[2]);
	        AssertEx.AreEqual("parser should get identifier end offset", 23, parser.IdentifierEndOffsets[2]);
	        AssertEx.AreEqual("parser should get power start offset", 24, Convert.ToInt32(parser.PowerStartOffset));
	        AssertEx.AreEqual("parser should get power end offset", 24, Convert.ToInt32(parser.PowerEndOffset));
	        AssertEx.AreEqual("parser should get data start offset", 25, parser.DataStartOffsets[0]);
	        AssertEx.AreEqual("parser should get data end offset", 25, parser.DataEndOffsets[0]);
	    }
	}
}