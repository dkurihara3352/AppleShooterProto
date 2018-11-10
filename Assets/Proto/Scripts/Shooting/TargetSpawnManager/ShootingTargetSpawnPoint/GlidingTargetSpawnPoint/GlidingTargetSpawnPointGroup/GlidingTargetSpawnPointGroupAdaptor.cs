using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IGlidingTargetSpawnPointGroupAdaptor: IShootingTargetSpawnPointGroupAdaptor{
		IGlidingTargetSpawnPointGroup GetGlidingTargetSpawnPointGroup();
	}
	public class GlidingTargetSpawnPointGroupAdaptor : AbsShootingTargetSpawnPointGroupAdaptor, IGlidingTargetSpawnPointGroupAdaptor {

		IGlidingTargetSpawnPointGroup thisTypedGroup{
			get{
				return (IGlidingTargetSpawnPointGroup)thisGroup;
			}
		}
		public IGlidingTargetSpawnPointGroup GetGlidingTargetSpawnPointGroup(){
			return thisTypedGroup;
		}
		protected override IShootingTargetSpawnPointGroup CreateGroup(){
			GlidingTargetSpawnPointGroup.IConstArg arg = new GlidingTargetSpawnPointGroup.ConstArg(
				this
			);
			return new GlidingTargetSpawnPointGroup(arg);
		}
		protected override IShootingTargetSpawnPointAdaptor[] CollectSpawnPointAdaptors(){
			return CollectTypedSpawnPointAdaptorFromChildren<IShootingTargetSpawnPointAdaptor>();
		}
	}
}
