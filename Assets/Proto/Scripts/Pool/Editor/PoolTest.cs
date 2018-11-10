using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using NSubstitute;
using UnityEditor;
using AppleShooterProto;

[TestFixture, Category("AppleShooterProto")]
public class PoolTest{
	[Test]
	public void Various(){
		float[] relativeProbs = new float[]{
			1f, 
			2f, 
			3f, 
			4f,
			5f, 
			6f, 
			7f, 
			8
		};
		IPool pool = new Pool(relativeProbs);
		
		int numOfAttempt = 1000;
		for(int i = 0; i < numOfAttempt; i ++){
			pool.Draw();
		}
		pool.Log();
	}
	[Test]
	public void BellCurveTest_Various(){
		IBellCurve curve = new BellCurve(
			1f,
			.3f,
			.8f,
			1.2f,
			20
		);

		int numOfAttemp = 10000;

		for(int i = 0; i < numOfAttemp; i ++)
			curve.Evaluate();
		
		curve.Log();
	}
}
