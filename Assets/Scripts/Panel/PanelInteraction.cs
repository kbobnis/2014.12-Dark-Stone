using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PanelInteraction : MonoBehaviour {

	public GameObject Text;

	void Awake() {
		UpdateImage();
	}

	private void UpdateImage() {
		Color c = GetComponent<Image>().color;
		GetComponent<Image>().color = new Color(c.r, c.g, c.b, 0);
		Text.SetActive(false);
	}

	
}
