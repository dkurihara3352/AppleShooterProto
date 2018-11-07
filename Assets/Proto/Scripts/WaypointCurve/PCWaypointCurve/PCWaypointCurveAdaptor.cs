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
		public ShootingTargetSpawnManagerAdaptor testTargetSpawnManagerAdaptor;
		IPCWaypointCurve thisTypedCurve{
			get{return thisWaypointCurve as IPCWaypointCurve;}
		}
		public LevelSectionShootingTargetSpawnerAdaptor spawnerAdaptor;
		public override void SetUpReference(){
			base.SetUpReference();
			ILevelSectionShootingTargetSpawner spawner = spawnerAdaptor.GetSpawner();
			thisTypedCurve.SetLevelSectionShootingTargetSpawner(spawner);
		}
		public override void FinalizeSetUp(){
			IWaypointCurve[] subordinateCurves = CollectSubordinateCurves();
			thisTypedCurve.SetSubordinateCurves(subordinateCurves);
			base.FinalizeSetUp();
		}
		public GlidingTargetWaypointCurveAdaptor[] glidingWaypointCurveAdaptors;
		IWaypointCurve[] CollectSubordinateCurves(){
			List<IWaypointCurve> resultList = new List<IWaypointCurve>();
			foreach(GlidingTargetWaypointCurveAdaptor adaptor in glidingWaypointCurveAdaptors)
				resultList.Add(adaptor.GetGlidingTargetWaypointCurve());
			return resultList.ToArray();
		}
	}
}
