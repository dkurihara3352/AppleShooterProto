﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface ISmoothFollowerAdaptor: IMonoBehaviourAdaptor{
	}
	public class SmoothFollowerAdaptor : MonoBehaviourAdaptor, ISmoothFollowerAdaptor {
		public MonoBehaviourAdaptor followTarget;
		public ProcessManager processManager;
		public float smoothCoefficient;
		public int processOrder = 100;
		public override void SetUp(){
			IAppleShooterProcessFactory processFactory = new AppleShooterProcessFactory(
				processManager
			);
			ISmoothFollowerConstArg arg = new SmoothFollowerConstArg(
				this,
				processFactory,
				smoothCoefficient,
				followTarget,
				processOrder
			);
			thisSmoothFollower = new SmoothFollower(arg);
		}
		ISmoothFollower thisSmoothFollower;
		public ISmoothFollower GetSmoothFollower(){
			return thisSmoothFollower;
		}
	}
}
