using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IGameStatsTracker: ISceneObject{
		void SetHeatManager(IHeatManager manager);

		void RegisterTargetDestroyed(IShootingTarget target);
	}
	public class GameStatsTracker : AbsSceneObject, IGameStatsTracker {

		public GameStatsTracker(
			IConstArg arg
		): base(arg){

		}
		IHeatManager thisHeatManager;
		public void SetHeatManager(IHeatManager manager){
			thisHeatManager = manager;
		}

		public void RegisterTargetDestroyed(IShootingTarget target){
			float heat = target.GetHeatBonus();
			thisHeatManager.AddHeat(heat);
		}

		/*  */
		public new interface IConstArg: AbsSceneObject.IConstArg{

		}
		public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IGameStatsTrackerAdaptor adaptor
			): base(adaptor){

			}
		}
	}
}
