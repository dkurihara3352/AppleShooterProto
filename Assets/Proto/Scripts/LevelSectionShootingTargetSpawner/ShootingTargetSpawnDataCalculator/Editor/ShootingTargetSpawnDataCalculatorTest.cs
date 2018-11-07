using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using AppleShooterProto;

[TestFixture, Category("AppleShooterProto")]
public class ShootingTargetSpawnDataCalculatorTest {
	float[] relativeProbabilities = new float[]{
		10f,
		3f,
		2f
	};
	int[] maxCount = new int[]{
		10,
		3,
		2
	};
	float spawnValueLimit = 100f;
	float[] spawnValue = new float[]{
		10f,
		30f,
		40f
	};
	[Test]
	public void GetTargetSpawnRelativeProbabilities_ReturnsExpected(){
		TestCalculator calculator = CreateTestCalculator();
		TargetSpawnDataInput[] inputs = CreateDefaultDataInput();
		float[] relativeProbabilities = calculator.GetTargetSpawnRelativeProbabilities_Test(inputs);

		Assert.That(relativeProbabilities.Length, Is.EqualTo(3));
		Assert.That(relativeProbabilities, Is.EqualTo(
			new float[]{
				relativeProbabilities[0],
				relativeProbabilities[1],
				relativeProbabilities[2]
			}
		));
	}
	[Test]
	public void GetSpawnValueByTargetType_ReturnsExpected(){
		TestCalculator calculator = CreateTestCalculator();
		IShootingTargetSpawnManager manager = calculator.GetManager();
		manager.GetSpawnValue(TargetType.Static).Returns(spawnValue[0]);
		manager.GetSpawnValue(TargetType.Fatty).Returns(spawnValue[1]);
		manager.GetSpawnValue(TargetType.Glider).Returns(spawnValue[2]);

		float[] spawnValueArray = calculator.GetSpawnValueByTargetType_Test();

		Assert.That(spawnValueArray, Is.EqualTo(
			new float[]{
				spawnValue[0],
				spawnValue[1],
				spawnValue[2]
			}
		));
	}
	[Test]
	public void GetNumberToCreateByTargetType_Demo(){
		TestCalculator calculator = CreateTestCalculator();
		IShootingTargetSpawnManager manager = calculator.GetManager();
		manager.GetSpawnValue(TargetType.Static).Returns(spawnValue[0]);
		manager.GetSpawnValue(TargetType.Fatty).Returns(spawnValue[1]);
		manager.GetSpawnValue(TargetType.Glider).Returns(spawnValue[2]);

		float[] spawnValueArray = calculator.GetSpawnValueByTargetType_Test();

		int[] numToCreate = calculator.GetNumberToCreateByTargetType_Test(spawnValueArray);

		int index = 0;
		float sum = 0f;
		foreach(int num in numToCreate){
			float spawnValue = spawnValueArray[index];
			float product = spawnValueArray[index] * num;
			Debug.Log(
				"spawnValue: " + spawnValue.ToString() + ", "+
 				"numToCreate: " + num.ToString() + ", " +
				"product: " + product.ToString()
			);
			sum += product;
			index ++;
		}
		Debug.Log(
			DKUtility.DebugHelper.BlueString(
				"sum: " + sum.ToString()
			)
		);
	}

	TestCalculator CreateTestCalculator(){
		float defaultSpawnValueLimit = 100f;
		TargetSpawnDataInput[] defaultDataInput = CreateDefaultDataInput();
		TestCalculator.IConstArg arg = new TestCalculator.ConstArg(
			defaultSpawnValueLimit,
			defaultDataInput,
			Substitute.For<IShootingTargetSpawnManager>()
		);
		return new TestCalculator(arg);
	}
	TargetSpawnDataInput[] CreateDefaultDataInput(){
		TargetSpawnDataInput staticInput = new TargetSpawnDataInput();
			staticInput.targetType = TargetType.Static;
			staticInput.relativeProbability = relativeProbabilities[0];
			staticInput.maxCount = maxCount[0];
			SpawnPointAdaptorEventPointPair pair_1 = new SpawnPointAdaptorEventPointPair();
				pair_1.eventPoint = .1f;
			SpawnPointAdaptorEventPointPair pair_2 = new SpawnPointAdaptorEventPointPair();
				pair_2.eventPoint = .2f;
			SpawnPointAdaptorEventPointPair pair_3 = new SpawnPointAdaptorEventPointPair();
				pair_3.eventPoint = .3f;
			staticInput.spawnPointAdaptorEventPointPairs = new SpawnPointAdaptorEventPointPair[]{
				pair_1,
				pair_2,
				pair_3
			};
		TargetSpawnDataInput fattyInput = new TargetSpawnDataInput();
			fattyInput.targetType = TargetType.Fatty;
			fattyInput.relativeProbability = relativeProbabilities[1];
			fattyInput.maxCount = maxCount[1];
			SpawnPointAdaptorEventPointPair pair_fatty_1 = new SpawnPointAdaptorEventPointPair();
				pair_fatty_1.eventPoint = .4f;
			SpawnPointAdaptorEventPointPair pair_fatty_2 = new SpawnPointAdaptorEventPointPair();
				pair_fatty_1.eventPoint = .5f;
			SpawnPointAdaptorEventPointPair pair_fatty_3 = new SpawnPointAdaptorEventPointPair();
				pair_fatty_1.eventPoint = .6f;
			fattyInput.spawnPointAdaptorEventPointPairs = new SpawnPointAdaptorEventPointPair[]{
				pair_fatty_1,
				pair_fatty_2,
				pair_fatty_3
			};
		TargetSpawnDataInput gliderInput = new TargetSpawnDataInput();
			gliderInput.targetType = TargetType.Glider;
			gliderInput.relativeProbability = relativeProbabilities[2];
			gliderInput.maxCount = maxCount[2];
			SpawnPointAdaptorEventPointPair pair_glider_1 = new SpawnPointAdaptorEventPointPair();
				pair_glider_1.eventPoint = .7f;
			SpawnPointAdaptorEventPointPair pair_glider_2 = new SpawnPointAdaptorEventPointPair();
				pair_glider_1.eventPoint = .8f;
			SpawnPointAdaptorEventPointPair pair_glider_3 = new SpawnPointAdaptorEventPointPair();
				pair_glider_1.eventPoint = .9f;
			gliderInput.spawnPointAdaptorEventPointPairs = new SpawnPointAdaptorEventPointPair[]{
				pair_glider_1,
				pair_glider_2,
				pair_glider_3
			};
		return new TargetSpawnDataInput[]{
			staticInput,
			fattyInput,
			gliderInput
		};
	}

	public class TestCalculator: ShootingTargetSpawnDataCalculator{
		public TestCalculator(
			IConstArg arg
		): base(arg){}
		public float[] GetTargetSpawnRelativeProbabilities_Test(
			TargetSpawnDataInput[] inputs
		){
			return this.GetTargetSpawnRelativeProbabilities(inputs);
		}
		public float[] GetSpawnValueByTargetType_Test(){
			return this.GetSpawnValueByTargetType();
		}
		public IShootingTargetSpawnManager GetManager(){
			return thisSpawnManager;
		}
		public int[] GetNumberToCreateByTargetType_Test(
			float[] spawnValue
		){
			return this.GetNumberToCreateByTargetType(spawnValue);
		}
	}
}
