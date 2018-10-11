using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IPrintStringWaypointEvent: IWaypointEvent{
		void SetFollower(IWaypointsFollower follower);
	}
	public class PrintStringWaypointEvent: AbsWaypointEvent, IPrintStringWaypointEvent{
		public PrintStringWaypointEvent(
			IConstArg arg
		): base(arg){
			thisString = arg.stringToPrint;
		}
		readonly string thisString;
		IWaypointsFollower thisFollower;
		public void SetFollower(IWaypointsFollower follower){
			thisFollower = follower;
		}
		public override void Execute(){
			Debug.Log(
				"normPos: " + thisFollower.GetNormalizedPositionInCurve().ToString() + ", \n" +
				thisString
			);
		}
		public interface IConstArg: IAbsConstArg{
			string stringToPrint{get;}
		}
		public struct ConstArg: IConstArg{
			public ConstArg(
				string stringToPrint,
				float eventPoint
			){
				thisEventPoint = eventPoint;
				thisStringToPrint = stringToPrint;
			}
			readonly float thisEventPoint;
			public float eventPoint{get{return thisEventPoint;}}
			readonly string thisStringToPrint;
			public string stringToPrint{get{return thisStringToPrint;}}
		}
	}



}

