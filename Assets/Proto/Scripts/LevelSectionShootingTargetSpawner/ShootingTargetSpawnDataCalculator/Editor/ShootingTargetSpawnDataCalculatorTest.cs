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
	int[] spawnValue = new int[]{
		10,
		30,
		40
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
	public void GetNumberToCreateByTargetType_Demo(){

		TestCalculator calculator = CreateTestCalculator();

        int[] spawnValueArray = new int[]{
            spawnValue[0],
            spawnValue[1],
            spawnValue[2]
        };

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
		int defaultSpawnValueLimit = 100;
		TargetSpawnDataInput[] defaultDataInput = CreateDefaultDataInput();
		TestCalculator.IConstArg arg = new TestCalculator.ConstArg(
			defaultSpawnValueLimit,
			defaultDataInput
		);
		return new TestCalculator(arg);
	}
	TargetSpawnDataInput[] CreateDefaultDataInput(){
		TargetSpawnDataInput staticInput = new TargetSpawnDataInput();
			staticInput.targetType = TargetType.Static;
			staticInput.relativeProbability = relativeProbabilities[0];
			staticInput.maxCount = maxCount[0];

			
		TargetSpawnDataInput fattyInput = new TargetSpawnDataInput();
			fattyInput.targetType = TargetType.Fatty;
			fattyInput.relativeProbability = relativeProbabilities[1];
			fattyInput.maxCount = maxCount[1];


		TargetSpawnDataInput gliderInput = new TargetSpawnDataInput();
			gliderInput.targetType = TargetType.Glider;
			gliderInput.relativeProbability = relativeProbabilities[2];
			gliderInput.maxCount = maxCount[2];


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
		public int[] GetSpawnValueByTargetType_Test(){
			return this.GetSpawnValueByTargetType();
		}
		public int[] GetNumberToCreateByTargetType_Test(
			int[] spawnValue
		){
			return this.GetNumberToCreateByTargetType(spawnValue);
		}
	}
}
