using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IPCWaypointCurve: IWaypointCurve{
		void SetLevelSectionShootingTargetSpawner(ILevelSectionShootingTargetSpawner spawner);
		void SetUpTargetSpawnEvents();
		void DespawnTargets();
		void SetSubordinateCurves(IWaypointCurve[] curves);
	}
	public class PCWaypointCurve: AbsWaypointCurve, IPCWaypointCurve{
		public PCWaypointCurve(IConstArg arg): base(arg){}
		public override void OnReserve(){
			DespawnTargets();
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
	}
}

