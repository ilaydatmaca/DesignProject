using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Board : MonoBehaviour {

	public int width;
	public int height;

	public int borderSize;

	public GameObject tilePrefab;
	public GameObject[] gamePiecePrefabs;

	public float swapTime = 0.5f;

	Tile[,] m_allTiles;
	GamePiece[,] m_allGamePieces;

	Tile m_clickedTile;
	Tile m_targetTile;

	void Start () 
	{
		m_allTiles = new Tile[width,height];
		m_allGamePieces = new GamePiece[width,height];

		SetupTiles();
		SetupCamera();
		FillRandom();
		HighlightMatches();
	}
	
	void SetupTiles()
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				GameObject tile = Instantiate (tilePrefab, new Vector3(i, j, 0), Quaternion.identity) as GameObject;

				tile.name = "Tile (" + i + "," + j + ")";

				m_allTiles[i,j] = tile.GetComponent<Tile>();

				tile.transform.parent = transform;

				m_allTiles[i,j].Init(i,j,this);

			}
		}
	}

	void SetupCamera()
	{
		Camera.main.transform.position = new Vector3((float)(width - 1)/2f, (float) (height-1) /2f, -10f);

		float aspectRatio = (float) Screen.width / (float) Screen.height;

		float verticalSize = (float) height / 2f + (float) borderSize;

		float horizontalSize = ((float) width / 2f + (float) borderSize ) / aspectRatio;

		Camera.main.orthographicSize = (verticalSize > horizontalSize) ? verticalSize: horizontalSize;

	}
		
	GameObject GetRandomGamePiece()
	{
		int randomIdx = Random.Range(0, gamePiecePrefabs.Length);

		if (gamePiecePrefabs[randomIdx] == null)
		{
			Debug.LogWarning("BOARD:  " + randomIdx + "does not contain a valid GamePiece prefab!");
		}

		return gamePiecePrefabs[randomIdx];
	}

	public void PlaceGamePiece(GamePiece gamePiece, int x, int y)
	{
		if (gamePiece == null)
		{
			Debug.LogWarning("BOARD:  Invalid GamePiece!");
			return;
		}

		gamePiece.transform.position = new Vector3(x, y, 0);
		gamePiece.transform.rotation = Quaternion.identity;

		if (IsWithinBounds(x,y))
		{
			m_allGamePieces[x,y] = gamePiece;
		}

		gamePiece.SetCoord(x,y);
	}

	bool IsWithinBounds(int x, int y)
	{
		return (x >= 0 && x < width && y>= 0 && y<height);
	}

	void FillRandom()
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				GameObject randomPiece = Instantiate(GetRandomGamePiece(), Vector3.zero, Quaternion.identity) as GameObject;

				if (randomPiece !=null)
				{
					randomPiece.GetComponent<GamePiece>().Init(this);
					PlaceGamePiece(randomPiece.GetComponent<GamePiece>(), i, j);
					randomPiece.transform.parent = transform;

				}



			}
		}
	}
		
	public void ClickTile(Tile tile)
	{
		if (m_clickedTile == null)
		{
			m_clickedTile = tile;
			//Debug.Log("clicked tile: " + tile.name);
		}
	}

	public void DragToTile(Tile tile)
	{
		if (m_clickedTile !=null && IsNextTo(tile,m_clickedTile))
		{
			m_targetTile = tile;
		}
	}

	public void ReleaseTile()
	{
		if (m_clickedTile !=null && m_targetTile !=null)
		{
			SwitchTiles(m_clickedTile, m_targetTile);
		}

		m_clickedTile = null;
		m_targetTile = null;
	}
		
	void SwitchTiles(Tile clickedTile, Tile targetTile)
	{
		StartCoroutine(SwitchTilesRoutine(clickedTile, targetTile));
	}

	IEnumerator SwitchTilesRoutine(Tile clickedTile, Tile targetTile)
	{
		GamePiece clickedPiece = m_allGamePieces[clickedTile.xIndex,clickedTile.yIndex];
		GamePiece targetPiece = m_allGamePieces[targetTile.xIndex,targetTile.yIndex];

		if (targetPiece !=null && clickedPiece !=null)
		{
			clickedPiece.Move(targetTile.xIndex, targetTile.yIndex, swapTime);
			targetPiece.Move(clickedTile.xIndex, clickedTile.yIndex, swapTime);

			yield return new WaitForSeconds(swapTime);

			List<GamePiece> clickedPieceMatches = FindMatchesAt(clickedTile.xIndex, clickedTile.yIndex);
			List<GamePiece> targetPieceMatches = FindMatchesAt(targetTile.xIndex, targetTile.yIndex);

			if (targetPieceMatches.Count == 0 && clickedPieceMatches.Count == 0)
			{
				clickedPiece.Move(clickedTile.xIndex, clickedTile.yIndex,swapTime);
				targetPiece.Move(targetTile.xIndex, targetTile.yIndex,swapTime);
			}

			yield return new WaitForSeconds(swapTime);

			HighlightMatchesAt(clickedTile.xIndex,clickedTile.yIndex);
			HighlightMatchesAt(targetTile.xIndex,targetTile.yIndex);
		}


	}
		
	bool IsNextTo(Tile start, Tile end)
	{
		if (Mathf.Abs(start.xIndex - end.xIndex) == 1 && start.yIndex == end.yIndex)
		{
			return true;
		}

		if (Mathf.Abs(start.yIndex - end.yIndex) == 1 && start.xIndex == end.xIndex)
		{
			return true;
		}

		return false;
	}

	// general search method; specify a starting coordinate (startX, startY) and use a Vector2 for direction
	// i.e. (1,0) = right, (-1,0) = left, (0,1) = up, (0,-1) = down; minLength is minimum number to be considered
	// a match

	List<GamePiece> FindMatches(int startX, int startY, Vector2 searchDirection, int minLength = 3)
	{
		List<GamePiece> matches = new List<GamePiece>();

		GamePiece startPiece = null;

		if (IsWithinBounds(startX, startY))
		{
			startPiece = m_allGamePieces[startX, startY];
		}

		if (startPiece !=null)
		{
			matches.Add(startPiece);
		}

		else
		{
			return null;
		}

		int nextX;
		int nextY;

		int maxValue = (width > height) ? width: height;

		for (int i = 1; i < maxValue - 1; i++)
		{
			nextX = startX + (int) Mathf.Clamp(searchDirection.x,-1,1) * i;
			nextY = startY + (int) Mathf.Clamp(searchDirection.y,-1,1) * i;

			if (!IsWithinBounds(nextX, nextY))
			{
				break;
			}

			GamePiece nextPiece = m_allGamePieces[nextX, nextY];

			if (nextPiece.matchValue == startPiece.matchValue && !matches.Contains(nextPiece))
			{
				matches.Add(nextPiece);
			}

			else
			{
				break;
			}
		}

		if (matches.Count >= minLength)
		{
			return matches;
		}
			
		return null;

	}

	List<GamePiece> FindVerticalMatches(int startX, int startY, int minLength = 3)
	{
		List<GamePiece> upwardMatches = FindMatches(startX, startY, new Vector2(0,1), 2);
		List<GamePiece> downwardMatches = FindMatches(startX, startY, new Vector2(0,-1), 2);

		if (upwardMatches == null)
		{
			upwardMatches = new List<GamePiece>();
		}

		if (downwardMatches == null)
		{
			downwardMatches = new List<GamePiece>();
		}

		var combinedMatches = upwardMatches.Union(downwardMatches).ToList();

		return (combinedMatches.Count >= minLength) ? combinedMatches : null;

	}

	List<GamePiece> FindHorizontalMatches(int startX, int startY, int minLength = 3)
	{
		List<GamePiece> rightMatches = FindMatches(startX, startY, new Vector2(1,0), 2);
		List<GamePiece> leftMatches = FindMatches(startX, startY, new Vector2(-1,0), 2);

		if (rightMatches == null)
		{
			rightMatches = new List<GamePiece>();
		}

		if (leftMatches == null)
		{
			leftMatches = new List<GamePiece>();
		}

		var combinedMatches = rightMatches.Union(leftMatches).ToList();

		return (combinedMatches.Count >= minLength) ? combinedMatches : null;

	}

	List<GamePiece> FindMatchesAt (int x, int y, int minLength = 3)
	{
		List<GamePiece> horizMatches = FindHorizontalMatches (x, y, minLength);
		List<GamePiece> vertMatches = FindVerticalMatches (x, y, minLength);

		if (horizMatches == null) 
		{
			horizMatches = new List<GamePiece> ();
		}

		if (vertMatches == null) 
		{
			vertMatches = new List<GamePiece> ();
		}
		var combinedMatches = horizMatches.Union (vertMatches).ToList ();
		return combinedMatches;
	}
		
	void HighlightTileOff(int x, int y)
	{
		SpriteRenderer spriteRenderer = m_allTiles[x,y].GetComponent<SpriteRenderer>();
		spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
	}

	void HighlightTileOn(int x, int y, Color col)
	{
		SpriteRenderer spriteRenderer = m_allTiles[x,y].GetComponent<SpriteRenderer>();
		spriteRenderer.color = col;
	}

	void HighlightMatchesAt (int x, int y)
	{
		HighlightTileOff (x, y);
		var combinedMatches = FindMatchesAt (x, y);
		if (combinedMatches.Count > 0) {
			foreach (GamePiece piece in combinedMatches) {
				HighlightTileOn (piece.xIndex, piece.yIndex, piece.GetComponent<SpriteRenderer> ().color);
			}
		}
	}

	void HighlightMatches()
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				HighlightMatchesAt (i,j);

			}
		}
	}

	void ClearPieceAt(int x, int y)
	{
		
	}


}
