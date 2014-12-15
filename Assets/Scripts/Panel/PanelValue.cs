using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelValue : MonoBehaviour {

	public GameObject TextValue;
	public int ActualValue;

	void Awake() {
		UpdateImage();
	}


	public void Prepare(int value) {
		ActualValue = value;
		UpdateImage();
	}

	private void UpdateImage() {
		GetComponent<Image>().enabled = false;
		TextValue.SetActive(false);
		if (ActualValue > 0) {
			GetComponent<Image>().enabled = true;
			TextValue.SetActive(true);
			TextValue.GetComponent<Text>().text = "" + ActualValue;
		}
	}

	
}
