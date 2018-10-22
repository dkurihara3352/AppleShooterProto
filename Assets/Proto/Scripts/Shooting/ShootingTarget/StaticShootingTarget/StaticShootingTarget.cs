using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AppleShooterProto{
	public interface IStaticShootingTarget: ITestShootingTarget{}
	public class StaticShootingTarget: TestShootingTarget, IStaticShootingTarget{
		public StaticShootingTarget(
			IConstArg arg
		): base(arg){
			Debug.Log(
				"arg.reserve is null: " + 
				(arg.reserve == null).ToString()
			);
			thisReserve = arg.reserve;
		}
		readonly IStaticShootingTargetReserve thisReserve;
		protected override void ResetTransformAtReserve(){
			base.ResetTransformAtReserve();
			thisReserve.PutTargetInArray(this);
		}
		public override void SetIndex(int index){
			base.SetIndex(index);
			thisTypedAdaptor.SetIndexOnTextMesh(index);
		}
		IStaticShootingTargetAdaptor thisTypedAdaptor{
			get{
				return (IStaticShootingTargetAdaptor)thisAdaptor;
			}
		}
		/* const */
			public new interface IConstArg: TestShootingTarget.IConstArg{
				IStaticShootingTargetReserve reserve{get;}
			}
			public new struct ConstArg: IConstArg{
				public ConstArg(
					float health,
					ITestShootingTargetAdaptor adaptor,
					Color defaultColor,
					IAppleShooterProcessFactory processFactory,
					float fadeTime,

					IStaticShootingTargetReserve reserve
				){
					thisHealth = health;
					thisAdaptor = adaptor;
					thisDefaultColor = defaultColor;
					thisProcessFactory =  processFactory;
					thisFadeTime = fadeTime;

					thisReserve = reserve;
				}
					readonly float thisHealth;
					public float health{get{return thisHealth;}}
					readonly ITestShootingTargetAdaptor thisAdaptor;
					public IShootingTargetAdaptor adaptor{get{return thisAdaptor;}}
					readonly Color thisDefaultColor;
					public Color defaultColor{get{return thisDefaultColor;}}
					readonly IAppleShooterProcessFactory thisProcessFactory;
					public IAppleShooterProcessFactory processFactory{get{return thisProcessFactory;}}
					readonly float thisFadeTime;
					public float fadeTime{get{return thisFadeTime;}}

					readonly IStaticShootingTargetReserve thisReserve;
					public IStaticShootingTargetReserve reserve{get{return thisReserve;}}
			}
	}
}

public class StaticShootingTarget : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
