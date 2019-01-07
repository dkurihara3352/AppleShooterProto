using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IGameStatsTrackerAdaptor: ISlickBowShootingMonoBehaviourAdaptor{
		IGameStatsTracker GetTracker();
	}
	public class GameStatsTrackerAdaptor : SlickBowShootingMonoBehaviourAdaptor, IGameStatsTrackerAdaptor {

		public override void SetUp(){
			thisTracker = CreateTracker();
		}
		IGameStatsTracker thisTracker;
		public IGameStatsTracker GetTracker(){
			return thisTracker;
		}
		IGameStatsTracker CreateTracker(){
			GameStatsTracker.IConstArg arg = new GameStatsTracker.ConstArg(
				this
			);
			return new GameStatsTracker(arg);
		}
		public HeatManagerAdaptor heatManagerAdaptor;
		public ScoreManagerAdaptor scoreManagerAdaptor;
		public CurrencyManagerAdaptor currencyManagerAdaptor;
		public override void SetUpReference(){
			IHeatManager heatManager = heatManagerAdaptor.GetHeatManager();
			thisTracker.SetHeatManager(heatManager);

			IScoreManager scoreManager = scoreManagerAdaptor.GetScoreManager();
			thisTracker.SetScoreManager(scoreManager);

			ICurrencyManager currencyManager = currencyManagerAdaptor.GetCurrencyManager();
			thisTracker.SetCurrencyManager(currencyManager);
		}
	}
}
