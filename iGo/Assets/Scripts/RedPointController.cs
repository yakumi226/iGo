using UnityEngine;
using System.Collections.Generic;

public class RedPointController : MonoBehaviour {

	private Vector3 position;
	private Vector3 screenToWorldPointPosition;
	private Vector3 redPoint_pos;

	// Use this for initialization
	void Start () {
		redPoint_pos.z = 0;
	}
	
	// Update is called once per frame
	void Update () {
		// Vector3でマウス位置座標を取得する
		position = Input.mousePosition;
		
		position.z = 15f;
		
		// マウス位置座標をスクリーン座標からワールド座標に変換する
		screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(position);

		redPoint_pos.x = 0.75f * Mathf.RoundToInt(( screenToWorldPointPosition.x / 0.75f ));
		redPoint_pos.y = 0.75f * Mathf.RoundToInt(( screenToWorldPointPosition.y / 0.75f ));

		// ワールド座標に変換されたマウス座標を代入
		gameObject.transform.position = redPoint_pos;
	}
}
