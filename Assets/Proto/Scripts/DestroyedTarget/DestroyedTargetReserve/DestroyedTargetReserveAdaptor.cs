using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IDestroyedTargetReserveAdaptor: IMonoBehaviourAdaptor{
		IDestroyedTargetReserve GetDestroyedTargetReserve();
	}
	public class DestroyedTargetReserveAdaptor : MonoBehaviourAdaptor, IDestroyedTargetReserveAdaptor {

		public override void SetUp(){
			DestroyedTargetReserve.IConstArg arg = new DestroyedTargetReserve.ConstArg(
				this	
			);
			thisReserve = new DestroyedTargetReserve(arg);
		}
		IDestroyedTargetReserve thisReserve;
		public IDestroyedTargetReserve GetDestroyedTargetReserve(){
			return thisReserve;
		}
		public override void SetUpReference(){
			IDestroyedTarget[] destroyedTargets = CreateDestroyedTargets();
			thisReserve.SetDestroyedTargets(destroyedTargets);
		}
		public int destroyedTargetsCount;
		public GameObject destroyedTargetPrefab;
		public PopUIReserveAdaptor popUIReserveAdaptor;
		public IDestroyedTargetAdaptor[] thisDestroyedTargetAdaptors;
		IDestroyedTarget[] CreateDestroyedTargets(){
			List<IDestroyedTarget> resultList = new List<IDestroyedTarget>();
			IPopUIReserve popUIReserve =  popUIReserveAdaptor.GetPopUIReserve();
			List<IDestroyedTargetAdaptor> adaptorsList = new List<IDestroyedTargetAdaptor>();
			for(int i = 0; i < destroyedTargetsCount; i ++){
				GameObject destroyedTargetGO = GameObject.Instantiate(
					destroyedTargetPrefab
				);
				IDestroyedTargetAdaptor adaptor = (IDestroyedTargetAdaptor)destroyedTargetGO.GetComponent(typeof(IDestroyedTargetAdaptor));
				adaptor.SetMonoBehaviourAdaptorManager(thisMonoBehaviourAdaptorManager);
				adaptor.SetPopUIReserve(popUIReserve);
				adaptor.SetIndex(i);
				adaptor.SetDestroyedTargetReserve(thisReserve);

				adaptor.SetUp();
				adaptor.SetUpReference();

				adaptorsList.Add(adaptor);

				IDestroyedTarget target = adaptor.GetDestroyedTarget();
				resultList.Add(target);
			}	
			thisDestroyedTargetAdaptors = adaptorsList.ToArray();
			return resultList.ToArray();
		}
		public override void FinalizeSetUp(){
			foreach(IDestroyedTargetAdaptor targetAdaptor in thisDestroyedTargetAdaptors){
				targetAdaptor.FinalizeSetUp();
			}
		}
	}
}
