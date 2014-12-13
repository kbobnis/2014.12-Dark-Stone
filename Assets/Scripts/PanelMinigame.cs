using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class PanelMinigame : MonoBehaviour {

	public GameObject PanelTiles;

	internal void Prepare() {

		List<List<TileTemplate>> templates = new List<List<TileTemplate>>();
		for (int i = 0; i < 10; i++) {
			List<TileTemplate> col = new List<TileTemplate>();
			for (int j = 0; j < 7; j++) {
				TileTemplate tt = new TileTemplate();
				col.Add(tt);
			}
			templates.Add(col);
		}

		templates[9][3].AddTemplate(Card.Lasia);
		templates[0][3].AddTemplate(Card.Dementor);

		PanelTiles.GetComponent<ScrollableList>().Build(templates);

		
		PanelSpell lasiaPanel = PanelTiles.GetComponent<ScrollableList>().ElementsToPut[7 * 9 + 3].GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>().PanelSpell.GetComponent<PanelSpell>();
		lasiaPanel.AddCard(Card.IceBolt);
		lasiaPanel.AddCard(Card.Mud);
		lasiaPanel.AddCard(Card.IceBolt);
		lasiaPanel.AddCard(Card.Mud);

		PanelSpell dementorPanel = PanelTiles.GetComponent<ScrollableList>().ElementsToPut[3].GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>().PanelSpell.GetComponent<PanelSpell>();
		dementorPanel.AddCard(Card.IceBolt);
		dementorPanel.AddCard(Card.Mud);
		/*
		PanelTile leftOrbTile = PanelTiles.GetComponent<ScrollableList>().ElementsToPut[7 * 9 + 1].GetComponent<PanelTile>();
		lasiaPanel.AddCard(Card.Orb);
		lasiaPanel.CastOn(leftOrbTile);

		PanelTile rightOrbTile = PanelTiles.GetComponent<ScrollableList>().ElementsToPut[7 * 9 + 5].GetComponent<PanelTile>();
		lasiaPanel.AddCard(Card.Orb);
		lasiaPanel.CastOn(rightOrbTile);

		PanelSpell leftOrb = leftOrbTile.PanelAvatar.GetComponent<PanelAvatar>().PanelSpell.GetComponent<PanelSpell>();
		leftOrb.AddCard(Card.Mud);
		leftOrb.AddCard(Card.IceBolt);
		leftOrb.AddCard(Card.IceBolt);

		PanelSpell rightOrb = rightOrbTile.PanelAvatar.GetComponent<PanelAvatar>().PanelSpell.GetComponent<PanelSpell>();
		rightOrb.AddCard(Card.IceBolt);
		rightOrb.AddCard(Card.Mud);
		rightOrb.AddCard(Card.Mud);
		 * */
	}
}
