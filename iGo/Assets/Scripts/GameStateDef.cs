//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.18444
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;


namespace GameStateDef
{
	public enum BoardState
	{
		EMPTY = 0,			// There is no stone on GameBoard.
		WALL = 1,			// There is GameBoard's Wall.
		BLACK_STONE = 2,	// There is a black stone.
		WHITE_STONE = 3,	// There is a white stone.
		ERROR = 4			// Error Status
	}
	public struct boardStruct{
		public Vector3 point;			// Point on GameBoard
		public BoardState status;		// Board Status
		public GameObject stone;		// Storage Stone Object Clone
		public bool isSearched;			// Searched Flag
	};

	public struct SearchPointStruct{
		public int x;					// Point-x of Array
		public int y;					// Point-y of Array
	};
}

