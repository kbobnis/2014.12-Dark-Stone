using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelValue : MonoBehaviour {

	public GameObject TextValue;

	private int _Value;

	public int Value {
		set {
			_Value = value;
			TextValue.GetComponent<Text>().text = "" + value;
		}
		get { return _Value; }
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
