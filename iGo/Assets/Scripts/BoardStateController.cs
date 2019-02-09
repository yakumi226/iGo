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
	private Queue<SearchPointStruct> [] m_destroyStonesQueue;
    private Queue<SearchPointStruct>[] m_emptySpaceQueue;

    // Queue and Stack for Game Record
    private Queue<SearchPointStruct> m_recordQueue;
    private Stack<SearchPointStruct> m_recordStack;
	
	private bool [] m_emptySpaceExsisted;

    // previous one destroyed flag
    private bool m_previousDestroyed;

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

        m_previousDestroyed = false;

		// Create Queue
		m_searchQueue = new Queue<SearchPointStruct> ();
		m_destroyStonesQueue = new Queue<SearchPointStruct> [4] ;
		m_emptySpaceQueue = new Queue<SearchPointStruct> [4];
        m_recordQueue = new Queue<SearchPointStruct>();
        m_recordStack = new Stack<SearchPointStruct>();

		for(i=0;i<4;i++){
			m_destroyStonesQueue[i] = new Queue<SearchPointStruct> ();
			m_emptySpaceQueue[i] = new Queue<SearchPointStruct> ();
		}
		
		// Is EmptySpace Exist Flag Clear 
		m_emptySpaceExsisted = new bool[4] {false,false,false,false};

		m_searchQueue.Clear ();
		m_searchQueue.TrimExcess ();
        m_recordQueue.Clear ();
        m_recordQueue.TrimExcess();
        m_recordStack.Clear();
        m_recordStack.TrimExcess();

		for(i=0;i<4;i++){
			m_destroyStonesQueue[i].Clear ();
			m_destroyStonesQueue[i].TrimExcess ();
			m_emptySpaceQueue[i].Clear ();
			m_emptySpaceQueue[i].TrimExcess ();
		}

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

        m_previousDestroyed = false;

        // Clear Queue
        m_searchQueue.Clear ();
		m_searchQueue.TrimExcess ();
        m_recordQueue.Clear();
        m_recordQueue.TrimExcess();
        m_recordStack.Clear();
        m_recordStack.TrimExcess();

        for (i=0;i<4;i++){
			m_destroyStonesQueue[i].Clear ();
			m_destroyStonesQueue[i].TrimExcess ();
			m_emptySpaceQueue[i].Clear ();
			m_emptySpaceQueue[i].TrimExcess ();
		}

		for(i=0;i<4;i++){
			m_emptySpaceExsisted[i] = false;
		}

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

        // Set stone point for Record
        SearchPointStruct setPoint;
        SearchPointStruct beforeSetPoint;

        // Check for "Kou";
        SearchPointStruct destroyPoint;

        int i;

        // Return Value 
        bool ret = true;

        // check "KOU" 
        bool kouFlag = false;

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

			// Set empty state.
			if(BoardState.EMPTY ==  state){
				// Set State
				m_boardArray[x,y].status = state;
			}
			// Set Stones
			else{
				// For check whether it will be destroyed 
				BoardState searchStone;

				//  Decide searching stone
				if( BoardState.BLACK_STONE ==  state ){
					// Search white stones that should be destoyed 
					searchStone = BoardState.WHITE_STONE;
				}
				else{
					// Search black stones that should be destoyed 
					searchStone = BoardState.BLACK_STONE;
				}
				
				// Search Destroy Stones
				if(true == searchDestroyStones(searchStone,state,x,y)){
                    // Check for "KOU"
                    for (i = 0; i < 4; i++)
                    {
                        if (1 == m_destroyStonesQueue[i].Count)
                        {
                            destroyPoint = m_destroyStonesQueue[i].Peek();
                            beforeSetPoint = m_recordStack.Peek();

                            if ((destroyPoint.x == beforeSetPoint.x) &&
                                (destroyPoint.y == beforeSetPoint.y) &&
                                (true == m_previousDestroyed) &&
                                (true == isSuicidePoint(state,x,y)) )
                            {
                                kouFlag = true;
                            }
                        }
                    }
                    if (false == kouFlag)
                    {
                        // Destroy Searched Stones
                        destroySearchedStones();

                        m_previousDestroyed = true;

                        // Can put a stone
                        ret = true;
                    }
                    else
                    {
                        // Cannot put a stone because "KOU"
                        ret = false;
                    }

                }
				else{
					//  Decide searching stone
					searchStone = state;

                    m_previousDestroyed = false;

                    // Check whether putting a stone is possible;
                    if (true == searchDestroyStones(searchStone,state,x,y)){
						// Cannot put a stone
						ret = false;
					}
					else{
						// Can put a stone
						ret = true;
					}
				}
				// Can put a stone
				if(true == ret){
					// Set State
					m_boardArray[x,y].status = state;
					
					//  Set a black stone on GameBoard.
					if( BoardState.BLACK_STONE ==  state ){
						// Create a black stone of object 
						m_boardArray[x,y].stone = (GameObject)Instantiate (m_blackStone,m_boardArray[x,y].point,Quaternion.identity);
					}
					//  Set a white stone on GameBoard.
					else{
						// Create a white stone of object 
						m_boardArray[x,y].stone = (GameObject)Instantiate (m_whiteStone,m_boardArray[x,y].point,Quaternion.identity);
					}
					// Regist Parant Object (GO-Board)
					m_boardArray[x,y].stone.transform.parent = this.transform;
				}
			}
			
		}
        // Make record
        if (true == ret)
        {
            setPoint.x = x;
            setPoint.y = y;
            m_recordQueue.Enqueue(setPoint);
            m_recordStack.Push(setPoint);
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

    // Search Process for "KOU" Suicide point
    private bool isSuicidePoint(BoardState putStone,int x,int y)
    {
        int i;

        bool ret = false;
        BoardState searchStone;
        BoardState [] tempState = new BoardState[4];

        //  Decide searching stone
        if (BoardState.BLACK_STONE == putStone)
        {
            // Search white stones that should be destoyed 
            searchStone = BoardState.WHITE_STONE;
        }
        else
        {
            // Search black stones that should be destoyed 
            searchStone = BoardState.BLACK_STONE;
        }

        tempState[0] = getBoardState(x - 1, y);
        tempState[1] = getBoardState(x, y + 1);
        tempState[2] = getBoardState(x + 1, y);
        tempState[3] = getBoardState(x, y - 1);

        for (i = 0; i < 4; i++)
        {
            if((searchStone == tempState[i]) ||
               (BoardState.WALL == tempState[i]) )
            {
                ret = true;
            }
            else
            {
                ret = false;
                break;
            }
        }


        return ret;
    }

    // Search Process for Destroy Stone
    private bool searchDestroyStones(BoardState searchStone,BoardState putStone,int x, int y){
		int i,j;
		BoardState tempState;
		SearchPointStruct searchPoint;
		SearchPointStruct searchPointNext;

		bool ret = false;

		// Paramter Range Check
		if( (1 > x) || ( 19 < x ) || ( 1 > y ) || ( 19 < y ) ){
			ret = false;
		}
		else if( (BoardState.BLACK_STONE != searchStone) && ( BoardState.WHITE_STONE != searchStone ) ){
			ret = false;
		}
		else{
			// Clear SearchQueue
			m_searchQueue.Clear ();
			m_searchQueue.TrimExcess ();

			for(i=0;i<4;i++){
				m_destroyStonesQueue[i].Clear ();
				m_destroyStonesQueue[i].TrimExcess ();
				m_emptySpaceQueue[i].Clear ();
				m_emptySpaceQueue[i].TrimExcess ();
			}

			// Clear Search Flag and Count
			for(i=0;i<4;i++){
				m_emptySpaceExsisted[i] = false;
			}

			// Searched Flag On
			m_boardArray[x,y].isSearched = true;

			for(i=0;i<4;i++){
				switch(i){
				case 0:
					/// ---Search to the left direction ---
					searchPointNext.x = x -1;
					searchPointNext.y = y;
					break;
				case 1:
					/// ---Search to the Upside direction ---
					searchPointNext.x = x;
					searchPointNext.y = y-1;
					break;
				case 2:
					/// ---Search to the right direction ---
					searchPointNext.x = x+1;
					searchPointNext.y = y;
					break;
				case 3:
					/// ---Search to the Downside direction ---
					searchPointNext.x = x;
					searchPointNext.y = y+1;
					break;
				default:
					searchPointNext.x = x;
					searchPointNext.y = y;
					break;
				}
				if(false == m_boardArray[searchPointNext.x,searchPointNext.y].isSearched){
					tempState = m_boardArray[searchPointNext.x,searchPointNext.y].status;
					
					if(searchStone == tempState){
						m_searchQueue.Enqueue (searchPointNext);
						m_destroyStonesQueue[i].Enqueue (searchPointNext);

					}
					else if(BoardState.EMPTY == tempState){
						m_emptySpaceExsisted[i] = true;
						m_emptySpaceQueue[i].Enqueue (searchPointNext);
					}
					else{
						// do nothing
					}
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
								searchPointNext.x = searchPoint.x;
								searchPointNext.y = searchPoint.y;
								break;
							}
							if(false == m_boardArray[searchPointNext.x,searchPointNext.y].isSearched){
								tempState = m_boardArray[searchPointNext.x,searchPointNext.y].status;
								if(BoardState.EMPTY == tempState){
									m_emptySpaceExsisted[i] = true;
									m_emptySpaceQueue[i].Enqueue (searchPointNext);
								}
								else if(searchStone == tempState){
									m_searchQueue.Enqueue (searchPointNext);
									m_destroyStonesQueue[i].Enqueue (searchPointNext);
								}
								else{
									// do nothing
								}
							}
						}
					}
					// Searched Flag On
					m_boardArray[searchPoint.x,searchPoint.y].isSearched = true;
				}
			}
			// Clear Searched Flag
			for(i=0;i<21;++i){
				for(j=0;j<21;++j){
					m_boardArray[i,j].isSearched = false;
				}
			}
			// Checking Destroy Stones are exist.
			if(searchStone != putStone){
				// <<< When it is different in serchStone and putStone >>>
				// If there are stones that should be destroyed in any direction(4-direction), Return that it is exist. 
				// true = destroyStone is exist, false = destroyStone is not exist.
				for(i=0;i<4;i++){
					if( (false == m_emptySpaceExsisted[i]) && (0 < m_destroyStonesQueue[i].Count) ){
						ret = true;
					}
				}
			}
			// Checking whether Putting Stone is possible.
			else{
				if( (false == m_emptySpaceExsisted[0] )
				 && (false == m_emptySpaceExsisted[1] )
				 && (false == m_emptySpaceExsisted[2] ) 
				 && (false == m_emptySpaceExsisted[3] ) )
				{
					// <<< When searchStone is the same as putStone >>>
					// When there is no empty space in all-direction, Return that player cannot put stone.
					// true = cannot , false = can
					ret = true;
				}
			}
		}
			
		return ret;
	}
	private bool destroySearchedStones(){
		int i;
		bool ret = false;
		BoardState tempState = BoardState.EMPTY;

		for(i=0;i<4;i++){
			if( false == m_emptySpaceExsisted[i]){
				foreach (SearchPointStruct tempPoint in m_destroyStonesQueue[i])
				{
					tempState = m_boardArray[tempPoint.x,tempPoint.y].status;
					
					if(BoardState.EMPTY != tempState){
						m_boardArray[tempPoint.x,tempPoint.y].status = BoardState.EMPTY;
						Destroy(m_boardArray[tempPoint.x,tempPoint.y].stone);
					}
				}
				ret = true;
			}
		}
		return ret;
	}
}
