using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class PanelMinigame : MonoBehaviour {

	public GameObject PanelTiles, PanelTilesBg, PanelInformation;

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

		PanelTilesBg.GetComponent<ScrollableList>().Build(templates);

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
			}
		}

		bool stillSomethingToDo;
		do {
			PanelInformation.GetComponent<PanelInformation>().SetText("Calculating");
			//checking what is moving
			stillSomethingToDo = false;
			List<KeyValuePair<PanelTile, Side>> toMove = new List<KeyValuePair<PanelTile, Side>>();
			foreach (GameObject go in elements) {
				PanelTile pt = go.GetComponent<PanelTile>();
				PanelAvatar pa = pt.PanelAvatar.GetComponent<PanelAvatar>();
				Side direction = pa.Model.Direction;
				if (pa.Model.MovesLeft > 0) {
					toMove.Add(new KeyValuePair<PanelTile, Side>(pt, direction));
					if (pa.Model.MovesLeft > 1) {
						stillSomethingToDo = true;
					}
				}
			}

			PanelInformation.GetComponent<PanelInformation>().SetText("Moving");
			//moving
			foreach (KeyValuePair<PanelTile, Side> oneMove in toMove) {
				oneMove.Key.PanelAvatar.AddComponent<Mover>().Prepare(oneMove.Value, 0.5f);
				oneMove.Key.PanelAvatar.GetComponent<Mover>().ToPercent(0.5f);

			}
			yield return new WaitForSeconds(0.5f);

			//check mid air collisions
			Dictionary<PanelTile, Side> midAirCollisions = new Dictionary<PanelTile, Side>();
			foreach (KeyValuePair<PanelTile, Side> move in toMove.ToArray()) {
				//if is not outside the board
				if (move.Key.Neighbours.ContainsKey(move.Value)) {
					PanelTile oppositeTile = move.Key.Neighbours[move.Value];
					Side oppositeSide = move.Value.Opposite();
					if (oppositeTile.PanelAvatar.GetComponent<PanelAvatar>().Model.Direction == oppositeSide && oppositeTile.PanelAvatar.GetComponent<PanelAvatar>().Model.MovesLeft > 0 ) {
						//mid air collision!
						if (!midAirCollisions.ContainsKey(move.Key) && !midAirCollisions.ContainsKey(oppositeTile)) {
							Debug.Log("Mid air collision: " + move.Key.gameObject.name + " with: " + oppositeTile.gameObject.name);
							midAirCollisions.Add(move.Key, move.Value);
						}
					}
				}
			}

			//battle out mid air collisions
			foreach (KeyValuePair<PanelTile, Side> kvp in midAirCollisions) {

				PanelTile oppositePt = kvp.Key.Neighbours[kvp.Value];
				kvp.Key.PanelAvatar.GetComponent<PanelAvatar>().BattleOutWith(oppositePt.PanelAvatar.GetComponent<PanelAvatar>());
			}

			
			PanelInformation.GetComponent<PanelInformation>().SetText("Moving 2");
			//moving
			foreach (KeyValuePair<PanelTile, Side> oneMove in toMove) {
				oneMove.Key.PanelAvatar.GetComponent<Mover>().ToPercent(1f);
			}
			yield return new WaitForSeconds(0.5f);

			//clearing out the toMove list
			foreach(KeyValuePair<PanelTile, Side> oneMove in toMove.ToArray()) {
				Mover m = oneMove.Key.PanelAvatar.GetComponent<Mover>();
				m.ResetToBase();
				Destroy(m);
				if (oneMove.Key.PanelAvatar.GetComponent<PanelAvatar>().Model.Card == null) {
					Debug.Log("Removed " + oneMove.Key.gameObject.name + " from toMove list");
					toMove.Remove(oneMove);
				}
			}

			//replacing
			foreach (KeyValuePair<PanelTile, Side> oneMove in toMove) {
				Debug.Log("Switching panel tile: " + oneMove.Key + " to side: " + oneMove.Value);
				SwitchPanelAvatar(oneMove.Key, oneMove.Value);
			}

			//updating when no collisions
			foreach (GameObject go in elements) {
				go.GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>().FlattenModelWhenNoBattles();
			}

			PanelInformation.GetComponent<PanelInformation>().SetText("Batlles");
			//battling
			foreach (GameObject go in elements) {
				go.GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>().BattleOut();
			}
			yield return new WaitForSeconds(0.25f);
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

		yield return new WaitForSeconds(0.1f);
		PanelInformation.GetComponent<PanelInformation>().DisableMe();
		yield return null;
	}

	private void SwitchPanelAvatar(PanelTile pt, Side direction) {
		//moves outside screen
		if (!pt.Neighbours.ContainsKey(direction)) {
			pt.PanelAvatar.GetComponent<PanelAvatar>().Model = new PanelAvatarModel();
		} else {
			Debug.Log("Removing model from " + pt.gameObject.name + " and adding to " + pt.Neighbours[direction].gameObject.name);
			pt.PanelAvatar.GetComponent<PanelAvatar>().Model.MovesLeft--;
			pt.Neighbours[direction].PanelAvatar.GetComponent<PanelAvatar>().AddModel(pt.PanelAvatar.GetComponent<PanelAvatar>().GetAndRemoveModel());
		}
	}

}
