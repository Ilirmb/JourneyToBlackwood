using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchController : MonoBehaviour {
	[SerializeField]
	private float speed = 10;
	[SerializeField]
	private Toggle Automove;

	public bool blockHorizontal;
    public float maxDistanceX = 160;
    public float minDistanceX = 0;
    public float minDistanceY = -20;
    public float maxDistanceY = 1.6f;

	private Vector2 touchStartPosition;
	private bool ignoreInput = false;
	public bool IgnoreInput {
		get { return ignoreInput; }
		set { ignoreInput = value; }
	}

	void Update () {
		if (ignoreInput || Automove.isOn) return;
		var blockVertical = transform.position.x < minDistanceX || transform.position.x > maxDistanceX;
		var blockHorizontal = transform.position.y < minDistanceY || transform.position.y > maxDistanceY || this.blockHorizontal;
		if (Input.GetKeyUp (KeyCode.Escape)) {
			if (Application.platform == RuntimePlatform.Android) {
				AndroidJavaObject activity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject> ("currentActivity");
				activity.Call<bool> ("moveTaskToBack", true);
			} else {
				Application.Quit ();
			}
		}
		if (Input.touchCount == 0) return;

		var touch = default (Touch);

		foreach (var t in Input.touches) {
			touch = t;
			break;
		}

		switch (touch.phase) {
			case TouchPhase.Began:
				touchStartPosition = touch.position;
				break;
			case TouchPhase.Moved:
				Vector3 swipeVector = touch.position - touchStartPosition;
				if (Mathf.Abs (swipeVector.x) > 0 || Mathf.Abs (swipeVector.y) > 0) {
					Direction direction;
					bool isVertical = Mathf.Abs (swipeVector.x) < Mathf.Abs (swipeVector.y);
					if (isVertical) {
						direction = swipeVector.y > 0 ? Direction.UP : Direction.DOWN;
					} else {
						direction = swipeVector.x > 0 ? Direction.RIGHT : Direction.LEFT;
					}
					Vector3 directionVector = Vector3.zero;
					switch (direction) {
						case Direction.UP:
							if(blockHorizontal) break;
							directionVector = Vector2.down;
							break;
						case Direction.DOWN:
							if(blockHorizontal) break;
							directionVector = Vector2.up;
							break;
						case Direction.LEFT:
							if(blockVertical) break;
							directionVector = Vector2.right;
							break;
						case Direction.RIGHT:
							if(blockVertical) break;
							directionVector = Vector2.left;
							break;
					}
					transform.position += directionVector * speed * Time.deltaTime;
					touchStartPosition = touch.position;
				}
				break;
		}
	}

	private enum Direction {
		UP,
		DOWN,
		LEFT,
		RIGHT
	}
}