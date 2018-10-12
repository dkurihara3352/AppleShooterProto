using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ITestShootingTargetReserve: IShootingTargetReserve<ITestShootingTarget>{
		ITestShootingTarget[] GetTestTargetsInReserve();
	}
	public class TestShootingTargetReserve : ITestShootingTargetReserve {
		public TestShootingTargetReserve(IConstArg arg){
			thisAdaptor = arg.adaptor;
			thisTotalTargesCount = arg.totalTargetsCount;
		}
		readonly int thisTotalTargesCount;
		ITestShootingTargetReserveAdaptor thisAdaptor;
		Queue<ITestShootingTarget> thisTargets;
		public ITestShootingTarget[] GetTestTargetsInReserve(){
			return thisTargets.ToArray();
		}
		public void Reserve(ITestShootingTarget target){
			target.ResetTarget();
			target.SetParent(thisAdaptor.GetTransform());
			target.ResetTransform();
			thisTargets.Enqueue(target);
		}
		public ITestShootingTarget Unreserve(){
			ITestShootingTarget result = thisTargets.Dequeue();
			return result;
		}
		public void GetTargetsReadyInReserve(){
			thisTargets = new Queue<ITestShootingTarget>();
			for(int i = 0; i < thisTotalTargesCount; i ++){
				ITestShootingTarget target = thisAdaptor.CreateTestShootingTarget();
				target.SetIndex(i);
				this.Reserve(target);
			}
		}
		/* Const */
		public interface IConstArg{
			ITestShootingTargetReserveAdaptor adaptor{get;}
			int totalTargetsCount{get;}
		}
		public struct ConstArg: IConstArg{
			public ConstArg(
				ITestShootingTargetReserveAdaptor adaptor,
				int totalTargetsCount
			){
				thisAdaptor = adaptor;
				thisTotalTargetsCount = totalTargetsCount;
			}
			readonly ITestShootingTargetReserveAdaptor thisAdaptor;
			public ITestShootingTargetReserveAdaptor adaptor{
				get{return thisAdaptor;}
			}
			readonly int thisTotalTargetsCount;
			public int totalTargetsCount{get{return thisTotalTargetsCount;}}
		}
	}
}
