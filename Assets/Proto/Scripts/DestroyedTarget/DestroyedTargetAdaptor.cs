using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IDestroyedTargetAdaptor: IInstatiableMonoBehaviourAdaptor{
		IDestroyedTarget GetDestroyedTarget();
		void SetDestroyedTargetReserve(IDestroyedTargetReserve reserve);
		/* used by client */
			void StartDestruction();
			void StopDestruction();
			void PopText(string text);
		void SetPopUIReserve(IPopUIReserve reserve);
		/* used by process */
			void PlayParticleSystem();
			void StopParticleSystem();
		void ResetAtReserve();
		void SetIndex(int index);
		int GetIndex();
	}
	public class DestroyedTargetAdaptor: InstatiableMonoBehaviourAdaptor, IDestroyedTargetAdaptor{
		public override void SetUp(){
			base.SetUp();
			thisDestroyedTarget = CreateDestroyedTarget();

			thisParticleSystem = GetComponent<ParticleSystem>();
		}
		public override void FinalizeSetUp(){
			thisDestroyedTarget.Deactivate();
		}
		IDestroyedTarget thisDestroyedTarget;
		IDestroyedTarget CreateDestroyedTarget(){
			DestroyedTarget.IConstArg arg = new DestroyedTarget.ConstArg(
				this
			);
			return new DestroyedTarget(arg);
		}
		public IDestroyedTarget GetDestroyedTarget(){
			return thisDestroyedTarget;
		}
		public float particleSystemLifeTime;
		public void StartDestruction(){
			StartParticleProcess();
		}
		IDestroyedTargetParticleProcess thisProcess;
		void StartParticleProcess(){
			StopParticleProcess();
			thisProcess = processFactory.CreateDestroyedTargetParticleProcess(
				this,
				particleSystemLifeTime
			);
			thisProcess.Run();
		}
		void StopParticleProcess(){
			if(thisProcess != null && thisProcess.IsRunning())
				thisProcess.Stop();
			thisProcess = null;
		}
		public void StopDestruction(){
			StopParticleProcess();
		}
		public void PlayParticleSystem(){
			thisParticleSystem.Play();
		}
		public void StopParticleSystem(){
			thisParticleSystem.Stop();
		}

		ParticleSystem thisParticleSystem;
		IPopUIReserve thisPopUIReserve;
		public void SetPopUIReserve(IPopUIReserve reserve){
			thisPopUIReserve = reserve;
		}
		public void PopText(string text){
			thisPopUIReserve.PopText(
				this,
				text
			);
		}
		int thisIndex;
		public void SetIndex(int index){
			thisIndex = index;
		}
		public int GetIndex(){
			return thisIndex;
		}
		IDestroyedTargetReserve thisDestroyedTargetReserve;
		public void SetDestroyedTargetReserve(IDestroyedTargetReserve reserve){
			thisDestroyedTargetReserve = reserve;
		}
		public void ResetAtReserve(){
			thisDestroyedTargetReserve.Reserve(this);

		}
	}
}

