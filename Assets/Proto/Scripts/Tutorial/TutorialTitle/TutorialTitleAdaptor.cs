using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface ITutorialTitleAdaptor : ISlickBowShootingMonoBehaviourAdaptor{
		ITutorialTitle GetTutorialTitle();
		float GetShowProcessTime();
		void SetAlpha(float value);
	}
	public class TutorialTitleAdaptor: SlickBowShootingMonoBehaviourAdaptor, ITutorialTitleAdaptor{
		public override void SetUp(){
			thisTutorialTitle = CreateTutorialTitle();
		}
		ITutorialTitle thisTutorialTitle;
		ITutorialTitle CreateTutorialTitle(){
			TutorialTitle.IConstArg arg = new TutorialTitle.ConstArg(
				this
			);
			return new TutorialTitle(arg);
		}
		public ITutorialTitle GetTutorialTitle(){
			return thisTutorialTitle;
		}
		public override void FinalizeSetUp(){
			thisTutorialTitle.Deactivate();
			SetAlpha(0f);
		}
		public override void SetUpReference(){
			base.SetUpReference();
			ITutorialBaseUIElement tutorialBaseUIElement = tutorialBaseUIAdaptor.GetTutorialBaseUIElement();
			thisTutorialTitle.SetTutorialBaseUIElement(tutorialBaseUIElement);

			ITutorialManager tutorialManager = tutorialManagerAdaptor.GetTutorialManager();
			thisTutorialTitle.SetTutorialManager(tutorialManager);
		}
		public TutorialBaseUIAdaptor tutorialBaseUIAdaptor;
		public float GetShowProcessTime(){
			return showProcessTime;
		}
		public float showProcessTime = 1f;
		public void SetAlpha(float value){
			canvasGroup.alpha = value;
		}
		public CanvasGroup canvasGroup;
		public TutorialManagerAdaptor tutorialManagerAdaptor;
	}
}


