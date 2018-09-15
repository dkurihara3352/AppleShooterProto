using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface ICoreGameplayInputScrollerAdaptor: IGenericSingleElementScrollerAdaptor{
		ICoreGameplayInputScroller GetInputScroller();
	}
	public class CoreGameplayInputScrollerAdaptor : GenericSingleElementScrollerAdaptor, ICoreGameplayInputScrollerAdaptor {
		public PlayerInputManagerAdaptor inputManagerAdaptor;
		protected override IUIElement CreateUIElement(IUIImage image){
			// IPlayerInputManager inputManager = inputManagerAdaptor.GetInputManager();
			ICoreGameplayInputScrollerConstArg arg = new CoreGameplayInputScrollerConstArg(
				// inputManager,
				
				relativeCursorLength,
				scrollerAxis,
				rubberBandLimitMultiplier,
				relativeCursorPosition,
				isEnabledInertia,
				locksInputAboveThisVelocity,

				thisDomainInitializationData.uim,
				thisDomainInitializationData.processFactory,
				thisDomainInitializationData.uiElementFactory,
				this,
				image,
				activationMode
			);
			return new CoreGameplayInputScroller(arg);
		}
		public ICoreGameplayInputScroller GetInputScroller(){
			if(GetUIElement() == null)
				Debug.Log("FUCK");
			return (ICoreGameplayInputScroller)GetUIElement();
		}
	}
}
