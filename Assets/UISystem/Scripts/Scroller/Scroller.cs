using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility.CurveUtility;
using DKUtility;
namespace UISystem{
	public interface IScroller: IUIElement{
		void SetUpScrollerElementAndCursor(IUIElement element);
		void UpdateRect();


		void SwitchRunningElementMotorProcess(IScrollerElementMotorProcess process, int dimension);
		void SetScrollerElementLocalPosOnAxis(float localPosOnAxis, int dimension);
		float GetElementCursorOffsetInPixel(float scrollerElementLocalPosOnAxis, int dimension);
		float GetNormalizedCursoredPositionOnAxis(float scrollerElementLocalPosOnAxis, int dimension);

		bool IsMovingWithSpeedOverNewScrollThreshold();
		void UpdateVelocity(float velocityOnAxis, int dimension);

		IUIElement GetScrollerElement();
		void SetUpCursorTransform();
		bool ScrollerElementIsUndersizedTo(Vector2 referenceLength, int dimension);

		void ResetDrag();
		void ClearTouchPositionCache();
		Vector2 GetVelocity();
		void PauseRunningMotorProcessRecursivelyUp();
		void CheckAndPerformDynamicBoundarySnapOnAxis(float deltaPosOnAxis, float velocity, int dimension);
		void CheckAndPerformStaticBoundarySnap();
	}
	public enum ScrollerAxis{
		Horizontal, Vertical, Both
	}
	public abstract class AbsScroller : UIElement, IScroller{
		public AbsScroller(
			IConstArg arg
		): base(arg){
			/* good here */
			thisScrollerAxis = arg.scrollerAxis;
			thisRelativeCursorPosition = MakeSureRelativeCursorPosIsClampedZeroToOne(arg.relativeCursorPosition);
			thisRubberBandLimitMultiplier = MakeRubberBandLimitMultiplierInRange(arg.rubberBandLimitMultiplier);
			thisIsEnabledInertia = arg.isEnabledInertia;
			thisInertiaDecay = arg.inertiaDecay;
			thisNewScrollSpeedThreshold = arg.newScrollSpeedThreshold;

			/* non dependent */
			thisRunningScrollerMotorProcess = new IScrollerElementMotorProcess[2];
			thisElementIsScrolledToIncreaseCursorOffsetCalculator = new ElementIsScrolledToIncreaseCursorOffsetCalculator(this);
		}
		/* SetUp */
			Vector2 MakeSureRelativeCursorPosIsClampedZeroToOne(Vector2 source){
				Vector2 result = source;
				for(int i = 0; i < 2; i ++)
					if(result[i] < 0f)
						result[i] = 0f;
					else if(result[i] > 1f)
						result[i] = 1f;
				return result;
			}
			const float thisMinimumRubberBandMultiplier = .01f;
			Vector2 MakeRubberBandLimitMultiplierInRange(Vector2 source){
				Vector2 result = new Vector2(source.x, source.y);
				for(int i = 0; i < 2; i++)
					if(result[i] <= 0f)
						result[i] = thisMinimumRubberBandMultiplier;
					else if(result[i] > 1f)
						result[i] = 1f;
				return result;
			}
			protected readonly ScrollerAxis thisScrollerAxis;
		/*  */
			
		/* ScrollerRect */
			public  void UpdateRect(){
				// SetUpScrollerRect();
				SetUpRubberBandCalculators();
			}
		/* Rubber */
			readonly protected Vector2 thisRubberBandLimitMultiplier;
			Vector2 thisRubberLimit;

			void SetUpRubberBandCalculators(){
				for(int i = 0; i < 2; i ++)
					if(thisShouldApplyRubberBand[i])
						thisRubberBandCalculator = CreateRubberBandCalculator();
			}
			protected abstract bool[] thisShouldApplyRubberBand{get;}// simply return true if wanna apply
			RubberBandCalculator[] thisRubberBandCalculator;		
			RubberBandCalculator[] CreateRubberBandCalculator(){
				RubberBandCalculator[] result = new RubberBandCalculator[2];
				thisRubberLimit = new Vector2();
				for(int i = 0; i < 2; i++){
					float rubberLimit =  thisRubberBandLimitMultiplier[i] * GetRectSize()[i];
					thisRubberLimit[i] = rubberLimit;
					result[i] = new RubberBandCalculator(1f, rubberLimit);
				}
				return result;
			}

