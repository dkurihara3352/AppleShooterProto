using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IArrowTrailFadeProcess: IProcess{}
	public class ArrowTrailFadeProcess: AbsConstrainedProcess, IArrowTrailFadeProcess{
		public ArrowTrailFadeProcess(
			IConstArg arg
		): base(arg){
			thisTrail = arg.arrowTrail;
			thisInitialAlpha = arg.initialAlpha;
		}
		readonly float thisInitialAlpha;
		readonly IArrowTrail thisTrail;

		protected override void UpdateProcessImple(float deltaT){
			float newAlpha = Mathf.Lerp(
				thisInitialAlpha,
				0f,
				thisNormalizedTime
			);
			thisTrail.SetAlpha(newAlpha);
		}
		protected override void StopImple(){
			thisTrail.Deactivate();
		}
		public interface IConstArg: IConstrainedProcessConstArg{
			IArrowTrail arrowTrail{get;}
			float initialAlpha{get;}
		}
		public class ConstArg: ConstrainedProcessConstArg, IConstArg{
			public ConstArg(
				IProcessManager processManager,
				float fadeTime,
				IArrowTrail trail,
				float initialAlpha
			): base(
				processManager,
				ProcessConstraint.ExpireTime,
				fadeTime
			){
				thisTrail = trail;
				thisInitialAlpha = initialAlpha;
			}
			readonly IArrowTrail thisTrail;
			public IArrowTrail arrowTrail{get{return thisTrail;}}
			readonly float thisInitialAlpha;
			public float initialAlpha{get{return thisInitialAlpha;}}
		}
	}
}
