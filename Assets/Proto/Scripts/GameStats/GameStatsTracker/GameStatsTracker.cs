using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IGameStatsTracker: IAppleShooterSceneObject{
		void SetHeatManager(IHeatManager manager);
		void SetScoreManager(IScoreManager scoreManager);
		void SetCurrencyManager(ICurrencyManager currencyManager);

		void RegisterTargetDestroyed(IShootingTarget target, bool isRare);
		void ResetStats();
		void MarkGameplayEnded();
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

		ICurrencyManager thisCurrencyManager;
		public void SetCurrencyManager(ICurrencyManager manager){
			thisCurrencyManager = manager;
		}

		public void RegisterTargetDestroyed(IShootingTarget target, bool isRare){
			if(!thisGameplayIsEnded){
				float heat = target.GetHeatBonus();
				thisHeatManager.AddHeat(heat);

				int score = target.GetDestructionScore();
				thisScoreManager.AddScore(score);

				if(isRare){
					int targetTier = target.GetTier();
					int currencyGained = /* thisRareDestructionCurrencyBonusCalculator.Calculate(targetTier) */100;
					thisCurrencyManager.AddGainedCurrency(currencyGained);
				}
			}
		}
		bool thisGameplayIsEnded = false;
		public void ResetStats(){
			thisHeatManager.ResetHeat();
			thisScoreManager.ClearScore();
			thisCurrencyManager.ClearGainedCurrency();
			thisGameplayIsEnded = false;
		}
		public void MarkGameplayEnded(){
			thisGameplayIsEnded = true;
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
