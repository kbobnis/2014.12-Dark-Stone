using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class PanelMinigame : MonoBehaviour {

	public GameObject PanelTiles, PanelTilesBg, PanelInformation, PanelCardPreview, PanelBottom, ButtonEndTurn;

	private AvatarModel MyModel, EnemysModel;
	private AvatarModel ActualTurnModel;

	internal void Prepare() {
		PanelInformation.SetActive(true);

		List<List<TileTemplate>> templates = new List<List<TileTemplate>>();
		for (int i = 0; i < 4; i++) {
			List<TileTemplate> col = new List<TileTemplate>();
			for (int j = 0; j < 5; j++) {
				TileTemplate tt = new TileTemplate();
				col.Add(tt);
			}
			templates.Add(col);
		}

		PanelTilesBg.GetComponent<ScrollableList>().Build(templates);

		templates[3][2].AddTemplate(Card.Lasia);
		templates[0][2].AddTemplate(Card.Dementor);

		PanelTiles.GetComponent<ScrollableList>().Build(templates);
		
		PanelAvatar lasiaAvatar = PanelTiles.GetComponent<ScrollableList>().ElementsToPut[3 * 5 + 2].GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>();
		AvatarModel lasiaModel = lasiaAvatar.Model;
		lasiaModel.Deck.Add(Card.Zombie);
		lasiaModel.Deck.Add(Card.HealingTouch);
		lasiaModel.Deck.Add(Card.Fireball);
		lasiaModel.Deck.Add(Card.IceBolt);
		lasiaModel.Deck.Add(Card.Mud);
		lasiaModel.Deck.Add(Card.IceBolt);
		lasiaModel.Deck.Add(Card.Mud);
		lasiaModel.OnBoard = true;
		lasiaAvatar.UpdateFromModel();

		PanelAvatar dementorAvatar = PanelTiles.GetComponent<ScrollableList>().ElementsToPut[2].GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>();
		AvatarModel dementorModel = dementorAvatar.Model;
		dementorModel.Deck.Add(Card.IceBolt);
		dementorModel.Deck.Add(Card.Mud);
		dementorModel.OnBoard = true;
		dementorAvatar.UpdateFromModel();

		MyModel = lasiaModel;
		EnemysModel = dementorModel;

		PanelBottom.GetComponent<PanelBottom>().Prepare(lasiaModel);

		ActualTurnModel = EnemysModel;
		EndTurn();

	}

	void Update() {
		if (MyModel != null && EnemysModel != null && (MyModel.ActualHealth <= 0 || EnemysModel.ActualHealth <= 0)) {
			PanelInformation.GetComponent<PanelInformation>().SetText((MyModel.ActualHealth > 0 ? "You won" : "You lost"));
		}
	}

	public void EndTurn() {
		if (PanelTiles.GetComponent<PanelTiles>().Mode != Mode.Ready) {
			PanelInformation.GetComponent<PanelInformation>().SetText("Finish " + PanelTiles.GetComponent<PanelTiles>().Mode + " first.");
		} else {
			StartCoroutine(EndTurnCoroutine());
		}
	}

	private IEnumerator EndTurnCoroutine() {

		ActualTurnModel = ActualTurnModel == MyModel?EnemysModel:MyModel;

		ActualTurnModel.AddCrystal();
		ActualTurnModel.RefillCrystals();
		ActualTurnModel.RefillMovements();
		ActualTurnModel.PullCardFromDeck();

		ButtonEndTurn.GetComponentInChildren<Text>().text = ActualTurnModel == MyModel?"End turn":"Enemys turn";

		yield return null;
	}

}
