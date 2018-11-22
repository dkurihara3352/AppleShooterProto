using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ITrajectory: IAppleShooterSceneObject{
		void DrawTrajectory(
			Vector3 aimDirection,
			float initialSpeed,
			float gravity,
			Vector3 launchPoint
		);
		void Clear();
	}
	public class Trajectory : AppleShooterSceneObject, ITrajectory {

		public Trajectory(
			IConstArg arg
		): base(arg){
			thisMaxT = arg.maxT;
			thisResolution = arg.resolution;
		}
		readonly float thisMaxT;
		readonly int thisResolution;
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
			thisTypedAdaptor.DrawTrajectory(trajectoryPoints);
		}
		ITrajectoryAdaptor thisTypedAdaptor{
			get{
				return (ITrajectoryAdaptor)thisAdaptor;
			}
		}
		public void Clear(){
			thisTypedAdaptor.Clear();
		}

		public new interface IConstArg: AppleShooterSceneObject.IConstArg{
			float maxT{get;}
			int resolution{get;}
		}
		public new class ConstArg: AppleShooterSceneObject.ConstArg, IConstArg{
			public ConstArg(
				ITrajectoryAdaptor adaptor,
				float maxT,
				int resolution
			): base(
				adaptor
			){
				thisMaxT = maxT;
				thisResolution = resolution;
			}
			readonly float thisMaxT;
			public float maxT{get{return thisMaxT;}}
			readonly int thisResolution;
			public int resolution{get{return thisResolution;}}
		}
	}
}
