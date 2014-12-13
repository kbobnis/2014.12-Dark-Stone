using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class PanelTiles : MonoBehaviour {

	internal void PointerDownOn(PanelTile panelTile) {
		Debug.Log("Touched on: " + panelTile.gameObject.name);

		//check if there is card on board with spell ready to cast
		PanelSpell sp = panelTile.PanelSpell.GetComponent<PanelSpell>();
		if (sp.TopCard() != null) {
			Debug.Log("card: " + sp.TopCard());
		}

	}
}