		/* ScrollerElement */
			public void SetUpScrollerElementAndCursor(IUIElement scrollerElement){
				//called in SetUpReference, in Finalize when this is UIElementGroupScroller

				thisScrollerElement = scrollerElement;
				// SetUpScrollerElementRect();
				SetUpCursorTransform();
				OnRectsSetUpComplete();
				PlaceScrollerElementAtInitialCursorValue();
			}
			protected virtual void OnRectsSetUpComplete(){
				thisElementCursorOffsetInPixelCalculator = new ElementCursorOffsetInPixelCalculator(
					this,
					thisCursorLength,
					thisCursorLocalPosition,
					thisScrollerElementSize
				);
			}
			protected IUIElement thisScrollerElement;
			public IUIElement GetScrollerElement(){
				return thisScrollerElement;
			}
			protected Vector2 thisScrollerElementSize{
				get{
					return thisScrollerElement.GetRectSize();
				}
			}
			// protected virtual void SetUpScrollerElementRect(){
			// 	IUIAdaptor scrollerElementAdaptor = thisScrollerElement.GetUIAdaptor();
			// 	thisScrollerElementLength = scrollerElementAdaptor.GetRectSize();
			// }
			/* Cursor Transform */
			public void SetUpCursorTransform(){
				thisCursorLength = CalcCursorLength();
				ClampCursorLengthToThisRect();
				thisCursorLocalPosition = CalcCursorLocalPos();

				Rect cursorRect = new Rect(thisCursorLocalPosition, thisCursorLength);
				((IScrollerAdaptor)thisUIAdaptor).SetCursorRect(cursorRect);
			}
			protected Vector2 thisCursorLength;
			readonly protected Vector2 thisRelativeCursorPosition;
			protected Vector2 thisCursorLocalPosition;
			protected abstract Vector2 CalcCursorLength();
			void ClampCursorLengthToThisRect(){
				for(int i = 0; i < 2; i ++){
					if(thisCursorLength[i] > GetRectSize()[i])
						thisCursorLength[i] = GetRectSize()[i];
				}
			}
			protected virtual Vector2 CalcCursorLocalPos(){
				Vector2 result = new Vector2();
				for(int i = 0; i < 2; i ++){
					float scrollerRectLength = GetRectSize()[i];
					float cursorLength = thisCursorLength[i];
					float diffL = scrollerRectLength - cursorLength;
					float localPos;
					if(thisRelativeCursorPosition[i] == 0f) 
						localPos = 0f;
					else
						localPos = thisRelativeCursorPosition[i] * diffL;
					result[i] = localPos;
				}
				return result;
			}
			/*  */
			protected Vector2 thisCursoredValue;
			protected void PlaceScrollerElementAtInitialCursorValue(){
				Vector2 initialCursorValue = GetInitialNormalizedCursoredPosition();
				PlaceScrollerElement(initialCursorValue);
			}
			protected abstract Vector2 GetInitialNormalizedCursoredPosition();
			public void SetScrollerElementLocalPosOnAxis(float localPosOnAxis, int dimension){
				Vector2 newScrollerElementLocalPos = thisScrollerElement.GetLocalPosition();
				newScrollerElementLocalPos[dimension] = localPosOnAxis;
				
				thisScrollerElement.SetLocalPosition(newScrollerElementLocalPos);
				float normalizedCursoredPositionOnAxis = GetNormalizedCursoredPositionOnAxis(localPosOnAxis, dimension);
				thisCursoredValue[dimension] = normalizedCursoredPositionOnAxis;
				OnScrollerElementDisplace(
					normalizedCursoredPositionOnAxis,
					dimension
				);
			}
			protected virtual void OnScrollerElementDisplace(
				float normalizedCursoredPositionOnAxis,
				int dimension
			){}
			protected void SetScrollerElementLocalPosition(Vector2 position){
				for(int i = 0; i < 2; i ++)
					SetScrollerElementLocalPosOnAxis(position[i], i);
			}
		/* Draggin in general */
			protected bool thisShouldProcessDrag;
		
