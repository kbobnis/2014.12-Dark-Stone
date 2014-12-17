using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PanelTile : MonoBehaviour {

	public GameObject PanelTiles, PanelAvatar, PanelInteraction;

	public Dictionary<Side, PanelTile> Neighbours = new Dictionary<Side, PanelTile>();

	public void Touched(BaseEventData bed) {
		PanelTiles.GetComponent<PanelTiles>().PointerDownOn(this);
	}

}
