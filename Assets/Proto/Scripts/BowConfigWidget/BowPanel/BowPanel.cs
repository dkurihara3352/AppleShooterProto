using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IBowPanel: IUIElement{
		void SetBowConfigWidget(IBowConfigWidget widget);
		void SetBowLockPane(IBowLockPane pane);
		void SetBowEquippedTextPane(IBowEquippedTextPane pane);
		void SetBowLevelPane(IBowStarsPane pane);
		void SetBowAttributeLevelPanes(IBowStarsPane[] panes);

		int GetIndex();
		void Lock(bool instantly);
		void Unlock(bool instantly);
		void SetEquippedness(bool isEquipped, bool instantly);
		void SetBowLevel(int level, bool instantly);
		void SetAttributeLevel(int index, int level, bool instantly);
	}
	public class BowPanel: UIElement, IBowPanel{
		public BowPanel(IConstArg arg): base(arg){}
		IBowPanelAdaptor thisBowPanelAdaptor{
			get{
				return (IBowPanelAdaptor)thisUIAdaptor;
			}
		}
		public int GetIndex(){
			return thisBowPanelAdaptor.GetBowIndex();
		}
		IBowLockPane thisBowLockPanel;
		public void SetBowLockPane(IBowLockPane panel){
			thisBowLockPanel = panel;
		}
		public void Lock(bool instantly){
			thisBowLockPanel.ActivateThruBackdoor(instantly);
			this.DeactivateRecursively(instantly);
		}
		public void Unlock(bool instantly){
			// Debug.Log(GetName() + " is unlocked");
			thisBowLockPanel.DeactivateThruBackdoor(instantly);
			this.ActivateRecursively(instantly);
		}
		protected override void OnUIActivate(){
			// Debug.Log(GetName() + " is activated");
		}
		protected override void OnUIDeactivate(){
			// Debug.Log(GetName() + " is deactivated");
		}
		

		IBowEquippedTextPane thisBowEquippedTextPane;
		public void SetBowEquippedTextPane(IBowEquippedTextPane pane){
			thisBowEquippedTextPane = pane;
		}
		public void SetEquippedness(bool isEquipped, bool instantly){
			if(isEquipped){
				if(instantly)
					thisBowEquippedTextPane.ShowEquippedText();
				else
					thisBowEquippedTextPane.StartShowTextProcess();
			}else{
				if(instantly)
					thisBowEquippedTextPane.HideEquippedText();
				else
					thisBowEquippedTextPane.StartHideTextProcess();
			}
		}

		IBowStarsPane thisBowLevelPane;
		public void SetBowLevelPane(IBowStarsPane pane){
			thisBowLevelPane = pane;
		}
		public void SetBowLevel(int level, bool instantly){
			if(instantly)
				thisBowLevelPane.UpdateLevel(level);
			else
				thisBowLevelPane.StartUpdateLevelProcess(level);
		}

		IBowStarsPane[] thisBowAttributeLevelPanes;
		public void SetBowAttributeLevelPanes(IBowStarsPane[] panes){
			thisBowAttributeLevelPanes = panes;
		}
		public void SetAttributeLevel(
			int index,
			int level,
			bool instantly
		){
			IBowStarsPane pane = thisBowAttributeLevelPanes[index];
			if(instantly)
				pane.UpdateLevel(level);
			else
				pane.StartUpdateLevelProcess(level);
		}

		IBowConfigWidget thisWidget;
		public void SetBowConfigWidget(IBowConfigWidget widget){
			thisWidget = widget;
		}
		public override void OnScrollerFocus(){
			base.OnScrollerFocus();
			if(thisWidget.IsActivated()){
				
				thisWidget.TrySetEquippedBow(GetIndex());
			}
		}
	}
}