			public void ResetDrag(){
				thisShouldProcessDrag = false;
				thisIsEvaluatedDrag = false;
				ClearTouchPositionCache();
			}
			// protected Vector2 thisTouchPosition;
			protected Vector2 thisElementLocalPositionAtTouch;
			Vector2 noTouchPosition = new Vector2(10000f, 10000f);
			public void ClearTouchPositionCache(){
				// thisTouchPosition = noTouchPosition;
				thisElementLocalPositionAtTouch = noTouchPosition;
				thisElementDisplacementSinceLastTouch = Vector2.zero;
			}
			void CacheTouchPosition(Vector2 touchPosition){
				// thisTouchPosition = touchPosition;
				thisElementLocalPositionAtTouch = thisScrollerElement.GetLocalPosition();

			}
			protected Vector2 thisElementDisplacementSinceLastTouch;
			/* OnBeginDrag */
				protected override void OnBeginDragImple(ICustomEventData eventData){
					if(thisTopmostScrollerInMotion != null){
						EvaluateDrag(eventData);
						thisUIManager.SetInputHandlingScroller(
							this, 
							UIManager.InputName.BeginDrag
						);
						if(thisIsTopmostScrollerInMotion){
							CacheTouchPosition(eventData.position);
						}else{
							thisTopmostScrollerInMotion.OnBeginDrag(eventData);
						}
					}else{
						EvaluateDrag(eventData);
						if(thisShouldProcessDrag){
							thisUIManager.SetInputHandlingScroller(this, UIManager.InputName.BeginDrag);
							CacheTouchPosition(eventData.position);
						}
						else
							base.OnBeginDragImple(eventData);
					}
				}
				bool thisIsEvaluatedDrag = false;
				void EvaluateDrag(ICustomEventData eventData){
					thisIsEvaluatedDrag = true;
					thisShouldProcessDrag = DetermineIfThisShouldProcessDrag(eventData.deltaPos);
				}
				bool DetermineIfThisShouldProcessDrag(Vector2 deltaPos){
					if(thisTopmostScrollerInMotion != null){
						return true;
					}else{
						if(thisScrollerAxis == ScrollerAxis.Both)
							return true;
						else{
							if(DeltaPosIsHorizontal(deltaPos))
								return thisScrollerAxis == ScrollerAxis.Horizontal;
							else
								return thisScrollerAxis == ScrollerAxis.Vertical;
						}
					}
				}
				bool DeltaPosIsHorizontal(Vector2 deltaPos){
					return Mathf.Abs(deltaPos.x) >= Mathf.Abs(deltaPos.y);
				}
				protected Vector2 CalcDragDeltaPos(Vector2 deltaP){
					if(thisScrollerAxis == ScrollerAxis.Both)
						return deltaP;
					else if(thisScrollerAxis == ScrollerAxis.Horizontal)
						return new Vector2(deltaP.x, 0f);
					else
						return new Vector2(0f, deltaP.y);
				}
			/* Drag */
				protected override void OnDragImple(ICustomEventData eventData){
					if(thisShouldProcessDrag){
						thisUIManager.SetInputHandlingScroller(this, UIManager.InputName.Drag);
						if(thisTopmostScrollerInMotion != null){
							if(thisIsTopmostScrollerInMotion){
								DisplaceScrollerElement(eventData.deltaPos);
							}
							else
								thisTopmostScrollerInMotion.OnDrag(eventData);
						}else{
							DisplaceScrollerElement(eventData.deltaPos);
						}
					}else{
						base.OnDragImple(eventData);
					}
				}
				Vector2 GetNonscaledDeltaPosition(Vector2 deltaPosition){
					Canvas canvas = thisUIManager.GetCanvas();
					Vector2 canvasLocalScale = canvas.transform.localScale;
					return new Vector2(
						deltaPosition.x / canvasLocalScale.x,
						deltaPosition.y / canvasLocalScale.y
					);
				}
				protected virtual void DisplaceScrollerElement(
					Vector2 deltaPosition
				){
					deltaPosition = GetNonscaledDeltaPosition(deltaPosition);
					thisElementDisplacementSinceLastTouch += deltaPosition;
					Vector2 displacement = CalcDragDeltaSinceTouch(
						thisElementDisplacementSinceLastTouch
					);
					Vector2 newElementLocalPosition =  GetScrollerElementRubberBandedLocalPosition(displacement);
					SetScrollerElementLocalPosition(newElementLocalPosition);
				}
				protected virtual Vector2 CalcDragDeltaSinceTouch(Vector2 displacementSinceTouch){
					Vector2 rawDisplacement = displacementSinceTouch;
					if(thisScrollerAxis == ScrollerAxis.Both)
						return rawDisplacement;
					else if(thisScrollerAxis == ScrollerAxis.Horizontal)
						return new Vector2(rawDisplacement.x, 0f);
					else
						return new Vector2(0f, rawDisplacement.y);
					
				}
				protected Vector2 GetScrollerElementRubberBandedLocalPosition(Vector2 displacement){
					Vector2 result = new Vector2();
					for(int i = 0; i < 2; i ++){
						float displacementAfterRubberBand;
						displacementAfterRubberBand = displacement[i];
						float prospectiveElementLocalPosOnAxis = thisElementLocalPositionAtTouch[i] + displacement[i];
						float cursorOffsetInPixel = GetElementCursorOffsetInPixel(prospectiveElementLocalPosOnAxis, i);
						if(cursorOffsetInPixel != 0f){
							float nonRubberedDisplacement = displacement[i];
							float rubberedDisplacement = 0f;
							if(cursorOffsetInPixel < 0f && displacement[i] > 0f){
								cursorOffsetInPixel *= -1f;
								nonRubberedDisplacement = displacement[i] - cursorOffsetInPixel;
								rubberedDisplacement = thisRubberBandCalculator[i].CalcRubberBandValue(cursorOffsetInPixel, invert: false);
							}else if(cursorOffsetInPixel > 0f && displacement[i] < 0f){
								cursorOffsetInPixel *= -1f;
								nonRubberedDisplacement = displacement[i] - cursorOffsetInPixel;
								rubberedDisplacement = thisRubberBandCalculator[i].CalcRubberBandValue(cursorOffsetInPixel, invert: true);
							}
							displacementAfterRubberBand = nonRubberedDisplacement + rubberedDisplacement;
						}		
						result[i] = thisElementLocalPositionAtTouch[i] + displacementAfterRubberBand;
					}
					return result;
				}
				IElementIsScrolledToIncreaseCursorOffsetCalculator thisElementIsScrolledToIncreaseCursorOffsetCalculator;
				protected bool ElementIsScrolledToIncreaseCursorOffset(float deltaPosOnAxis, float scrollerElementLocalPosOnAxis, int dimension){
					return thisElementIsScrolledToIncreaseCursorOffsetCalculator.Calculate(deltaPosOnAxis, scrollerElementLocalPosOnAxis, dimension);
				}
				protected bool thisRequiresToCheckForHorizontalAxis{
					get{return thisScrollerAxis == ScrollerAxis.Horizontal || thisScrollerAxis == ScrollerAxis.Both;}
				}
				protected bool thisRequiresToCheckForVerticalAxis{
					get{return thisScrollerAxis == ScrollerAxis.Vertical || thisScrollerAxis == ScrollerAxis.Both;}
				}
			/*  */
		/* Rect calculation */
			public bool ScrollerElementIsUndersizedTo(Vector2 referenceLength, int dimension){
				return thisScrollerElementSize[dimension] <= referenceLength[dimension];
			}
			protected float GetNormalizedPosition(float scrollerElementLocalPosOnAxis, Vector2 referenceLength, Vector2 referenceMin, int dimension){
				/*  (0f, 0f) if cursor rests on top left corner of the element
					(1f, 1f) if cursor rests on bottom right corner of the element
					value below 0f and over 1f indicates the element's displacement beyond cursor bounds
				*/
				if(ScrollerElementIsUndersizedTo(referenceLength, dimension)){
					return 0f;
				}else{
					float referenceLengthOnAxis = referenceLength[dimension];
					float referenceMinOnAxis = referenceMin[dimension];
					return (referenceMinOnAxis - scrollerElementLocalPosOnAxis)/ (thisScrollerElementSize[dimension] - referenceLengthOnAxis);

				}
			}
			public float GetNormalizedCursoredPositionOnAxis(float scrollerElementLocalPosOnAxis, int dimension){
				return GetNormalizedPosition(scrollerElementLocalPosOnAxis, thisCursorLength, thisCursorLocalPosition, dimension);
			}
			protected float GetNormalizedScrollerPosition(float scrollerElementLocalPosOnAxis, int dimension){
				return GetNormalizedPosition(scrollerElementLocalPosOnAxis, GetRectSize(), Vector2.zero, dimension);
			}

