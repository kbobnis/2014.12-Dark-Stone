using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class PanelMinigame : MonoBehaviour {

	public GameObject PanelTiles, PanelInformation;

	internal void Prepare() {
		PanelInformation.SetActive(true);

		List<List<TileTemplate>> templates = new List<List<TileTemplate>>();
		for (int i = 0; i < 7; i++) {
			List<TileTemplate> col = new List<TileTemplate>();
			for (int j = 0; j < 5; j++) {
				TileTemplate tt = new TileTemplate();
				col.Add(tt);
			}
			templates.Add(col);
		}

		templates[6][2].AddTemplate(Card.Lasia);
		templates[0][2].AddTemplate(Card.Dementor);

		PanelTiles.GetComponent<ScrollableList>().Build(templates);
		
		PanelSpell lasiaPanel = PanelTiles.GetComponent<ScrollableList>().ElementsToPut[5 * 6 + 2].GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>().PanelSpell.GetComponent<PanelSpell>();
		lasiaPanel.AddCard(Card.IceBolt);
		lasiaPanel.AddCard(Card.Mud);
		lasiaPanel.AddCard(Card.IceBolt);
		lasiaPanel.AddCard(Card.Mud);

		PanelSpell dementorPanel = PanelTiles.GetComponent<ScrollableList>().ElementsToPut[2].GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>().PanelSpell.GetComponent<PanelSpell>();
		dementorPanel.AddCard(Card.IceBolt);
		dementorPanel.AddCard(Card.Mud);
	}

	public void EndTurn() {
		StartCoroutine(EndTurnCoroutine());
	}

	private IEnumerator EndTurnCoroutine() {

		//refill moves
		PanelInformation.GetComponent<PanelInformation>().SetText("Refilling moves");
		List<GameObject> elements = PanelTiles.GetComponent<ScrollableList>().ElementsToPut;
		foreach (GameObject go in elements) {
			PanelTile pt = go.GetComponent<PanelTile>();
			PanelAvatar pa = pt.PanelAvatar.GetComponent<PanelAvatar>();
			Side direction = pa.PanelDirection.GetComponent<PanelDirection>().Side;
			pa.PanelDirection.GetComponent<PanelDirection>().RefillMoves();
		}
		yield return new WaitForSeconds(1f);

		bool stillSomethingToDo;
		int iteration = 1;
		do {
			//moving
			stillSomethingToDo = false;
			foreach (GameObject go in elements) {
				PanelTile pt = go.GetComponent<PanelTile>();
				PanelAvatar pa = pt.PanelAvatar.GetComponent<PanelAvatar>();
				Side direction = pa.PanelDirection.GetComponent<PanelDirection>().Side;
				if (pa.PanelDirection.GetComponent<PanelDirection>().MovesLeft > 0) {
					pa.PanelDirection.GetComponent<PanelDirection>().MovesLeft--;
					MovePanelAvatar(pt, direction, 1);
					if (pa.PanelDirection.GetComponent<PanelDirection>().MovesLeft > 0) {
						stillSomethingToDo = true;
					}
				}
			}
			PanelInformation.GetComponent<PanelInformation>().SetText("Moving " + iteration++);
			yield return new WaitForSeconds(1f);

		} while (stillSomethingToDo);

		//interact spells

		//end
		PanelInformation.GetComponent<PanelInformation>().SetText("New turn");
		yield return new WaitForSeconds(0.5f);
		PanelInformation.GetComponent<PanelInformation>().DisableMe();
		yield return null;
	}

	private void MovePanelAvatar(PanelTile pt, Side direction, int distance) {

		if (distance > 1){
			throw new Exception("Programmer, what to do if distance is " + distance + " we want to move tiles by one");
		} else {
			//moves outside screen
			if (!pt.Neighbours.ContainsKey(direction)) {
				pt.PanelAvatar.GetComponent<PanelAvatar>().Clear();
			} else {
				if (!pt.Neighbours[direction].PanelAvatar.GetComponent<PanelAvatar>().IsEmpty()) {
					throw new Exception("we want to move on another panel avatar. collision collision. implement collisions");
				} else { //no collision, can move
					Debug.Log("Switching pa " + pt.gameObject.name + " with: " + pt.Neighbours[direction].gameObject.name);
					GameObject pa = pt.Neighbours[direction].PanelAvatar;
					pt.Neighbours[direction].PanelAvatar = pt.PanelAvatar;
					pt.PanelAvatar.gameObject.transform.parent = pt.Neighbours[direction].gameObject.transform;
					pt.PanelAvatar.gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
					pt.PanelAvatar.gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);

					pt.PanelAvatar = pa;
					pa.gameObject.transform.parent = pt.gameObject.transform;
					pa.gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
					pa.gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
					
				}
				
			}
		}
	}
}
