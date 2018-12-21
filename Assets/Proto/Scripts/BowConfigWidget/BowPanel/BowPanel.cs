using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IBowPanel: IUIElement{
		void SetBowLockPanel(IBowLockPane pane);
		void SetBowEquippedTextPanel(IBowEquippedTextPane pane);
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
		public void SetBowLockPanel(IBowLockPane panel){
			thisBowLockPanel = panel;
		}
		public void Lock(bool instantly){
			thisBowLockPanel.ActivateRecursively(instantly);
			this.DeactivateRecursively(instantly);
		}
		public void Unlock(bool instantly){
			thisBowLockPanel.DeactivateRecursively(instantly);
			this.ActivateRecursively(instantly);
		}

		IBowEquippedTextPane thisBowEquippedTextPane;
		public void SetBowEquippedTextPanel(IBowEquippedTextPane pane){
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
	}
}
