using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace UISystem{
	public interface IPopUpProcess: IProcess{
		bool IsHiding();
	}
	public interface IAlphaPopUpProcess: IPopUpProcess{}
	public class AlphaPopUpProcess : AbsConstrainedProcess, IAlphaPopUpProcess {
		public AlphaPopUpProcess(
			IConstArg arg
		): base(arg){
			thisEngine = arg.engine;
			thisHides = arg.hides;
			thisInitialAlpha = thisEngine.GetAphaOnImplementor();
		}
		readonly IPopUpStateEngine thisEngine;
		readonly bool thisHides;
		public bool IsHiding(){
			return thisHides;
		}
		float targetAlpha{
			get{
				return thisHides? 0f: 1f;
			}
		}
		readonly float thisInitialAlpha;
		
		protected override void UpdateProcessImple(float deltaT){
			float newAlpha = Mathf.Lerp(
				thisInitialAlpha,
				targetAlpha,
				thisNormalizedTime
			);
			thisEngine.SetAlphaOnImplementor(newAlpha);
		}


		protected override void ExpireImple(){
			base.ExpireImple();
			thisEngine.SetAlphaOnImplementor(targetAlpha);
			if(thisHides)
				thisEngine.SwitchToHiddenState();
			else
				thisEngine.SwitchToShownState();
		}


		public interface IConstArg: IConstrainedProcessConstArg{
			IPopUpStateEngine engine{get;}
			bool hides{get;}
		}
		public class ConstArg: ConstrainedProcessConstArg, IConstArg{
			public ConstArg(
				IProcessManager processManager,
				float expireTime,

				IPopUpStateEngine engine,
				bool hides
			): base(
				processManager,
				ProcessConstraint.ExpireTime,
				expireTime
			){

				thisHides = hides;
				thisEngine = engine;
			}
			readonly bool thisHides;
			public bool hides{get{return thisHides;}}
			readonly IPopUpStateEngine thisEngine;
			public IPopUpStateEngine engine{get{return thisEngine;}}
		}
	}





}
