using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace SlickBowShooting{
	public interface IHeatCountDownProcess: IProcess{}
	public class HeatCountDownProcess : AbsProcess, IHeatCountDownProcess {
		public HeatCountDownProcess(IConstArg arg): base(arg){
			thisStateEngine = arg.heatManagerStateEngine;
			thisHeatDecayRate = arg.heatDecayRate;
		}
		readonly IHeatManagerStateEngine thisStateEngine;
		readonly float thisHeatDecayRate;
		float elapsedTimeSinceLastTick = 0f;
		protected override void UpdateProcessImple(float deltaT){
			DecreaseHeatEverySecond(deltaT);
		}
		void DecreaseHeatEverySecond(float deltaTime){
			elapsedTimeSinceLastTick += deltaTime;
			if(elapsedTimeSinceLastTick >= 1f){
				int quotient = Mathf.FloorToInt(elapsedTimeSinceLastTick);
				for(int i = 0; i < quotient; i ++){
					float maxHeat = thisStateEngine.GetMaxHeat();
					float decayAmount = maxHeat * thisHeatDecayRate;
					thisStateEngine.TickAwayHeat(decayAmount);
				}
				elapsedTimeSinceLastTick = elapsedTimeSinceLastTick - quotient;
			}
		}

		public new interface IConstArg: AbsProcess.IConstArg{
			IHeatManagerStateEngine heatManagerStateEngine{get;}
			float heatDecayRate{get;}
		}
		public new class ConstArg: AbsProcess.ConstArg, IConstArg{
			public ConstArg(
				IProcessManager processManager,
				IHeatManagerStateEngine heatManagerStateEngine,
				float heatDecayRate
			): base(
				processManager
			){
				thisHeatManagerStateEngine = heatManagerStateEngine;
				thisHeatDecayRate = heatDecayRate;
			}
			readonly IHeatManagerStateEngine thisHeatManagerStateEngine;
			public IHeatManagerStateEngine heatManagerStateEngine{
				get{return thisHeatManagerStateEngine;}
			}
			readonly float thisHeatDecayRate;
			public float heatDecayRate{
				get{return thisHeatDecayRate;}
			}
		}
	}
}
