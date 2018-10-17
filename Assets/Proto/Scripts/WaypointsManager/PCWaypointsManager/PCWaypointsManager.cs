using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IPCWaypointsManager: IWaypointsManager{
		List<IPCWaypointCurve> GetAllPCWaypointCurves();
		IPCWaypointCurve GetCurrentPCCurve();
		IPCWaypointCurve GetNextPCCurve();
		List<IPCWaypointCurve> GetPCWaypointCurvesInSequence();
	}
	public class PCWaypointsManager: WaypointsManager, IPCWaypointsManager{
		public PCWaypointsManager(WaypointsManager.IConstArg arg): base(arg){}
		public List<IPCWaypointCurve> GetAllPCWaypointCurves(){
			return CreateTypedCurves(GetAllWaypointCurves());
		}
		public IPCWaypointCurve GetCurrentPCCurve(){
			return this.GetCurrentCurve() as IPCWaypointCurve;
		}
		public IPCWaypointCurve GetNextPCCurve(){
			return this.GetNextCurve() as IPCWaypointCurve;
		}
		public List<IPCWaypointCurve> GetPCWaypointCurvesInSequence(){
			return CreateTypedCurves(GetWaypointCurvesInSequence());
		}
		/*  */
		List<IWaypointCurve> CreateAbsCurves(List<IPCWaypointCurve> source){
			List<IWaypointCurve> result = new List<IWaypointCurve>();
			foreach(IPCWaypointCurve typedCurve in source)
				result.Add(typedCurve);
			return result;
		}
		List<IPCWaypointCurve> CreateTypedCurves(List<IWaypointCurve> source){
			List<IPCWaypointCurve> result = new List<IPCWaypointCurve>();
			foreach(IWaypointCurve untypedCurve in source)
				result.Add((IPCWaypointCurve)untypedCurve);
			return result;
		}
	}	
}
