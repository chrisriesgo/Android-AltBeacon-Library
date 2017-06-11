using System;
using System.Collections.Generic;
using System.Linq;
using AltBeaconOrg.BoundBeacon;
using AltBeaconOrg.BoundBeacon.Service.Scanner;
using NUnit.Framework;

namespace AndroidAltBeaconLibrary.UnitTests
{
	[TestFixture]
	public class ScanFilterUtilsTest
	{
		[Test]
	    public void testGetAltBeaconScanFilter() {
	        BeaconParser parser = new AltBeaconParser();
	        BeaconManager.SetsManifestCheckingDisabled(true); // no manifest available in robolectric
	        var scanFilterDatas = new ScanFilterUtils().CreateScanFilterDataForBeaconParser(parser);
	        AssertEx.AreEqual("scanFilters should be of correct size", 1, scanFilterDatas.Count);
	        ScanFilterUtils.ScanFilterData sfd = scanFilterDatas[0];
	        AssertEx.AreEqual("manufacturer should be right", 0x0118, sfd.Manufacturer);
	        AssertEx.AreEqual("mask length should be right", 2, sfd.Mask.Count);
	        AssertEx.AreEqual("mask should be right", new byte[] {(byte)0xff, (byte)0xff}, sfd.Mask);
	        AssertEx.AreEqual("filter should be right", new byte[] {(byte)0xbe, (byte)0xac}, sfd.Filter);
	    }
	    [Test]
	    public void testGenericScanFilter() {
	        BeaconParser parser = new BeaconParser();
	        parser.SetBeaconLayout("m:2-3=1111,i:4-6,p:24-24");
	        BeaconManager.SetsManifestCheckingDisabled(true); // no manifest available in robolectric
	        var scanFilterDatas = new ScanFilterUtils().CreateScanFilterDataForBeaconParser(parser);
	        AssertEx.AreEqual("scanFilters should be of correct size", 1, scanFilterDatas.Count);
	        ScanFilterUtils.ScanFilterData sfd = scanFilterDatas[0];
	        AssertEx.AreEqual("manufacturer should be right", 0x004c, sfd.Manufacturer);
	        AssertEx.AreEqual("mask length should be right", 2, sfd.Mask.Count);
	        AssertEx.AreEqual("mask should be right", new byte[]{(byte) 0xff, (byte) 0xff}, sfd.Mask.ToArray());
	        AssertEx.AreEqual("filter should be right", new byte[] {(byte)0x11, (byte) 0x11}, sfd.Filter.ToArray());
	        AssertEx.Null("serviceUuid should be null", sfd.ServiceUuid);
	    }
	    [Test]
	    public void testEddystoneScanFilterData() {
	        BeaconParser parser = new BeaconParser();
	        parser.SetBeaconLayout(BeaconParser.EddystoneUidLayout);
	        BeaconManager.SetsManifestCheckingDisabled(true); // no manifest available in robolectric
	        var scanFilterDatas = new ScanFilterUtils().CreateScanFilterDataForBeaconParser(parser);
	        AssertEx.AreEqual("scanFilters should be of correct size", 1, scanFilterDatas.Count);
	        ScanFilterUtils.ScanFilterData sfd = scanFilterDatas[0];
	        AssertEx.AreEqual("serviceUuid should be right", new Java.Lang.Long(0xfeaa).LongValue(), sfd.ServiceUuid.LongValue());
	    }
	
	    [Test]
	    public void testZeroOffsetScanFilter() {
	        BeaconParser parser = new BeaconParser();
	        parser.SetBeaconLayout("m:0-3=11223344,i:4-6,p:24-24");
	        BeaconManager.SetsManifestCheckingDisabled(true); // no manifest available in robolectric
	        var scanFilterDatas = new ScanFilterUtils().CreateScanFilterDataForBeaconParser(parser);
	        AssertEx.AreEqual("scanFilters should be of correct size", 1, scanFilterDatas.Count);
	        ScanFilterUtils.ScanFilterData sfd = scanFilterDatas[0];
	        AssertEx.AreEqual("manufacturer should be right", 0x004c, sfd.Manufacturer);
	        AssertEx.AreEqual("mask length should be right", 2, sfd.Mask.Count);
	        AssertEx.AreEqual("mask should be right", new byte[] {(byte)0xff, (byte)0xff}, sfd.Mask.ToArray());
	        AssertEx.AreEqual("filter should be right", new byte[] {(byte)0x33, (byte)0x44}, sfd.Filter.ToArray());
	    }
	}
}
