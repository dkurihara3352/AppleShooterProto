using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IArrowReserveAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IArrowReserve GetArrowReserve();
	}
	public class ArrowReserveAdaptor: AppleShooterMonoBehaviourAdaptor, IArrowReserveAdaptor{
		public override void SetUp(){
			thisReserve = CreateArrowReserve();
			thisArrowAdaptors = CreateArrowAdaptors();
		}
		IArrowReserve thisReserve;
		IArrowReserve CreateArrowReserve(){
			ArrowReserve.IConstArg arg = new ArrowReserve.ConstArg(
				this
			);
			return new ArrowReserve(arg);
		}
		public IArrowReserve GetArrowReserve(){
			return thisReserve;
		}
		IArrowAdaptor[] thisArrowAdaptors;
		public int arrowCounts;
		public GameObject arrowPrefab;
		public int collisionDetectionIntervalFrameCount = 1;
		public LaunchPointAdaptor launchPointAdaptor;
		public ShootingManagerAdaptor shootingManagerAdaptor;
		IArrowAdaptor[] CreateArrowAdaptors(){
			List<IArrowAdaptor> resultList = new List<IArrowAdaptor>();
			for(int i = 0; i < arrowCounts; i ++){
				GameObject arrowGO = GameObject.Instantiate(
					arrowPrefab
				);
				IArrowAdaptor adaptor = (IArrowAdaptor)arrowGO.GetComponent(typeof(IArrowAdaptor));
				adaptor.SetIndex(i);
				adaptor.SetArrowReserveAdaptor(this);
				adaptor.SetCollisionDetectionIntervalFrameCount(collisionDetectionIntervalFrameCount);
				adaptor.SetLaunchPointAdaptor(launchPointAdaptor);
				adaptor.SetShootingManagerAdaptor(shootingManagerAdaptor);

				adaptor.SetUp();

				resultList.Add(adaptor);
			}
			return resultList.ToArray();
		}
		public override void SetUpReference(){
			IArrow[] arrows = CreateArrows();
			thisReserve.SetSceneObjects(arrows);
		}
		IArrow[] CreateArrows(){
			List<IArrow> resultList = new List<IArrow>();
			foreach(IArrowAdaptor adaptor in thisArrowAdaptors)
				resultList.Add(adaptor.GetArrow());
			return resultList.ToArray();
		}
	}
}

