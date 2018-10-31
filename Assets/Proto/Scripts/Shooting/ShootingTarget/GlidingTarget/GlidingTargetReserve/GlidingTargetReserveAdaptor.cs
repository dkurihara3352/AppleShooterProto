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
			thisTargetAdaptors = GetAdaptors(glidingTargets);
		}
		IGlidingTargetAdaptor[] thisTargetAdaptors;
		IGlidingTargetAdaptor[] GetAdaptors(IGlidingTarget[] targets){
			List<IGlidingTargetAdaptor> resultList = new List<IGlidingTargetAdaptor>();
			foreach(IGlidingTarget target in targets)
				resultList.Add((IGlidingTargetAdaptor)target.GetAdaptor());
			return resultList.ToArray();
		}
		public override void FinalizeSetUp(){
			foreach(IGlidingTargetAdaptor adaptor in thisTargetAdaptors)
				adaptor.FinalizeSetUp();
		}

		public int targetCounts;
		public GameObject glidingTargetPrefab;
		public IPopUIReserveAdaptor popUIReserveAdaptor;
		public IDestroyedTargetReserveAdaptor destroyedTargetReserveAdaptor;

		IGlidingTarget[] CreateGlidingTargets(){
			List<IGlidingTarget> resultList = new List<IGlidingTarget>();
			IPopUIReserve popUIReserve = popUIReserveAdaptor.GetPopUIReserve();
			IDestroyedTargetReserve destroyedTargetReserve = destroyedTargetReserveAdaptor.GetDestroyedTargetReserve();

			for(int i = 0; i < targetCounts; i ++){
				GameObject targetGO = GameObject.Instantiate(
					glidingTargetPrefab
				);
				IGlidingTargetAdaptor targetAdaptor = (IGlidingTargetAdaptor)targetGO.GetComponent(typeof(IGlidingTargetAdaptor));

				targetAdaptor.SetIndex(i);
				targetAdaptor.SetMonoBehaviourAdaptorManager(
					thisMonoBehaviourAdaptorManager
				);
				targetAdaptor.SetGlidingTargetReserve(thisReserve);
				targetAdaptor.SetPopUIReserve(popUIReserve);
				targetAdaptor.SetDestroyedTargetReserve(destroyedTargetReserve);

				targetAdaptor.SetUp();
				targetAdaptor.SetUpReference();

				IGlidingTarget target = targetAdaptor.GetGlidingTarget();

				resultList.Add(target);
			}
			return resultList.ToArray();
		}

	}
}
