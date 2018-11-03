using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IGlidingTargetReserveAdaptor: IMonoBehaviourAdaptor{
		IGlidingTargetReserve GetGlidingTargetReserve();
	}
	public class GlidingTargetReserveAdaptor : MonoBehaviourAdaptor, IGlidingTargetReserveAdaptor{
		IGlidingTargetReserve thisReserve;
		public IGlidingTargetReserve GetGlidingTargetReserve(){
			return thisReserve;
		}
		public override void SetUp(){
			thisReserve = CreateGlidingTargetReserve();
			thisGlidingTargetAdaptors = CreateGlidingTargetAdaptors();
		}
		IGlidingTargetReserve CreateGlidingTargetReserve(){
			GlidingTargetReserve.IConstArg arg = new GlidingTargetReserve.ConstArg(
				this
			);
			return new GlidingTargetReserve(arg);
		}
		public override void SetUpReference(){
			IGlidingTarget[] glidingTargets = CreateGlidingTargets();
			thisReserve.SetSceneObjects(glidingTargets);
		}
		IGlidingTargetAdaptor[] thisGlidingTargetAdaptors;

		public int targetCounts;
		public GameObject glidingTargetPrefab;
		public PopUIReserveAdaptor popUIReserveAdaptor;
		public DestroyedTargetReserveAdaptor destroyedTargetReserveAdaptor;

		IGlidingTargetAdaptor[] CreateGlidingTargetAdaptors(){

			List<IGlidingTargetAdaptor> resultList = new List<IGlidingTargetAdaptor>();

			for(int i = 0; i < targetCounts; i ++){
				GameObject targetGO = GameObject.Instantiate(
					glidingTargetPrefab
				);
				IGlidingTargetAdaptor targetAdaptor = (IGlidingTargetAdaptor)targetGO.GetComponent(typeof(IGlidingTargetAdaptor));

				targetAdaptor.SetIndex(i);
				targetAdaptor.SetGlidingTargetReserve(thisReserve);
				targetAdaptor.SetPopUIReserveAdaptor(popUIReserveAdaptor);
				targetAdaptor.SetDestroyedTargetReserveAdaptor(destroyedTargetReserveAdaptor);

				targetAdaptor.SetUp();

				resultList.Add(targetAdaptor);
			}
			return resultList.ToArray();
		}

		IGlidingTarget[] CreateGlidingTargets(){
			List<IGlidingTarget> resultList = new List<IGlidingTarget>();
			foreach(IGlidingTargetAdaptor adaptor in thisGlidingTargetAdaptors){
				resultList.Add(adaptor.GetGlidingTarget());
			}
			return resultList.ToArray();
		}

	}
}
