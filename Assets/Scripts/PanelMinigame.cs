using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelMinigame : MonoBehaviour {

	public GameObject PanelTiles;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

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

		Card orb = new Card("Orb", 10, SpriteManager.OrbAnimations, new Dictionary<ParamType, int>(){ { ParamType.Health, 30} });

		templates[9][3].AddTemplate(Card.Lasia);
		templates[0][3].AddTemplate(Card.Dementor);

		PanelTiles.GetComponent<ScrollableList>().Build(templates);

		GameObject lasia = PanelTiles.GetComponent<ScrollableList>().ElementsToPut[7 * 9 + 3];
		PanelCardOnBoard lasiaPanel = lasia.GetComponent<PanelTile>().PanelCardOnBoard.GetComponent<PanelCardOnBoard>();
		
		lasiaPanel.Cast(orb, PanelTiles.GetComponent<ScrollableList>().ElementsToPut[7 * 9 + 1].GetComponent<PanelTile>().PanelCardOnBoard.GetComponent<PanelCardOnBoard>());

		PanelCardOnBoard leftOrb = PanelTiles.GetComponent<ScrollableList>().ElementsToPut[7 * 9 + 1].GetComponent<PanelTile>().PanelCardOnBoard.GetComponent<PanelCardOnBoard>();
		leftOrb.AddCardTemplate(Card.Mud);
		leftOrb.AddCardTemplate(Card.IceBolt);
		leftOrb.AddCardTemplate(Card.IceBolt);
		
		PanelCardOnBoard rightOrb = PanelTiles.GetComponent<ScrollableList>().ElementsToPut[7 * 9 + 5].GetComponent<PanelTile>().PanelCardOnBoard.GetComponent<PanelCardOnBoard>();
		lasiaPanel.Cast(orb, rightOrb);
		rightOrb.AddCardTemplate(Card.IceBolt);
	}
}
