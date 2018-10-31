using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IStaticShootingTargetReserveAdaptor: IMonoBehaviourAdaptor{
		IStaticShootingTargetReserve GetStaticShootingTargetReserve();
	}
	public class StaticShootingTargetReserveAdaptor : MonoBehaviourAdaptor, IStaticShootingTargetReserveAdaptor {
		public override void SetUp(){
			thisReserve = CreateStaticShootingTargetReserve();
		}
		IStaticShootingTargetReserve thisReserve;
		IStaticShootingTargetReserve CreateStaticShootingTargetReserve(){
			StaticShootingTargetReserve.IConstArg arg = new StaticShootingTargetReserve.ConstArg(
				this
			);
			return new StaticShootingTargetReserve(arg);
		}
		public IStaticShootingTargetReserve GetStaticShootingTargetReserve(){
			return thisReserve;
		}
		public override void SetUpReference(){
			IStaticShootingTarget[] targets = CreateStaticShootingTargets();
			thisReserve.SetSceneObjects(targets);
		}
		public int totalTargetsCount;
		public GameObject staticShootingTargetPrefab;
		public PopUIReserveAdaptor popUIReserveAdaptor;
		public DestroyedTargetReserveAdaptor destroyedTargetReserveAdaptor;
		IStaticShootingTargetAdaptor[] thisTargetAdaptors;
		public IStaticShootingTarget[] CreateStaticShootingTargets(){

			IPopUIReserve popUIReserve = popUIReserveAdaptor.GetPopUIReserve();
			IDestroyedTargetReserve destroyedTargetReserve = destroyedTargetReserveAdaptor.GetDestroyedTargetReserve();
			List<IStaticShootingTarget> resultList = new List<IStaticShootingTarget>();
			List<IStaticShootingTargetAdaptor> targetAdaptorsList = new List<IStaticShootingTargetAdaptor>();

			for(int i = 0; i < totalTargetsCount; i ++){
				GameObject targetGO = GameObject.Instantiate(
					staticShootingTargetPrefab,
					Vector3.zero,
					Quaternion.identity
				);
				IStaticShootingTargetAdaptor targetAdaptor = targetGO.GetComponent(typeof(IStaticShootingTargetAdaptor)) as IStaticShootingTargetAdaptor;
				targetAdaptorsList.Add(targetAdaptor);
				targetAdaptor.SetMonoBehaviourAdaptorManager(
					thisMonoBehaviourAdaptorManager
				);
				targetAdaptor.SetIndex(i);
				targetAdaptor.SetTargetReserve(thisReserve);
				targetAdaptor.SetPopUIReserve(popUIReserve);
				targetAdaptor.SetDestroyedTargetReserve(destroyedTargetReserve);

				targetAdaptor.SetUp();
				targetAdaptor.SetUpReference();
				
				IStaticShootingTarget target = targetAdaptor.GetStaticShootingTarget();
				
				resultList.Add(target);
			}
			thisTargetAdaptors = targetAdaptorsList.ToArray();
			return resultList.ToArray();
		}
		public override void FinalizeSetUp(){
			foreach(IStaticShootingTargetAdaptor targetAdaptor in thisTargetAdaptors){
				targetAdaptor.FinalizeSetUp();
			}
		}
	}
}
