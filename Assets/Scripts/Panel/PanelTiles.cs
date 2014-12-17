using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class PanelTiles : MonoBehaviour {

	public GameObject PanelInformation;

	public Mode Mode = Mode.Ready;

	internal void PointerDownOn(PanelTile panelTile) {
		Debug.Log("Clicked");


		switch (Mode) {
			case global::Mode.Ready: {
				if (panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model.MovesLeft > 0) {
					foreach (Side s in SideMethods.AllSides()) {
						if (panelTile.Neighbours.ContainsKey(s)) {
							panelTile.Neighbours[s].PanelInteraction.GetComponent<PanelInteraction>().CanMoveHere(panelTile);
						}
					}
					Mode = global::Mode.MovingElement;
					Debug.Log("Can move, has " + panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model.MovesLeft + " moves left");
				}
				break;
			}
			case global::Mode.MovingElement: {
				PanelTile whatWantsToMoveHere = panelTile.PanelInteraction.GetComponent<PanelInteraction>().WhatWantsToMoveHere;
				if (whatWantsToMoveHere != null) {
					whatWantsToMoveHere.PanelAvatar.GetComponent<PanelAvatar>().Model.MovesLeft--;
					panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model = whatWantsToMoveHere.PanelAvatar.GetComponent<PanelAvatar>().Model;
					whatWantsToMoveHere.PanelAvatar.GetComponent<PanelAvatar>().Model = new AvatarModel();
				} 
				DisableAllPanelsInteraction();
				Mode = global::Mode.Ready;

				break;
			}
			default: throw new NotImplementedException("Implement working with mode: " + Mode);
		}
	}

	private void DisableAllPanelsInteraction() {
		foreach (GameObject tile in GetComponent<ScrollableList>().ElementsToPut) {
			tile.GetComponent<PanelTile>().PanelInteraction.GetComponent<PanelInteraction>().Clear();
		}
	}
}

public enum Mode {
	Ready, MovingElement
}
