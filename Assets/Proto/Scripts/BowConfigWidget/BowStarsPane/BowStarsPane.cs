using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;
using DKUtility;

namespace AppleShooterProto{
	public interface IBowStarsPane: IUIElement, IProcessHandler{
		void UpdateLevel(int level);
		void StartUpdateLevelProcess(int level);
	}
	public class BowStarsPane: UIElement, IBowStarsPane{
		public BowStarsPane(IConstArg arg): base(arg){
			thisUpdateBowLevelProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisBowLevelPaneAdaptor.GetProcessTime()
			);
		}
		IBowStarsPaneAdaptor thisBowLevelPaneAdaptor{
			get{
				return (IBowStarsPaneAdaptor)thisUIAdaptor;
			}
		}
		IProcessSuite thisUpdateBowLevelProcessSuite;

		int thisStepCount{
			get{
				return thisBowLevelPaneAdaptor.GetStepCount();
			}
		}
		public void UpdateLevel(int level){
			float targetFill = CalcFill(level);
			thisBowLevelPaneAdaptor.SetFill(targetFill);
		}
		protected float CalcFill(int level){
			float result = 0f;
			int quotient = level/ thisStepCount;
			result += quotient * 1f;
			int modulo = level - quotient * thisStepCount;
			result += modulo/(thisStepCount * 1f);
			return result;
		}
		float thisInitialFill;
		float thisTargetFill;
		public void StartUpdateLevelProcess(int level){
			thisInitialFill = thisBowLevelPaneAdaptor.GetFill();
			thisTargetFill = CalcFill(level);
			thisUpdateBowLevelProcessSuite.Start();
		}

		public void OnProcessRun(IProcessSuite suite){
		}
		public void OnProcessUpdate(
			float deltaTime,
			float normalizedTime,
			IProcessSuite suite
		){
			if(suite == thisUpdateBowLevelProcessSuite){
				AnimationCurve fillCurve = thisBowLevelPaneAdaptor.GetFillCurve();
				float normalizedNewFill = fillCurve.Evaluate(normalizedTime);
				float newFill = Mathf.Lerp(
					thisInitialFill,
					thisTargetFill,
					normalizedNewFill
				);
				thisBowLevelPaneAdaptor.SetFill(newFill);
			}
		}
		public void OnProcessExpire(IProcessSuite suite){
			if(suite == thisUpdateBowLevelProcessSuite)
				thisBowLevelPaneAdaptor.SetFill(thisTargetFill);
		}
	}
}

