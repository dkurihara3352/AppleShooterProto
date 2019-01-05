using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;
using UISystem;

namespace AppleShooterProto{
	public interface IBowConfigWidget: IAppleShooterSceneObject, IActivationStateImplementor, IActivationStateHandler{
		void SetBowPanelGroupScroller(IUIElementGroupScroller scroller);
		void SetBowPanels(IBowPanel[] panels);
		void SetPlayerDataManager(IPlayerDataManager manager);
		void SetBowDataCalculator(IBowDataCalculator calculator);
		void SetResourcePanel(IResourcePanel resourcePanel);
		void SetCurrencyPane(ICurrencyPane pane);
		void SetBowUnlockButtons(IBowUnlockButton[] buttons);
		void SetBowConfigLabelPopText(IPopText popText);

		void TrySetEquippedBow(int index);
		void IncreaseAttributeLevel(int attributeIndex);
		void UnlockPanel(int panelIndex);
	}
	public class BowConfigWidget: AppleShooterSceneObject, IBowConfigWidget{
		public BowConfigWidget(IConstArg arg): base(arg){
			thisActivationStateEngine = new ActivationStateEngine(this);
		}

		IActivationStateEngine thisActivationStateEngine;
		public void Activate(){
			thisActivationStateEngine.Activate();
		}
		public void Deactivate(){
			thisActivationStateEngine.Deactivate();
		}
		public bool IsActivated(){
			return thisActivationStateEngine.IsActivated();
		}

