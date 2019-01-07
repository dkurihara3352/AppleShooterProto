﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IGameStatsTracker: ISlickBowShootingSceneObject{
		void SetHeatManager(IHeatManager manager);
		void SetScoreManager(IScoreManager scoreManager);
		void SetCurrencyManager(ICurrencyManager currencyManager);

		void RegisterTargetDestroyed(IShootingTarget target, bool isRare);
		void ResetStats();
		void MarkGameplayEnded();
	}
	public class GameStatsTracker : SlickBowShootingSceneObject, IGameStatsTracker {

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
					int currencyGained = CalculateCurrencyGained(targetTier);
					thisCurrencyManager.AddGainedCurrency(currencyGained);

					IDestroyedTarget destroyedTarget = target.GetDestroyedTarget();
					destroyedTarget.PopText(
						currencyGained.ToString(),
						new Color(.8f, .2f, .9f)
					);
				}
			}
		}
		int CalculateCurrencyGained(int tier){
			if(tier == 0)
				return 10;
			else if(tier == 1)
				return 100;
			else if(tier == 2)
				return 500;
			else return 0;
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
		public new interface IConstArg: SlickBowShootingSceneObject.IConstArg{

		}
		public new class ConstArg: SlickBowShootingSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IGameStatsTrackerAdaptor adaptor
			): base(adaptor){

			}
		}
	}
}
