using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IStaticTargetSpawnPointGroupAdaptor: IShootingTargetSpawnPointGroupAdaptor{
		IStaticTargetSpawnPointGroup GetStaticSpawnPointGroup();
	}
	public class StaticTargetSpawnPointGroupAdaptor: AbsShootingTargetSpawnPointGroupAdaptor, IStaticTargetSpawnPointGroupAdaptor{
		
		IStaticTargetSpawnPointGroup thisTypedGroup{
			get{
				return (IStaticTargetSpawnPointGroup)thisGroup;
			}
		}
		public IStaticTargetSpawnPointGroup GetStaticSpawnPointGroup(){
			return thisTypedGroup;
		}
		protected override IShootingTargetSpawnPointGroup CreateGroup(){
			StaticTargetSpawnPointGroup.IConstArg arg = new StaticTargetSpawnPointGroup.ConstArg(
				this
			);
			return new StaticTargetSpawnPointGroup(arg);
		}
		protected override IShootingTargetSpawnPointAdaptor[] CollectSpawnPointAdaptors(){
			return CollectTypedSpawnPointAdaptorFromChildren<IStaticTargetSpawnPointAdaptor>();
		}
	}
}

