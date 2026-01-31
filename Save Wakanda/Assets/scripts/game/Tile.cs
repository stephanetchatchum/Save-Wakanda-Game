using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents a single tile in the sliding puzzle.
/// Stores current position, correct position, and handles visual display.
/// </summary>
public class Tile : MonoBehaviour
{
    [Header("Tile Properties")]
    public int tileId;
    public int currentRow;
    public int currentCol;
    public int correctRow;
    public int correctCol;

    [Header("Visual Components")]
    public Image tileImage;
    public Text tileNumberText; // Optional: shows tile number for debugging
    
    private PuzzleManager puzzleManager;
    private Button tileButton;
    private RectTransform rectTransform;

    // Animation properties
    private Vector2 targetPosition;
    private bool isMoving = false;
    private float moveSpeed = 10f;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        tileButton = GetComponent<Button>();
        
        if (tileButton != null)
        {
            tileButton.onClick.AddListener(OnTileClicked);
        }
    }

    void Update()
    {
        // Smooth movement animation
        if (isMoving)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(
                rectTransform.anchoredPosition, 
                targetPosition, 
                Time.deltaTime * moveSpeed
            );

            if (Vector2.Distance(rectTransform.anchoredPosition, targetPosition) < 0.1f)
            {
                rectTransform.anchoredPosition = targetPosition;
                isMoving = false;
            }
        }
    }

    /// <summary>
    /// Initialize the tile with its properties
    /// </summary>
    public void Initialize(int id, int curRow, int curCol, int corRow, int corCol, PuzzleManager manager)
    {
        tileId = id;
        currentRow = curRow;
        currentCol = curCol;
        correctRow = corRow;
        correctCol = corCol;
        puzzleManager = manager;

        // Optional: Display tile number for debugging
        if (tileNumberText != null)
        {
            tileNumberText.text = (tileId + 1).ToString();
        }
    }

    /// <summary>
    /// Set the sprite for this tile (portion of the mask image)
    /// </summary>
    public void SetTileSprite(Sprite sprite)
    {
        if (tileImage != null)
        {
            tileImage.sprite = sprite;
        }
    }

    /// <summary>
    /// Move tile to a new position with animation
    /// </summary>
    public void MoveTo(int newRow, int newCol, float tileSize, float spacing)
    {
        currentRow = newRow;
        currentCol = newCol;
        
        targetPosition = new Vector2(
            currentCol * (tileSize + spacing),
            -currentRow * (tileSize + spacing)
        );
        
        isMoving = true;
    }

    /// <summary>
    /// Check if this tile is in its correct position
    /// </summary>
    public bool IsInCorrectPosition()
    {
        return currentRow == correctRow && currentCol == correctCol;
    }

    /// <summary>
    /// Called when tile is clicked
    /// </summary>
    private void OnTileClicked()
    {
        if (puzzleManager != null)
        {
            puzzleManager.OnTileClicked(this);
        }
    }

    /// <summary>
    /// Highlight this tile (for showing movable tiles)
    /// </summary>
    public void SetHighlight(bool highlighted)
    {
        if (tileImage != null)
        {
            tileImage.color = highlighted ? new Color(1f, 1f, 1f, 1f) : new Color(0.8f, 0.8f, 0.8f, 1f);
        }
    }
}
