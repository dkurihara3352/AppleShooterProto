using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AppleShooterProto{
	
	public interface ITargetData{
		TargetType targetType{get;}
		int tier{get;}
		float heatBonus{get;}
		int health{get;}
		int destructionScore{get;}
	}
	[CreateAssetMenu(menuName = "Custom/TargetData", fileName = "targetData")]
	public class TargetData : ScriptableObject, ITargetData {
		public TargetType _targetType;
		public TargetType targetType{get{return _targetType;}}
		public int _tier;
		public int tier{get{return _tier;}}
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
		public int _destructionScore;
		public int destructionScore{
			get{
				return _destructionScore;
			}
		}
	}
}
