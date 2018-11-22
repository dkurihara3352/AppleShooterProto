using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;
using UnityBase;

namespace AppleShooterProto{
	public interface ISmoothFollowerAdaptor: IAppleShooterMonoBehaviourAdaptor{
		ISmoothFollower GetSmoothFollower();
	}
	public class SmoothFollowerAdaptor : AppleShooterMonoBehaviourAdaptor, ISmoothFollowerAdaptor {
		public MonoBehaviourAdaptor followTarget;
		public float smoothCoefficient;
		public int processOrder = 100;
		public override void SetUp(){
			thisSmoothFollower = CreateSmoothFollower();
		}
		ISmoothFollower thisSmoothFollower;
		public ISmoothFollower GetSmoothFollower(){
			return thisSmoothFollower;
		}
		ISmoothFollower CreateSmoothFollower(){
			SmoothFollower.IConstArg arg = new SmoothFollower.ConstArg(
				this,
				smoothCoefficient,
				followTarget,
				processOrder
			);
			return new SmoothFollower(arg);
		}
	}
}
