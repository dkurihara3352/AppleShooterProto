using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IBowAttribute{
		string GetName();
	}
	public class BowAttribute : IBowAttribute {
		public BowAttribute(IConstArg arg){
			thisName = arg.name;
		}
		protected string thisName;
		public string GetName(){
			return thisName;
		}

		public interface IConstArg{
			string name{get;}
		}
		public class ConstArg: IConstArg{
			public ConstArg(
				string name
			){
				thisName = name;
			}
			readonly string thisName;
			public string name{get{return thisName;}}
		}
	}
}
