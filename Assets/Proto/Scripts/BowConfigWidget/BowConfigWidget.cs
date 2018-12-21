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
			thisBowPanelScroller.SnapToGroupElement(equippedBowIndex);
		}
		void LoadAndFeedAllPanelsWithPlayerData(){

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
			}

		}
		public void DeactivateImple(){
			return;
		}

	}
}

