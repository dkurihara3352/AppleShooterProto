using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IWaypointEventAdaptor: ISlickBowShootingMonoBehaviourAdaptor{
		IWaypointEvent GetWaypointEvent();
	}
}
