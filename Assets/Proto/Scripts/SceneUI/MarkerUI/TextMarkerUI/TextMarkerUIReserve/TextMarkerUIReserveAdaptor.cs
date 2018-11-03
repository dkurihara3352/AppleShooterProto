using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ITextMarkerUIReserveAdaptor: IMarkerUIReserveAdaptor{
		ITextMarkerUIReserve GetTextMarkerUIReserve();
	}
	public class TextMarkerUIReserveAdaptor: MarkerUIReserveAdaptor, ITextMarkerUIReserveAdaptor{
		protected override IMarkerUIReserve CreateReserve(){
			TextMarkerUIReserve.IConstArg arg = new TextMarkerUIReserve.ConstArg(
				this
			);
			return new TextMarkerUIReserve(arg);
		}
		public ITextMarkerUIReserve GetTextMarkerUIReserve(){
			return (ITextMarkerUIReserve)thisReserve;
		}
	}
}

