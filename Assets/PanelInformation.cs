using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PanelInformation : MonoBehaviour {

	public GameObject Text;

	public void Awake() {
		DisableMe();
	}

	public void SetText(string text) {
		GetComponent<Image>().enabled = true;
		Text.SetActive(true);
		Text.GetComponent<Text>().text = text;
	}

	private void DisableMe() {
		GetComponent<Image>().enabled = false;
		Text.SetActive(false);
	}


	public void Disable(BaseEventData bed) {
		DisableMe();
	}
}
