using UnityEngine;
using System.Collections.Generic;
using GameStateDef;

public class GameBoardManager : MonoBehaviour {

	// Mouse Cursor Object Prefab
	public GameObject cursor;

	// Red Point Object Prefab
	public GameObject redPoint;

	// Black Stone Object Prefab
	public GameObject blackStone;

	// White Stone Object Prefab
	public GameObject whiteStone;
	 
	// Mouse Cursor Object Clone
	private GameObject cursorClone;

	// Red Point Object Clone
	private GameObject redPointClone;

	// Black Stone Object Prefab
	private GameObject blackStoneClone;
	
	// White Stone Object Prefab
	private GameObject whiteStoneClone;

	// SE manager
	public AudioClip[] se;

	// Game Board Array Manager	
	private BoardStateController boardController;

	// Cursor Position
	private Vector3 temp_cursor_pos;
	private Vector3 cursor_pos;

	// Put Point Position
	private Vector3 redPoint_pos;

	// Counter
	private int count;

	// Use this for initialization
	void Awake () {
		count = 0;

		// Get BoardStateController Class
		boardController = GetComponent<BoardStateController>();

		// Initialization Game Board Controller Class
		boardController.initialize ();
	}

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown(){
		int x,y,rand;
		// Get Mouse Position
		temp_cursor_pos = Input.mousePosition;
		temp_cursor_pos.z = 15f;
		
		// Translate mousePosition(ScreenPoint) to Worldpoint 
		cursor_pos = Camera.main.ScreenToWorldPoint(temp_cursor_pos);
		
		x = Mathf.RoundToInt( (cursor_pos.x / 0.75f) + 10f);
		y = Mathf.RoundToInt( (cursor_pos.y / (-0.75f) ) + 10f);

		// Player can put stone when there is no stones.  
		if( BoardState.EMPTY == boardController.getBoardState(x,y) ){
			count++;
			if( 0 == (count % 2) ) {
				boardController.setBoard (BoardState.WHITE_STONE,x,y);
			}
			else{
				boardController.setBoard (BoardState.BLACK_STONE,x,y);
			}
			rand = Random.Range (0,2);
			AudioSource.PlayClipAtPoint(se[rand],cursor_pos);
		}
	}


	void OnMouseEnter(){
		// Hide Mouse Cursor 
		Screen.showCursor = false;

		// Get Mouse Position
		temp_cursor_pos = Input.mousePosition;
		temp_cursor_pos.z = 15f;

		// Translate mousePosition(ScreenPoint) to Worldpoint 
		cursor_pos = Camera.main.ScreenToWorldPoint(temp_cursor_pos);
		 
		redPoint_pos.x = 0.75f * Mathf.RoundToInt( cursor_pos.x / 0.75f);
		redPoint_pos.y = (-0.75f) * Mathf.RoundToInt( cursor_pos.y / (-0.75f));
		redPoint_pos.z = 0;

		// Create Cursor Object
		cursorClone =(GameObject)Instantiate(cursor,cursor_pos,Quaternion.identity);

		// Create Red Point Object
		redPointClone =(GameObject)Instantiate(redPoint,redPoint_pos,Quaternion.identity);
	}

	void OnMouseExit(){
		// Show Mouse Cursor 
		Screen.showCursor = true;

		// Destroy Cursor Object
		Destroy(cursorClone);
		cursorClone = null;

		// Destroy RedPoint Object
		Destroy(redPointClone);
		redPointClone = null;
	}
}
