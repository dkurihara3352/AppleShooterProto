using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IFlyingTargetReserveAdaptor: IMonoBehaviourAdaptor{
		IFlyingTargetReserve GetFlyingTargetReserve();
	}
	public class FlyingTargetReserveAdaptor: MonoBehaviourAdaptor, IFlyingTargetReserveAdaptor{
		IFlyingTargetReserve thisReserve;
		public override void SetUp(){
			thisReserve = CreateFlyingTargetReserve();
		}
		IFlyingTargetReserve CreateFlyingTargetReserve(){
			FlyingTargetReserve.IConstArg arg = new FlyingTargetReserve.ConstArg(
				this
			);
			return new FlyingTargetReserve(arg);
		}
		public IFlyingTargetReserve GetFlyingTargetReserve(){
			return thisReserve;
		}
		public override void SetUpReference(){
			IFlyingTarget[] targets = CreateFlyingTargets();
			thisReserve.SetSceneObjects(targets);

			thisFlyingTargetAdaptors = GetFlyingTargetAdaptors(targets);
		}
		public int targetsCount;
		public GameObject flyingTargetPrefab;
		public PopUIReserveAdaptor popUIReserveAdaptor;
		public DestroyedTargetReserveAdaptor destoryedTargetReserveAdaptor;
		IFlyingTarget[] CreateFlyingTargets(){

			List<IFlyingTarget> resultList = new List<IFlyingTarget>();

			IPopUIReserve popUIReserve = popUIReserveAdaptor.GetPopUIReserve();
			IDestroyedTargetReserve destroyedTargetReserve = destoryedTargetReserveAdaptor.GetDestroyedTargetReserve();

			for(int i = 0; i < targetsCount; i++){
				GameObject targetGO = GameObject.Instantiate(
					flyingTargetPrefab
				);
				IFlyingTargetAdaptor targetAdaptor = (IFlyingTargetAdaptor)targetGO.GetComponent(typeof(IFlyingTargetAdaptor));
				targetAdaptor.SetMonoBehaviourAdaptorManager(
					thisMonoBehaviourAdaptorManager
				);
				targetAdaptor.SetFlyingTargetReserve(thisReserve);
				targetAdaptor.SetIndex(i);
				targetAdaptor.SetPopUIReserve(popUIReserve);
				targetAdaptor.SetDestroyedTargetReserve(destroyedTargetReserve);
				
				targetAdaptor.SetUp();
				
				targetAdaptor.SetUpReference();

				IFlyingTarget target = targetAdaptor.GetFlyingTarget();
				resultList.Add(target);
			}
			return resultList.ToArray();
		}
		IFlyingTargetAdaptor[] thisFlyingTargetAdaptors;
		public IFlyingTargetAdaptor[] GetFlyingTargetAdaptors(IFlyingTarget[] targets){
			List<IFlyingTargetAdaptor> resultList = new List<IFlyingTargetAdaptor>();
			foreach(IFlyingTarget target in targets)
				resultList.Add((IFlyingTargetAdaptor)target.GetAdaptor());
			return resultList.ToArray();
		}
		public override void FinalizeSetUp(){
			foreach(IFlyingTargetAdaptor adaptor in thisFlyingTargetAdaptors)
				adaptor.FinalizeSetUp();
		}

	}
}

