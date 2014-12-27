using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelBottom : MonoBehaviour {

	public GameObject PanelMana, PanelHand;

	private AvatarModel _HeroModel;

	public AvatarModel HeroModel {
		set { _HeroModel = value; UpdateFromModel(); }
	}

	// Use this for initialization
	void Start () {
		List<List<TileTemplate>> templates = new List<List<TileTemplate>>();
		for (int i = 0; i < 2; i++) {
			List<TileTemplate> col = new List<TileTemplate>();
			for (int j = 0; j < 5; j++) {
				TileTemplate tt = new TileTemplate();
				col.Add(tt);
			}
			templates.Add(col);
		}

		PanelHand.GetComponent<ScrollableList>().Build(templates);
	}
	
	// Update is called once per frame
	private void UpdateFromModel () {
		PanelMana.GetComponent<PanelMana>().AvatarModel = _HeroModel;

		List<GameObject> gos = PanelHand.GetComponent<ScrollableList>().ElementsToPut;
		for (int i = 0; i < gos.Count; i++) {
			gos[i].SetActive(_HeroModel.Hand.Count > i);
			if (_HeroModel.Hand.Count > i) {
				gos[i].GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>().PanelAvatarCard.GetComponent<PanelAvatarCard>().PreviewCardHand(_HeroModel,_HeroModel.Hand[i]);
			}

		}
	}

}
