using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;
namespace AppleShooterProto{
	public interface IDestroyedTargetReserveAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IDestroyedTargetReserve GetDestroyedTargetReserve();
	}
	public class DestroyedTargetReserveAdaptor : AppleShooterMonoBehaviourAdaptor, IDestroyedTargetReserveAdaptor {

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

			for(int i = 0; i < destroyedTargetsCount; i ++){

				GameObject destroyedTargetGO = GameObject.Instantiate(
					destroyedTargetPrefab
				);

				IDestroyedTargetAdaptor adaptor = (IDestroyedTargetAdaptor)destroyedTargetGO.GetComponent(typeof(IDestroyedTargetAdaptor));

				adaptor.SetPopUIReserveAdaptor(popUIReserveAdaptor);
				adaptor.SetDestroyedTargetReserveAdaptor(this);
				adaptor.SetIndex(i);
				adaptor.SetUp();

				resultList.Add(adaptor);
			}

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
