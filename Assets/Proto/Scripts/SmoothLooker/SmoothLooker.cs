using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;
using UnityBase;

namespace AppleShooterProto{
	public interface ISmoothLooker: IAppleShooterSceneObject{
		void StartSmoothLook();
		void StopSmoothLook();
		void SetLookAtTarget(IMonoBehaviourAdaptor target);
		void RotateToward(Quaternion to, float step);
		void SetSmoothCoefficient(float k);
	}
	public class SmoothLooker : AppleShooterSceneObject, ISmoothLooker {
		public SmoothLooker(
			IConstArg arg
		): base(arg){
			thisSmoothCoefficient = arg.smoothCoefficient;
			thisProcessOrder = arg.processOrder;
		}
		IMonoBehaviourAdaptor thisLookAtTarget;
		public void SetLookAtTarget(IMonoBehaviourAdaptor target){
			thisLookAtTarget = target;
			if(thisProcess != null && thisProcess.IsRunning())
				thisProcess.UpdateLookAtTarget(thisLookAtTarget);
		}
		ISmoothLookProcess thisProcess;
		float thisSmoothCoefficient;
		public void SetSmoothCoefficient(float k){
			thisSmoothCoefficient = k;
			if(thisProcess != null && thisProcess.IsRunning())
				thisProcess.UpdateSmoothCoefficient(k);
		}
		readonly int thisProcessOrder;
		public void StartSmoothLook(){
			thisProcess = thisAppleShooterProcessFactory.CreateSmoothLookProcess(
				this,
				thisLookAtTarget,
				thisSmoothCoefficient,
				thisProcessOrder
			);
			thisProcess.Run();
		}
		public void StopSmoothLook(){
			if(thisProcess != null && thisProcess.IsRunning())
				thisProcess.Stop();
			thisProcess = null;
		}
		public void RotateToward(Quaternion to, float step){
			Quaternion newRotation = Quaternion.RotateTowards(GetRotation(), to, step);
			thisAdaptor.SetRotation(newRotation);
		}
		/*  */
		public new interface IConstArg: AppleShooterSceneObject.IConstArg{
			float smoothCoefficient{get;}
			int processOrder{get;}
		}
		public new class ConstArg: AppleShooterSceneObject.ConstArg, IConstArg{
			public ConstArg(
				ISmoothLookerAdaptor adaptor,
				float smoothCoefficient,
				int processOrder
			):base( adaptor){
				thisSmoothCoefficient = smoothCoefficient;
				thisProcessOrder = processOrder;
			}
			readonly float thisSmoothCoefficient;
			public float smoothCoefficient{get{return thisSmoothCoefficient;}}
			readonly int thisProcessOrder;
			public int processOrder{get{return thisProcessOrder;}}
		}
	}
}
