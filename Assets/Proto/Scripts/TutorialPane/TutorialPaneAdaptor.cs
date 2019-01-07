using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface ITutorialPaneAdaptor: IUIAdaptor{
		ITutorialPane GetTutorialPane();
		void ToggleRaycastBlock(bool toggles);
	}
	public class TutorialPaneAdaptor: UIAdaptor, ITutorialPaneAdaptor{
		protected override IUIElement CreateUIElement(){
			return CreateTutorialPane();
		}
		ITutorialPane CreateTutorialPane(){
			TutorialPane.IConstArg arg = new TutorialPane.ConstArg(
				this,
				activationMode
			);
			return new TutorialPane(arg);
		}
		ITutorialPane thisTutorialPane{
			get{
				return (ITutorialPane)thisUIElement;
			}
		}
		public ITutorialPane GetTutorialPane(){
			return thisTutorialPane;
		}
		public override void SetUpReference(){
			base.SetUpReference();
			IPopText popText = labelPopTextAdaptor.GetPopText();
			thisTutorialPane.SetTutorialLabelPopText(popText);
			ICoreGameplayInputScroller scroller = inputScrollerAdaptor.GetInputScroller();
			thisTutorialPane.SetCoreGameplayInputScroller(scroller);
			ITutorialPaneInvertAxisButton[] buttons = CollectInvertButtons();
			thisTutorialPane.SetTutorialPaneInvertAxisButtons(buttons);
			IPlayerDataManager playerDataManager = playerDataManagerAdaptor.GetPlayerDataManager();
			thisTutorialPane.SetPlayerDataManager(playerDataManager);
			IGameplayWidget gameplayWidget = gameplayWidgetAdaptor.GetGameplayWidget();
			thisTutorialPane.SetGameplayWidget(gameplayWidget);
		}
		public PopTextAdaptor labelPopTextAdaptor;
		public CoreGameplayInputScrollerAdaptor inputScrollerAdaptor;
		public TutorialPaneInvertAxisButtonAdaptor[] buttonAdaptors;
		ITutorialPaneInvertAxisButton[] CollectInvertButtons(){
			ITutorialPaneInvertAxisButton[] result = new ITutorialPaneInvertAxisButton[2];
			for(int i = 0; i < 2; i ++){
				ITutorialPaneInvertAxisButton button = buttonAdaptors[i].GetTutorialPaneInvertAxisButton();
				result[i] = button;
			}
			return result;
		}

		public void ToggleRaycastBlock(bool toggles){
			thisCanvasGroup.blocksRaycasts = toggles;
		}
		public PlayerDataManagerAdaptor playerDataManagerAdaptor;
		public GameplayWidgetAdaptor gameplayWidgetAdaptor;
	}
}


