using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ITrajectory{
		void DrawTrajectory(
			Vector3 aimDirection,
			float initialSpeed,
			float gravity,
			Vector3 launchPoint
		);
		void Clear();
	}
	public class Trajectory : ITrajectory {

		public Trajectory(
			ITrajectoryConstArg arg
		){
			thisMaxT = arg.maxT;
			thisResolution = arg.resolution;
			thisAdaptor = arg.adaptor;
		}
		readonly float thisMaxT;
		readonly int thisResolution;
		readonly ITrajectoryAdaptor thisAdaptor;
		public void DrawTrajectory(
			Vector3 aimDirection,
			float initialSpeed,
			float gravity,
			Vector3 launchPoint
		){
			Vector3 initialVelocity = aimDirection * initialSpeed;
			Vector3[] trajectoryPoints = new Vector3[thisResolution + 1];
			for(int i = 0; i < thisResolution + 1; i++){

				float t = (thisMaxT / thisResolution) * i;
				float xPosition = t * initialVelocity.x;
				float yPosition = t * initialVelocity.y - (gravity * t * t);
				float zPosition = t * initialVelocity.z;

				Vector3 trajectoryPoint = new Vector3(
					xPosition,
					yPosition,
					zPosition
				) + launchPoint;

				trajectoryPoints[i] = trajectoryPoint;
			}
			thisAdaptor.DrawTrajectory(trajectoryPoints);
		}
		public void Clear(){
			thisAdaptor.Clear();
		}
	}


	public interface ITrajectoryConstArg{
		float maxT{get;}
		int resolution{get;}
		ITrajectoryAdaptor adaptor{get;}
	}
	public struct TrajectoryConstArg: ITrajectoryConstArg{
		public TrajectoryConstArg(
			float maxT,
			int resolution,
			ITrajectoryAdaptor adaptor
		){
			thisMaxT = maxT;
			thisResolution = resolution;
			thisAdaptor = adaptor;
		}
		readonly float thisMaxT;
		public float maxT{get{return thisMaxT;}}
		readonly int thisResolution;
		public int resolution{get{return thisResolution;}}
		readonly ITrajectoryAdaptor thisAdaptor;
		public ITrajectoryAdaptor adaptor{get{return thisAdaptor;}}
	}
}
