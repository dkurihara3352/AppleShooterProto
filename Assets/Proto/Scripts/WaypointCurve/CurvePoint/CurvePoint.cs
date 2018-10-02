using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ICurvePoint{
		// Vector3 position{get;}
		float GetNormalizedT();
		Vector3 GetPosition();
		Quaternion GetRotation();
		Vector3 GetUpDirection();
		Vector3 GetForwardDirection();
		void SetPosition(Vector3 position);
		void LookAt(Vector3 forward, Vector3 up);
	}
	public class CurvePoint: ICurvePoint{
		public CurvePoint(
			ICurvePointAdaptor adaptor,
			float normalizedT
		){
			thisAdaptor = adaptor;
			thisNormalizedT = normalizedT;
		}
		readonly float thisNormalizedT;
		public float GetNormalizedT(){
			return thisNormalizedT;
		}
		readonly ICurvePointAdaptor thisAdaptor;
		public ICurvePointAdaptor adaptor{get{return thisAdaptor;}}
		public Vector3 position{
			get{
				return thisAdaptor.GetPosition();
			}
		}
		public Vector3 GetPosition(){
			return thisAdaptor.GetPosition();
		}
		public Quaternion GetRotation(){
			return thisAdaptor.GetRotation();
		}
		public Vector3 GetUpDirection(){
			return thisAdaptor.GetUpDirection();
		}
		public Vector3 GetForwardDirection(){
			return thisAdaptor.GetForwardDirection();
		}
		public void SetPosition(Vector3 position){
			thisAdaptor.SetPosition(position);
		}
		public void LookAt(Vector3 forward, Vector3 up){
			thisAdaptor.LookAt(forward, up);
		}
	}
}