			/* ElementCursorOffsetInPixel calculation */
			IElementCursorOffsetInPixelCalculator thisElementCursorOffsetInPixelCalculator;
			public float GetElementCursorOffsetInPixel(float scrollerElementLocalPosOnAxis, int dimension){
				// /* used to calculate rubberbanding */
				return thisElementCursorOffsetInPixelCalculator.Calculate(scrollerElementLocalPosOnAxis, dimension);
			}
			protected float GetNormalizedCursoredPositionFromPosInElementSpace(float positionInElementSpaceOnAxis, int dimension){
				float prospectiveElementLocalPosOnAxis = thisCursorLocalPosition[dimension] - positionInElementSpaceOnAxis;
				return GetNormalizedCursoredPositionOnAxis(prospectiveElementLocalPosOnAxis, dimension);
			}
			protected void PlaceScrollerElement(Vector2 targetCursorValue){
				Vector2 newLocalPos = CalcLocalPositionFromNormalizedCursoredPosition(targetCursorValue);
				thisScrollerElement.SetLocalPosition(newLocalPos);
			}
			protected Vector2 CalcLocalPositionFromNormalizedCursoredPosition(Vector2 normalizedCursoredPosition){
				Vector2 result = new Vector2();
				for(int i = 0; i < 2; i ++){
					result[i] = CalcLocalPositionFromNormalizedCursoredPositionOnAxis(normalizedCursoredPosition[i], i);
				}
				return result;
			}
			protected float CalcLocalPositionFromNormalizedCursoredPositionOnAxis(float normalizedCursoredPositionOnAxis, int dimension){
				if(ScrollerElementIsUndersizedTo(thisCursorLength, dimension))
					return thisCursorLocalPosition[dimension];
				else{
					float scrollerElementLocalPosOnAxis = thisCursorLocalPosition[dimension] - (normalizedCursoredPositionOnAxis * (thisScrollerElementSize[dimension] - thisCursorLength[dimension]));
					return scrollerElementLocalPosOnAxis;
				}
			}

