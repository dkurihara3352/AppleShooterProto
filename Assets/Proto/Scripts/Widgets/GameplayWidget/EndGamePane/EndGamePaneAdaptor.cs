using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IEndGamePaneAdaptor: IUIAdaptor{
		IEndGamePane GetEndGamePane();
		float GetShowResultLabelMasterProcessTime();
		float GetShowLabelProcessTime();
		float GetScoreMasterProcessTime();
		float GetShowHighScoreProcessStartTime();
		float GetUpdateHighScoreMasterProcessTime();
		float GetUpdateHighScoreProcessTime();
		float GetCurrencyMasterProcessTime();
		float GetUpdateCurrencyProcessStartTime();
		float GetShowWatchADButtonProcessStartTime();
		float GetUpdateCurrencyProcessTime();
		float GetShowButtonClusterProcessTime();
	}
	public class EndGamePaneAdaptor: UIAdaptor, IEndGamePaneAdaptor{
		protected override IUIElement CreateUIElement(){
			EndGamePane.IConstArg arg = new EndGamePane.ConstArg(
				this,
				activationMode
			);
			return new EndGamePane(arg);
		}
		IEndGamePane thisEndGamePane{
			get{
				return (IEndGamePane)thisUIElement;
			}
		}
		public IEndGamePane GetEndGamePane(){
			return thisEndGamePane;
		}
		public float showResultLabelMasterProcessTime = 1f;
		public float GetShowResultLabelMasterProcessTime(){
			return showResultLabelMasterProcessTime;
		}
		public float showLabelProcessTime = .5f;
		public float GetShowLabelProcessTime(){
			return showLabelProcessTime;
		}
		public float scoreMasterProcessTime = 2f;
		public float GetScoreMasterProcessTime(){
			return scoreMasterProcessTime;
		}
		public float showHighScoreProcessStartTime = 1f;
		public float GetShowHighScoreProcessStartTime(){
			return showHighScoreProcessStartTime;
		}
		public float updateHighScoreMasterProcessTime = 2f;
		public float GetUpdateHighScoreMasterProcessTime(){
			return updateHighScoreMasterProcessTime;
		}
		public float updateHighScoreProcessTime = 1f;
		public float GetUpdateHighScoreProcessTime(){
			return updateHighScoreProcessTime;
		}
		public float currencyMasterProcessTime = 4f;
		public float GetCurrencyMasterProcessTime(){
			return currencyMasterProcessTime;
		}
		public float updateCurrencyProcessStartTime = 1f;
		public float GetUpdateCurrencyProcessStartTime(){
			return updateCurrencyProcessStartTime;
		}
		public float showWatchADButtonProcessStartTime = 2f;
		public float GetShowWatchADButtonProcessStartTime(){
			return showWatchADButtonProcessStartTime;
		}
		public float updateCurrencyProcessTime = 1f;
		public float GetUpdateCurrencyProcessTime(){
			return updateCurrencyProcessTime;
		}
		public float showButtonClusterProcessTime = 1f;
		public float GetShowButtonClusterProcessTime(){
			return showButtonClusterProcessTime;
		}

		public override void SetUpReference(){
			base.SetUpReference();
			IResultLabelPane resultLabelPane = resultLabelPaneAdaptor.GetResultLabelPane();
			thisEndGamePane.SetResultLabelPane(resultLabelPane);

			IResultScorePane scorePane = resultScorePaneAdaptor.GetResultScorePane();
			thisEndGamePane.SetResultScorePane(scorePane);

			IResultHighScorePane highScorePane = resultHighScorePaneAdaptor.GetResultHighScorePane();
			thisEndGamePane.SetResultHighScorePane(highScorePane);

			IResultCurrencyPane currencyPane = resultCurrencyPaneAdaptor.GetResultCurrencyPane();
			thisEndGamePane.SetResultCurrencyPane(currencyPane);

			IWatchADButton button = watchADButtonAdaptor.GetWatchADButton();
			thisEndGamePane.SetWatchADButton(button);

			IMainMenuButtonCluster cluster = buttonClusterAdaptor.GetMainMenuButtonCluster();
			thisEndGamePane.SetMainMenuButtonCluster(cluster);

			IPlayerDataManager playerDataManager = playerDataManagerAdaptor.GetPlayerDataManager();
			thisEndGamePane.SetPlayerDataManager(playerDataManager);

			IUIElementGroupScroller rootScroller = (IUIElementGroupScroller)rootScrollerAdaptor.GetUIElement();
			thisEndGamePane.SetRootScroller(rootScroller);
		}
		public ResultLabelPaneAdaptor resultLabelPaneAdaptor;
		public ResultScorePaneAdaptor resultScorePaneAdaptor;
		public ResultHighScorePaneAdaptor resultHighScorePaneAdaptor;
		public ResultCurrencyPaneAdaptor resultCurrencyPaneAdaptor;
		public WatchADButtonAdaptor watchADButtonAdaptor;
		public MainMenuButtonClusterAdaptor buttonClusterAdaptor;
		public PlayerDataManagerAdaptor playerDataManagerAdaptor;
		public UIElementGroupScrollerAdaptor rootScrollerAdaptor;
	}
}

