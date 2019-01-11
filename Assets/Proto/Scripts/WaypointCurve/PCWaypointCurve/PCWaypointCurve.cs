using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IPCWaypointCurve: IWaypointCurve{
		void SetLevelSectionShootingTargetSpawner(ILevelSectionShootingTargetSpawner spawner);
		void SetUpTargetSpawnEvents();
		void SetSubordinateCurves(IWaypointCurve[] curves);
		void SetLevelMechanisms(ILevelMechanism[] mechanisms);
		void DespawnTargets();
	}
	public class PCWaypointCurve: AbsWaypointCurve, IPCWaypointCurve{
		public PCWaypointCurve(IConstArg arg): base(arg){}
		public override void OnReserve(){
			DespawnTargets();
			// ToggleObstacleColliders(false);
			// ToggleRenderers(false);
			CallAllLevelMechanismsOnLevelDeactivate();
			DisableWaypointCurveRootGameObject();
		}
		IPCWaypointCurveAdaptor thisPCWaypointCurveAdaptor{
			get{
				return (IPCWaypointCurveAdaptor)thisAdaptor;
			}
		}
		ILevelSectionShootingTargetSpawner thisSpawner;
		public void SetLevelSectionShootingTargetSpawner(ILevelSectionShootingTargetSpawner spawner){
			thisSpawner = spawner;
		}
		/* target Spawn */
			
			public void DespawnTargets(){
				thisSpawner.Despawn();
			}
			IShootingTargetSpawnWaypointEvent[] thisSpawnEvents;
			public override IWaypointEvent[] GetWaypointEvents(){
				IWaypointEvent[] sceneWaypointEvents = thisWaypointEvents;
				List<IWaypointEvent> allWaypointEvents = new List<IWaypointEvent>();
				allWaypointEvents.AddRange(sceneWaypointEvents);
				allWaypointEvents.AddRange(thisSpawnEvents);
				WaypointEventComparer comparer = new WaypointEventComparer();
				
				allWaypointEvents.Sort(comparer);

				return allWaypointEvents.ToArray();
			}
			public void SetUpTargetSpawnEvents(){
				thisSpawner.SetUpSpawnWaypointEvents();
				IShootingTargetSpawnWaypointEvent[] spawnEvents = thisSpawner.GetSpawnWaypointEvents();
				thisSpawnEvents = spawnEvents;
			}
		/* CurveUpdate */
			public override void CalculateCurve(){
				base.CalculateCurve();
				CalculateAllSubordinateCurves();
			}
			IWaypointCurve[] thisSubordinateCurves;
			public void SetSubordinateCurves(IWaypointCurve[] curves){
				thisSubordinateCurves  = curves;
			}
			void CalculateAllSubordinateCurves(){
				if(thisSubordinateCurves != null){
					foreach(IWaypointCurve curve in thisSubordinateCurves){
						curve.CalculateCurve();
					}
				}
			}
		/*  */
			public override void OnUnreserve(){
				// ToggleObstacleColliders(true);
				// ToggleRenderers(true);
				EnableWaypointCurveRootGameObject();
				CallAllLevelMechanismOnLevelActivate();
			}
			void ToggleObstacleColliders(bool toggled){
				thisPCWaypointCurveAdaptor.ToggleObstacleColliders(toggled);
			}
			void ToggleRenderers(bool toggled){
				thisPCWaypointCurveAdaptor.ToggleRenderers(toggled);
			}
		/*  */
			void CallAllLevelMechanismsOnLevelDeactivate(){
				foreach(ILevelMechanism mech in thisLevelMechanisms)
					mech.OnLevelDeactivate();
			}
			void CallAllLevelMechanismOnLevelActivate(){
				foreach(ILevelMechanism mech in thisLevelMechanisms)
					mech.OnLevelActivate();
			}
			void EnableWaypointCurveRootGameObject(){
				thisPCWaypointCurveAdaptor.EnableGameObject();
			}
			void DisableWaypointCurveRootGameObject(){
				thisPCWaypointCurveAdaptor.DisableGameObject();
			}
			ILevelMechanism[] thisLevelMechanisms;
			public void SetLevelMechanisms(ILevelMechanism[] mechanisms){
				thisLevelMechanisms = mechanisms;
			}
	}
}