		/* Release */
			protected override void OnReleaseImple(){
				thisUIManager.SetInputHandlingScroller(this, UIManager.InputName.Release);
			}
		/* Tap */
			protected override void OnTapImple(int tapCount){
				thisUIManager.SetInputHandlingScroller(this, UIManager.InputName.Tap);
			}
		/* Swipe */
			protected override void OnSwipeImple(ICustomEventData eventData){
				if(!thisIsEvaluatedDrag)
					this.OnBeginDragImple(eventData);
				if(thisShouldProcessDrag){
					thisUIManager.SetInputHandlingScroller(this, UIManager.InputName.Swipe);
					if(thisTopmostScrollerInMotion != null){
						if(thisIsTopmostScrollerInMotion){
							ProcessSwipe(eventData);
						}else{
							thisTopmostScrollerInMotion.OnSwipe(eventData);
							CheckAndPerformStaticBoundarySnap();
							ResetDrag();
						}
					}else{
						ProcessSwipe(eventData);
					}
				}else{
					base.OnSwipeImple(eventData);
					CheckAndPerformStaticBoundarySnap();
					ResetDrag();
				}
			}
			protected virtual void ProcessSwipe(ICustomEventData eventData){
				if(thisIsEnabledInertia){
					Vector2 nonscaledVelocity = GetNonscaledDeltaPosition(eventData.velocity);

					ResetDrag();

					if(InitialVelocityIsOverThreshold(/* eventData.velocity */nonscaledVelocity))
						DisableScrollInputRecursively(this);

					for(int i = 0; i < 2; i ++){
						if(!this.IsOutOfBounds(i)){
							StartInertialScrollOnAxis(
								/* eventData.velocity */nonscaledVelocity,
								i
							);
						}else{
							CheckAndPerformStaticBoundarySnapOnAxis(i);
						}
					}
					CheckAndPerformStaticBoundarySnapFrom(thisProximateParentScroller);
				}else
					CheckAndPerformStaticBoundarySnapFrom(this);
			}
			protected bool InitialVelocityIsOverThreshold(Vector2 velocity){
				return velocity.sqrMagnitude >= thisNewScrollSpeedThreshold * thisNewScrollSpeedThreshold;
			}
			readonly protected bool thisIsEnabledInertia;
			readonly float thisInertiaDecay;
			protected void StartInertialScrollOnAxis(
				Vector2 velocity,
				int axis
			){
				float decelerationOnAxis = CalcDecelerationOnAxis(
					velocity,
					axis
				);
				IInertialScrollProcess process = thisUISystemProcessFactory.CreateInertialScrollProcess(
					velocity[axis],
					decelerationOnAxis,
					this,
					thisScrollerElement,
					axis,
					thisInertiaDecay
				);
				process.Run();
			}
			float CalcDecelerationOnAxis(
				Vector2 velocity,
				int axis
			){
				if(
					(thisScrollerAxis == ScrollerAxis.Horizontal && axis == 0) ||
					(thisScrollerAxis == ScrollerAxis.Vertical && axis == 1)
				){
					return 1f;
				}else{
					if(thisScrollerAxis == ScrollerAxis.Both){
						float sine;
						float cosine;
						DKUtility.Calculator.CalcSineAndCosine(
							velocity,
							out sine,
							out cosine
						);
						if(sine < 0f)
							sine *= -1f;
						if(cosine < 0f)
							cosine *= -1f;
						if(axis == 0)
							return cosine;
						else
							return sine;
					}
				}
				return 0f;
			}

