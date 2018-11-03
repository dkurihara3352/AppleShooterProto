using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DKUtility{
	public static class DebugHelper {
		public static void PrintInRed(string text){
			Debug.Log("<color=#ff0000ff>" + text + "</color>");
		}
		public static void PrintInPink(string text){
			Debug.Log("<color=#ff00ffff>" + text + "</color>");
		}
		public static void PrintInBlue(string text){
			Debug.Log("<color=#0000ffff>" + text + "</color>");
		}
		public static void PrintInGreen(string text){
			Debug.Log("<color=#00ff00ff>" + text + "</color>");
		}
		public static string RedString(string text){
			return "<color=#ff0000ff>" + text + "</color>";
		}
		public static string GreenString(string text){
			return "<color=#00ff00ff>" + text + "</color>";
		}
		public static string BlueString(string text){
			return "<color=#0000ffff>" + text + "</color>";
		}
	}
}
