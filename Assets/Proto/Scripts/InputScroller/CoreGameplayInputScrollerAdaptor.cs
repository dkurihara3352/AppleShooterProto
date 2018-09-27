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
		protected override void SetUpUIElementReferenceImple(){
			base.SetUpUIElementReferenceImple();
			IUIAdaptor scrollerElementAdaptor = thisChildUIAdaptors[0];
			Vector2 newRectLength = CalculateScrollerElementRectLength();
			scrollerElementAdaptor.SetRectLength(newRectLength);

		}
		public PlayerCameraAdaptor playerCameraAdaptor;
		public Vector2 panAngleMultiplier = Vector2.one;
		Vector2 maxPanAngle{
			get{return playerCameraAdaptor.GetMaxPanAngle();}
		}
		Vector2 thisScreenLength{
			get{return new Vector2(
				Screen.width,
				Screen.height
			);}
		}
		Vector2 CalculateScrollerElementRectLength(){
			Vector2 bothFOVs = playerCameraAdaptor.GetDefaultFOVs();
			Vector2 result = new Vector2();
			for(int i = 0; i < 2; i++){
				float degreePerPixel = bothFOVs[i]/ thisScreenLength[i];
				float multipliedDPP = degreePerPixel * panAngleMultiplier[i];
				result[i] = (maxPanAngle[i] / multipliedDPP) +thisScreenLength[i];
			}
			return result;
		}
		public ICoreGameplayInputScroller GetInputScroller(){
			return (ICoreGameplayInputScroller)GetUIElement();
		}
		public float GetScrollMultiplier(){
			return GetInputScroller().GetScrollMultiplier();
		}
	}
}
