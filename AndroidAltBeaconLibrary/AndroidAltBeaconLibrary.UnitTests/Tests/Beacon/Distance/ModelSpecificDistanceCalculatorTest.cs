using System;
using AltBeaconOrg.BoundBeacon.Distance;
using Android.Content;
using NUnit.Framework;

namespace AndroidAltBeaconLibrary.UnitTests
{
	[TestFixture]
	public class ModelSpecificDistanceCalculatorTest
	{
		[Test]
	    public void testCalculatesDistance() {	
	        ModelSpecificDistanceCalculator distanceCalculator = new ModelSpecificDistanceCalculator(null, null);
	        Double distance = distanceCalculator.CalculateDistance(-59, -59);
	        AssertEx.AreEqual("Distance should be 1.0 for same power and rssi", 1.0, distance, 0.1);
	    }
	
	    [Test]
	    public void testSelectsDefaultModel() {
	        ModelSpecificDistanceCalculator distanceCalculator = new ModelSpecificDistanceCalculator(null, null);
	        AssertEx.AreEqual("Default model should be Nexus 5", "Nexus 5", distanceCalculator.Model.Model);
	    }
	
	    [Test]
	    public void testSelectsNexus4OnExactMatch() {
	        AndroidModel model = new AndroidModel("4.4.2", "KOT49H","Nexus 4","LGE");
	
	        ModelSpecificDistanceCalculator distanceCalculator = new ModelSpecificDistanceCalculator(null, null, model);
	        AssertEx.AreEqual("should be Nexus 4", "Nexus 4", distanceCalculator.Model.Model);
	    }
	
		[Test]
		public void testCalculatesDistanceForMotoXPro() {
			Context applicationContext = Android.App.Application.Context;
	
			AndroidModel model = new AndroidModel("5.0.2", "LXG22.67-7.1", "Moto X Pro", "XT1115");
			ModelSpecificDistanceCalculator distanceCalculator = new ModelSpecificDistanceCalculator(applicationContext, null, model);
			AssertEx.AreEqual("should be Moto X Pro", "Moto X Pro", distanceCalculator.Model.Model);
			Double distance = distanceCalculator.CalculateDistance(-49, -58);
			AssertEx.AreEqual("Distance should be as predicted by coefficients at 3 meters", 2.661125466, distance, 0.1);
		}
	
		[Test]
		[Ignore("Can't test private methods")]
		public void testConcurrentModificationException() {
			//Context applicationContext = Android.App.Application.Context;
	
			//AndroidModel model = new AndroidModel("4.4.2", "KOT49H", "Nexus 4", "LGE");
			//String modelMapJson =
			//	"{\"models\":[ \"coefficient1\": 0.89976,\"coefficient2\": 7.7095,\"coefficient3\": 0.111," +
			//	"\"version\":\"4.4.2\",\"build_number\":\"KOT49H\",\"model\":\"Nexus 4\"," +
			//	"\"manufacturer\":\"LGE\"},{\"coefficient1\": 0.42093,\"coefficient2\": 6.9476," +
			//	"\"coefficient3\": 0.54992,\"version\":\"4.4.2\",\"build_number\":\"LPV79\"," +
			//	"\"model\":\"Nexus 5\",\"manufacturer\":\"LGE\",\"default\": true}]}";
			//ModelSpecificDistanceCalculator distanceCalculator =
			//		new ModelSpecificDistanceCalculator(applicationContext, null, model);
	
			//Runnable runnable2 = new Runnable() {
			//	@Override
			//	public void run() {
			//		try {
			//			while (true) {
			//				distanceCalculator.buildModelMapWithLock(modelMapJson);
			//			}
			//		} catch (Exception ex) {
			//			ex.printStackTrace();
			//		}
			//	}
			//};
	
			//Thread thread2 = new Thread(runnable2);
			//thread2.start();
	
			//int i = 0;
			//while (++i < 1000 && thread2.getState() != Thread.State.TERMINATED) {
			//	distanceCalculator.findCalculatorForModelWithLock(model);
			//}
	
			//thread2.interrupt();
		}
	}
}
