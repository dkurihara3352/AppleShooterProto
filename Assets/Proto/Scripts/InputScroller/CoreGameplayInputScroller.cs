using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface ICoreGameplayInputScroller: IGenericSingleElementScroller{}
	public class CoreGameplayInputScroller : GenericSingleElementScroller, ICoreGameplayInputScroller {

		public CoreGameplayInputScroller(
			ICoreGameplayInputScrollerConstArg arg
		): base(
			arg
		){
			thisInputManager = arg.inputManager;
		}
		Vector2 thisInitialNormalizedCursorPosition = new Vector2(.5f, .5f);
		protected override Vector2 GetInitialNormalizedCursoredPosition(){
			return thisInitialNormalizedCursorPosition;
		}
		readonly IPlayerInputManager thisInputManager;
		protected override void OnScrollerElementDisplace(
			float normalizedCursoredPositionOnAxis, 
			int dimension
		){
			thisInputManager.LookAround(
				normalizedCursoredPositionOnAxis,
				dimension
			);
		}
	}


	public interface ICoreGameplayInputScrollerConstArg: IGenericSingleElementScrollerConstArg{
		IPlayerInputManager inputManager{get;}
	}
	public class CoreGameplayInputScrollerConstArg: GenericSingleElementScrollerConstArg, ICoreGameplayInputScrollerConstArg{
		public CoreGameplayInputScrollerConstArg(
			IPlayerInputManager inputManager

			Vector2 relativeCursorLength, 
			ScrollerAxis scrollerAxis, 
			Vector2 rubberBandLimitMultiplier, 
			Vector2 relativeCursorPosition, 
			bool isEnabledInertia, 
			float newScrollSpeedThreshold,

			IUIManager uim, 
			IUISystemProcessFactory processFactory, 
			IUIElementFactory uieFactory, 
			IGenericSingleElementScrollerAdaptor uia, 
			IUIImage image,
			ActivationMode activationMode

		):base(

			relativeCursorLength,
			scrollerAxis, 
			relativeCursorPosition, 
			rubberBandLimitMultiplier, 
			isEnabledInertia, 
			newScrollSpeedThreshold,

			uim, 
			processFactory, 
			uieFactory, 
			uia, 
			image,
			activationMode
		){

		}
	}
}

