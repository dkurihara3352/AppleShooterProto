using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IStaticTargetSpawnPointGroup: IShootingTargetSpawnPointGroup{
	}
	public class StaticTargetSpawnPointGroup : AbsShootingTargetSpawnPointGroup, IStaticTargetSpawnPointGroup {
		
		public StaticTargetSpawnPointGroup(
			IConstArg arg
		): base(
			arg
		){}
		/*  */
		public new interface IConstArg: AbsShootingTargetSpawnPointGroup.IConstArg{

		}
		public new class ConstArg: AbsShootingTargetSpawnPointGroup.ConstArg, IConstArg{
			public ConstArg(
				IStaticTargetSpawnPointGroupAdaptor adaptor
			): base(
				adaptor
			){}
		}
	}

}
