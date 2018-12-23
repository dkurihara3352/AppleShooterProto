using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using AppleShooterProto;

[TestFixture, Category("AppleShooterProto")]
public class GUIManagerTest {
	[Test, TestCaseSource(typeof(CalculateMinAvailableDigitTestCase), "case1")]
	public void CalculateMinAvailableDigit_Various(
		string baseFileName,
		string[] filePaths,
		int expected
	){
		TestGUIManager testGUIManager = new TestGUIManager();
		int actual  = testGUIManager.CalculateMinAvailableDigit_Test(
			baseFileName,
			filePaths
		);

		Assert.That(actual, Is.EqualTo(expected));
	}
	public class CalculateMinAvailableDigitTestCase{
		public static object[] case1 = {
			new object[]{
				"baseFileName",
				new string[]{
				},
				0
			},
			new object[]{
				"baseFileName",
				new string[]{
					"baseFileName_0.dat"
				},
				1
			},
			new object[]{
				"baseFileName",
				new string[]{
					"baseFileName_0.dat",
					"baseFileName_1.dat",
					"baseFileName_2.dat"
				},
				3
			},
			new object[]{
				"baseFileName",
				new string[]{
					"baseFileName_0.dat",
					"baseFileName_2.dat",
					"baseFileName_3.dat"
				},
				1
			},
			new object[]{
				"baseFileName",
				new string[]{
					"baseFileName_0.dat",
					"baseFileName_1.dat",
					"baseFileName_2.dat",
					"baseFileName_3.dat",
					"baseFileName_10.dat",
				},
				4
			},
			new object[]{
				"baseFileName",
				new string[]{
					"baseFileName_10.dat",
					"baseFileName_3.dat",
					"baseFileName_2.dat",
					"baseFileName_0.dat",
					"baseFileName_1.dat",
				},
				4
			},
		};
	}

	class TestGUIManager: GUIManager{
		public int CalculateMinAvailableDigit_Test(
			string baseFileName,
			string[] filePaths
		){
			return this.CalculateMinAvailableDigit(
				baseFileName,
				filePaths
			);
		}
	}
}
