using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IPCWaypointCurveAdaptor: IWaypointCurveAdaptor{}
	public class PCWaypointCurveAdaptor: AbsWaypointCurveAdaptor, IPCWaypointCurveAdaptor{
		public override void SetUp(){
			AbsWaypointCurve.IConstArg arg = new AbsWaypointCurve.ConstArg(
				this,
				thisControlPoints,
				thisCurvePoints
			);
			thisWaypointCurve = new PCWaypointCurve(arg);
		}
		IPCWaypointCurve thisTypedCurve{
			get{return thisWaypointCurve as IPCWaypointCurve;}
		}
		public LevelSectionShootingTargetSpawnerAdaptor spawnerAdaptor;
		public override void SetUpReference(){
			base.SetUpReference();
			ILevelSectionShootingTargetSpawner spawner = spawnerAdaptor.GetSpawner();
			thisTypedCurve.SetLevelSectionShootingTargetSpawner(spawner);

			IWaypointCurve[] subordinateCurves = CollectSubordinateCurves();
			thisTypedCurve.SetSubordinateCurves(subordinateCurves);
		}
		// public GlidingTargetSpawnPointGroupAdaptor gliderSpawnPointGroupAdaptor;
		public GlidingTargetWaypointCurveGroupAdaptor gliderWaypointCurveGroupAdaptor;
		IWaypointCurve[] CollectSubordinateCurves(){
			List<IWaypointCurve> resultList = new List<IWaypointCurve>();
			// IGlidingTargetSpawnPointGroup group = gliderSpawnPointGroupAdaptor.GetGlidingTargetSpawnPointGroup();
			IGlidingTargetWaypointCurveGroup group = gliderWaypointCurveGroupAdaptor.GetCurveGroup();
			IGlidingTargetWaypointCurve[] curves = group.GetCurves();
			// IGlidingTargetSpawnPoint[] points = group.GetGlidingTargetSpawnPoints();
			// Debug.Log(group.GetName() + ", points: " + points.Length.ToString());
			foreach(IGlidingTargetWaypointCurve curve in curves){
				resultList.Add(curve);
			}
			PrintList(resultList);
			return resultList.ToArray();
		}
		void PrintList(List<IWaypointCurve> list){
			string result = "";
			foreach(IWaypointCurve curve in list){
				if(curve == null)
					result += "null, ";
				else
					result += "nonnull, ";
 			}
			 Debug.Log(result);
		}
	}
}