			protected virtual void StartInertialScroll(Vector2 swipeVelocity){

				if(thisScrollerAxis == ScrollerAxis.Horizontal){
					IInertialScrollProcess process = thisUISystemProcessFactory.CreateInertialScrollProcess(
						swipeVelocity[0], 
						1f, 
						this, 
						thisScrollerElement, 
						0,
						thisInertiaDecay
					);
					process.Run();
				}else if(thisScrollerAxis == ScrollerAxis.Vertical){
					IInertialScrollProcess process = thisUISystemProcessFactory.CreateInertialScrollProcess(
						swipeVelocity[1], 
						1f, 
						this, 
						thisScrollerElement, 
						1,
						thisInertiaDecay
					);
					process.Run();
				}else{
					float sine;
					float cosine;
					DKUtility.Calculator.CalcSineAndCosine(swipeVelocity, out sine, out cosine);
					if(sine < 0f)
						sine *= -1f;
					if(cosine < 0f)
						cosine *= -1f;

					IInertialScrollProcess horizontalProcess = thisUISystemProcessFactory.CreateInertialScrollProcess(
						swipeVelocity[0], 
						cosine, 
						this, 
						thisScrollerElement, 
						0,
						thisInertiaDecay
					);
					horizontalProcess.Run();
					IInertialScrollProcess verticalProcess = thisUISystemProcessFactory.CreateInertialScrollProcess(
						swipeVelocity[1], 
						sine, 
						this, 
						thisScrollerElement, 
						1,
						thisInertiaDecay
					);
					verticalProcess.Run();
				}
			}
			public virtual void CheckAndPerformDynamicBoundarySnapOnAxis(float deltaPosOnAxis, float velocity, int dimension){
				float scrollerElementLocalPosOnAxis = thisScrollerElement.GetLocalPosition()[dimension];
				if(deltaPosOnAxis != 0f)
					if(ElementIsScrolledToIncreaseCursorOffset(deltaPosOnAxis, scrollerElementLocalPosOnAxis, dimension)){
						OnDynamicBoundaryCheckSuccess(deltaPosOnAxis, velocity, dimension);
					}
				OnDynamicBoundaryCheckFail(deltaPosOnAxis, velocity, dimension);
			}
			protected virtual void OnDynamicBoundaryCheckSuccess(float deltaPosOnAxis, float velocityOnAxis, int dimension){
				float snapTargetNormPos;
				if(deltaPosOnAxis > 0f)
					snapTargetNormPos = 0f;
				else	
					snapTargetNormPos = 1f;
				SnapTo(snapTargetNormPos, velocityOnAxis, dimension);
				return;
			}
			protected virtual void OnDynamicBoundaryCheckFail(float delatPosOnAxis, float velocityOnAxis, int dimension){
				return;
			}
			protected virtual void CheckAndPerformStaticBoundarySnapOnAxis(int dimension){
				float scrollerElementLocalPosOnAxis = thisScrollerElement.GetLocalPosition()[dimension];
				float cursorOffset = GetElementCursorOffsetInPixel(scrollerElementLocalPosOnAxis, dimension);

				if(cursorOffset != 0f){
					float snapTargetNormPos;
					if(cursorOffset < 0f)
						snapTargetNormPos = 0f;
					else
						snapTargetNormPos = 1f;


					SnapTo(snapTargetNormPos, 0f, dimension);
					return;
				}else{
					OnStaticBoundaryCheckFail(dimension);
				}
			}
			protected virtual void OnStaticBoundaryCheckFail(int dimension){
				for(int i = 0; i < 2; i ++)
					UpdateVelocity(0f, i);
			}
			public void CheckAndPerformStaticBoundarySnap(){
				for(int i = 0; i < 2; i ++)
					CheckAndPerformStaticBoundarySnapOnAxis(i);
			}
			protected void SnapTo(float targetNormalizedCursoredPosOnAxis, float initVelOnAxis, int dimension){

				UpdateVelocity(initVelOnAxis, dimension);
				Vector2 curVelocity = GetVelocity();
				if(InitialVelocityIsOverThreshold(curVelocity))
					DisableScrollInputRecursively(this);
					
				float targetElementLocalPosOnAxis = CalcLocalPositionFromNormalizedCursoredPositionOnAxis(targetNormalizedCursoredPosOnAxis, dimension);
				IScrollerElementSnapProcess newProcess = thisUISystemProcessFactory.CreateScrollerElementSnapProcess(
					this, 
					thisScrollerElement, 
					targetElementLocalPosOnAxis, 
					initVelOnAxis, 
					dimension
				);
				newProcess.Run();
			}
		/* Scroller Hieracrchy */
			public override void DisableScrollInputRecursively(IScroller disablingScroller){
				if(this == disablingScroller){// initiating
					if(thisUIManager.ShowsInputability())
						TurnTo(Color.blue);
				}
				thisTopmostScrollerInMotion = disablingScroller;
				thisScrollerElement.DisableScrollInputRecursively(disablingScroller);
			}
			public override void EnableScrollInputSelf(){
				if(thisIsTopmostScrollerInMotion){
					if(thisUIManager.ShowsInputability())
						TurnTo(GetUIImage().GetDefaultColor());
				}
				thisTopmostScrollerInMotion = null;
			}
			protected bool thisIsTopmostScrollerInMotion{
				get{
					if(thisTopmostScrollerInMotion != null)
						return this == thisTopmostScrollerInMotion;
					else
						return false;
				}
			}
		/* motor process & OnTouch */
			protected IScrollerElementMotorProcess[] thisRunningScrollerMotorProcess;
			public void SwitchRunningElementMotorProcess(IScrollerElementMotorProcess process, int dimension){
				PauseRunningElementMotorProcess(dimension);
				thisRunningScrollerMotorProcess[dimension] = process;
			}
			Vector2 thisVelocity;
			public Vector2 GetVelocity(){return thisVelocity;}
			public void UpdateVelocity(float velocityOnAxis, int dimension){
				thisVelocity[dimension] = velocityOnAxis;
				CheckAndTriggerScrollInputEnable();
			}
			bool IsOutOfBounds(int axis){
				float normalizedCursoredPosition;
				return IsOutOfBounds(axis, out normalizedCursoredPosition);
			}
			bool IsOutOfBounds(int dimension, out float normalizedCursoredPosition){
				float scrollerElementLocalPosOnAxis = thisScrollerElement.GetLocalPosition()[dimension];
				float thisNormalizedCursoredPosition = GetNormalizedCursoredPositionOnAxis(
					scrollerElementLocalPosOnAxis, 
					dimension
				);
				normalizedCursoredPosition = thisNormalizedCursoredPosition;
				return thisNormalizedCursoredPosition > 1f || thisNormalizedCursoredPosition < 0f;
			}


