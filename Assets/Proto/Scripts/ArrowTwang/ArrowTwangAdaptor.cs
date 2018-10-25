using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IArrowTwangAdaptor: IMonoBehaviourAdaptor{
		void SetProcessManager(IProcessManager processManager);
		IArrowTwang GetArrowTwang();
		void Twang();
		void StopTwang();
		void StartTwangAnimation();
		void StopTwangAnimation();
		void  SetTwangMagnitude(float twangMagnitude);
		float GetTwangMagnitude(float normalizedTime);
	}
	public class ArrowTwangAdaptor: MonoBehaviourAdaptor, IArrowTwangAdaptor{
		protected override void Awake(){
			animator = CollectAnimator();
			twangHash = Animator.StringToHash("Twang");
			stopHash = Animator.StringToHash("Stop");
			magnitudeHash = Animator.StringToHash("Magnitude");
			return;
		}

		IProcessManager thisProcessManager;
		public void SetProcessManager(IProcessManager processManager){
			thisProcessManager = processManager;
		}
		IAppleShooterProcessFactory thisProcessFactory;

		public override void SetUp(){
			thisProcessFactory = new AppleShooterProcessFactory(thisProcessManager);
			ArrowTwang.IConstArg arg = new ArrowTwang.ConstArg(this);
			thisTwang = new ArrowTwang(arg);
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
			thisProcess =  thisProcessFactory.CreateArrowTwangProcess(
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
