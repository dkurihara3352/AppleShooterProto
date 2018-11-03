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
}
