using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AppleShooterProto{
	
	public interface ITargetData{
		float heatBonus{get;}
		int health{get;}
		int spawnValue{get;}
		int destructionScore{get;}
	}
	[CreateAssetMenu(menuName = "Custom/TargetData", fileName = "targetData")]
	public class TargetData : ScriptableObject, ITargetData {
		public float _heatBonus;
		public float heatBonus{
			get{
				return _heatBonus;
			}
		}
		public int _health;
		public int health{
			get{
				return _health;
			}
		}
		public int _spawnValue;
		public int spawnValue{
			get{
				return _spawnValue;
			}
		}
		public int _destructionScore;
		public int destructionScore{
			get{
				return _destructionScore;
			}
		}

	}
}
