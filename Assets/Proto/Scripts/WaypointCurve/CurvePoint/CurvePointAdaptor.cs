using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ICurvePointAdaptor{
		void SetNormalizedT(float normalizedT);
		void SetUp();
		ICurvePoint GetCurvePoint();
		void SetPosition(Vector3 position);
		Vector3 GetPosition();
		void LookAt(
			Vector3 forward,
			Vector3 up
		);
		Quaternion GetRotation();
		Vector3 GetUpDirection();
		Vector3 GetForwardDirection();
	}
	public class CurvePointAdaptor : MonoBehaviour, ICurvePointAdaptor {
		public void SetNormalizedT(float normalizedT){
			thisNormalizedT = normalizedT;
		}
		float thisNormalizedT;
		public void SetUp(){
			thisCurvePoint = new CurvePoint(
				this,
				thisNormalizedT
			);
		}
		ICurvePoint thisCurvePoint;
		public ICurvePoint GetCurvePoint(){
			return thisCurvePoint;
		}
		public void SetPosition(Vector3 position){
			this.transform.position = position;
		}
		public Vector3 GetPosition(){
			return this.transform.position;
		}
		public void LookAt(
			Vector3 forward,
			Vector3 up
		){
			this.transform.rotation = Quaternion.LookRotation(forward, up);
		}
		public Quaternion GetRotation(){
			return this.transform.rotation;
		}
		public Vector3 GetUpDirection(){
			return this.transform.up;
		}
		public Vector3 GetForwardDirection(){
			return this.transform.forward;
		}
	}	
}
