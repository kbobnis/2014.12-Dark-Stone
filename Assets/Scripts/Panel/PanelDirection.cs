using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelDirection : MonoBehaviour {

	public GameObject ImageDirection;

	private Side Side = Side.None;

	void Awake() {
		UpdateImage();
	}

	private void UpdateImage() {
		GetComponent<Image>().enabled = false;
		ImageDirection.SetActive(false);
		if (Side != global::Side.None) {
			GetComponent<Image>().enabled = true;
			ImageDirection.SetActive(true);
			Quaternion old = ImageDirection.GetComponent<RectTransform>().rotation;
			ImageDirection.GetComponent<RectTransform>().Rotate(0, 0, Side.ToRotation());
		}
	}

	internal void Prepare(Side side) {
		Side = side;
		UpdateImage();
	}
}
