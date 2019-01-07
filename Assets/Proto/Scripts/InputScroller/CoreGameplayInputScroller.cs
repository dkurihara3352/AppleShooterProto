using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface ICoreGameplayInputScroller: IGenericSingleElementScroller{
		void SetPlayerInputManager(IPlayerInputManager inputManager);

		void SetScrollMultiplier(float multiplier);
		float GetScrollMultiplier();
		void SnapToCenter();
		void SetAxisInversion(int axis, bool toggled);
		bool GetAxisInversion(int axis);
	}
	public class CoreGameplayInputScroller : GenericSingleElementScroller, ICoreGameplayInputScroller {

		public CoreGameplayInputScroller(
			IConstArg arg
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
			if(thisInputManager != null)
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
				adjustedVelocity,
				thisUIManager
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
		public void SetAxisInversion(int axis, bool inverted){
			if(axis == 0)
				inverted = !inverted;
			thisCoreGameplayInputScrollerAdaptor.SetAxisInversion(axis, inverted);
		}
		public bool GetAxisInversion(int axis){
			bool result = thisCoreGameplayInputScrollerAdaptor.InvertsAxis(axis);
			if(axis == 0)
				return !result;
			else
				return result;
		}
		ICoreGameplayInputScrollerAdaptor thisCoreGameplayInputScrollerAdaptor{
			get{
				return (ICoreGameplayInputScrollerAdaptor)thisUIAdaptor;
			}
		}
		/*  */
		public new interface IConstArg: GenericSingleElementScroller.IConstArg{
		}
		public new class ConstArg: GenericSingleElementScroller.ConstArg, IConstArg{
			public ConstArg(
				Vector2 relativeCursorLength, 
				ScrollerAxis scrollerAxis, 
				Vector2 rubberBandLimitMultiplier, 
				Vector2 relativeCursorPosition, 
				bool isEnabledInertia, 
				float inertiaDecay,
				float newScrollSpeedThreshold,

				IGenericSingleElementScrollerAdaptor adaptor, 
				ActivationMode activationMode

			):base(
				relativeCursorLength,
				scrollerAxis, 
				relativeCursorPosition, 
				rubberBandLimitMultiplier, 
				isEnabledInertia, 
				inertiaDecay,
				newScrollSpeedThreshold,

				adaptor, 
				activationMode
			){
			}
		}
	}
}

