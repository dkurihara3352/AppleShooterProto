using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IBowData{}
	public class BowData : IBowData {
		public int index;
		public bool unlocked = false;
		public int level = 0;
		int maxTier = 3;
		int maxLevel{
			get{
				return tierSteps * maxTier;
			}
		}

		public int tier{
			get{
				return CalculateTier();
			}
		}
		int tierSteps = 4;
		int CalculateTier(){
			return (level -1) % tierSteps;
		}
		IBowAttribute[] thisAttributes = new IBowAttribute[3];

		void OnEnable(){
			thisAttributes = CreateAttributes();
		}
		int attributesCount = 3;
		IBowAttribute[] CreateAttributes(){
			IBowAttribute[] result = new IBowAttribute[attributesCount];
			for(int i = 0; i < attributesCount; i ++){
				BowAttribute.IConstArg arg = new BowAttribute.ConstArg("bowAtt " + i.ToString());
				result[i] = new BowAttribute(arg);
			}
			Debug.Log(
				"BowData " + index.ToString() + " bowAtts: " + GetAttString(result)
			);
			return result;
		}
		string GetAttString(IBowAttribute[] atts){
			string result = "";
			foreach(IBowAttribute att in atts){
				result += att.GetName() + ", ";
			}
			return result;
		}
	}
}
