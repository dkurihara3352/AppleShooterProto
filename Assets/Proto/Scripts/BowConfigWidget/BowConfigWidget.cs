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
		void SetResourcePanel(IResourcePanel resourcePanel);


		void TrySetEquippedBow(int index);
		void IncreaseAttributeLevel(int attributeIndex);
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
		}
		IResourcePanel thisResourcePanel;
		public void SetResourcePanel(IResourcePanel panel){
			thisResourcePanel = panel;
		}
		void LoadAndFeedAllPanelsWithPlayerData(){
			
			thisPlayerDataManager.SetFileIndex(0);
			thisPlayerDataManager.Load();
			int equippedBowIndex = thisPlayerDataManager.GetEquippedBowIndex();
			IBowConfigData[] configDataArray = thisPlayerDataManager.GetBowConfigDataArray();

			foreach(IBowPanel bowPanel in thisBowPanels){
				int index = bowPanel.GetIndex();
				IBowConfigData bowConfigData = configDataArray[index];
				if(!bowConfigData.IsUnlocked()){
					bowPanel.Lock(instantly: true);
				}else{
					bowPanel.Unlock(instantly: true);
				}
				if(index == equippedBowIndex)
					bowPanel.SetEquippedness(isEquipped: true, instantly: true);
				else
					bowPanel.SetEquippedness(isEquipped: false, instantly: true);
				
				int bowLevel = bowConfigData.GetBowLevel();
				bowPanel.SetBowLevel(bowLevel, instantly: false);

				int[] attributeLevels = bowConfigData.GetAttributeLevels();
				int attributeIndex = 0;
				foreach(int attributeLevel in attributeLevels)
					bowPanel.SetAttributeLevel(
						attributeIndex++, 
						attributeLevel,
						true
					);
				Debug.Log(
					"BowPanel " + bowPanel.GetIndex().ToString() + ", " +
					"unlocked: " + bowConfigData.IsUnlocked().ToString() + ", " +
					"equipped: " + (index == equippedBowIndex).ToString() + ", " +
					"bowLevel: " + bowConfigData.GetBowLevel().ToString() + ", " +
					"attLevels: " + DKUtility.DebugHelper.GetIndicesString(attributeLevels)
				);
			}
		}
		public void DeactivateImple(){
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

			IBowConfigData configData = thisPlayerDataManager.GetBowConfigDataArray()[equippedBowIndex];
			int bowLevel = configData.GetBowLevel();
			panel.SetBowLevel(bowLevel, false);

			int attributeLevel = configData.GetAttributeLevels()[attributeIndex];
			panel.SetAttributeLevel(
				attributeIndex,
				attributeLevel, 
				false
			);

			thisPlayerDataManager.Save();
		}
	}
}

