using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IDrawableCurve{
		void Calculate();
		ICurveControlPoint[] GetCurveControlPoints();
		ICurvePoint[] GetCurvePoints();
		Color GetLineColor();
		Color GetUpDirColor();
		float GetUpDirLineLength();
		Vector3 GetControlPointDrawSize();
		Vector3 GetControlPointHandleSize();
		Color GetControlPointColor();
		Color GetHandleColor();
	}
}

