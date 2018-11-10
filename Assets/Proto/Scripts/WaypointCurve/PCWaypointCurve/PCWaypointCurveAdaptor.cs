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
		public GlidingTargetSpawnPointGroupAdaptor gliderSpawnPointGroupAdaptor;
		IWaypointCurve[] CollectSubordinateCurves(){
			List<IWaypointCurve> resultList = new List<IWaypointCurve>();
			IGlidingTargetSpawnPointGroup group = gliderSpawnPointGroupAdaptor.GetGlidingTargetSpawnPointGroup();
			IGlidingTargetSpawnPoint[] points = group.GetGlidingTargetSpawnPoints();
			foreach(IGlidingTargetSpawnPoint point in points){
				resultList.Add(point.GetGlidingTargetWaypointCurve());
			}
			return resultList.ToArray();
		}
	}
}
