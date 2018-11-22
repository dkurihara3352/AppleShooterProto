using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityBase{
	public interface IGenericMonoBehaviourAdaptor: IMonoBehaviourAdaptor{

	}
	public class GenericMonoBehaviourAdaptor : MonoBehaviourAdaptor, IGenericMonoBehaviourAdaptor {
	}
}