			/*  */
			void CheckAndTriggerScrollInputEnable(){
				if(thisTopmostScrollerInMotion != null){
					if(thisIsTopmostScrollerInMotion){
						CheckForScrollInputEnable();
					}else
						return;
				}else{//null
					if(this.IsMovingWithSpeedOverNewScrollThreshold())
						CheckForScrollInputEnable();
				}
			}
			public override void CheckForScrollInputEnable(){
				if(thisIsTopmostScrollerInMotion){
					if(!this.IsMovingWithSpeedOverNewScrollThreshold()){
						EnableScrollInputSelf();
						thisScrollerElement.CheckForScrollInputEnable();
					}
				}else{
					if(this.IsMovingWithSpeedOverNewScrollThreshold()){
						DisableScrollInputRecursively(this);
					}else{
						EnableScrollInputRecursively();
					}
				}
			}
			readonly float thisNewScrollSpeedThreshold;
			public bool IsMovingWithSpeedOverNewScrollThreshold(){
				return thisVelocity.sqrMagnitude >= thisNewScrollSpeedThreshold * thisNewScrollSpeedThreshold;
			}

			protected override void OnTouchImple(int touchCount){
				thisUIManager.SetInputHandlingScroller(this, UIManager.InputName.Touch);
			}
			public void PauseRunningMotorProcessRecursivelyUp(){
				PauseAllRunningElementMotorProcess();
				if(thisProximateParentScroller != null)
					thisProximateParentScroller.PauseRunningMotorProcessRecursivelyUp();
			}
			void PauseRunningElementMotorProcess(int dimension){
				if(thisRunningScrollerMotorProcess[dimension] != null){
					thisRunningScrollerMotorProcess[dimension].Stop();
				}
			}
			void PauseAllRunningElementMotorProcess(){
				for(int i = 0; i < 2; i ++)
					PauseRunningElementMotorProcess(i);
			}


