using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IPCWaypointCurveAdaptor: IWaypointCurveAdaptor{
		void ToggleObstacleColliders(bool toggled);
		void ToggleRenderers(bool toggled);
	}
	public class PCWaypointCurveAdaptor: AbsWaypointCurveAdaptor, IPCWaypointCurveAdaptor{
		public override void SetUp(){
			Calculate();
			AbsWaypointCurve.IConstArg arg = new AbsWaypointCurve.ConstArg(
				this,
				thisControlPoints,
				thisCurvePoints
			);
			thisWaypointCurve = new PCWaypointCurve(arg);
			thisLevelColliders = CollectLevelColliders();
			thisMeshRenderers = CollectMeshRenderers();
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
		public GlidingTargetWaypointCurveGroupAdaptor gliderWaypointCurveGroupAdaptor;
		IWaypointCurve[] CollectSubordinateCurves(){
			List<IWaypointCurve> resultList = new List<IWaypointCurve>();
			IGlidingTargetWaypointCurveAdaptor[] curveAdaptors = gliderWaypointCurveGroupAdaptor.GetCurveAdaptors();
			foreach(IGlidingTargetWaypointCurveAdaptor curveAdaptor in curveAdaptors){
				resultList.Add(curveAdaptor.GetWaypointCurve());
			}
			return resultList.ToArray();
		}
		public void ToggleObstacleColliders(bool toggled){
			foreach(Collider col in thisLevelColliders)
				col.enabled = toggled;
		}
		public Transform levelParent;
		Collider[] thisLevelColliders;
		Collider[] CollectLevelColliders(){
			return levelParent.GetComponentsInChildren<Collider>();
		}
		public void ToggleRenderers(bool toggled){
			foreach(MeshRenderer renderer in thisMeshRenderers)
				renderer.enabled = toggled;
		}
		MeshRenderer[] thisMeshRenderers;
		MeshRenderer[] CollectMeshRenderers(){
			return levelParent.GetComponentsInChildren<MeshRenderer>();
		}
	}
}
