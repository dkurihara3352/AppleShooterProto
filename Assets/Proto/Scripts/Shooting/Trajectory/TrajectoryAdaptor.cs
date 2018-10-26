using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ITrajectoryAdaptor: IMonoBehaviourAdaptor{
		void DrawTrajectory(Vector3[] trajectoryPoints);
		ITrajectory GetTrajectory();
		void Clear();
	}
	public class TrajectoryAdaptor : MonoBehaviourAdaptor, ITrajectoryAdaptor {
		public LineRenderer lineRenderer;
		public void DrawTrajectory(
			Vector3[] trajectoryPoints
		){
			thisTrajectoryPoints = trajectoryPoints;
			lineRenderer.numPositions = thisTrajectoryPoints.Length;
			lineRenderer.SetPositions(thisTrajectoryPoints);
		}
		Vector3[] thisTrajectoryPoints;
		
		public void Clear(){
			thisTrajectoryPoints = null;
			lineRenderer.numPositions = 0;
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
