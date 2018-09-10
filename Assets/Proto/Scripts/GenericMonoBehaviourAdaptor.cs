using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IGenericMonoBehaviourAdaptor: IMonoBehaviourAdaptor{

	}
	public class GenericMonoBehaviourAdaptor : MonoBehaviourAdaptor, IGenericMonoBehaviourAdaptor {
	}
}
