using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IArrowTwangAdaptor: IInstatiableMonoBehaviourAdaptor{
	
		IArrowTwang GetArrowTwang();
		void Twang();
		void StopTwang();
		void StartTwangAnimation();
		void StopTwangAnimation();
		void  SetTwangMagnitude(float twangMagnitude);
		float GetTwangMagnitude(float normalizedTime);
	}
	public class ArrowTwangAdaptor: InstatiableMonoBehaviourAdaptor, IArrowTwangAdaptor{
		public override void SetUp(){
			ArrowTwang.IConstArg arg = new ArrowTwang.ConstArg(this);
			thisTwang = new ArrowTwang(arg);

			animator = CollectAnimator();
			twangHash = Animator.StringToHash("Twang");
			stopHash = Animator.StringToHash("Stop");
			magnitudeHash = Animator.StringToHash("Magnitude");
		}
		IArrowTwang thisTwang;
		public IArrowTwang GetArrowTwang(){
			return thisTwang;
		}
		public Animator animator;
		IArrowTwangProcess thisProcess;
		public float twangTime;
		public void Twang(){
			StopTwang();
			thisProcess =  processFactory.CreateArrowTwangProcess(
				this,
				twangTime
			);
			thisProcess.Run();
		}
		public void StopTwang(){
			if(thisProcess!= null && thisProcess.IsRunning())
				thisProcess.Stop();
			thisProcess = null;
		}
		int twangHash;
		int stopHash;
		int magnitudeHash;
		Animator CollectAnimator(){
			return (Animator)this.GetComponent(typeof(Animator));
		}
		public void StartTwangAnimation(){
			animator.SetTrigger(twangHash);
		}
		public void StopTwangAnimation(){
			animator.SetTrigger(stopHash);
		}
		public AnimationCurve twangMagnitudeCurve;
		public float GetTwangMagnitude(float normalizedTime){
			return twangMagnitudeCurve.Evaluate(normalizedTime);
		}
		public void SetTwangMagnitude(float twangMagnitude){
			animator.SetFloat(
				magnitudeHash,
				twangMagnitude
			);
		}
	}
}
