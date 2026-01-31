using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace SlidingPuzzle
{
    /// <summary>
    /// Core sliding puzzle logic - handles tile arrangement, moves, and win detection
    /// </summary>
    public class PuzzleManager : MonoBehaviour
    {
        [Header("Configuration")]
        public PuzzleConfiguration currentPuzzle;
        
        [Header("Debug")]
        public bool debugMode = false;
        [Tooltip("Press this key to auto-solve puzzle (debug only)")]
        public KeyCode debugWinKey = KeyCode.W;
        
        // Puzzle state
        private int[,] tileGrid;
        private Vector2Int emptyTilePos;
        private int gridSize;
        
        // Stats tracking
        private int moveCount = 0;
        private float startTime = 0f;
        private float solveTime = 0f;
        
        // Events
        public System.Action OnPuzzleSolved;
        public System.Action<int[,]> OnTilesChanged;
        public System.Action<int, float> OnStatsUpdated; // moveCount, elapsedTime
        
        private bool isPuzzleSolved = false;
        
        public int GridSize => gridSize;
        public int[,] TileGrid => tileGrid;
        public Vector2Int EmptyTilePosition => emptyTilePos;
        public bool IsSolved => isPuzzleSolved;
        public int MoveCount => moveCount;
        public float ElapsedTime => isPuzzleSolved ? solveTime : (Time.time - startTime);
        
        void Update()
        {
            if (debugMode && Input.GetKeyDown(debugWinKey))
            {
                Debug.Log("[Debug] Auto-solving puzzle!");
                SolvePuzzle();
            }
            
            // Update stats every frame while puzzle is active
            if (!isPuzzleSolved && tileGrid != null)
            {
                OnStatsUpdated?.Invoke(moveCount, Time.time - startTime);
            }
        }
        
        /// <summary>
        /// Initialize a new puzzle
        /// </summary>
        public void InitializePuzzle(PuzzleConfiguration config)
        {
            currentPuzzle = config;
            gridSize = config.gridSize;
            isPuzzleSolved = false;
            
            // Reset stats
            moveCount = 0;
            startTime = Time.time;
            solveTime = 0f;
            
            // Create the grid
            tileGrid = new int[gridSize, gridSize];
            
            // Fill with sequential numbers (0 represents empty space)
            int number = 1;
            for (int y = 0; y < gridSize; y++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    if (y == gridSize - 1 && x == gridSize - 1)
                    {
                        tileGrid[x, y] = 0; // Empty tile at bottom-right
                        emptyTilePos = new Vector2Int(x, y);
                    }
                    else
                    {
                        tileGrid[x, y] = number++;
                    }
                }
            }
            
            // Shuffle the puzzle
            ShufflePuzzle();
            
            OnTilesChanged?.Invoke(tileGrid);
            
            Debug.Log($"Puzzle initialized: {gridSize}x{gridSize} grid");
        }
        
        /// <summary>
        /// Attempt to move a tile at the given grid position
        /// </summary>
        public bool TryMoveTile(int x, int y)
        {
            if (isPuzzleSolved) return false;
            
            // Check if this tile is adjacent to the empty space
            int dx = Mathf.Abs(x - emptyTilePos.x);
            int dy = Mathf.Abs(y - emptyTilePos.y);
            
            bool isAdjacent = (dx == 1 && dy == 0) || (dx == 0 && dy == 1);
            
            if (!isAdjacent) return false;
            
            // Swap the tile with the empty space
            tileGrid[emptyTilePos.x, emptyTilePos.y] = tileGrid[x, y];
            tileGrid[x, y] = 0;
            emptyTilePos = new Vector2Int(x, y);
            
            // Increment move counter
            moveCount++;
            
            OnTilesChanged?.Invoke(tileGrid);
            
            // Check for win condition
            if (CheckWinCondition())
            {
                isPuzzleSolved = true;
                solveTime = Time.time - startTime;
                Debug.Log($"Puzzle Solved in {moveCount} moves and {solveTime:F2} seconds!");
                OnPuzzleSolved?.Invoke();
            }
            else
            {
                // Update stats
                OnStatsUpdated?.Invoke(moveCount, Time.time - startTime);
            }
            
            return true;
        }
        
        /// <summary>
        /// Shuffle the puzzle using random valid moves
        /// </summary>
        private void ShufflePuzzle()
        {
            int shuffleMoves = gridSize * gridSize * 50; // More moves for better shuffle
            
            for (int i = 0; i < shuffleMoves; i++)
            {
                List<Vector2Int> validMoves = GetValidMoves();
                if (validMoves.Count > 0)
                {
                    Vector2Int randomMove = validMoves[Random.Range(0, validMoves.Count)];
                    
                    // Swap without triggering events
                    tileGrid[emptyTilePos.x, emptyTilePos.y] = tileGrid[randomMove.x, randomMove.y];
                    tileGrid[randomMove.x, randomMove.y] = 0;
                    emptyTilePos = randomMove;
                }
            }
            
            Debug.Log($"Puzzle shuffled with {shuffleMoves} moves");
        }
        
        /// <summary>
        /// Get all tiles adjacent to the empty space
        /// </summary>
        private List<Vector2Int> GetValidMoves()
        {
            List<Vector2Int> moves = new List<Vector2Int>();
            
            // Check all four directions
            Vector2Int[] directions = {
                new Vector2Int(0, 1),  // Up
                new Vector2Int(0, -1), // Down
                new Vector2Int(1, 0),  // Right
                new Vector2Int(-1, 0)  // Left
            };
            
            foreach (var dir in directions)
            {
                int newX = emptyTilePos.x + dir.x;
                int newY = emptyTilePos.y + dir.y;
                
                if (newX >= 0 && newX < gridSize && newY >= 0 && newY < gridSize)
                {
                    moves.Add(new Vector2Int(newX, newY));
                }
            }
            
            return moves;
        }
        
        /// <summary>
        /// Check if the puzzle is in the solved state
        /// </summary>
        private bool CheckWinCondition()
        {
            int expectedNumber = 1;
            
            for (int y = 0; y < gridSize; y++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    // Last tile should be 0 (empty)
                    if (y == gridSize - 1 && x == gridSize - 1)
                    {
                        return tileGrid[x, y] == 0;
                    }
                    
                    if (tileGrid[x, y] != expectedNumber)
                    {
                        return false;
                    }
                    
                    expectedNumber++;
                }
            }
            
            return true;
        }
        
        /// <summary>
        /// Debug function to instantly solve the puzzle
        /// </summary>
        private void SolvePuzzle()
        {
            int number = 1;
            for (int y = 0; y < gridSize; y++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    if (y == gridSize - 1 && x == gridSize - 1)
                    {
                        tileGrid[x, y] = 0;
                        emptyTilePos = new Vector2Int(x, y);
                    }
                    else
                    {
                        tileGrid[x, y] = number++;
                    }
                }
            }
            
            OnTilesChanged?.Invoke(tileGrid);
            isPuzzleSolved = true;
            OnPuzzleSolved?.Invoke();
        }
    }
}
