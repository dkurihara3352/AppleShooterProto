using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IWaypointEventAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IWaypointEvent GetWaypointEvent();
	}
}
