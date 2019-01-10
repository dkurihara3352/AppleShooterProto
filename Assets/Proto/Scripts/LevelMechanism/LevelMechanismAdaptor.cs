using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface ILevelMechanismAdaptor: ISlickBowShootingMonoBehaviourAdaptor{
		ILevelMechanism GetLevelMechanism();
	}
}

