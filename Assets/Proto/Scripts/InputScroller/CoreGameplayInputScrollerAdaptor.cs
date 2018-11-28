using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface ICoreGameplayInputScrollerAdaptor: IGenericSingleElementScrollerAdaptor{
		ICoreGameplayInputScroller GetInputScroller();
	}
	public class CoreGameplayInputScrollerAdaptor : GenericSingleElementScrollerAdaptor, ICoreGameplayInputScrollerAdaptor {
		protected override IUIElement CreateUIElement(){
			CoreGameplayInputScroller.IConstArg arg = new CoreGameplayInputScroller.ConstArg(
				
				relativeCursorLength,
				scrollerAxis,
				rubberBandLimitMultiplier,
				relativeCursorPosition,
				isEnabledInertia,
				inertiaDecay,
				locksInputAboveThisVelocity,

				this,
				activationMode
			);
			return new CoreGameplayInputScroller(arg);
		}
		public override void SetUpReference(){
			base.SetUpReference();
			IPlayerInputManager inputManager = GetInputManager();
			thisInputScroller.SetPlayerInputManager(inputManager);
		}
		public PlayerInputManagerAdaptor inputManagerAdaptor;
		IPlayerInputManager GetInputManager(){
			return inputManagerAdaptor.GetInputManager();
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
		ICoreGameplayInputScroller thisInputScroller{
			get{
				return (ICoreGameplayInputScroller)GetUIElement();
			}
		}
		public ICoreGameplayInputScroller GetInputScroller(){
			return thisInputScroller;
		}
		public float GetScrollMultiplier(){
			return GetInputScroller().GetScrollMultiplier();
		}
	}
}
