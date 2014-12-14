using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class PanelMinigame : MonoBehaviour {

	public GameObject PanelTiles, PanelInformation;

	internal void Prepare() {
		PanelInformation.SetActive(true);

		List<List<TileTemplate>> templates = new List<List<TileTemplate>>();
		for (int i = 0; i < 9; i++) {
			List<TileTemplate> col = new List<TileTemplate>();
			for (int j = 0; j < 6; j++) {
				TileTemplate tt = new TileTemplate();
				col.Add(tt);
			}
			templates.Add(col);
		}

		templates[8][3].AddTemplate(Card.Lasia);
		templates[0][3].AddTemplate(Card.Dementor);

		PanelTiles.GetComponent<ScrollableList>().Build(templates);
		
		PanelSpell lasiaPanel = PanelTiles.GetComponent<ScrollableList>().ElementsToPut[6 * 8 + 3].GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>().PanelSpell.GetComponent<PanelSpell>();
		lasiaPanel.AddCard(Card.IceBolt);
		lasiaPanel.AddCard(Card.Mud);
		lasiaPanel.AddCard(Card.IceBolt);
		lasiaPanel.AddCard(Card.Mud);

		PanelSpell dementorPanel = PanelTiles.GetComponent<ScrollableList>().ElementsToPut[3].GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>().PanelSpell.GetComponent<PanelSpell>();
		dementorPanel.AddCard(Card.IceBolt);
		dementorPanel.AddCard(Card.Mud);
	}
}
