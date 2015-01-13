using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class PanelTiles : MonoBehaviour {

	internal PanelTile FindTileForModel(AvatarModel model) {
		foreach (GameObject go in GetComponent<ScrollableList>().ElementsToPut) {
			if (go.GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>().Model == model) {
				return go.GetComponent<PanelTile>();
			}
		}
		return null;
	}

	internal bool IsModelOfCard(Card c) {
		foreach (GameObject go in GetComponent<ScrollableList>().ElementsToPut) {
			if (go.GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>().Model != null) {
				if (go.GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>().Model.Card == c) {
					return true;
				}
			}
		}
		return false;
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

	internal List<AvatarModel> GetAllAvatarModels() {
		List<AvatarModel> tmp = new List<AvatarModel>();
		foreach (GameObject go in GetComponent<ScrollableList>().ElementsToPut) {
			PanelTile pt = go.GetComponent<PanelTile>();
			AvatarModel am = go.GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>().Model;
			if (am != null) {
				tmp.Add(am);
			}
		}
		return tmp;
	}

	internal List<PanelAvatar> GetAllPanelAvatars() {
		List<PanelAvatar> tmp = new List<PanelAvatar>();
		foreach (GameObject go in GetComponent<ScrollableList>().ElementsToPut) {
			PanelTile pt = go.GetComponent<PanelTile>();
			PanelAvatar pa = go.GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>();
			if (pa != null) {
				tmp.Add(pa);
			}
		}
		return tmp;
	}

	internal List<PanelTile> GetAllPanelTiles() {
		List<PanelTile> tmp = new List<PanelTile>();
		foreach (GameObject go in GetComponent<ScrollableList>().ElementsToPut) {
			PanelTile pt = go.GetComponent<PanelTile>();
			tmp.Add(pt);
		}
		return tmp;
	}
}


