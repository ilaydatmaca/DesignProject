using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Board : MonoBehaviour {

	public int width;
	public int height;

	public int borderSize;

	public GameObject tileNormalPrefab;
	public GameObject tileObstaclePrefab;
	public GameObject[] gamePiecePrefabs;

	public GameObject adjacentBombPrefab;
	public GameObject rowBombPrefab;
	public GameObject columnBombPrefab;

	public GameObject m_clickedTileBomb;
	public GameObject m_targetTileBomb;

	public float swapTime = 0.5f;

	Tile[,] m_allTiles;
	GamePiece[,] m_allGamePieces;

	Tile m_clickedTile;
	Tile m_targetTile;

	bool m_playerInputEnabled = true;

	public StartingObject[] startingTiles;
	public StartingObject[] startingGamePieces;

	ParticleManager m_particleManager;

	public int fillYOffset = 10;
	public float fillMoveTime = 0.5f;

	[System.Serializable]
	public class StartingObject
	{
		public GameObject prefab;
		public int x;
		public int y;
		public int z;
	}

	void Start () 
	{
		m_allTiles = new Tile[width,height];
		m_allGamePieces = new GamePiece[width,height];

		SetupTiles();
		SetupGamePieces();
		SetupCamera();
		FillBoard(fillYOffset, fillMoveTime);
		m_particleManager = GameObject.FindWithTag("ParticleManager").GetComponent<ParticleManager>();

	}

	void MakeTile (GameObject prefab, int x, int y, int z = 0)
	{
		if (prefab !=null && IsWithinBounds(x,y))
		{
			GameObject tile = Instantiate (prefab, new Vector3 (x, y, z), Quaternion.identity) as GameObject;
			tile.name = "Tile (" + x + "," + y + ")";
			m_allTiles [x, y] = tile.GetComponent<Tile> ();
			tile.transform.parent = transform;
			m_allTiles [x, y].Init (x, y, this);
		}
	}

	void MakeGamePiece ( GameObject prefab,int x, int y, int falseYOffset = 0, float moveTime = 0.1f)
	{
		if (prefab != null && IsWithinBounds(x,y)) 
		{
			prefab.GetComponent<GamePiece> ().Init (this);
			PlaceGamePiece (prefab.GetComponent<GamePiece> (), x, y);

			if (falseYOffset != 0) 
			{
				prefab.transform.position = new Vector3 (x, y + falseYOffset, 0);
				prefab.GetComponent<GamePiece> ().Move (x, y, moveTime);
			}

			prefab.transform.parent = transform;
		}
	}

	GameObject MakeBomb(GameObject prefab, int x, int y)
	{
		if (prefab != null && IsWithinBounds(x, y))
		{
			GameObject bomb = Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
			bomb.GetComponent<Bomb>().Init(this);
			bomb.GetComponent<Bomb>().SetCoord(x, y);
			bomb.transform.parent = this;
			return bomb;
		}

		return null;
	}

	void SetupTiles()
	{
		foreach (StartingObject sTile in startingTiles)
		{
			if (sTile != null)
			{
				MakeTile(sTile.prefab, sTile.x, sTile.y, sTile.z);
			}

		}

		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				if (m_allTiles[i,j] == null)
				{
					MakeTile (tileNormalPrefab, i,j);
				}
			}
		}
	}

	void SetupGamePieces()
	{
		foreach (StartingObject sPiece in startingGamePieces)
		{
			if (sPiece !=null)
			{
				GameObject piece = Instantiate(sPiece.prefab, new Vector3(sPiece.x, sPiece.y, 0), Quaternion.identity) as GameObject;
				MakeGamePiece(piece, sPiece.x, sPiece.y, fillYOffset, fillMoveTime);
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

	GamePiece FillRandomAt (int x, int y, int falseYOffset = 0, float moveTime = 0.1f)
	{
		if (IsWithinBounds(x,y))
		{
			GameObject randomPiece = Instantiate (GetRandomGamePiece (), Vector3.zero, Quaternion.identity) as GameObject;
			MakeGamePiece (randomPiece,x, y, falseYOffset, moveTime);
			return randomPiece.GetComponent<GamePiece>();
		}
		return null;
	}

    void FillBoard(int falseYOffset = 0, float moveTime = 0.1f)
    {
        int maxInterations = 100;
        int iterations = 0;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (m_allGamePieces[i, j] == null && m_allTiles[i, j].tileType != TileType.Obstacle)
                {
                    GamePiece piece = FillRandomAt(i, j, falseYOffset, moveTime);
                    iterations = 0;

                    while (HasMatchOnFill(i, j))
                    {
                        ClearPieceAt(i, j);
                        piece = FillRandomAt(i, j, falseYOffset, moveTime);
                        iterations++;

                        if (iterations >= maxInterations)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }

    bool HasMatchOnFill(int x, int y, int minLength = 3)
    {
        List<GamePiece> leftMatches = FindMatches(x, y, new Vector2(-1, 0), minLength);
        List<GamePiece> downwardMatches = FindMatches(x, y, new Vector2(0, -1), minLength);

        if (leftMatches == null)
        {
            leftMatches = new List<GamePiece>();
        }

        if (downwardMatches == null)
        {
            downwardMatches = new List<GamePiece>();
        }

        return (leftMatches.Count > 0 || downwardMatches.Count > 0);

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
		if (m_playerInputEnabled)
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
				else
				{
					yield return new WaitForSeconds(swapTime);
					Vector2 swipeDirection = new Vector2(targetTile.xIndex - clickedTile.xIndex,
						targetTile.yIndex - targetTile.xIndex);
					m_clickedTileBomb = DropBomb(clickedTile.xIndex, clickedTile.yIndex, swipeDirection,
						clickedPieceMatches);
					m_targetTileBomb = DropBomb(targetTile.xIndex, targetTile.yIndex, swipeDirection,
						targetPieceMatches);
					ClearAndRefillBoard(clickedPieceMatches.Union(targetPieceMatches).ToList());

				}
			}
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

			if (nextPiece == null)
			{
				break;
			}
			else
			{
				if (nextPiece.matchValue == startPiece.matchValue && !matches.Contains(nextPiece))
				{
					matches.Add(nextPiece);
				}

				else
				{
					break;
				}
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

	List<GamePiece> FindMatchesAt (List<GamePiece> gamePieces, int minLength = 3)
	{
		List<GamePiece> matches = new List<GamePiece>();

		foreach (GamePiece piece in gamePieces)
		{
			matches = matches.Union(FindMatchesAt(piece.xIndex, piece.yIndex, minLength)).ToList();
		}
		return matches;

	}

	List<GamePiece> FindAllMatches()
	{
		List<GamePiece> combinedMatches = new List<GamePiece>();

		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				var matches = FindMatchesAt(i,j);
				combinedMatches = combinedMatches.Union(matches).ToList();
			}
		}
		return combinedMatches;
	}

	void HighlightTileOff(int x, int y)
	{
		if (m_allTiles[x,y].tileType != TileType.Breakable)
		{
			SpriteRenderer spriteRenderer = m_allTiles[x,y].GetComponent<SpriteRenderer>();
			spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
		}
	}

	void HighlightTileOn(int x, int y, Color col)
	{
		if (m_allTiles[x,y].tileType != TileType.Breakable)
		{
			SpriteRenderer spriteRenderer = m_allTiles[x,y].GetComponent<SpriteRenderer>();
			spriteRenderer.color = col;
		}
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

	void HighlightPieces(List<GamePiece> gamePieces)
	{
		foreach (GamePiece piece in gamePieces)
		{
			if (piece !=null)
			{
				HighlightTileOn(piece.xIndex, piece.yIndex, piece.GetComponent<SpriteRenderer>().color);
			}
		}
	}

	void ClearPieceAt(int x, int y)
	{
		GamePiece pieceToClear = m_allGamePieces[x,y];

		if (pieceToClear !=null)
		{
			m_allGamePieces[x,y] = null;
			Destroy(pieceToClear.gameObject);
		}

		//HighlightTileOff(x,y);
	}

	void ClearBoard()
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				ClearPieceAt(i,j);
			}
		}
	}

	void ClearPieceAt(List<GamePiece> gamePieces)
	{
		foreach (GamePiece piece in gamePieces)
		{
			if (piece !=null)
			{
				ClearPieceAt(piece.xIndex, piece.yIndex);
				if (m_particleManager !=null)
				{
					m_particleManager.ClearPieceFXAt(piece.xIndex,piece.yIndex);
				}
			}
		}
	}

	void BreakTileAt(int x, int y)
	{
		Tile tileToBreak = m_allTiles[x,y];

		if (tileToBreak != null && tileToBreak.tileType == TileType.Breakable)
		{
			if (m_particleManager !=null)
			{
				m_particleManager.BreakTileFXAt(tileToBreak.breakableValue, x, y, 0);
			}

			tileToBreak.BreakTile();
		}
	}

	void BreakTileAt(List<GamePiece> gamePieces)
	{
		foreach (GamePiece piece in gamePieces)
		{
			if (piece != null)
			{
				BreakTileAt(piece.xIndex, piece.yIndex);
			}
		}
	}

	List<GamePiece> CollapseColumn(int column, float collapseTime = 0.1f)
	{
		List<GamePiece> movingPieces = new List<GamePiece>();

		for (int i = 0; i < height - 1; i++)
		{
			if (m_allGamePieces[column,i] == null && m_allTiles[column,i].tileType != TileType.Obstacle)
			{
				for (int j = i + 1; j < height; j++)
				{
					if (m_allGamePieces[column,j] !=null)
					{
						m_allGamePieces[column,j].Move(column, i, collapseTime * (j-i));

						m_allGamePieces[column,i] = m_allGamePieces[column,j];
						m_allGamePieces[column,i].SetCoord(column,i);

						if (!movingPieces.Contains(m_allGamePieces[column,i]))
						{
							movingPieces.Add(m_allGamePieces[column,i]);
						}

						m_allGamePieces[column,j] = null;

						break;
					}
				}
			}
		}
		return movingPieces;
	}

	List<GamePiece> CollapseColumn(List<GamePiece> gamePieces)
	{
		List<GamePiece> movingPieces = new List<GamePiece>();

		List<int> columnsToCollapse = GetColumns(gamePieces);

		foreach (int column in columnsToCollapse)
		{
			movingPieces = movingPieces.Union(CollapseColumn(column)).ToList();
		}

		return movingPieces;
	}

	List<int> GetColumns (List<GamePiece> gamePieces)
	{
		List<int> columns = new List<int>();

		foreach (GamePiece piece in gamePieces)
		{
			if (!columns.Contains(piece.xIndex))
			{
				columns.Add(piece.xIndex);
			}
		}

		return columns;
	}

	void ClearAndRefillBoard(List<GamePiece> gamePieces)
	{
		StartCoroutine(ClearAndRefillBoardRoutine(gamePieces));
	}

	IEnumerator ClearAndRefillBoardRoutine(List<GamePiece> gamePieces)
	{
		m_playerInputEnabled = false;

		List<GamePiece> matches = gamePieces;

		do 
		{
			yield return StartCoroutine(ClearAndCollapseRoutine(matches));

			// add pause here 
			yield return null;

			yield return StartCoroutine(RefillRoutine());

			matches = FindAllMatches();

			yield return new WaitForSeconds(0.2f);

		}
		while (matches.Count != 0);

		m_playerInputEnabled = true;

	}

	IEnumerator ClearAndCollapseRoutine(List<GamePiece> gamePieces)
	{
		List<GamePiece> movingPieces = new List<GamePiece>();
		List<GamePiece> matches = new List<GamePiece>();

		//HighlightPieces(gamePieces);
		yield return new WaitForSeconds(0.2f);

		bool isFinished = false;

		while (!isFinished)
		{
			List<GamePiece> bombedPieces = GetBombedPieces(gamePieces);
			gamePieces = gamePieces.Union(bombedPieces).ToList();

			ClearPieceAt(gamePieces);
			BreakTileAt(gamePieces);

			if (m_clickedTileBomb != null)
			{
				ActivateBomb(m_clickedTileBomb);
				m_clickedTileBomb = null;
			}
			if (m_targetTileBomb != null)
			{
				ActivateBomb(m_targetTileBomb);
				m_targetTileBomb = null;
			}

			yield return new WaitForSeconds(0.25f);

			movingPieces = CollapseColumn(gamePieces);
			while (!IsCollapsed(movingPieces))
			{
				yield return null;
			}
			yield return new WaitForSeconds(0.2f);

			matches = FindMatchesAt(movingPieces);

			if (matches.Count == 0)
			{
				isFinished = true;
				break;
			}
			else
			{
				yield return StartCoroutine(ClearAndCollapseRoutine(matches));
			}
		}
		yield return null;
	}

	IEnumerator RefillRoutine()
	{
		FillBoard(fillYOffset, fillMoveTime);

		yield return null;

	}

	bool IsCollapsed(List<GamePiece> gamePieces)
	{
		foreach (GamePiece piece in gamePieces)
		{
			if (piece !=null)
			{
				if (piece.transform.position.y - (float) piece.yIndex > 0.001f)
				{
					return false;
				}
			}
		}
		return true;
	}

	List<GamePiece> GetRowPieces(int row)
	{
		List<GamePiece> gamePieces = new List<GamePiece>();

		for (int i = 0; i < width; i++)
		{
			if (m_allGamePieces[i, row] !=null)
			{
				gamePieces.Add(m_allGamePieces[i, row]);
			}
		}
		return gamePieces;
	}

	List<GamePiece> GetColumnPieces(int column)
	{
		List<GamePiece> gamePieces = new List<GamePiece>();

		for (int i = 0; i < height; i++)
		{
			if (m_allGamePieces[column,i] !=null)
			{
				gamePieces.Add(m_allGamePieces[column,i]);
			}
		}
		return gamePieces;
	}

	List<GamePiece> GetAdjacentPieces(int x, int y, int offset = 1)
	{
		List<GamePiece> gamePieces = new List<GamePiece>();

		for (int i = x - offset; i <= x + offset; i++)
		{
			for (int j = y - offset; j <= y + offset; j++)
			{
				if (IsWithinBounds(i,j))
				{
					gamePieces.Add(m_allGamePieces[i,j]);
				}

			}
		}

		return gamePieces;
	}

	List<GamePiece> GetBombedPieces(List<GamePiece> gamePieces)
	{
		List<GamePiece> allPiecesToClear = new List<GamePiece>();

		foreach (GamePiece piece in gamePieces)
		{
			if (piece !=null)
			{
				List<GamePiece> piecesToClear = new List<GamePiece>();

				Bomb bomb = piece.GetComponent<Bomb>();

				if (bomb !=null)
				{
					switch (bomb.bombType)
					{
						case BombType.Column:
							piecesToClear = GetColumnPieces(bomb.xIndex);
							break;
						case BombType.Row:
							piecesToClear = GetRowPieces(bomb.yIndex);
							break;
						case BombType.Adjacent:
							piecesToClear = GetAdjacentPieces(bomb.xIndex, bomb.yIndex, 1);
							break;
						case BombType.Color:
							
							break;
					}

					allPiecesToClear = allPiecesToClear.Union(piecesToClear).ToList();
				}
			}
		}

		return allPiecesToClear;
	}

	bool IsCornerMatch(List<GamePiece> gamePieces)
	{
		bool vertical = false;
		bool horizontal = false;
		int xStart = -1;
		int yStart = -1;

		foreach (GamePiece piece in gamePieces )
		{
			if (piece != null)
			{
				if (xStart == -1 || yStart == -1)
				{
					xStart = piece.xIndex;
					yStart = piece.yIndex;
					continue;
				}
				else if (piece.xIndex != xStart && piece.yIndex == yStart)
				{
					horizontal = true;
				}
				else if (piece.xIndex == xStart && piece.yIndex != yStart)
				{
					vertical = true;
				}
			}
		}

		return (horizontal & vertical);
	}

	GameObject DropBomb(int x, int y, Vector2 swapDirection, List<GamePiece> gamePieces)
	{
		GameObject bomb = null;
		if (gamePieces.Count >= 4)
		{
			if (IsCornerMatch(gamePieces))
			{
				if (IsCornerMatch(gamePieces))
				{
					if (adjacentBombPrefab != null)
					{
						bomb = MakeBomb(adjacentBombPrefab, x, y);
					}
				}
			}
			else
			{
				if (swapDirection.x != 0)
				{
					if (rowBombPrefab != null)
					{
						bomb = MakeBomb(rowBombPrefab, x, y);

					}
				}
				if (swapDirection.y != 0)
				{
					if (columnBombPrefab != null)
					{
						bomb = MakeBomb(columnBombPrefab, x, y);

					}
				}
			}
		}

		return bomb;
	}

	void ActivateBomb(GameObject bomb)
	{
		int x = (int)bomb.transform.position.x;
		int y = (int)bomb.transform.position.y;

		if (IsWithinBounds(x, y))
		{
			m_allGamePieces[x, y] = bomb.GetComponent<GamePiece>();
		}
	}
}
