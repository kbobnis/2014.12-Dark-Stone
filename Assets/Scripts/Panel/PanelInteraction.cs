using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PanelInteraction : MonoBehaviour {

	public GameObject Text;

	private Mode Mode;
	public Card Card;
	public PanelTile WhoIsCasting;
	public PanelTile CastingFrom;
	public Side CastingSide;

	void Awake() {
		UpdateImage();
	}

	private void UpdateImage() {
		GetComponent<Image>().enabled = false;
		Text.SetActive(false);

		if (Mode == global::Mode.SpellPositioning || Mode == Mode.SpellDirectioning) {
			GetComponent<Image>().enabled = true;
			Text.SetActive(true);
			if (Mode == Mode.SpellPositioning) {
				Text.GetComponent<Text>().text = "Position: " + Card.Name;
			} else if (Mode == Mode.SpellDirectioning){
				Text.GetComponent<Text>().text = "Direct: " + Card.Name;
			}
		}
	}

	internal void Prepare(Mode mode, Card c, PanelTile whoIsCasting, PanelTile castingFrom, Side castingSide) {
		WhoIsCasting = whoIsCasting;
		Mode = mode;
		Card = c;
		CastingFrom = castingFrom;
		CastingSide = castingSide;
		UpdateImage();
	}

	internal void DisableWhat(Mode disableWhat) {
		if (Mode == disableWhat || disableWhat == Mode.All) {
			Prepare(Mode.Ready, null, null, null, Side.None);
		}
	}

	internal bool IsInMode(global::Mode mode) {
		return Mode == mode;
	}
}
