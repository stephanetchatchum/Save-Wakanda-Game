using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Main game manager that controls the sliding tile puzzle.
/// Handles initialization, shuffling, movement, and win detection.
/// </summary>
public class PuzzleManager : MonoBehaviour
{
    [Header("Current Level")]
    public LevelData currentLevel;

    [Header("UI References")]
    public RectTransform gridContainer;
    public GameObject tilePrefab;
    public Image completeMaskImage; // Shows full image when puzzle is solved
    public Text movesText;
    public Text levelText;
    public GameObject winPanel;

    [Header("Grid Settings")]
    public float tileSize = 100f;
    public float tileSpacing = 2f;

    // Internal state
    private List<Tile> tiles = new List<Tile>();
    private Vector2Int emptyPosition;
    private int moveCount = 0;
    private bool isPuzzleComplete = false;
    private int gridSize;

    void Start()
    {
        if (currentLevel != null)
        {
            InitializePuzzle();
        }
        else
        {
            Debug.LogError("No level data assigned to PuzzleManager!");
        }
    }

    /// <summary>
    /// Initialize the puzzle with the current level data
    /// </summary>
    public void InitializePuzzle()
    {
        // Clear existing tiles
        ClearPuzzle();

        gridSize = currentLevel.gridSize;
        emptyPosition = new Vector2Int(gridSize - 1, gridSize - 1);
        moveCount = 0;
        isPuzzleComplete = false;

        UpdateUI();
        SetupGrid();
        CreateTiles();
        
        if (completeMaskImage != null)
        {
            completeMaskImage.gameObject.SetActive(false);
        }
        
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Setup the grid container size
    /// </summary>
    void SetupGrid()
    {
        if (gridContainer != null)
        {
            float totalSize = gridSize * tileSize + (gridSize - 1) * tileSpacing;
            gridContainer.sizeDelta = new Vector2(totalSize, totalSize);
            
            // Apply grid background color
            Image gridBg = gridContainer.GetComponent<Image>();
            if (gridBg != null)
            {
                gridBg.color = currentLevel.gridBackgroundColor;
            }
        }
    }

    /// <summary>
    /// Create all tiles for the puzzle
    /// </summary>
    void CreateTiles()
    {
        int totalTiles = gridSize * gridSize;
        
        for (int i = 0; i < totalTiles - 1; i++) // -1 because one space is empty
        {
            int row = i / gridSize;
            int col = i % gridSize;

            GameObject tileObj = Instantiate(tilePrefab, gridContainer);
            Tile tile = tileObj.GetComponent<Tile>();
            
            if (tile != null)
            {
                tile.Initialize(i, row, col, row, col, this);
                
                // Set tile sprite (portion of the mask image)
                Sprite tileSprite = CreateTileSprite(currentLevel.maskSprite, row, col);
                tile.SetTileSprite(tileSprite);
                
                // Position the tile
                tile.MoveTo(row, col, tileSize, tileSpacing);
                
                // Apply tile background color
                if (tile.tileImage != null)
                {
                    tile.tileImage.color = currentLevel.tileBackgroundColor;
                }
                
                tiles.Add(tile);
            }
            
            // Set tile size
            RectTransform rt = tileObj.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.sizeDelta = new Vector2(tileSize, tileSize);
            }
        }
    }

    /// <summary>
    /// Create a sprite for a specific tile (cropped section of the full image)
    /// </summary>
    Sprite CreateTileSprite(Sprite fullSprite, int row, int col)
    {
        if (fullSprite == null) return null;

        Texture2D fullTexture = fullSprite.texture;
        int pieceWidth = fullTexture.width / gridSize;
        int pieceHeight = fullTexture.height / gridSize;

        int x = col * pieceWidth;
        int y = fullTexture.height - (row + 1) * pieceHeight; // Unity's y-axis is bottom-up

        Texture2D pieceTexture = new Texture2D(pieceWidth, pieceHeight);
        Color[] pixels = fullTexture.GetPixels(x, y, pieceWidth, pieceHeight);
        pieceTexture.SetPixels(pixels);
        pieceTexture.Apply();

        Sprite pieceSprite = Sprite.Create(
            pieceTexture,
            new Rect(0, 0, pieceWidth, pieceHeight),
            new Vector2(0.5f, 0.5f)
        );

        return pieceSprite;
    }

