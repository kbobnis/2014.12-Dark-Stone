using UnityEngine;
using System.Collections;
using System;

public class Mover : MonoBehaviour {

	private Side Side = Side.None;
	private float AnimationTime, StartTime;
	private float FullDeltaX, FullDeltaY;
	private float ToPercentF, FromPercentF;

	private float IdleTime;

	private Vector2 BaseOffsetMin, BaseOffsetMax;

	void Update () {
		if (Side != Side.None) {
			float percentAnimation = FromPercentF + (Time.time - StartTime - IdleTime) / AnimationTime;
			if (percentAnimation < ToPercentF) {
				GetComponent<RectTransform>().offsetMin = new Vector2(FullDeltaX * percentAnimation, FullDeltaY * percentAnimation);
				GetComponent<RectTransform>().offsetMax = new Vector2(FullDeltaX * percentAnimation,  FullDeltaY * percentAnimation);
			} else {
				IdleTime += Time.deltaTime;
			}
		}
	}

	public void ResetToBase() {
		GetComponent<RectTransform>().offsetMin = BaseOffsetMin;
		GetComponent<RectTransform>().offsetMax = BaseOffsetMax;
	}



	internal void Prepare(Side side, float animationTime) {
		Side = side;
		AnimationTime = animationTime;
		StartTime = Time.time;

		BaseOffsetMin = GetComponent<RectTransform>().offsetMin;
		BaseOffsetMax = GetComponent<RectTransform>().offsetMax;

		FullDeltaX = 0;
		FullDeltaY = 0;
		ToPercentF = 1f;

		//y
		switch(side){
			case global::Side.Up:
			case global::Side.UpLeft:
			case global::Side.UpRight:
				FullDeltaY = GetComponent<RectTransform>().rect.size.y;
				break;
			case global::Side.Down:
			case global::Side.DownLeft: 
			case global::Side.DownRight:
				FullDeltaY = -1 * GetComponent<RectTransform>().rect.size.y;
				break;
		}

		//x
		switch (side) {
			case global::Side.Left:
			case global::Side.UpLeft:
			case global::Side.DownLeft:
				FullDeltaX = -1 * GetComponent<RectTransform>().rect.size.x;
				break;
			case global::Side.Right:
			case global::Side.DownRight:
			case global::Side.UpRight:
				FullDeltaX = 1 * GetComponent<RectTransform>().rect.size.x;
				break;
		}
		
	}

	internal void ToPercent(float p) {
		ToPercentF = p;
	}

	internal void FromPercent(float p) {
		FromPercentF = p;
	}
}
