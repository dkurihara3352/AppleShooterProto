using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IArrowTwang{
		void Twang();
		void StopTwang();
	}
	public class ArrowTwang: IArrowTwang{
		public ArrowTwang(
			IConstArg arg
		){
			thisAdaptor = arg.adaptor;
		}
		readonly IArrowTwangAdaptor thisAdaptor;
		public void Twang(){
			thisAdaptor.Twang();
		}
		public void StopTwang(){
			thisAdaptor.StopTwang();
		}
		/* Const */
		public interface IConstArg{
			IArrowTwangAdaptor adaptor{get;}
		}
		public struct ConstArg: IConstArg{
			public ConstArg(
				IArrowTwangAdaptor adaptor
			){
				thisAdaptor = adaptor;
			}
			readonly IArrowTwangAdaptor thisAdaptor;
			public IArrowTwangAdaptor adaptor{get{return thisAdaptor;}}
		}

	}

}

