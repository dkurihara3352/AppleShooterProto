﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IStaticShootingTargetReserveAdaptor: IMonoBehaviourAdaptor{
		IStaticShootingTargetReserve GetStaticShootingTargetReserve();
		IStaticShootingTarget CreateStaticShootingTarget();
	}
	public class StaticShootingTargetReserveAdaptor : MonoBehaviourAdaptor, IStaticShootingTargetReserveAdaptor {
		public override void SetUp(){
			StaticShootingTargetReserve.IConstArg arg = new StaticShootingTargetReserve.ConstArg(
				this,
				totalTargetsCount
			);
			thisReserve =  new StaticShootingTargetReserve(arg);
		}
		public int totalTargetsCount;
		IStaticShootingTargetReserve thisReserve;
		public IStaticShootingTargetReserve GetStaticShootingTargetReserve(){
			return thisReserve;
		}
		public GameObject staticShootingTargetPrefab;
		public ProcessManager processManager;
		public IStaticShootingTarget CreateStaticShootingTarget(){
			GameObject targetGO = GameObject.Instantiate(
				staticShootingTargetPrefab,
				Vector3.zero,
				Quaternion.identity
			);
			IStaticShootingTargetAdaptor targetAdaptor = targetGO.GetComponent(typeof(IStaticShootingTargetAdaptor)) as IStaticShootingTargetAdaptor;
			targetAdaptor.SetProcessManager(processManager);
			targetAdaptor.SetReserveTransform(this.transform);
			targetAdaptor.SetTargetReserve(thisReserve);
			
			targetAdaptor.SetUp();
			targetAdaptor.SetUpReference();
			IStaticShootingTarget target = targetAdaptor.GetShootingTarget() as IStaticShootingTarget;
			if(target == null)
				throw new System.InvalidOperationException(
					"target not available??"
				);
			return target;
		}
	}
}
