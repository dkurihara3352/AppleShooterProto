using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BowDataGUIManager : MonoBehaviour, AppleShooterProto.BowDataCalculator.IAdaptor{

	[Range(0f, 1f)]
	public float topLeftWidthRatio = .5f;
	[Range(0f, 1f)]
	public float topLeftHeightRatio = .5f;
	Rect topLeft;
	Rect bottomLeft;
	Rect topRight;
	Rect bottomRight;
	void CalcRect(){
		topLeft = CalcGUIRect(
			new Vector2(
				topLeftWidthRatio,
				topLeftHeightRatio
			),
			new Vector2(
				0f,
				0f
			)
		);
		topRight = CalcGUIRect(
			new Vector2(
				1f - topLeftWidthRatio,
				topLeftHeightRatio
			),
			new Vector2(
				1f,
				0f
			)
		);
		bottomLeft = CalcGUIRect(
			new Vector2(
				topLeftWidthRatio,
				1f - topLeftHeightRatio
			),
			new Vector2(
				0f,
				1f
			)
		);
		bottomRight = CalcGUIRect(
			new Vector2(
				1f - topLeftWidthRatio,
				1f - topLeftHeightRatio
			),
			new Vector2(
				1f,
				1f
			)
		);
	}
	void OnEnable(){
		if(thisCalculator == null)
			thisCalculator = new AppleShooterProto.BowDataCalculator(this);

	}
	void Awake(){
		if(thisCalculator == null)
			thisCalculator = new AppleShooterProto.BowDataCalculator(this);
	}
	AppleShooterProto.IBowDataCalculator thisCalculator;
	void OnGUI(){
		CalcRect();
		DrawTopLeft();
		DrawTopRight();
		DrawBottomLeft();
		DrawBottomRight();
	}
	void DrawTopLeft(){
		GUI.Box(topLeft, "");
		DrawAttributeControl(topLeft);
	}
	void DrawTopRight(){
		GUI.Box(topRight, "");
		DrawAttributeValue(topRight);
	}
	void DrawBottomLeft(){
		GUI.Box(bottomLeft, "bottomLeft");
	}
	void DrawBottomRight(){
		GUI.Box(bottomRight, "bottomRight");
	}
	Rect CalcGUIRect(
		Vector2 normalizedSize,
		Vector2 normalizedPosition
	){
		Rect screenRect = new Rect(
			0f, 0f, Screen.width, Screen.height
		);
		return CalcSubRect(
			screenRect,
			normalizedSize,
			normalizedPosition
		);
	}
	Rect CalcSubRect(
		Rect sourceRect,
		Vector2 normalizedSize,
		Vector2 normalizedPosition
	){
		MakeSureValuesAreValid(normalizedSize, normalizedPosition);
		float width = sourceRect.width * normalizedSize.x;
		float height = sourceRect.height * normalizedSize.y;

		float posX = (sourceRect.width - width) * normalizedPosition.x + sourceRect.x;
		float posY = (sourceRect.height - height) * normalizedPosition.y + sourceRect.y;
		return new Rect(posX, posY, width, height);
	}
	void MakeSureValuesAreValid(Vector2 normalizedSize, Vector2 normalizedPosition){
		if(
			normalizedSize.x < 0f ||
			normalizedSize.x > 1f ||
			normalizedSize.y < 0f ||
			normalizedSize.y > 1f ||
			
			normalizedPosition.x < 0f ||
			normalizedPosition.x > 1f ||
			normalizedPosition.y < 0f ||
			normalizedPosition.y > 1f
		)
			throw new System.InvalidOperationException(
				"you'd be better throw yourself off a building now that you're that dumb enough to make such a introductory mistake"
			);
	}
	Rect GetVerSubRect(Rect source, int index, int count){
		float width = source.width;
		float height = source.height/ count;
		float posX = source.x;
		float posY = index * height + source.y;
		return new Rect(
			posX,
			posY,
			width,
			height
		);
	}
	Rect GetHorSubRect(
		Rect source,
		int index,
		int count
	){
		float width = source.width/ count;
		float height = source.height;
		float posX = index * width + source.x;
		float posY = source.y;
		return new Rect(
			posX,
			posY,
			width,
			height
		);
	}
	[Range(1f, 5f)]
	public float thisAttributeMultiplier = 1.2f;
	public float GetAttributeMultiplier(){
		return thisAttributeMultiplier;
	}
	public int thisMaxAttributeLevel = 5;
	public int GetMaxAttributeLevel(){
		return thisMaxAttributeLevel;
	}
	public int baseCost = 10;
	public int GetBaseCost(){
		return baseCost;
	}
	[Range(0f, 2f)]
	public float initialTotalAttriValue = 0f;

	void DrawAttributeValue(Rect rect){
		thisCalculator.CalcAttributeData();
		string result = "";

		for(int i = 0; i < thisMaxAttributeLevel + 1; i ++){
			result += "level: " + i.ToString() + ", ";
			float attriValue = thisCalculator.GetAttributeValue(i);
			result += "attValue: " + attriValue.ToString("N3") + ", ";
			float prevAV = thisCalculator.GetPrevAttributeValue(i);
			float deltaAV = thisCalculator.GetDeltaAttributeValue(i);
			result += "deltaAV: " + deltaAV.ToString("N3") + ", ";
			float coinDepreValue = thisCalculator.CalcCoinDepreciationValue(prevAV + initialTotalAttriValue);
			int cost = thisCalculator.CalcCost(i, coinDepreValue);
			result += "cost: " + cost.ToString() + ", ";
			
			result += "\n";
		}
		GUI.Label(rect, result);
	}
	public float thisMaxCoinDepreValue = 30f;
	public float GetMaxCoinDepreciationValue(){
		return thisMaxCoinDepreValue;
	}
	/*  */
	void DrawAttributeControl(Rect rect){
		Rect sub_0 = GetVerSubRect(rect, 0, 4);
		Rect sub_1 = GetVerSubRect(rect, 1, 4);
		Rect sub_2 = GetVerSubRect(rect, 2, 4);
		Rect sub_3 = GetVerSubRect(rect, 3, 4);
		DrawAttributeOverview(sub_0);
		DrawIndivAttributeControl(
			0,
			sub_1,
			"strength"
		);
		DrawIndivAttributeControl(
			1,
			sub_2,
			"quickness"
		);
		DrawIndivAttributeControl(
			2,
			sub_3,
			"crit"
		);
	}
	void DrawIndivAttributeControl(
		int index,
		Rect rect,
		string attName
	){
		thisCalculator.CalcAttributeData();
		float labelRectRatio = .3f;
		Rect labelRect = CalcSubRect(
			rect,
			new Vector2(
				labelRectRatio,
				1f
			),
			new Vector2(
				0f,
				0f
			)
		);
		Rect buttonRect = CalcSubRect(
			rect,
			new Vector2(
				1f - labelRectRatio,
				1f
			),
			new Vector2(
				labelRectRatio,
				0f
			)
		);
		string labelString = "";
		labelString += attName + "\n";
		int attributeLevel = thisCalculator.GetAttributeLevel(index);
		labelString += "lv " + attributeLevel.ToString() + "\n";
		float attributeValue = thisCalculator.GetCurrentAttributeValueByIndex(index);
		labelString += "av: " + attributeValue.ToString();
		GUI.Label(
			labelRect,
			labelString
		);
		if(attributeLevel < thisMaxAttributeLevel){
			int nextLevel = attributeLevel + 1;
			float totalAttributeValue = thisCalculator.GetTotalAttributeValue();
			float coinDepreValue = thisCalculator.CalcCoinDepreciationValue(totalAttributeValue);
			int nextCost = thisCalculator.CalcCost(
				nextLevel,
				coinDepreValue
			);
			if(GUI.Button(
				buttonRect,
				nextCost.ToString()
			)){
				thisCalculator.IncrementAttributeLevel(
					index, nextCost
				);
			}
		}else
			GUI.Label(
				buttonRect,
				"max"
			);
		
	}
	/* attack */
		float CalcInitAttack(float attriValue){
			return Mathf.Lerp(
				thisMinInitAttack,
				thisMaxInitAttack,
				attriValue
			);
		}
		float CalcTermAttack(float attriValue){
			return Mathf.Lerp(
				thisMinTermAttack,
				thisMaxTermAttack,
				attriValue
			);
		}
		float thisMinInitAttack{
			get{
				return Mathf.Lerp(
					thisMinAttack,
					thisMaxAttack,
					0.05f
				);
			}
		}
		float thisMaxInitAttack{
			get{
				return Mathf.Lerp(
					thisMinAttack,
					thisMaxAttack,
					.3f
				);
			}
		}
		float thisMinAttack = 10f;
		float thisMaxAttack = 300f;
		float thisMinTermAttack{
			get{
				return Mathf.Lerp(
					thisMinAttack,
					thisMaxAttack,
					.35f
				);
			}
		}
		float thisMaxTermAttack{
			get{
				return Mathf.Lerp(
					thisMinAttack,
					thisMaxAttack,
					1f
				);
			}
		}
	/* quickness */
		float GetFireRate(float attriValue){
			return Mathf.Lerp(
				thisMinFireRate,
				thisMaxFireRate,
				attriValue
			);
		}
		float thisMinFireRate = .8f;
		float thisMaxFireRate = .3f;
	/* crit */
		float GetCritMult(float attriValue){
			return Mathf.Lerp(
				thisMinCrit,
				thisMaxCrit,
				attriValue
			);
		}
		float thisMinCrit = 1.3f;
		float thisMaxCrit = 8f;
	void DrawAttributeOverview(Rect rect){
		Rect sub_0 = GetHorSubRect(rect, 0, 3);
		Rect sub_1 = GetHorSubRect(rect, 1, 3);
		Rect sub_2 = GetHorSubRect(rect, 2, 3);
		string labelString = "";
		int bowLevel = thisCalculator.GetBowLevel();
		labelString += "bowLevel: " + bowLevel.ToString() + "\n";
		float totalAttributeValue = thisCalculator.GetTotalAttributeValue();
		labelString += "totalAV: " + totalAttributeValue.ToString() + "\n";
		int totalSpentCoin = thisCalculator.GetTotalSpentCoin();
		labelString += "totalCoin: " + totalSpentCoin.ToString() + "\n";
		labelString += "val/coin: " + (totalAttributeValue/ totalSpentCoin).ToString() + "\n";
		labelString += "depreValue: " + thisCalculator.CalcCoinDepreciationValue(totalAttributeValue);

		string labelString2 = "";
		float strengthAttValue = thisCalculator.GetCurrentAttributeValueByIndex(0);
		float initAttack = CalcInitAttack(strengthAttValue);
		float termAttack = CalcTermAttack(strengthAttValue);
		float quicknessAttValue = thisCalculator.GetCurrentAttributeValueByIndex(1);
		float fireRate = GetFireRate(quicknessAttValue);
		float critAttValue = thisCalculator.GetCurrentAttributeValueByIndex(2);
		float critMult = GetCritMult(critAttValue);
		labelString2 += "attack: " + initAttack.ToString("N1") + " to "  + termAttack.ToString("N1") + "\n";
		labelString2 += "fireRate: " + fireRate.ToString("N2") + "\n";
		labelString2 += "critMult: "  + critMult.ToString("N2") + "\n";
		labelString2 += "dps(crit): " + (initAttack * 1f/fireRate).ToString("N1") + " to " + (termAttack * 1f/fireRate).ToString("N1") + " ";
		labelString2 += "(" + (initAttack * critMult * 1f/fireRate).ToString("N1") + " to " + (termAttack * 1f/fireRate * critMult).ToString("N1") + ")" + "\n";
		float expectedCritRate = 1f/3f;
		float normalHitRate = 1f - expectedCritRate;
		labelString2 += "weightedDPS: " + ((normalHitRate * (initAttack * 1f/fireRate)) + (expectedCritRate * ((initAttack * critMult * 1f/fireRate)))).ToString("N1");
		labelString2 += " to " + ((normalHitRate * (termAttack * 1f/fireRate)) + (expectedCritRate * ((termAttack * critMult * 1f/fireRate)))).ToString("N1");

		GUI.Label(
			sub_0,
			labelString
		);
		GUI.Label(
			sub_1,
			labelString2
		);
		if(GUI.Button(
			sub_2,
			"clear"
		))
			thisCalculator.ClearAttribute();
	}
}
