using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using AppleShooterProto;

[TestFixture, Category("AppleShooterProto")]
public class HeatManagerTest{
	[Test, TestCaseSource(typeof(GetComboTimeTestCase), "case1")]
	public void GetComboTime_Demo(float normalizedComboValue, float expected){
		IHeatManagerAdaptor heatManagerAdaptor;
		TestHeatManager manager = CreateTestHeatManager(out heatManagerAdaptor);
		heatManagerAdaptor.GetStandardComboTime().Returns(.5f);
		heatManagerAdaptor.GetMaxComboTimeMultiplier().Returns(3f);
		heatManagerAdaptor.GetMinComboValue().Returns(.05f);
		heatManagerAdaptor.GetMaxComboValue().Returns(.5f);

		float actual = manager.GetComboTime(normalizedComboValue);

		Assert.That(actual, Is.EqualTo(expected));
	}
	public class GetComboTimeTestCase{
		public static object[] case1 = {
			new object[]{.01f, .1f},
			new object[]{.05f, .5f},
			new object[]{.1f, 1f * Mathf.Lerp(1f, 3f, .1f)},
			new object[]{.2f, 2f * Mathf.Lerp(1f, 3f, (.2f - .05f)/.5f)}
		};
	}

	class TestHeatManager: HeatManager{
		public TestHeatManager(IConstArg arg): base(arg){}
	}
	TestHeatManager CreateTestHeatManager(out IHeatManagerAdaptor adaptor){
		IHeatManagerAdaptor mockAdaptor = Substitute.For<IHeatManagerAdaptor>();
		mockAdaptor.GetMonoBehaviourAdaptorManager().Returns(Substitute.For<IAppleShooterMonoBehaviourAdaptorManager>());
		TestHeatManager.IConstArg arg = new TestHeatManager.ConstArg(
			mockAdaptor,
			0f,
			0f,
			0f,
			0f,
			0f
		);
		adaptor = mockAdaptor;
		return new TestHeatManager(arg);
	}
}
