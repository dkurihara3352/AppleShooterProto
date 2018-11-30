using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IGameStatsTracker: IAppleShooterSceneObject{
		void SetHeatManager(IHeatManager manager);
		void SetScoreManager(IScoreManager scoreManager);

		void RegisterTargetDestroyed(IShootingTarget target);
	}
	public class GameStatsTracker : AppleShooterSceneObject, IGameStatsTracker {

		public GameStatsTracker(
			IConstArg arg
		): base(arg){

		}
		IHeatManager thisHeatManager;
		public void SetHeatManager(IHeatManager manager){
			thisHeatManager = manager;
		}
		IScoreManager thisScoreManager;
		public void SetScoreManager(IScoreManager scoreManager){
			thisScoreManager = scoreManager;
		}

		public void RegisterTargetDestroyed(IShootingTarget target){
			float heat = target.GetHeatBonus();
			thisHeatManager.AddHeat(heat);

			int score = target.GetDestructionScore();
			thisScoreManager.AddScore(score);
		}

		/*  */
		public new interface IConstArg: AppleShooterSceneObject.IConstArg{

		}
		public new class ConstArg: AppleShooterSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IGameStatsTrackerAdaptor adaptor
			): base(adaptor){

			}
		}
	}
}
