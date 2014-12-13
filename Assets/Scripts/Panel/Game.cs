using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System;

public class Game : MonoBehaviour {

	public GameObject PanelMinigame;


	public static Game Me;
	public string UserName = "Anonymous";
	
	void Awake () {
		Me = this;

		StartCoroutine(Prepare());
	}

	private IEnumerator Prepare() {
		yield return new WaitForSeconds(0.5f);
		PanelMinigame.GetComponent<PanelMinigame>().Prepare();
	}



}
