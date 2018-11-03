using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IStaticShootingTargetAdaptor: IShootingTargetAdaptor{
		IStaticShootingTarget GetStaticShootingTarget();
		void SetStaticShootingTargetReserveAdaptor(IStaticShootingTargetReserveAdaptor reserveAdaptor);
		void SetIndexOnTextMesh(int index);
	}
	public class StaticShootingTargetAdaptor: AbsShootingTargetAdaptor, IStaticShootingTargetAdaptor{
		public override void SetUp(){
			base.SetUp();
			thisTextMesh = CollectTextMesh();
			SetIndexOnTextMesh(thisIndex);
		}
		protected override IShootingTarget CreateShootingTarget(){
			StaticShootingTarget.IConstArg arg = new StaticShootingTarget.ConstArg(
				thisIndex,
				health,
				thisDefaultColor,
				this
			);
			return new StaticShootingTarget(arg);

		}
		IStaticShootingTarget thisStaticShootingTarget{
			get{
				return(IStaticShootingTarget)thisShootingTarget;
			}
		}
		public IStaticShootingTarget GetStaticShootingTarget(){
			return thisStaticShootingTarget;
		}
		// public IStaticShootingTargetReserve reserve;
		// public void SetTargetReserve(IStaticShootingTargetReserve reserve){
		// 	this.reserve = reserve;
		// }
		IStaticShootingTargetReserveAdaptor thisReserveAdaptor;
		public void SetStaticShootingTargetReserveAdaptor(IStaticShootingTargetReserveAdaptor adaptor){
			thisReserveAdaptor = adaptor;
		}
		public override void SetUpReference(){
			base.SetUpReference();
			IStaticShootingTargetReserve reserve = thisReserveAdaptor.GetStaticShootingTargetReserve();
			thisStaticShootingTarget.SetStaticShootingTargetReserve(reserve);
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

