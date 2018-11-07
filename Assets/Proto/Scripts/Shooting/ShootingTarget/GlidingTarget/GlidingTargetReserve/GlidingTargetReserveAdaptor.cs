using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IGlidingTargetReserveAdaptor: IShootingTargetReserveAdaptor{
		IGlidingTargetReserve GetGlidingTargetReserve();
	}
	public class GlidingTargetReserveAdaptor : AbsShootingTargetReserveAdaptor, IGlidingTargetReserveAdaptor{
		IGlidingTargetReserve thisTypedReserve{
			get{
				return (IGlidingTargetReserve)thisReserve;
			}
		}
		public IGlidingTargetReserve GetGlidingTargetReserve(){
			return thisTypedReserve;
		}
		public override void SetUp(){
			thisReserve = CreateGlidingTargetReserve();
			thisGlidingTargetAdaptors = CreateGlidingTargetAdaptors();
		}
		IGlidingTargetReserve CreateGlidingTargetReserve(){
			GlidingTargetReserve.IConstArg arg = new GlidingTargetReserve.ConstArg(
				this,
				reservedSpace
			);
			return new GlidingTargetReserve(arg);
		}
		public override void SetUpReference(){
			IGlidingTarget[] glidingTargets = CreateGlidingTargets();
			thisTypedReserve.SetSceneObjects(glidingTargets);
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
				targetAdaptor.SetGlidingTargetReserve(thisTypedReserve);
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
