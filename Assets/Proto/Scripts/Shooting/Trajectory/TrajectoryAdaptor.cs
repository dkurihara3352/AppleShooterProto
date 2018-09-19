using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ITrajectoryAdaptor: IMonoBehaviourAdaptor{
		void DrawTrajectory(Vector3[] trajectoryPoints);
		ITrajectory GetTrajectory();
	}
	public class TrajectoryAdaptor : MonoBehaviourAdaptor, ITrajectoryAdaptor {

		public void DrawTrajectory(
			Vector3[] trajectoryPoints
		){
			if(!draw)
				draw = true;
			thisTrajectoryPoints = trajectoryPoints;
		}
		bool draw = false;
		Vector3[] thisTrajectoryPoints;
		public void OnDrawGizmos(){
			Gizmos.color = Color.green;
			if(draw){
				foreach(Vector3 position in thisTrajectoryPoints)
					Gizmos.DrawWireSphere(position, .1f);
			}
		}
		ITrajectory thisTrajectory;
		public float maxT = 3f;
		public int resolution = 10;
		public override void SetUp(){
			ITrajectoryConstArg arg = new TrajectoryConstArg(
				maxT,
				resolution,
				this
			);
			thisTrajectory = new Trajectory(
				arg
			);
		}
		public ITrajectory GetTrajectory(){
			return thisTrajectory;
		}
	}
}
