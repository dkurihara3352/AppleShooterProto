using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface ITestShootingTargetReserveAdaptor: IMonoBehaviourAdaptor{
		ITestShootingTargetReserve GetTestShootingTargetReserve();
		ITestShootingTarget CreateTestShootingTarget();
	}
	public class TestShootingTargetReserveAdaptor : MonoBehaviourAdaptor, ITestShootingTargetReserveAdaptor {
		public override void SetUp(){
			TestShootingTargetReserve.IConstArg arg = new TestShootingTargetReserve.ConstArg(
				this,
				totalTargetsCount
			);
			thisReserve =  new TestShootingTargetReserve(arg);
		}
		public int totalTargetsCount;
		ITestShootingTargetReserve thisReserve;
		public ITestShootingTargetReserve GetTestShootingTargetReserve(){
			return thisReserve;
		}
		public GameObject testShootingTargetPrefab;
		public ProcessManager processManager;
		public ITestShootingTarget CreateTestShootingTarget(){
			GameObject targetGO = GameObject.Instantiate(
				testShootingTargetPrefab,
				Vector3.zero,
				Quaternion.identity
			);
			ITestShootingTargetAdaptor targetAdaptor = targetGO.GetComponent(typeof(ITestShootingTargetAdaptor)) as ITestShootingTargetAdaptor;
			targetAdaptor.SetProcessManager(processManager);
			
			targetAdaptor.SetUp();
			targetAdaptor.SetUpReference();
			ITestShootingTarget target = targetAdaptor.GetShootingTarget() as ITestShootingTarget;
			if(target == null)
				throw new System.InvalidOperationException(
					"target not available??"
				);
			return target;
		}
	}
}
