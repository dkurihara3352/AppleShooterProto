using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IStaticShootingTargetAdaptor: ITestShootingTargetAdaptor{
		void SetTargetReserve(IStaticShootingTargetReserve reserve);
		void SetIndexOnTextMesh(int index);
	}
	public class StaticShootingTargetAdaptor: TestShootingTargetAdaptor, IStaticShootingTargetAdaptor{
		protected override void Awake(){
			base.Awake();
			thisTextMesh = CollectTextMesh();
		}
		protected override void SetUpTarget(){
			IAppleShooterProcessFactory processFactory = new AppleShooterProcessFactory(
				thisProcessManager
			);
			StaticShootingTarget.IConstArg arg = new StaticShootingTarget.ConstArg(
				health,
				this,
				defaultColor,
				processFactory,
				fadeTime,

				reserve
			);
			thisShootingTarget = new StaticShootingTarget(arg);
		}
		public IStaticShootingTargetReserve reserve;
		public void SetTargetReserve(IStaticShootingTargetReserve reserve){
			this.reserve = reserve;
		}
		TextMesh thisTextMesh;
		public void SetIndexOnTextMesh(int index){
			thisTextMesh.text  =index.ToString();
		}
		public TextMesh CollectTextMesh(){
			Component[] childComponents = this.transform.GetComponentsInChildren(typeof(Component));
			foreach(Component comp in childComponents){
				if(comp is TextMesh)
					return (TextMesh)comp;
			}
			return null;
		}
	}	
}

