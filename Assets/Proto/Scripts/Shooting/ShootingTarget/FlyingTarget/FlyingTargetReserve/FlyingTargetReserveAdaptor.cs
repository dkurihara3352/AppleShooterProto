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
			thisFlyingTargetAdaptors = CreateFlyingTargetAdaptors();
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
		}
		public int targetsCount;
		public GameObject flyingTargetPrefab;
		public PopUIReserveAdaptor popUIReserveAdaptor;
		public DestroyedTargetReserveAdaptor destroyedTargetReserveAdaptor;
		IFlyingTargetAdaptor[] thisFlyingTargetAdaptors;
		IFlyingTargetAdaptor[] CreateFlyingTargetAdaptors(){
			List<IFlyingTargetAdaptor> resultList = new List<IFlyingTargetAdaptor>();


			for(int i = 0; i < targetsCount; i++){
				GameObject targetGO = GameObject.Instantiate(
					flyingTargetPrefab
				);
				IFlyingTargetAdaptor targetAdaptor = (IFlyingTargetAdaptor)targetGO.GetComponent(typeof(IFlyingTargetAdaptor));

				targetAdaptor.SetFlyingTargetReserve(thisReserve);
				targetAdaptor.SetIndex(i);
				targetAdaptor.SetPopUIReserveAdaptor(popUIReserveAdaptor);
				targetAdaptor.SetDestroyedTargetReserveAdaptor(destroyedTargetReserveAdaptor);
				
				targetAdaptor.SetUp();

				resultList.Add(targetAdaptor);
			}
			return resultList.ToArray();

		}
		IFlyingTarget[] CreateFlyingTargets(){
			List<IFlyingTarget> resultList = new List<IFlyingTarget>();
			foreach(IFlyingTargetAdaptor adaptor in thisFlyingTargetAdaptors)
				resultList.Add(adaptor.GetFlyingTarget());
			return resultList.ToArray();
		}
	}
}

