using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class PanelTiles : MonoBehaviour {

	internal PanelTile FindTileForModel(AvatarModel model) {
		foreach (GameObject go in GetComponent<ScrollableList>().ElementsToPut) {
			if (go.GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>().Model == model) {
				return go.GetComponent<PanelTile>();
			}
		}
		return null;
	}

	internal void UpdateAdjacentModels() {
		foreach (GameObject go in GetComponent<ScrollableList>().ElementsToPut) {
			
			PanelTile pt = go.GetComponent<PanelTile>();
			AvatarModel am = go.GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>().Model;
			if (am != null) {

				foreach (Side s in SideMethods.AdjacentSides()) {
					if (pt.Neighbours.ContainsKey(s)) {
						am.AdjacentModels[s] = pt.Neighbours[s].PanelAvatar.GetComponent<PanelAvatar>().Model;
					}
				}
			}
		}
	}
}


