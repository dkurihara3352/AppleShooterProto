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
			thisStaticShootingTargetAdaptors = CreateStaticShootingTargetAdaptors();
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
		IStaticShootingTargetAdaptor[] thisStaticShootingTargetAdaptors;
		public IStaticShootingTargetAdaptor[] CreateStaticShootingTargetAdaptors(){

			List<IStaticShootingTargetAdaptor> resultList = new List<IStaticShootingTargetAdaptor>();

			for(int i = 0; i < totalTargetsCount; i ++){
				GameObject targetGO = GameObject.Instantiate(
					staticShootingTargetPrefab,
					Vector3.zero,
					Quaternion.identity
				);
				IStaticShootingTargetAdaptor targetAdaptor = targetGO.GetComponent(typeof(IStaticShootingTargetAdaptor)) as IStaticShootingTargetAdaptor;

				targetAdaptor.SetIndex(i);
				targetAdaptor.SetStaticShootingTargetReserveAdaptor(this);
				targetAdaptor.SetPopUIReserveAdaptor(popUIReserveAdaptor);
				targetAdaptor.SetDestroyedTargetReserveAdaptor(destroyedTargetReserveAdaptor);

				targetAdaptor.SetUp();

				resultList.Add(targetAdaptor);
			}
			return resultList.ToArray();
		}
		IStaticShootingTarget[] CreateStaticShootingTargets(){
			List<IStaticShootingTarget> resultList = new List<IStaticShootingTarget>();
			foreach(IStaticShootingTargetAdaptor adaptor in thisStaticShootingTargetAdaptors)
				resultList.Add(adaptor.GetStaticShootingTarget());
			return resultList.ToArray();
		}
	}
}
