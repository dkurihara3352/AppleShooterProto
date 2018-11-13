using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IHeatImageWaitForNextAdditionProcess: IProcess{}
	public class HeatImageWaitForNextAdditionProcess : AbsConstrainedProcess, IHeatImageWaitForNextAdditionProcess {
		
		public HeatImageWaitForNextAdditionProcess(
			IConstArg arg
		): base(arg){
			thisHeatImage = arg.heatImage;
		}
		IHeatImage thisHeatImage;
		protected override void ExpireImple(){
			thisHeatImage.StartSmoothFollowDeltaImageProcess();
		}

		public interface IConstArg: IConstrainedProcessConstArg{
			IHeatImage heatImage{get;}
		}
		public class ConstArg: ConstrainedProcessConstArg, IConstArg{
			public ConstArg(
				IProcessManager processManager,
				float comboTime,
				IHeatImage heatImage
			): base(
				processManager,
				ProcessConstraint.ExpireTime,
				comboTime
			){
				thisHeatImage = heatImage;
			}
			readonly IHeatImage thisHeatImage;
			public IHeatImage heatImage{get{return thisHeatImage;}}
		}
	}
}
