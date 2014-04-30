using UnityEngine;
using System.Collections.Generic;

public class CursorController : MonoBehaviour {

	private Vector3 position;
	private Vector3 screenToWorldPointPosition;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// Vector3でマウス位置座標を取得する
		position = Input.mousePosition;

		position.z = 15f;

		// マウス位置座標をスクリーン座標からワールド座標に変換する
		screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(position);

		// ワールド座標に変換されたマウス座標を代入
		gameObject.transform.position = screenToWorldPointPosition;
	}
}
