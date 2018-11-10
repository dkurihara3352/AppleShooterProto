using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface ICoreGameplayInputScroller: IGenericSingleElementScroller{
		void SetScrollMultiplier(float multiplier);
		float GetScrollMultiplier();
		void SetPlayerInputManager(IPlayerInputManager inputManager);
		void SnapToCenter();
		// void AbortSnap();
	}
	public class CoreGameplayInputScroller : GenericSingleElementScroller, ICoreGameplayInputScroller {

		public CoreGameplayInputScroller(
			ICoreGameplayInputScrollerConstArg arg
		): base(
			arg
		){
		}
		Vector2 thisInitialNormalizedCursorPosition = new Vector2(.5f, .5f);
		protected override Vector2 GetInitialNormalizedCursoredPosition(){
			return thisInitialNormalizedCursorPosition;
		}
		IPlayerInputManager thisInputManager;
		public void SetPlayerInputManager(IPlayerInputManager inputManager){
			thisInputManager = inputManager;
		}
		
		protected override void OnScrollerElementDisplace(
			float normalizedCursoredPositionOnAxis, 
			int dimension
		){
			thisInputManager.OnScrollerElementDisplace(
				normalizedCursoredPositionOnAxis,
				dimension
			);
		}
		float minMult = 0.5f;
		float maxMult = 1f;
		public void SetScrollMultiplier(float multiplier){
			thisScrollMultiplier = Mathf.Lerp(minMult, maxMult, multiplier);
			// thisScrollMultiplier = multiplier;
		}
		float thisScrollMultiplier = 1.0f;
		public float GetScrollMultiplier(){
			return thisScrollMultiplier;
		}
		ICustomEventData CreateAdjustedData(ICustomEventData source){
			Vector3 adjustedDeltaPosition = source.deltaPos * thisScrollMultiplier;
			Vector3 adjustedVelocity = source.velocity * thisScrollMultiplier;

			return new CustomEventData(
				source.position,
				adjustedDeltaPosition,
				adjustedVelocity
			);
		}
		protected override void DisplaceScrollerElement(Vector2 deltaPosition){
			base.DisplaceScrollerElement(deltaPosition * thisScrollMultiplier);
		}
		protected override void ProcessSwipe(ICustomEventData eventData){
			// ICustomEventData adjustedData = CreateAdjustedData(eventData);
			base.ProcessSwipe(/* adjustedData */eventData);
			thisInputManager.ProcessSwipe(/* adjustedData */eventData);
		}
		protected override void OnTapImple(int tapCount){
			base.OnTapImple(tapCount);
			thisInputManager.OnTap(tapCount);
		}
		protected override void OnReleaseImple(){
			base.OnReleaseImple();
			thisInputManager.OnRelease();
		}
		protected override void OnTouchImple(int touchCount){
			base.OnTouchImple(touchCount);
			thisInputManager.OnTouch(touchCount);
		}
		protected override void OnBeginDragImple(ICustomEventData eventData){
			ICustomEventData adjustedData = CreateAdjustedData(eventData);
			base.OnBeginDragImple(adjustedData);
		}
		protected override void OnDragImple(ICustomEventData eventData){
			ICustomEventData adjustedData = CreateAdjustedData(eventData);
			base.OnDragImple(adjustedData);
			thisInputManager.OnDrag(eventData);
		}
		/*  */
		public void SnapToCenter(){
			SnapTo(.5f, 0f, 0);
			SnapTo(.5f, 0f, 1);
		}
	}


	public interface ICoreGameplayInputScrollerConstArg: IGenericSingleElementScrollerConstArg{
		// IPlayerInputManager inputManager{get;}
	}
	public class CoreGameplayInputScrollerConstArg: GenericSingleElementScrollerConstArg, ICoreGameplayInputScrollerConstArg{
		public CoreGameplayInputScrollerConstArg(
			// IPlayerInputManager inputManager,

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
			// thisInputManager = inputManager;
		}
		// readonly IPlayerInputManager thisInputManager;
		// public IPlayerInputManager inputManager{get{return thisInputManager;}}
	}
}

