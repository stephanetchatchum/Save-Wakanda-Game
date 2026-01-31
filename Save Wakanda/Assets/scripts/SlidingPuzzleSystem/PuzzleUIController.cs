using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace SlidingPuzzle
{
    /// <summary>
    /// Manages the UI display and interaction for the sliding puzzle
    /// Attach to a Canvas or UI panel that contains the puzzle
    /// </summary>
    public class PuzzleUIController : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("The PuzzleManager component")]
        public PuzzleManager puzzleManager;
        
        [Tooltip("Parent RectTransform where tiles will be spawned")]
        public RectTransform tileContainer;
        
        [Tooltip("Prefab for individual puzzle tiles (should have Image and Button)")]
        public GameObject tilePrefab;
        
        [Header("Visual Settings")]
        [Tooltip("Spacing between tiles in pixels")]
        public float tileSpacing = 5f;
        
        [Tooltip("Size of the puzzle area")]
        public float puzzleSize = 500f;
        
        [Tooltip("Show numbers on tiles")]
        public bool showTileNumbers = true;
        
        [Header("Stats Display")]
        [Tooltip("Text element to show move count")]
        public Text moveCountText;
        
        [Tooltip("Text element to show timer")]
        public Text timerText;
        
        [Header("Audio (Optional)")]
        public AudioClip tileClickSound;
        public AudioClip puzzleSolvedSound;
        private AudioSource audioSource;
        
        private List<GameObject> tileObjects = new List<GameObject>();
        private Dictionary<GameObject, Vector2Int> tilePositions = new Dictionary<GameObject, Vector2Int>();
        private Texture2D currentPuzzleImage;
        
        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null && (tileClickSound != null || puzzleSolvedSound != null))
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        
        void OnEnable()
        {
            if (puzzleManager != null)
            {
                puzzleManager.OnTilesChanged += UpdateTileDisplay;
                puzzleManager.OnPuzzleSolved += OnPuzzleSolved;
                puzzleManager.OnStatsUpdated += UpdateStatsDisplay;
            }
        }
        
        void OnDisable()
        {
            if (puzzleManager != null)
            {
                puzzleManager.OnTilesChanged -= UpdateTileDisplay;
                puzzleManager.OnPuzzleSolved -= OnPuzzleSolved;
                puzzleManager.OnStatsUpdated -= UpdateStatsDisplay;
            }
        }
        
        /// <summary>
        /// Create the initial tile UI elements
        /// </summary>
        public void SetupPuzzleUI(PuzzleConfiguration config)
        {
            // Clear existing tiles
            ClearTiles();
            
            currentPuzzleImage = config.puzzleImage;
            int gridSize = config.gridSize;
            
            float tileSize = (puzzleSize - (tileSpacing * (gridSize - 1))) / gridSize;
            
            // Create tiles
            for (int y = 0; y < gridSize; y++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    Vector2Int gridPos = new Vector2Int(x, y);
                    GameObject tileObj = Instantiate(tilePrefab, tileContainer);
                    
                    RectTransform tileRect = tileObj.GetComponent<RectTransform>();
                    tileRect.sizeDelta = new Vector2(tileSize, tileSize);
                    
                    // Position the tile
                    float xPos = x * (tileSize + tileSpacing);
                    float yPos = -y * (tileSize + tileSpacing);
                    tileRect.anchoredPosition = new Vector2(xPos, yPos);
                    
                    // Set up the button - capture the tile object itself
                    Button button = tileObj.GetComponent<Button>();
                    if (button != null)
                    {
                        GameObject capturedTile = tileObj;
                        button.onClick.AddListener(() => OnTileClicked(capturedTile));
                    }
                    
                    tileObjects.Add(tileObj);
                    tilePositions[tileObj] = gridPos;
                }
            }
            
            Debug.Log($"Created {gridSize}x{gridSize} puzzle UI");
            
            // Force initial display update
            if (puzzleManager != null && puzzleManager.TileGrid != null)
            {
                UpdateTileDisplay(puzzleManager.TileGrid);
            }
        }
        
        /// <summary>
        /// Force update the display (useful for initial setup)
        /// </summary>
        public void ForceUpdateDisplay()
        {
            if (puzzleManager != null && puzzleManager.TileGrid != null)
            {
                UpdateTileDisplay(puzzleManager.TileGrid);
            }
        }
        
        /// <summary>
        /// Update the visual display of tiles based on current grid state
        /// </summary>
        private void UpdateTileDisplay(int[,] tileGrid)
        {
            int gridSize = puzzleManager.GridSize;
            
            // Update each tile object based on its current position
            for (int i = 0; i < tileObjects.Count; i++)
            {
                GameObject tileObj = tileObjects[i];
                Vector2Int gridPos = tilePositions[tileObj];
                int x = gridPos.x;
                int y = gridPos.y;
                
                int tileNumber = tileGrid[x, y];
                
                if (tileNumber == 0)
                {
                    // Empty tile - hide it
                    tileObj.SetActive(false);
                }
                else
                {
                    tileObj.SetActive(true);
                    
                    // Set the tile image portion
                    Image tileImage = tileObj.GetComponent<Image>();
                    if (tileImage != null && currentPuzzleImage != null)
                    {
                        SetTileImage(tileImage, tileNumber - 1, gridSize);
                    }
                    
                    // Show tile number
                    Text tileText = tileObj.GetComponentInChildren<Text>();
                    if (tileText != null)
                    {
                        if (showTileNumbers)
                        {
                            tileText.text = tileNumber.ToString();
                            tileText.gameObject.SetActive(true);
                        }
                        else
                        {
                            tileText.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Update the stats display (moves and time)
        /// </summary>
        private void UpdateStatsDisplay(int moves, float time)
        {
            if (moveCountText != null)
            {
                moveCountText.text = $"Moves: {moves}";
            }
            
            if (timerText != null)
            {
                int minutes = Mathf.FloorToInt(time / 60f);
                int seconds = Mathf.FloorToInt(time % 60f);
                timerText.text = $"Time: {minutes:00}:{seconds:00}";
            }
        }
        
        /// <summary>
        /// Set the sprite for a tile based on its number and position in the original image
        /// </summary>
        private void SetTileImage(Image tileImage, int tileIndex, int gridSize)
        {
            if (currentPuzzleImage == null) return;
            
            // Calculate which portion of the image this tile should show
            int tileX = tileIndex % gridSize;
            int tileY = tileIndex / gridSize;
            
            float tileSizeX = 1f / gridSize;
            float tileSizeY = 1f / gridSize;
            
            // Create a sprite from the portion of the texture
            int pixelWidth = currentPuzzleImage.width / gridSize;
            int pixelHeight = currentPuzzleImage.height / gridSize;
            
            Rect spriteRect = new Rect(
                tileX * pixelWidth,
                currentPuzzleImage.height - (tileY + 1) * pixelHeight, // Flip Y
                pixelWidth,
                pixelHeight
            );
            
            Sprite tileSprite = Sprite.Create(
                currentPuzzleImage,
                spriteRect,
                new Vector2(0.5f, 0.5f),
                100f
            );
            
            tileImage.sprite = tileSprite;
        }
        
        /// <summary>
        /// Handle tile click
        /// </summary>
        private void OnTileClicked(GameObject clickedTile)
        {
            if (!tilePositions.ContainsKey(clickedTile))
            {
                Debug.LogWarning("Clicked tile not found in position map!");
                return;
            }
            
            Vector2Int gridPos = tilePositions[clickedTile];
            bool moved = puzzleManager.TryMoveTile(gridPos.x, gridPos.y);
            
            if (moved && tileClickSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(tileClickSound);
            }
        }
        
        /// <summary>
        /// Called when puzzle is solved
        /// </summary>
        private void OnPuzzleSolved()
        {
            if (puzzleSolvedSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(puzzleSolvedSound);
            }
            
            Debug.Log("Puzzle UI: Puzzle solved!");
        }
        
        /// <summary>
        /// Clear all tile objects
        /// </summary>
        private void ClearTiles()
        {
            foreach (var tile in tileObjects)
            {
                Destroy(tile);
            }
            tileObjects.Clear();
            tilePositions.Clear();
        }
    }
}
