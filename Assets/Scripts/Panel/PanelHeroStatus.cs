using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PanelHeroStatus : MonoBehaviour {

	public GameObject PanelManaCrystal, PanelHand, Deck, PanelSpecialPower, AvatarCardPrefab;

	private AvatarModel _HeroModel;

	public AvatarModel HeroModel {
		set { _HeroModel = value; UpdateFromModel(); }
	}

	void Awake() {


		GameObject newItem = Instantiate(AvatarCardPrefab) as GameObject;

		newItem.name = "Special power";
		newItem.GetComponent<PanelAvatarCard>().WhereAmI = WhereAmI.SpecialPower;
		newItem.transform.parent = PanelSpecialPower.transform;

		//move and size the new item
		RectTransform rectTransform = newItem.GetComponent<RectTransform>();

		rectTransform.offsetMin = new Vector2(0, 0);
		rectTransform.offsetMax = new Vector2(0, 0);
		rectTransform.rotation = PanelSpecialPower.GetComponent<RectTransform>().rotation;
	}

	// Use this for initialization
	void Start () {
		List<List<TileTemplate>> templates = new List<List<TileTemplate>>();
		for (int i = 0; i < 1; i++) {
			List<TileTemplate> col = new List<TileTemplate>();
			for (int j = 0; j < 10; j++) {
				TileTemplate tt = new TileTemplate();
				col.Add(tt);
			}
			templates.Add(col);
		}

		if (PanelHand.GetComponent<ScrollableList>() != null) {
			PanelHand.GetComponent<ScrollableList>().Build(templates);
		}
	}
	
	// Update is called once per frame
	private void UpdateFromModel () {
		PanelManaCrystal.GetComponentInChildren<Text>().text = _HeroModel.ActualMana + "/" + _HeroModel.MaxMana;

		if (PanelHand.GetComponent<ScrollableList>() != null) {
			List<GameObject> gos = PanelHand.GetComponent<ScrollableList>().ElementsToPut;
			for (int i = 0; i < gos.Count; i++) {
				gos[i].SetActive(_HeroModel.Hand.Count > i);
				if (_HeroModel.Hand.Count > i && gos[i].GetComponent<PanelTile>() != null) {
					gos[i].GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>().PanelAvatarCard.GetComponent<PanelAvatarCard>().PreviewCardHand(_HeroModel, _HeroModel.Hand[i], WhereAmI.Hand);
				}

			}
		} else if (PanelHand.GetComponent<PanelHand>() != null) {
			PanelHand.GetComponent<PanelHand>().UpdateWith(_HeroModel, _HeroModel.Hand);
		}

		Deck.GetComponentInChildren<Text>().text = "" + _HeroModel.Deck.Count;

		PanelSpecialPower.SetActive(false);
		if (_HeroModel.Card.Effects.ContainsKey(Effect.HerosSpecialPower)) {
			PanelSpecialPower.SetActive(true);
			PanelSpecialPower.GetComponentInChildren<PanelAvatarCard>().PreviewCardHand(_HeroModel, _HeroModel.Card.Effects[Effect.HerosSpecialPower], WhereAmI.SpecialPower);
		} 
	}

}
