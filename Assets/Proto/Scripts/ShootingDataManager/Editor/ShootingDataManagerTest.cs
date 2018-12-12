using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using AppleShooterProto;

[TestFixture, Category("AppleShooterProto")]
public class ShootingDataManagerTest {
	[Test, TestCaseSource(typeof(GetLevelTierTestCase), "cases")]
	public void GetLevelTier_Various(int sourceLevel, int expected){
		IShootingDataManagerAdaptor adaptor;
		TestShootingDataManager manager = CreateTestShootingDataManager(out adaptor);

		int actual = manager.GetLevelTier_Test(sourceLevel);

		Assert.That(actual, Is.EqualTo(expected));
	}
	public class GetLevelTierTestCase{
		public static object[] cases = {
			new object[]{0, 0},
			new object[]{1, 0},
			new object[]{2, 0},
			new object[]{3, 0},
			new object[]{4, 0},
			
			new object[]{5, 1},
			new object[]{6, 1},
			new object[]{7, 1},
			new object[]{8, 1},
			
			new object[]{9, 2},
			new object[]{10, 2},
			new object[]{11, 2},
			new object[]{12, 2},
		};
	}
	[Test, TestCaseSource(typeof(GetScaledLevelTestCase), "case1")]
	public void GetScaledLevel_Various(int sourceLevel, int expected){
		IShootingDataManagerAdaptor adaptor;
		TestShootingDataManager manager = CreateTestShootingDataManager(out adaptor);
		
		int actual = manager.GetScaledLevel_Test(sourceLevel);
		
		Assert.That(actual, Is.EqualTo(expected));
	}
	public class GetScaledLevelTestCase{
		public static object[] case1 = {
			new object[]{0, 0},
			new object[]{1, 1},
			new object[]{2, 2},
			new object[]{3, 3},
			new object[]{4, 4},

			new object[]{5, 6},
			new object[]{6, 8},
			new object[]{7, 10},
			new object[]{8, 12},
			
			new object[]{9, 16},
			new object[]{10, 20},
			new object[]{11, 24},
			new object[]{12, 28},
		};
	}
	[Test, TestCaseSource(typeof(CalculateMaxScaledLevelTestCase), "case1")]
	public void CalculateMaxScaledLevel_Various(int tierSteps, int tierCount, int expected){
		IShootingDataManagerAdaptor adaptor;
		TestShootingDataManager manager = CreateTestShootingDataManager(out adaptor);
		adaptor.GetTierSteps().Returns(tierSteps);
		adaptor.GetTierCount().Returns(tierCount);

		int actual =  manager.CalculateMaxScaledLevel_Test();

		Assert.That(actual, Is.EqualTo(expected));
	}
	public class CalculateMaxScaledLevelTestCase{
		public static object[] case1 = {
			new object[]{4, 3, 28}
		};
	}
	[Test, TestCaseSource(typeof(GetNormalizedScaledLevelTestCase), "case1")]
	public void GetNormalizedScaledLevel_Various(int scaledLevel, int maxScaledLevel, float expected){
		IShootingDataManagerAdaptor adaptor;
		TestShootingDataManager manager = CreateTestShootingDataManager(out adaptor);

		float actual = manager.GetNormalizedScaledLevel_Test(scaledLevel, maxScaledLevel);

		Assert.That(actual, Is.EqualTo(expected));
	}
	public class GetNormalizedScaledLevelTestCase{
		public static object[] case1 = {
			new object[]{0, 28, 0f},
			new object[]{1, 28, 1f/ 28},
			new object[]{2, 28, 2f/ 28},
			new object[]{3, 28, 3f/ 28},
			new object[]{4, 28, 4f/ 28},
			new object[]{6, 28, 6f/ 28},
			new object[]{8, 28, 8f/ 28},
			new object[]{10, 28, 10f/ 28},
			new object[]{12, 28, 12f/ 28},
			new object[]{16, 28, 16f/ 28},
			new object[]{20, 28, 20f/ 28},
			new object[]{24, 28, 24f/ 28},
			new object[]{28, 28, 1f},
		};
	}
	class TestShootingDataManager: ShootingDataManager{
		public TestShootingDataManager(IConstArg arg): base (arg){}
		public int GetLevelTier_Test(int sourceLevel){
			return GetLevelTier(sourceLevel);
		}
		public int GetScaledLevel_Test(int sourceLevel){
			return GetScaledLevel(sourceLevel);
		}
		public int CalculateMaxScaledLevel_Test(){
			return CalculateMaxScaledLevel();
		}
		public float GetNormalizedScaledLevel_Test(int scaledLevel, int maxScaledLevel){
			return GetNormalizedScaledLevel(scaledLevel, maxScaledLevel);
		}
	}
	TestShootingDataManager CreateTestShootingDataManager(out IShootingDataManagerAdaptor adaptor){
		TestShootingDataManager.IConstArg mockArg = Substitute.For<TestShootingDataManager.IConstArg>();
		IShootingDataManagerAdaptor thisAdaptor = Substitute.For<IShootingDataManagerAdaptor>();
		adaptor = thisAdaptor;
		mockArg.adaptor.Returns(thisAdaptor);
		adaptor.GetTierSteps().Returns(4);
		adaptor.GetTierCount().Returns(3);
		adaptor.GetTierLevelMultipliers().Returns(new int[3]{1, 2, 4});
		return new TestShootingDataManager(mockArg);
	}
}
