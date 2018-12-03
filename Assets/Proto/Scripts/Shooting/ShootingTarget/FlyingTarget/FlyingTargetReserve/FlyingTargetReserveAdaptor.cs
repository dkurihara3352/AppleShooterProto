using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;

namespace AppleShooterProto{
	public interface IFlyingTargetReserveAdaptor: IShootingTargetReserveAdaptor{
		IFlyingTargetReserve GetFlyingTargetReserve();
	}
	public class FlyingTargetReserveAdaptor: AbsShootingTargetReserveAdaptor, IFlyingTargetReserveAdaptor{
		IFlyingTargetReserve thisTypedReserve{
			get{
				return (IFlyingTargetReserve)thisReserve;
			}
		}
		public override void SetUp(){
			thisReserve = CreateFlyingTargetReserve();
			thisFlyingTargetAdaptors = CreateFlyingTargetAdaptors();
		}
		IFlyingTargetReserve CreateFlyingTargetReserve(){
			FlyingTargetReserve.IConstArg arg = new FlyingTargetReserve.ConstArg(
				this,
				reservedSpace
			);
			return new FlyingTargetReserve(arg);
		}
		public IFlyingTargetReserve GetFlyingTargetReserve(){
			return thisTypedReserve;
		}
		public override void SetUpReference(){
			IFlyingTarget[] targets = CreateFlyingTargets();
			thisTypedReserve.SetSceneObjects(targets);
		}
		IFlyingTargetAdaptor[] thisFlyingTargetAdaptors;
		IFlyingTargetAdaptor[] CreateFlyingTargetAdaptors(){
			List<IFlyingTargetAdaptor> resultList = new List<IFlyingTargetAdaptor>();


			for(int i = 0; i < targetCount; i++){
				GameObject targetGO = GameObject.Instantiate(
					shootingTargetPrefab
				);
				IFlyingTargetAdaptor targetAdaptor = (IFlyingTargetAdaptor)targetGO.GetComponent(typeof(IFlyingTargetAdaptor));

				targetAdaptor.SetFlyingTargetReserve(thisTypedReserve);
				targetAdaptor.SetIndex(i);
				targetAdaptor.SetPopUIReserveAdaptor(popUIReserveAdaptor);
				targetAdaptor.SetDestroyedTargetReserveAdaptor(destroyedTargetReserveAdaptor);
				targetAdaptor.SetGameStatsTrackerAdaptor(gameStatsTrackerAdaptor);
				targetAdaptor.SetShootingManagerAdaptor(shootingManagerAdaptor);
				
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

