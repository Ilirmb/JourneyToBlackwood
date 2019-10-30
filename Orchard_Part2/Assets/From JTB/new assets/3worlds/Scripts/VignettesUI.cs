using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VignettesUI : MonoBehaviour {
	private Dropdown switcher;
	[SerializeField]
	private Image image;

	[SerializeField]
	private List<Sprite> options;

	void Start () {
		switcher = GetComponent<Dropdown> ();
		switcher.captionText.text = "Vingette Type";
	}
	public void OnChange () {
		var index = switcher.value;
		var sprite = options[switcher.value];
		var isNotNull = (bool)(sprite != null);
		image.gameObject.SetActive(isNotNull);
		if (isNotNull) {
			image.sprite = sprite;
		}
	}
}