		IUIElementGroupScroller thisBowPanelScroller;
		public void SetBowPanelGroupScroller(IUIElementGroupScroller scroller){
			thisBowPanelScroller = scroller;
		}
		IPlayerDataManager thisPlayerDataManager;
		public void SetPlayerDataManager(IPlayerDataManager manager){
			thisPlayerDataManager = manager;
		}
		IBowDataCalculator thisCalculator;
		public void SetBowDataCalculator(IBowDataCalculator calculator){
			thisCalculator = calculator;
		}
		IBowPanel[] thisBowPanels;
		public void SetBowPanels(IBowPanel[] panels){
			thisBowPanels = panels;
		}
		
		
		public void ActivateImple(){
			LoadAndFeedAllPanelsWithPlayerData();
			int equippedBowIndex = thisPlayerDataManager.GetEquippedBowIndex();
			thisBowPanelScroller.PlaceGroupElementUnderCursor(equippedBowIndex);

			if(!thisResourcePanel.IsShown())//temp
					thisResourcePanel.Show();

			int currency = thisPlayerDataManager.GetCurrency();
			thisCurrencyPane.StartCurrencyUpdateProcess(currency);//temp

			foreach(IBowUnlockButton button in thisBowUnlockButtons){
				int unlockCost = thisPlayerDataManager.GetBowUnlockCostArray()[button.GetPanelIndex()];
				button.SetCostText(unlockCost);
			}
			thisBowConfigLabelPopText.Pop("Bow Customization", false);
		}
		IPopText thisBowConfigLabelPopText;
		public void SetBowConfigLabelPopText(IPopText popText){
			thisBowConfigLabelPopText = popText;
		}
		IResourcePanel thisResourcePanel;
		public void SetResourcePanel(IResourcePanel panel){
			thisResourcePanel = panel;
		}
		ICurrencyPane thisCurrencyPane;
		public void SetCurrencyPane(ICurrencyPane pane){
			thisCurrencyPane = pane;
		}
		void LoadAndFeedAllPanelsWithPlayerData(){
			
			thisPlayerDataManager.SetFileIndex(0);
			thisPlayerDataManager.Load();
			int equippedBowIndex = thisPlayerDataManager.GetEquippedBowIndex();
			IBowConfigData[] configDataArray = thisPlayerDataManager.GetBowConfigDataArray();

			foreach(IBowPanel bowPanel in thisBowPanels){
				int bowIndex = bowPanel.GetIndex();
				IBowConfigData bowConfigData = configDataArray[bowIndex];
				if(!bowConfigData.IsUnlocked()){
					bowPanel.Lock(instantly: true);
				}else{
					bowPanel.Unlock(instantly: true);
				}
				if(bowIndex == equippedBowIndex)
					bowPanel.SetEquippedness(isEquipped: true, instantly: true);
				else
					bowPanel.SetEquippedness(isEquipped: false, instantly: true);
				
				int bowLevel = bowConfigData.GetBowLevel();
				bowPanel.SetBowLevel(bowLevel, instantly: false);
				
				UpdateAttributePanel(bowPanel, bowConfigData);
			}
			UpdateUnlockButtons();
		}
		void UpdateAttributePanel(
			IBowPanel bowPanel,
			IBowConfigData bowConfigData
		){
				int[] attributeLevels = bowConfigData.GetAttributeLevels();
				int bowIndex = bowPanel.GetIndex();
				int attributeIndex = 0;
			
				foreach(int attributeLevel in attributeLevels){
					bowPanel.SetAttributeLevel(
						attributeIndex, 
						attributeLevel,
						true
					);
					IBowAttributeLevelUpHoldButton levelUpButton = bowPanel.GetBowAttributeLevelUpHoldButtons()[attributeIndex];
					if(attributeLevel == thisPlayerDataManager.GetMaxAttributeLevel())
						levelUpButton.MaxOut();
					else{

						int nextLevel = attributeLevel + 1;
						int nextCost = CalculateCost(
							bowIndex,
							nextLevel
						);
						levelUpButton.SetNextCost(nextCost);
						
						int currency = thisPlayerDataManager.GetCurrency();
						if(currency < nextCost)
							levelUpButton.InvalidateForShortMoney();
						else
							levelUpButton.ValidateForLevelUp();
					}
					attributeIndex += 1;
				}
				int equippedBowIndex = thisPlayerDataManager.GetEquippedBowIndex();
				Debug.Log(
					"BowPanel " + bowPanel.GetIndex().ToString() + ", " +
					"unlocked: " + bowConfigData.IsUnlocked().ToString() + ", " +
					"equipped: " + (bowIndex == equippedBowIndex).ToString() + ", " +
					"bowLevel: " + bowConfigData.GetBowLevel().ToString() + ", " +
					"attLevels: " + DKUtility.DebugHelper.GetIndicesString(attributeLevels)
				);
		}
		IBowUnlockButton[] thisBowUnlockButtons;
		public void SetBowUnlockButtons(IBowUnlockButton[] buttons){
			thisBowUnlockButtons = buttons;
		}
		void UpdateUnlockButtons(){
			int currency = thisPlayerDataManager.GetCurrency();
			foreach(IBowUnlockButton unlockButton in thisBowUnlockButtons){
				int index = unlockButton.GetPanelIndex();
				IBowConfigData configData = thisPlayerDataManager.GetBowConfigDataArray()[index];
				if(!configData.IsUnlocked()){
					int unlockCost = thisPlayerDataManager.GetBowUnlockCostArray()[index];
					if(currency < unlockCost)
						unlockButton.InvalidateForShortMoney();
					else
						unlockButton.ValidateForUnlock();
				}
			}
		}
		int CalculateCost(
			int bowIndex,
			int level
		){
			IBowConfigData bowConfigData = thisPlayerDataManager.GetBowConfigDataArray()[bowIndex];
			int[] levels = bowConfigData.GetAttributeLevels();
			float coinDepreValue = CalculateCoinDepreciationValue(
				levels,
				thisCalculator
			);
			int cost = thisCalculator.CalcCost(
				level,
				coinDepreValue
			);
			return cost;
		}
		float CalculateCoinDepreciationValue(
			int[] levels,
			IBowDataCalculator calculator
		){
			float totalAttributeValues = 0f;
			foreach(int level in levels){
				float attributeValue = calculator.GetAttributeValue(level);
				totalAttributeValues += attributeValue;
			}
			return calculator.CalcCoinDepreciationValue(
				totalAttributeValues
			);
		}
		public void DeactivateImple(){
			thisBowConfigLabelPopText.Unpop(false);
			return;
		}
		/* Data Manipulation */
		public void TrySetEquippedBow(int index){
			if(!thisPlayerDataManager.PlayerDataIsLoaded())
				thisPlayerDataManager.Load();
			int prevEquippedBowIndex = thisPlayerDataManager.GetEquippedBowIndex();
			if(prevEquippedBowIndex != index){
				IBowConfigData configData = thisPlayerDataManager.GetBowConfigDataArray()[index];
				if(configData.IsUnlocked()){
					thisPlayerDataManager.SetEquippedBow(index);
					thisPlayerDataManager.Save();

					IBowPanel panelToEquip = thisBowPanels[index];
					IBowPanel panelToUnequip = thisBowPanels[prevEquippedBowIndex];

					panelToEquip.SetEquippedness(true, false);
					panelToUnequip.SetEquippedness(false, false);
				}
			}
		}
		public void IncreaseAttributeLevel(int attributeIndex){
			if(!thisPlayerDataManager.PlayerDataIsLoaded())
				thisPlayerDataManager.Load();
			
			thisPlayerDataManager.IncreaseAttributeLevel(attributeIndex);

			int equippedBowIndex = thisPlayerDataManager.GetEquippedBowIndex();
			IBowPanel panel = thisBowPanels[equippedBowIndex];
			IBowConfigData[] dataArray = thisPlayerDataManager.GetBowConfigDataArray();
			IBowConfigData data = dataArray[equippedBowIndex];
			int bowLevel = data.GetBowLevel();
			panel.SetBowLevel(bowLevel, false);
			
			IBowAttributeLevelUpHoldButton button = panel.GetBowAttributeLevelUpHoldButtons()[attributeIndex];

			int currency = thisPlayerDataManager.GetCurrency();
			int newCurrency = currency - button.GetCost();
			UpdateCurrency(newCurrency);

			foreach(IBowPanel bowPanel in thisBowPanels){
				IBowConfigData configData = dataArray[bowPanel.GetIndex()];
				UpdateAttributePanel(
					bowPanel,
					configData
				);
			}
			UpdateUnlockButtons();
			thisPlayerDataManager.Save();
		}
		void UpdateCurrency(int newCurrency){
			thisPlayerDataManager.SetCurrency(newCurrency);
			thisCurrencyPane.StartCurrencyUpdateProcess(newCurrency);
		}
		public void UnlockPanel(int index){
			if(!thisPlayerDataManager.PlayerDataIsLoaded())
				thisPlayerDataManager.Load();

			IBowConfigData data = thisPlayerDataManager.GetBowConfigDataArray()[index];
			data.Unlock();

			int currency = thisPlayerDataManager.GetCurrency();
			int cost = thisPlayerDataManager.GetBowUnlockCostArray()[index];
			int newCurrency = currency - cost;
			UpdateCurrency(newCurrency);
			
			TrySetEquippedBow(index);

			IBowPanel panel = thisBowPanels[index];
			panel.Unlock(false);
		}
	}
}

