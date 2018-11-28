using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IGenericSingleElementScrollerAdaptor: IScrollerAdaptor{
	}
	public class GenericSingleElementScrollerAdaptor: AbsScrollerAdaptor<IGenericSingleElementScroller>, IGenericSingleElementScrollerAdaptor{
		public Vector2 relativeCursorLength;
		protected override IUIElement CreateUIElement(){
			GenericSingleElementScroller.IConstArg arg = new GenericSingleElementScroller.ConstArg(
				relativeCursorLength, 
				
				scrollerAxis, 
				relativeCursorPosition, 
				rubberBandLimitMultiplier, 
				isEnabledInertia, 
				inertiaDecay,
				locksInputAboveThisVelocity,
				
				this, 
				activationMode
			);
			return new GenericSingleElementScroller(arg);
		}
	}
}

