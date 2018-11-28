using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IGenericSingleElementScroller: IScroller{}
	public class GenericSingleElementScroller: AbsScroller, IGenericSingleElementScroller{
		public GenericSingleElementScroller(
			IConstArg arg
		): base(arg){
			thisRelativeCursorLength = MakeRelativeCursorLengthInRange(arg.relativeCursorLength);
		}
		Vector2 MakeRelativeCursorLengthInRange(Vector2 source){
			Vector2 newSizeV2 = new Vector2(source.x, source.y);
			for(int i = 0; i < 2; i ++){
				if(newSizeV2[i] <= 0f)
					throw new System.InvalidOperationException("relativeCursorLength must be greater than 0");
				else if(newSizeV2[i] > 1f)
					newSizeV2[i] = 1f;
			}
			return newSizeV2;
		}
		protected readonly Vector2 thisRelativeCursorLength;
		protected override bool[] thisShouldApplyRubberBand{
			get{return new bool[]{true, true};}
		}
		protected override Vector2 GetInitialNormalizedCursoredPosition(){
			return Vector2.zero;
		}
		protected override Vector2 CalcCursorLength(){
			Vector2 relativeCursorLength = thisRelativeCursorLength;
			float cursorWidth = thisRectLength[0] * relativeCursorLength.x;
			float cursorHeight = thisRectLength[1] * relativeCursorLength.y;
			return new Vector2(cursorWidth, cursorHeight);
		}

		public new interface IConstArg: AbsScroller.IConstArg{
			Vector2 relativeCursorLength{get;}
		}
		public new class ConstArg: AbsScroller.ConstArg, IConstArg{
			public ConstArg(
				Vector2 relativeCursorLength, 

				ScrollerAxis scrollerAxis, 
				Vector2 relativeCursorPosition, 
				Vector2 rubberBandLimitMultiplier, 
				bool isEnabledInertia, 
				float inertiaDecay,
				float newScrollSpeedThreshold,

				IGenericSingleElementScrollerAdaptor adaptor, 
				ActivationMode activationMode
			):base(
				scrollerAxis, 
				relativeCursorPosition, 
				rubberBandLimitMultiplier, 
				isEnabledInertia, 
				inertiaDecay,
				newScrollSpeedThreshold,

				adaptor,
				activationMode
			){
				thisRelativeCursorLength = relativeCursorLength;
			}
			protected Vector2 thisRelativeCursorLength;
			public Vector2 relativeCursorLength{
				get{return thisRelativeCursorLength;}
			}
		}
	}


	

}
