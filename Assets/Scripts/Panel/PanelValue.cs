using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelValue : MonoBehaviour {

	private int ActualValue;
	public GameObject Text;

	void Awake() {
		UpdateImage();
	}


	public void Prepare(int value) {
		ActualValue = value;
		UpdateImage();
	}

	private void UpdateImage() {
		GetComponent<Image>().enabled = false;
		Text.SetActive( false);
		if (ActualValue > 0) {
			GetComponent<Image>().enabled = true;
			Text.SetActive(true);
			Text.GetComponent<Text>().text = "" + ActualValue;
		}
	}
}
