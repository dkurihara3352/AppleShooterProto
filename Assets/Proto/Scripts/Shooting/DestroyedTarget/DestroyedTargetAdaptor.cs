using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IDestroyedTargetAdaptor: IMonoBehaviourAdaptor{
		IDestroyedTarget GetDestroyedTarget();
		void SetDestroyedTargetReserve(IDestroyedTargetReserve reserve);
		/* used by client */
			void StartDestruction();
			void StopDestruction();
		void SetPopUIReserve(IPopUIReserve reserve);
		/* used by process */
			void PlayParticleSystem();
			void StopParticleSystem();
		void SetIndex(int index);
		int GetIndex();
	}
	public class DestroyedTargetAdaptor: MonoBehaviourAdaptor, IDestroyedTargetAdaptor{
		public override void SetUp(){
			base.SetUp();
			thisDestroyedTarget = CreateDestroyedTarget();

			thisParticleSystem = GetComponent<ParticleSystem>();
		}
		public override void SetUpReference(){
			thisDestroyedTarget.SetDestroyedTargetReserve(thisDestroyedTargetReserve);
			thisDestroyedTarget.SetPopUIReserve(thisPopUIReserve);
		}
		public override void FinalizeSetUp(){
			thisDestroyedTarget.Deactivate();
		}
		IDestroyedTarget thisDestroyedTarget;
		IDestroyedTarget CreateDestroyedTarget(){
			DestroyedTarget.IConstArg arg = new DestroyedTarget.ConstArg(
				thisIndex,
				this
			);
			return new DestroyedTarget(arg);
		}
		public IDestroyedTarget GetDestroyedTarget(){
			return thisDestroyedTarget;
		}
		IDestroyedTargetReserve thisDestroyedTargetReserve;
		public void SetDestroyedTargetReserve(IDestroyedTargetReserve reserve){
			thisDestroyedTargetReserve = reserve;
		}
		IPopUIReserve thisPopUIReserve;
		public void SetPopUIReserve(IPopUIReserve reserve){
			thisPopUIReserve = reserve;
		}

		/* process */
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
		/* particle */
			ParticleSystem thisParticleSystem;
			public void PlayParticleSystem(){
				thisParticleSystem.Play();
			}
			public void StopParticleSystem(){
				thisParticleSystem.Stop();
			}
		/* misc */

		int thisIndex;
		public void SetIndex(int index){
			thisIndex = index;
		}
		public int GetIndex(){
			return thisIndex;
		}
	}
}

