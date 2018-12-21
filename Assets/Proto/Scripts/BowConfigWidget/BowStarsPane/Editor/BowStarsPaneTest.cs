using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using AppleShooterProto;
using UISystem;

[TestFixture, Category("AppleShooterProto")]
public class BowStarsPaneTest{
	[Test, TestCaseSource(typeof(CalcFillTestCase), "case1")]
	public void CalcFill_Various(
		int level,
		int stepCount,
		float expected
	){
		TestBowStarsPane.IConstArg arg = Substitute.For<TestBowStarsPane.IConstArg>();
		arg.activationMode.Returns(UISystem.ActivationMode.None);
		IBowStarsPaneAdaptor mockAdaptor = Substitute.For<IBowStarsPaneAdaptor>();
			IUISystemMonoBehaviourAdaptorManager mockMBAManager = Substitute.For<IUISystemMonoBehaviourAdaptorManager>();
			mockAdaptor.GetMonoBehaviourAdaptorManager().Returns(mockMBAManager);
			mockAdaptor.GetProcessTime().Returns(1f);
			mockAdaptor.GetStepCount().Returns(stepCount);
		arg.adaptor.Returns(mockAdaptor);

		TestBowStarsPane testPane = new TestBowStarsPane(arg);

		float actual = testPane.CalcFill_Test(level);

		Assert.That(
			actual,
			Is.EqualTo(expected)
		);
	}
	public class CalcFillTestCase{
		public static object[] case1 = {
			new object[]{
				1,
				1,
				1f
			},
			new object[]{
				1,
				5,
				.2f
			},
			new object[]{
				0,
				1,
				0f
			},
			new object[]{
				5,
				1,
				5f
			},
			new object[]{
				5,
				5,
				1f
			},
		};
	}
	class TestBowStarsPane: BowStarsPane{
		public TestBowStarsPane(IConstArg arg): base(arg){}
		public float CalcFill_Test(int level){
			return this.CalcFill(level);
		}
	}
}
