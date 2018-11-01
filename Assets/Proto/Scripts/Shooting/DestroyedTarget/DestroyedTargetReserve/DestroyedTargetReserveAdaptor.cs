using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IDestroyedTargetReserveAdaptor: IMonoBehaviourAdaptor{
		IDestroyedTargetReserve GetDestroyedTargetReserve();
	}
	public class DestroyedTargetReserveAdaptor : MonoBehaviourAdaptor, IDestroyedTargetReserveAdaptor {

		public override void SetUp(){
			thisReserve = CreateDestroyedTargetReserve();
			thisDestroyedTargetAdaptors = CreateDestroyedTargetAdaptors();
		}
		IDestroyedTargetReserve thisReserve;
		IDestroyedTargetReserve CreateDestroyedTargetReserve(){
			DestroyedTargetReserve.IConstArg arg = new DestroyedTargetReserve.ConstArg(
				this	
			);
			return new DestroyedTargetReserve(arg);

		}
		public IDestroyedTargetReserve GetDestroyedTargetReserve(){
			return thisReserve;
		}
		public override void SetUpReference(){
			IDestroyedTarget[] destroyedTargets = CreateDestroyedTargets();
			thisReserve.SetSceneObjects(destroyedTargets);
		}
		public int destroyedTargetsCount;
		public GameObject destroyedTargetPrefab;
		public PopUIReserveAdaptor popUIReserveAdaptor;
		public IDestroyedTargetAdaptor[] thisDestroyedTargetAdaptors;
		IDestroyedTargetAdaptor[] CreateDestroyedTargetAdaptors(){

			List<IDestroyedTargetAdaptor> resultList = new List<IDestroyedTargetAdaptor>();

			IPopUIReserve popUIReserve =  popUIReserveAdaptor.GetPopUIReserve();

			for(int i = 0; i < destroyedTargetsCount; i ++){

				GameObject destroyedTargetGO = GameObject.Instantiate(
					destroyedTargetPrefab
				);

				IDestroyedTargetAdaptor adaptor = (IDestroyedTargetAdaptor)destroyedTargetGO.GetComponent(typeof(IDestroyedTargetAdaptor));

				adaptor.SetPopUIReserve(popUIReserve);
				adaptor.SetIndex(i);
				adaptor.SetDestroyedTargetReserve(thisReserve);
				adaptor.SetUp();

				resultList.Add(adaptor);
			}

			return resultList.ToArray();
		}
		IDestroyedTarget[] Obs_CreateDestroyedTargets(){

			List<IDestroyedTarget> resultList = new List<IDestroyedTarget>();
			IPopUIReserve popUIReserve =  popUIReserveAdaptor.GetPopUIReserve();

			List<IDestroyedTargetAdaptor> adaptorsList = new List<IDestroyedTargetAdaptor>();
			for(int i = 0; i < destroyedTargetsCount; i ++){
				GameObject destroyedTargetGO = GameObject.Instantiate(
					destroyedTargetPrefab
				);
				IDestroyedTargetAdaptor adaptor = (IDestroyedTargetAdaptor)destroyedTargetGO.GetComponent(typeof(IDestroyedTargetAdaptor));
				// adaptor.SetMonoBehaviourAdaptorManager(thisMonoBehaviourAdaptorManager);
				adaptor.SetPopUIReserve(popUIReserve);
				adaptor.SetIndex(i);
				adaptor.SetDestroyedTargetReserve(thisReserve);

				// adaptor.SetUp();
				// adaptor.SetUpReference();

				adaptorsList.Add(adaptor);

				IDestroyedTarget target = adaptor.GetDestroyedTarget();
				resultList.Add(target);
			}

			thisDestroyedTargetAdaptors = adaptorsList.ToArray();

			return resultList.ToArray();
		}
		IDestroyedTarget[] CreateDestroyedTargets(){
			List<IDestroyedTarget> resultList = new List<IDestroyedTarget>();
			foreach(IDestroyedTargetAdaptor adaptor in thisDestroyedTargetAdaptors)
				resultList.Add(adaptor.GetDestroyedTarget());
			return resultList.ToArray();
		}
	}
}
