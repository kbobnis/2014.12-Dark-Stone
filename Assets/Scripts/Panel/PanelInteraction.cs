using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PanelInteraction : MonoBehaviour {

	public GameObject Text;

	private Mode Mode;
	private Card Card;
	public PanelTile WhoIsCasting;

	void Awake() {
		UpdateImage();
	}

	private void UpdateImage() {
		GetComponent<Image>().enabled = false;
		Text.SetActive(false);

		if (Mode == global::Mode.SpellCasting) {
			GetComponent<Image>().enabled = true;
			Text.SetActive(true);
			Text.GetComponent<Text>().text = "Cast: " + Card.Name;
		}
	}

	internal void Prepare(Mode mode, Card c, PanelTile whoIsCasting) {
		WhoIsCasting = whoIsCasting;
		Mode = mode;
		Card = c;
		UpdateImage();
	}

	internal bool IsInMode(global::Mode mode) {
		return Mode == mode;
	}
}
