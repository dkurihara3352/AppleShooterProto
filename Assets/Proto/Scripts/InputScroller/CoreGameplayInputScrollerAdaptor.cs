using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface ICoreGameplaceInputScrollerAdaptor: IGenericSingleElementScrollerAdaptor{}
	public class CoreGameplayInputScrollerAdaptor : GenericSingleElementScrollerAdaptor, ICoreGameplaceInputScrollerAdaptor {
		public PlayerInputManagerAdaptor inputManagerAdaptor;
		protected override IUIElement CreateUIElement(IUIImage image){
			IPlayerInputManager inputManager = inputManagerAdaptor.GetInputManager();
			ICoreGameplayInputScrollerConstArg arg = new CoreGameplayInputScrollerConstArg(
				inputManager,
				
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
	}
}
