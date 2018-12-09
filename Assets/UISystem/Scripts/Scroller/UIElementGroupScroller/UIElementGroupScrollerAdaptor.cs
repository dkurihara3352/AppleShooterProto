using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IUIElementGroupScrollerAdaptor: IScrollerAdaptor{
		int[] GetCursorSize();
	}
	public class UIElementGroupScrollerAdaptor: AbsScrollerAdaptor<IUIElementGroupScroller>, IUIElementGroupScrollerAdaptor{
		public int initiallyCursoredElementIndex = 0;
		public int[] cursorSize = new int[2]{1, 1};
		public int[] GetCursorSize(){
			return cursorSize;
		}
		public float startSearchSpeed = 200f;
		public bool swipeToSnapNext = false;
		public bool activatesCursoredElementsOnly = false;
		
		protected override IUIElement CreateUIElement(/* IUIImage image */){
			UIElementGroupScroller.IConstArg arg = new UIElementGroupScroller.ConstArg(
				initiallyCursoredElementIndex, 
				cursorSize, 
				startSearchSpeed, 
				activatesCursoredElementsOnly,

				relativeCursorPosition, 
				scrollerAxis, 
				rubberBandLimitMultiplier, 
				isEnabledInertia, 
				inertiaDecay,
				swipeToSnapNext, 
				locksInputAboveThisVelocity,
				
				this, 
				activationMode
			);
			return new UIElementGroupScroller(arg);
		}
		protected override void SetUpScrollerReference(){
			return;
		}
		public override void RecalculateRect(){
			thisScroller.UpdateRect();
			IUIElementGroup thisUIElementGroup = GetUIElementGroup();
			IUIElementGroupAdaptor adaptor = (IUIElementGroupAdaptor)thisUIElementGroup.GetUIAdaptor();
			adaptor.Fuck();
			SetUpScrollerElementAndCursor();
		}
		IUIElementGroup GetUIElementGroup(){
			return (IUIElementGroup)GetScrollerElement();
		}
		public override void FinalizeSetUp(){
			SetUpScrollerElementAndCursor();
			base.FinalizeSetUp();
		}

	}
}