    /// <summary>
    /// Shuffle the puzzle using valid random moves
    /// </summary>
    public void ShufflePuzzle()
    {
        int shuffleMoves = currentLevel.shuffleMoves;
        
        for (int i = 0; i < shuffleMoves; i++)
        {
            List<Tile> movableTiles = GetMovableTiles();
            if (movableTiles.Count > 0)
            {
                Tile randomTile = movableTiles[Random.Range(0, movableTiles.Count)];
                SwapTileWithEmpty(randomTile, false); // false = don't count moves during shuffle
            }
        }
        
        moveCount = 0;
        UpdateUI();
    }

    /// <summary>
    /// Get all tiles that can currently be moved (adjacent to empty space)
    /// </summary>
    List<Tile> GetMovableTiles()
    {
        List<Tile> movableTiles = new List<Tile>();
        
        foreach (Tile tile in tiles)
        {
            if (IsAdjacentToEmpty(tile))
            {
                movableTiles.Add(tile);
            }
        }
        
        return movableTiles;
    }

    /// <summary>
    /// Check if a tile is adjacent to the empty space
    /// </summary>
    bool IsAdjacentToEmpty(Tile tile)
    {
        int rowDiff = Mathf.Abs(tile.currentRow - emptyPosition.x);
        int colDiff = Mathf.Abs(tile.currentCol - emptyPosition.y);
        
        return (rowDiff == 1 && colDiff == 0) || (rowDiff == 0 && colDiff == 1);
    }

    /// <summary>
    /// Called when a tile is clicked
    /// </summary>
    public void OnTileClicked(Tile tile)
    {
        if (isPuzzleComplete) return;
        
        if (IsAdjacentToEmpty(tile))
        {
            SwapTileWithEmpty(tile, true); // true = count this move
            CheckWinCondition();
        }
    }

    /// <summary>
    /// Swap a tile with the empty space
    /// </summary>
    void SwapTileWithEmpty(Tile tile, bool countMove)
    {
        Vector2Int oldEmptyPos = emptyPosition;
        
        emptyPosition = new Vector2Int(tile.currentRow, tile.currentCol);
        tile.MoveTo(oldEmptyPos.x, oldEmptyPos.y, tileSize, tileSpacing);
        
        if (countMove)
        {
            moveCount++;
            UpdateUI();
        }
    }

    /// <summary>
    /// Check if the puzzle is complete
    /// </summary>
    void CheckWinCondition()
    {
        bool allCorrect = true;
        
        foreach (Tile tile in tiles)
        {
            if (!tile.IsInCorrectPosition())
            {
                allCorrect = false;
                break;
            }
        }
        
        if (allCorrect)
        {
            OnPuzzleComplete();
        }
    }

    /// <summary>
    /// Called when puzzle is successfully solved
    /// </summary>
    void OnPuzzleComplete()
    {
        isPuzzleComplete = true;
        
        // Show complete mask image
        if (completeMaskImage != null && currentLevel.maskSprite != null)
        {
            completeMaskImage.sprite = currentLevel.maskSprite;
            completeMaskImage.gameObject.SetActive(true);
        }
        
        // Show win panel
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
        
        Debug.Log($"Puzzle Complete! Moves: {moveCount}");
    }

    /// <summary>
    /// Update UI elements (moves, level name, etc.)
    /// </summary>
    void UpdateUI()
    {
        if (movesText != null)
        {
            movesText.text = $"Moves: {moveCount}";
        }
        
        if (levelText != null && currentLevel != null)
        {
            levelText.text = $"Level {currentLevel.levelNumber}: {currentLevel.levelName}";
        }
    }

    /// <summary>
    /// Clear all tiles from the grid
    /// </summary>
    void ClearPuzzle()
    {
        foreach (Tile tile in tiles)
        {
            if (tile != null)
            {
                Destroy(tile.gameObject);
            }
        }
        tiles.Clear();
    }

    /// <summary>
    /// Reset puzzle to solved state
    /// </summary>
    public void ResetPuzzle()
    {
        InitializePuzzle();
    }

    /// <summary>
    /// Load a specific level
    /// </summary>
    public void LoadLevel(LevelData level)
    {
        currentLevel = level;
        InitializePuzzle();
    }

    void OnDestroy()
    {
        ClearPuzzle();
    }
}
