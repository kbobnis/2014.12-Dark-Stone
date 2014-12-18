using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PanelTile : MonoBehaviour {

	public GameObject PanelMinigame, PanelAvatar, PanelInteraction;

	public Dictionary<Side, PanelTile> Neighbours = new Dictionary<Side, PanelTile>();

	public void Touched(BaseEventData bed) {
		try {
			PanelMinigame.GetComponent<PanelMinigame>().PointerDownOn(this);
		} catch (Exception e) {
			Debug.Log("Exception: " + e);
		}
	}

}
