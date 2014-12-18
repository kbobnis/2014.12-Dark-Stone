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
}


