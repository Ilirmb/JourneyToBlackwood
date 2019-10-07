using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnderwaterUI : MonoBehaviour {
	private Dropdown waterSwitch;

	[SerializeField]
	private List<GameObject> options;

	void Start () {
		waterSwitch = GetComponent<Dropdown> ();
		waterSwitch.captionText.text = "Water Type";

	}
	public void OnChange () {
		var selectedIndex = waterSwitch.value;
		foreach (var option in options) {
			if (option != null) {
				option.SetActive (false);
			}
		}
		var gameObject = options[waterSwitch.value];
		if (gameObject != null) {
			gameObject.SetActive (true);
		}
	}

}