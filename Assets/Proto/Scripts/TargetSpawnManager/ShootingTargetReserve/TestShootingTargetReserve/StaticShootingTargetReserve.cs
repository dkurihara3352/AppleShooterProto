using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IStaticShootingTargetReserve: IShootingTargetReserve<IStaticShootingTarget>{
		IStaticShootingTarget[] GetTestTargetsInReserve();
		void PutTargetInArray(IStaticShootingTarget target);
	}
	public class StaticShootingTargetReserve : IStaticShootingTargetReserve {
		public StaticShootingTargetReserve(IConstArg arg){
			thisAdaptor = arg.adaptor;
			thisTotalTargesCount = arg.totalTargetsCount;
		}
		readonly int thisTotalTargesCount;
		IStaticShootingTargetReserveAdaptor thisAdaptor;
		Queue<IStaticShootingTarget> thisTargets;
		public IStaticShootingTarget[] GetTestTargetsInReserve(){
			return thisTargets.ToArray();
		}
		public void Reserve(IStaticShootingTarget target){
			target.Deactivate();
			PutTargetInArray(target);
			thisTargets.Enqueue(target);
		}
		public void PutTargetInArray(IStaticShootingTarget target){
			int index = target.GetIndex();
			float posX = index * 4f;
		 	Vector3 offset = new Vector3(posX, 0f, 0f);
			Vector3 newPos = target.GetPosition() + offset;
			target.SetPosition(newPos);
		}
		public IStaticShootingTarget Unreserve(){
			IStaticShootingTarget result = thisTargets.Dequeue();
			result.Activate();
			return result;
		}
		public void GetTargetsReadyInReserve(){
			thisTargets = new Queue<IStaticShootingTarget>();
			for(int i = 0; i < thisTotalTargesCount; i ++){
				IStaticShootingTarget target = thisAdaptor.CreateStaticShootingTarget();
				target.SetIndex(i);
				this.Reserve(target);
			}
		}
		/* Const */
		public interface IConstArg{
			IStaticShootingTargetReserveAdaptor adaptor{get;}
			int totalTargetsCount{get;}
		}
		public struct ConstArg: IConstArg{
			public ConstArg(
				IStaticShootingTargetReserveAdaptor adaptor,
				int totalTargetsCount
			){
				thisAdaptor = adaptor;
				thisTotalTargetsCount = totalTargetsCount;
			}
			readonly IStaticShootingTargetReserveAdaptor thisAdaptor;
			public IStaticShootingTargetReserveAdaptor adaptor{
				get{return thisAdaptor;}
			}
			readonly int thisTotalTargetsCount;
			public int totalTargetsCount{get{return thisTotalTargetsCount;}}
		}
	}
}
