using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DKUtility.CurveUtility{
	public static class Bezier{
		public static Vector3 CubicBezierV3(
			Vector3 p0,
			Vector3 p1,
			Vector3 p2, 
			Vector3 p3,
			float t
		){
			float oneMinusT = 1f - t;
			Vector3 result;
			Vector3 term1 = Mathf.Pow(oneMinusT, 3f) * p0;
			Vector3 term2 = 3f * t * Mathf.Pow(oneMinusT, 2f) * p1;
			Vector3 term3 =  3f * Mathf.Pow(t, 2f) * oneMinusT * p2;
			Vector3 term4 = Mathf.Pow(t, 3f) * p3;
			result = term1 + term2 + term3 + term4;
			return result;
		}
	}
}