		/* Const */
			public new interface IConstArg: UIElement.IConstArg{
				ScrollerAxis scrollerAxis{get;}
				Vector2 relativeCursorPosition{get;}
				Vector2 rubberBandLimitMultiplier{get;}
				bool isEnabledInertia{get;}
				float inertiaDecay{get;}
				float newScrollSpeedThreshold{get;}
			}
			public new class ConstArg: UIElement.ConstArg, IConstArg{
				public ConstArg(
					ScrollerAxis scrollerAxis, 
					Vector2 relativeCursorPosition, 
					Vector2 rubberBandLimitMultiplier, 
					bool isEnabledInertia, 
					float inertiaDecay,
					float newScrollSpeedThreshold,

					IScrollerAdaptor adaptor, 
					ActivationMode activationMode
				): base(
					adaptor,
					activationMode
				){
					thisScrollerAxis = scrollerAxis;
					thisRelativeCursorPos = relativeCursorPosition;
					thisRubberBandLimitMultiplier = rubberBandLimitMultiplier;
					thisIsEnabledInertia = isEnabledInertia;
					thisInertiaDecay = inertiaDecay;
					thisNewScrollSpeedThreshold = newScrollSpeedThreshold;
				}
				readonly ScrollerAxis thisScrollerAxis;
				public ScrollerAxis scrollerAxis{
					get{return thisScrollerAxis;}
				}
				readonly Vector2 thisRelativeCursorPos;
				public Vector2 relativeCursorPosition{get{return thisRelativeCursorPos;}}
				readonly Vector2 thisRubberBandLimitMultiplier;
				public Vector2 rubberBandLimitMultiplier{get{return thisRubberBandLimitMultiplier;}}
				readonly bool thisIsEnabledInertia;
				public bool isEnabledInertia{get{return thisIsEnabledInertia;}}
				readonly float thisInertiaDecay;
				public float inertiaDecay{get{return thisInertiaDecay;}}
				readonly float thisNewScrollSpeedThreshold;
				public float newScrollSpeedThreshold{get{return thisNewScrollSpeedThreshold;}}
			}
	}
}
