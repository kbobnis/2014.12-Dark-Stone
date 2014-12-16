using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelDirection : MonoBehaviour {

	public GameObject ImageDirection, ImageMovesLeft;

	public Side Side = Side.None;
	private int _MovesLeft;

	public int MovesLeft {
		get { return _MovesLeft; }
		set { _MovesLeft = value; UpdateImage();  }
	}

	void Awake() {
		UpdateImage();
	}

	private void UpdateImage() {
		GetComponent<Image>().enabled = false;
		ImageDirection.SetActive(false);
		ImageMovesLeft.SetActive(false);
		if (Side != global::Side.None) {
			GetComponent<Image>().enabled = true;
			ImageDirection.SetActive(true);
			Quaternion old = ImageDirection.GetComponent<RectTransform>().rotation;
			ImageDirection.GetComponent<RectTransform>().rotation = new Quaternion();
			ImageDirection.GetComponent<RectTransform>().Rotate(0, 0, Side.ToRotation());

			if (_MovesLeft > 0) {
				ImageMovesLeft.SetActive(true);
				ImageMovesLeft.GetComponent<Text>().text = "" + _MovesLeft;
			}
		}
	}

	internal void Prepare(Side side) {
		Side = side;
		UpdateImage();
	}

}
