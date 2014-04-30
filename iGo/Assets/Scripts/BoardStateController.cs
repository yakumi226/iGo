using UnityEngine;
using System.Collections.Generic;
using GameStateDef;

public class BoardStateController : MonoBehaviour {

	// Regist Stones Prefabs
	public GameObject m_blackStone;
	public GameObject m_whiteStone;

	// Array of Game Board 
	private boardStruct [,] m_boardArray;

	// Board State Queue for Searching
	private Queue<SearchPointStruct> m_searchQueue;
	private Queue<SearchPointStruct> m_leftSideQueue;
	private Queue<SearchPointStruct> m_rightSideQueue;
	private Queue<SearchPointStruct> m_upSideQueue;
	private Queue<SearchPointStruct> m_downSideQueue;
	// Use this for initialization
	void Awake () {
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	// IF::Initialize GameBoard
	public void initialize()
	{
		int i,j;

		// Create Array
		m_boardArray = new boardStruct[21,21];

		// Create Queue
		m_searchQueue = new Queue<SearchPointStruct>();
		m_leftSideQueue = new Queue<SearchPointStruct>();
		m_rightSideQueue = new Queue<SearchPointStruct>();
		m_upSideQueue = new Queue<SearchPointStruct>();
		m_downSideQueue = new Queue<SearchPointStruct>();

		m_searchQueue.Clear ();
		m_searchQueue.TrimExcess();
		m_leftSideQueue.Clear ();
		m_leftSideQueue.TrimExcess();
		m_rightSideQueue.Clear ();
		m_rightSideQueue.TrimExcess();
		m_upSideQueue.Clear ();
		m_upSideQueue.TrimExcess();
		m_downSideQueue.Clear ();
		m_downSideQueue.TrimExcess();


		// Make wall and Fill Other are with Empty Space.
		for(i=0;i<21;++i){
			for(j=0;j<21;++j){
				m_boardArray[i,j].point.x = 0.75f * (i-10);
				m_boardArray[i,j].point.y = -0.75f * (j-10);
				m_boardArray[i,j].point.z = 0;
				m_boardArray[i,j].stone = null;
				if( (0 == i) || (0 == j) || (20 == i) || (20 == j) ){
					m_boardArray[i,j].status = BoardState.WALL;
				}
				else{
					m_boardArray[i,j].status = BoardState.EMPTY;
				}
				m_boardArray[i,j].isSearched = false;
			}
		}

		return;
	}

	// IF::Clear Board
	public void clearBoard(){
		int i,j;

		// Clear Queue
		m_searchQueue.Clear ();
		m_searchQueue.TrimExcess();
		m_leftSideQueue.Clear ();
		m_leftSideQueue.TrimExcess();
		m_rightSideQueue.Clear ();
		m_rightSideQueue.TrimExcess();
		m_upSideQueue.Clear ();
		m_upSideQueue.TrimExcess();
		m_downSideQueue.Clear ();
		m_downSideQueue.TrimExcess();


		// Make wall and Fill Other are with Empty Space.
		for(i=0;i<21;++i){
			for(j=0;j<21;++j){
				m_boardArray[i,j].point.z = 0;
				m_boardArray[i,j].stone = null;
				m_boardArray[i,j].isSearched = false;
				if( (0 == i) || (0 == j) || (20 == i) || (20 == j) ){
					// do nothing
				}
				else{
					if(BoardState.EMPTY != m_boardArray[i,j].status){
						Destroy(m_boardArray[i,j].stone);
					}
					m_boardArray[i,j].status = BoardState.EMPTY;
				}
			}
		}
	}

	// IF::Set state to Gameboard 
	public bool setBoard( BoardState state,int x, int y){
		// Return Value 
		bool ret = true;

		// Paramter Range Check
		if( BoardState.WALL == state ){
			ret = false;
		}
		else if( (1 > x) || ( 19 < x ) || ( 1 > y ) || ( 19 < y) ){
			ret = false;
		}
		else{
			// is Object exist?
			if(null != m_boardArray[x,y].stone){
				// If object is already exist, object destroy
				Destroy(m_boardArray[x,y].stone.gameObject);
				m_boardArray[x,y].stone = null;
			}
			// Set State
			m_boardArray[x,y].status = state;

			//  Set a black stone on GameBoard.
			if( BoardState.BLACK_STONE ==  m_boardArray[x,y].status ){
				m_boardArray[x,y].stone = (GameObject)Instantiate (m_blackStone,m_boardArray[x,y].point,Quaternion.identity);

				m_boardArray[x,y].stone.transform.parent = this.transform;

				searchDestroyStone(m_boardArray[x,y].status,x,y);
			}
			//  Set a white stone on GameBoard.
			else if ( BoardState.WHITE_STONE == m_boardArray[x,y].status ) {
				m_boardArray[x,y].stone = (GameObject)Instantiate (m_whiteStone,m_boardArray[x,y].point,Quaternion.identity);

				m_boardArray[x,y].stone.transform.parent = this.transform;
			
				searchDestroyStone(m_boardArray[x,y].status,x,y);
			}
			//  Set empty state.
			else{
			}
		}

		return ret;
	}

	// IF:Get state from Gameboard
	public BoardState getBoardState( int x, int y){
		// Return Value
		BoardState ret = BoardState.ERROR;

		// Paramter Range Check
		if( (0 > x) || ( 20 < x ) || ( 0 > y ) || ( 20 < y) ){
			ret = BoardState.ERROR;
		}
		else{
			ret = m_boardArray[x,y].status;
		}
		return ret;
	}

	// Search Process for Destroy Stone
	private bool searchDestroyStone(BoardState state,int x, int y){
		int i,j;
		BoardState searchStone;
		BoardState tempState;
		SearchPointStruct searchPoint;
		SearchPointStruct searchPointNext;

		bool ret = true;
		bool [] emptySpaceExsisted = new bool[4] {false,false,false,false};

		// Paramter Range Check
		if( (1 > x) || ( 19 < x ) || ( 1 > y ) || ( 19 < y ) ){
			ret = false;
		}
		else if( (BoardState.BLACK_STONE != state) && ( BoardState.WHITE_STONE != state ) ){
			ret = false;
		}
		else{

			m_leftSideQueue.Clear ();
			m_leftSideQueue.TrimExcess();
			m_rightSideQueue.Clear ();
			m_rightSideQueue.TrimExcess();
			m_upSideQueue.Clear ();
			m_upSideQueue.TrimExcess();
			m_downSideQueue.Clear ();
			m_downSideQueue.TrimExcess();

			// Decide Search Stones Color
			if(BoardState.BLACK_STONE == state){
				searchStone = BoardState.WHITE_STONE;
			}
			else{
				searchStone = BoardState.BLACK_STONE;
			}

			// Searched Flag On
			m_boardArray[x,y].isSearched = true;

			for(i=0;i<4;i++){
				switch(i){
				case 0:
					/// ---Search to the left direction ---
					searchPointNext.x = x -1;
					searchPointNext.y = y;

					if(false == m_boardArray[searchPointNext.x,searchPointNext.y].isSearched){
						tempState = m_boardArray[searchPointNext.x,searchPointNext.y].status;
						
						if(searchStone == tempState){
							m_searchQueue.Enqueue (searchPointNext);
							m_leftSideQueue.Enqueue (searchPointNext);
						}
						else if(BoardState.EMPTY == tempState){
							emptySpaceExsisted[i] = true;
						}
						else{
							// do nothing
						}
					}
					break;
				case 1:
					/// ---Search to the Upside direction ---
					searchPointNext.x = x;
					searchPointNext.y = y-1;

					if(false == m_boardArray[searchPointNext.x,searchPointNext.y].isSearched){
						tempState = m_boardArray[searchPointNext.x,searchPointNext.y].status;
						
						if(searchStone == tempState){
							m_searchQueue.Enqueue (searchPointNext);
							m_upSideQueue.Enqueue (searchPointNext);
						}
						else if(BoardState.EMPTY == tempState){
							emptySpaceExsisted[i] = true;
						}
						else{
							// do nothing
						}
					}
					break;
				case 2:
					/// ---Search to the right direction ---
					searchPointNext.x = x+1;
					searchPointNext.y = y;

					if(false == m_boardArray[searchPointNext.x,searchPointNext.y].isSearched){
						tempState = m_boardArray[searchPointNext.x,searchPointNext.y].status;
						
						if(searchStone == tempState){
							m_searchQueue.Enqueue (searchPointNext);
							m_rightSideQueue.Enqueue (searchPointNext);
						}
						else if(BoardState.EMPTY == tempState){
							emptySpaceExsisted[i] = true;
						}
						else{
							// do nothing
						}
					}
					break;
				case 3:
					/// ---Search to the Downside direction ---
					searchPointNext.x = x;
					searchPointNext.y = y+1;

					if(false == m_boardArray[searchPointNext.x,searchPointNext.y].isSearched){
						tempState = m_boardArray[searchPointNext.x,searchPointNext.y].status;

						if(searchStone == tempState){
							m_searchQueue.Enqueue (searchPointNext);
							m_downSideQueue.Enqueue (searchPointNext);
						}
						else if(BoardState.EMPTY == tempState){
							emptySpaceExsisted[i] = true;
						}
						else{
							// do nothing
						}
					}
					break;
				default:
					searchPointNext.x = x;
					searchPointNext.y = y;
					break;
				}

				while(0 < m_searchQueue.Count){
					searchPoint = m_searchQueue.Dequeue ();

					// Checking Flag for Already Searched
					if(false == m_boardArray[searchPoint.x,searchPoint.y].isSearched){

						for(j=0;j<4;j++){
							switch(j){
							case 0:
								/// ---Search to the left direction ---
								searchPointNext.x = searchPoint.x -1;
								searchPointNext.y = searchPoint.y;
								break;
							case 1:
								/// ---Search to the Upside direction ---
								searchPointNext.x = searchPoint.x;
								searchPointNext.y = searchPoint.y-1;
								break;
							case 2:
								/// ---Search to the right direction ---
								searchPointNext.x = searchPoint.x+1;
								searchPointNext.y = searchPoint.y;
								break;
							case 3:
								/// ---Search to the Downside direction ---
								searchPointNext.x = searchPoint.x;
								searchPointNext.y = searchPoint.y+1;
								break;
							default:
								break;
							}
							tempState = m_boardArray[searchPointNext.x,searchPointNext.y].status;
								
							if(BoardState.EMPTY == tempState){
								emptySpaceExsisted[i] = true;
							}
							else if(searchStone == tempState){
								if(false == m_boardArray[searchPointNext.x,searchPointNext.y].isSearched){
									m_searchQueue.Enqueue (searchPointNext);
									switch(i){
									case 0:
										m_leftSideQueue.Enqueue (searchPointNext);
										break;
									case 1:
										m_upSideQueue.Enqueue (searchPointNext);
										break;
									case 2:
										m_rightSideQueue.Enqueue (searchPointNext);
										break;
									case 3:
										m_downSideQueue.Enqueue (searchPointNext);
										break;
									default:
										break;
									}
								}
							}
							else{
								// do nothing
							}
						}
					}
					// Searched Flag On
					m_boardArray[searchPoint.x,searchPoint.y].isSearched = true;

				}
			}
		}

		for(i=0;i<21;++i){
			for(j=0;j<21;++j){
				m_boardArray[i,j].isSearched = false;
			}
		}

		//[TEST:: Stone Destroy]
		for(i=0;i<4;i++){
			if( false == emptySpaceExsisted[i]){
				switch(i){
				case 0:
					foreach (SearchPointStruct tempPoint in m_leftSideQueue)
					{
						tempState = m_boardArray[tempPoint.x,tempPoint.y].status;

						if(BoardState.EMPTY != tempState){
							m_boardArray[tempPoint.x,tempPoint.y].status = BoardState.EMPTY;
							Destroy(m_boardArray[tempPoint.x,tempPoint.y].stone);
						}
					}
					break;
				case 1:
					foreach (SearchPointStruct tempPoint in m_upSideQueue)
					{
						tempState = m_boardArray[tempPoint.x,tempPoint.y].status;
						
						if(BoardState.EMPTY != tempState){
							m_boardArray[tempPoint.x,tempPoint.y].status = BoardState.EMPTY;
							Destroy(m_boardArray[tempPoint.x,tempPoint.y].stone);
						}
					}
					break;
				case 2:
					foreach (SearchPointStruct tempPoint in m_rightSideQueue)
					{
						tempState = m_boardArray[tempPoint.x,tempPoint.y].status;
						
						if(BoardState.EMPTY != tempState){
							m_boardArray[tempPoint.x,tempPoint.y].status = BoardState.EMPTY;
							Destroy(m_boardArray[tempPoint.x,tempPoint.y].stone);
						}
					}
					break;
				case 3:
					foreach (SearchPointStruct tempPoint in m_downSideQueue)
					{
						tempState = m_boardArray[tempPoint.x,tempPoint.y].status;
						
						if(BoardState.EMPTY != tempState){
							m_boardArray[tempPoint.x,tempPoint.y].status = BoardState.EMPTY;
							Destroy(m_boardArray[tempPoint.x,tempPoint.y].stone);
						}
					}
					break;
				default:
					break;
				}
			}
		}

		return ret;
	}

}
