using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ITargetSpawnManager{
		void Spawn();
		void Despawn();
		int[] GetSpawnPointIndices();
	}
}
