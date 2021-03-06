﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IGlidingTargetSpawnPoint: IShootingTargetSpawnPoint{
		IGlidingTargetWaypointCurve GetGlidingTargetWaypointCurve();
		void SetGlidingTargetWaypointCurve(IGlidingTargetWaypointCurve curve);
	}
	public class GlidingTargetSpawnPoint: AbsShootingTargetSpawnPoint, IGlidingTargetSpawnPoint{
		public GlidingTargetSpawnPoint(
			IConstArg arg
		): base(arg){

		}
		IGlidingTargetWaypointCurve thisCurve;
		public void SetGlidingTargetWaypointCurve(IGlidingTargetWaypointCurve curve){
			thisCurve = curve;
		}
		public IGlidingTargetWaypointCurve GetGlidingTargetWaypointCurve(){
			return thisCurve;
		}
		public override string GetName(){
			return "glider " + GetIndex().ToString();
		}
		/*  */
		public new interface IConstArg: AbsShootingTargetSpawnPoint.IConstArg{
			
		}
		public new class ConstArg: AbsShootingTargetSpawnPoint.ConstArg, IConstArg{
			public ConstArg(
				float eventPoint,
				IGlidingTargetSpawnPointAdaptor adaptor
			): base(
				eventPoint,
				adaptor
			){
			}
		}
	}
}

