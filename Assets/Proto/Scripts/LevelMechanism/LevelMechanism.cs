using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface ILevelMechanism: ISlickBowShootingSceneObject{
		void OnLevelActivate();
		void OnLevelDeactivate();
	}
}

