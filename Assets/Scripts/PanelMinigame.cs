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
		
		PanelAvatar lasiaAvatar = PanelTiles.GetComponent<ScrollableList>().ElementsToPut[5 * 6 + 2].GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>();
		PanelAvatarModel lasiaModel = lasiaAvatar.Model;
		lasiaModel.CardStack.Add(Card.Zombie);
		lasiaModel.CardStack.Add(Card.Fireball);
		lasiaModel.CardStack.Add(Card.IceBolt);
		lasiaModel.CardStack.Add(Card.Mud);
		lasiaModel.CardStack.Add(Card.IceBolt);
		lasiaModel.CardStack.Add(Card.Mud);
		lasiaAvatar.UpdateFromModel();

		PanelAvatar dementorAvatar = PanelTiles.GetComponent<ScrollableList>().ElementsToPut[2].GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>();
		PanelAvatarModel dementorModel = dementorAvatar.Model;
		dementorModel.CardStack.Add(Card.IceBolt);
		dementorModel.CardStack.Add(Card.Mud);
		dementorAvatar.UpdateFromModel();

	}

	public void EndTurn() {
		if (PanelTiles.GetComponent<PanelTiles>().Mode != Mode.Ready) {
			PanelInformation.GetComponent<PanelInformation>().SetText("Finish " + PanelTiles.GetComponent<PanelTiles>().Mode + " first.");
		} else {
			StartCoroutine(EndTurnCoroutine());
		}
	}

	private IEnumerator EndTurnCoroutine() {

		//refill moves
		List<GameObject> elements = PanelTiles.GetComponent<ScrollableList>().ElementsToPut;
		foreach (GameObject go in elements) {
			PanelAvatar pa = go.GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>();
			pa.Model.MovesLeft = pa.Model.Speed;
			if (pa.Model.Card != null) {
				int speed = pa.Model.Card.Params.ContainsKey(ParamType.Speed)? pa.Model.Card.Params[ParamType.Speed] : 0;
				Debug.Log(pa.Model.Card.Name + " has moves left: " + pa.Model.MovesLeft + " and moves: " + speed + ", moves in model : " + pa.Model.Speed);
			}
		}

		bool stillSomethingToDo;
		do {
			PanelInformation.GetComponent<PanelInformation>().SetText("Moving");
			//checking what is moving
			stillSomethingToDo = false;
			List<KeyValuePair<PanelTile, Side>> toMove = new List<KeyValuePair<PanelTile, Side>>();
			foreach (GameObject go in elements) {
				PanelTile pt = go.GetComponent<PanelTile>();
				PanelAvatar pa = pt.PanelAvatar.GetComponent<PanelAvatar>();
				Side direction = pa.Model.Direction;
				if (pa.Model.MovesLeft > 0) {
					pa.Model.MovesLeft--;
					toMove.Add(new KeyValuePair<PanelTile, Side>(pt, direction));
					if (pa.Model.MovesLeft > 0) {
						stillSomethingToDo = true;
					}
				}
			}

			//moving
			foreach (KeyValuePair<PanelTile, Side> oneMove in toMove) {
				ShiftPanelAvatar(oneMove.Key, oneMove.Value);
			}
			toMove.Clear();

			//updating when no collisions
			foreach (GameObject go in elements) {
				go.GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>().FlattenModelWhenNoBattles();
				go.GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>().ShowBattleSignIfBattle();
			}

			yield return new WaitForSeconds(0.5f);

			PanelInformation.GetComponent<PanelInformation>().SetText("Batlles");
			//battling
			foreach (GameObject go in elements) {
				go.GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>().BattleOut();
			}
			yield return new WaitForSeconds(0.5f);
		} while (stillSomethingToDo);

		PanelInformation.GetComponent<PanelInformation>().SetText("New turn");
		//mana update
		foreach (GameObject go in elements) {
			PanelAvatar pa = go.GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>();
			if (pa.Model.IncreaseManaEachTurn > 0){
				pa.Model.ActualMana += pa.Model.IncreaseManaEachTurn;
				pa.UpdateFromModel();
			}
		}

		yield return new WaitForSeconds(0.25f);
		PanelInformation.GetComponent<PanelInformation>().DisableMe();
		yield return null;
	}

	private void ShiftPanelAvatar(PanelTile pt, Side direction) {
		//moves outside screen
		if (!pt.Neighbours.ContainsKey(direction)) {
			pt.PanelAvatar.GetComponent<PanelAvatar>().Model = new PanelAvatarModel();
		} else {
			pt.Neighbours[direction].PanelAvatar.GetComponent<PanelAvatar>().AddModel(pt.PanelAvatar.GetComponent<PanelAvatar>().GetAndRemoveModel());
		}
	}

}
