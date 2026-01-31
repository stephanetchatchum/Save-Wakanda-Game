using UnityEngine;
using System.Collections.Generic;

namespace SlidingPuzzle
{
    /// <summary>
    /// Main manager that orchestrates the entire puzzle game flow
    /// Handles multiple puzzles, player state, and progression
    /// </summary>
    public class PuzzleGameManager : MonoBehaviour
    {
        [Header("Puzzle Configurations")]
        [Tooltip("List of all puzzle configurations in order")]
        public List<PuzzleConfiguration> puzzleConfigs = new List<PuzzleConfiguration>();
        
        [Header("System References")]
        public PuzzleManager puzzleManager;
        public PuzzleUIController puzzleUIController;
        public MaskRewardSystem maskRewardSystem;
        public GhostCounter ghostCounter;
        
        [Header("UI")]
        [Tooltip("The canvas/panel that contains the puzzle UI")]
        public GameObject puzzleUIPanel;
        
        [Header("Player Control")]
        [Tooltip("Should player movement be locked during puzzle?")]
        public bool lockPlayerDuringPuzzle = true;
        
        [Tooltip("Reference to player controller (to disable during puzzle)")]
        public MonoBehaviour playerController;
        
        [Header("Cursor Settings")]
        public bool showCursorDuringPuzzle = true;
        
        [Header("Camera (Optional)")]
        [Tooltip("Camera to switch to during puzzle (leave null to keep current)")]
        public Camera puzzleCamera;
        private Camera mainCamera;
        
        // State tracking
        private int currentPuzzleIndex = 0;
        private bool isPuzzleActive = false;
        private Vector3 puzzleTablePosition;
        
        public bool IsPuzzleActive => isPuzzleActive;
        public int CurrentPuzzleIndex => currentPuzzleIndex;
        public int TotalPuzzles => puzzleConfigs.Count;
        
        void Start()
        {
            // Initial setup
            if (puzzleUIPanel != null)
            {
                puzzleUIPanel.SetActive(false);
            }
            
            // Store main camera
            mainCamera = Camera.main;
            
            // Setup ghost counter
            if (ghostCounter != null)
            {
                ghostCounter.SetTotalGhosts(puzzleConfigs.Count);
            }
            
            // Validate setup
            ValidateSetup();
        }
        
        /// <summary>
        /// Start the puzzle at the given world position
        /// </summary>
        public void StartPuzzle(Vector3 tablePosition)
        {
            if (isPuzzleActive)
            {
                Debug.LogWarning("Puzzle already active!");
                return;
            }
            
            if (currentPuzzleIndex >= puzzleConfigs.Count)
            {
                Debug.Log("All puzzles completed!");
                return;
            }
            
            puzzleTablePosition = tablePosition;
            PuzzleConfiguration config = puzzleConfigs[currentPuzzleIndex];
            
            Debug.Log($"Starting puzzle {currentPuzzleIndex + 1}/{puzzleConfigs.Count}");
            
            // Setup systems
            puzzleManager.InitializePuzzle(config);
            puzzleUIController.SetupPuzzleUI(config);
            maskRewardSystem.SetConfiguration(config);
            
            // Show UI
            if (puzzleUIPanel != null)
            {
                puzzleUIPanel.SetActive(true);
            }
            
            // Lock player
            if (lockPlayerDuringPuzzle && playerController != null)
            {
                playerController.enabled = false;
            }
            
            // Cursor settings
            if (showCursorDuringPuzzle)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            
            // Camera switch
            if (puzzleCamera != null && mainCamera != null)
            {
                mainCamera.enabled = false;
                puzzleCamera.enabled = true;
            }
            
            isPuzzleActive = true;
            
            // Subscribe to puzzle solved event
            puzzleManager.OnPuzzleSolved += OnPuzzleSolved;
        }
        
        /// <summary>
        /// Called when current puzzle is solved
        /// </summary>
        private void OnPuzzleSolved()
        {
            Debug.Log("Puzzle solved! Awarding mask...");
            
            // Award the mask
            maskRewardSystem.AwardMask();
            
            // Close UI AFTER the full sequence finishes:
            // timeBeforeExplosion (2s) + explosionDuration (1s) + ghostDestroyDelay (3s) + small buffer
            float totalSequenceTime = maskRewardSystem.timeBeforeExplosion 
                                    + maskRewardSystem.explosionDuration 
                                    + maskRewardSystem.currentConfig.ghostDestroyDelay 
                                    + 0.5f;
            
            Debug.Log($"Ghost sequence will take {totalSequenceTime}s — closing UI after that");
            Invoke(nameof(ClosePuzzleUI), 1f);
        }
        
        /// <summary>
        /// Close the puzzle UI and return to gameplay
        /// </summary>
        private void ClosePuzzleUI()
        {
            Debug.Log("Closing puzzle UI and restoring player controls...");
            
            // Hide UI
            if (puzzleUIPanel != null)
            {
                puzzleUIPanel.SetActive(false);
            }
            
            // Unlock player - CRITICAL FIX
            if (lockPlayerDuringPuzzle && playerController != null)
            {
                playerController.enabled = true;
                Debug.Log("Player controller re-enabled");
            }
            
            /* Restore cursor - back to gameplay mode
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Debug.Log("Cursor locked and hidden");*/
            
            // Restore camera
            if (puzzleCamera != null && mainCamera != null)
            {
                puzzleCamera.enabled = false;
                mainCamera.enabled = true;
            }
            
            isPuzzleActive = false;
            
            // Unsubscribe
            if (puzzleManager != null)
            {
                puzzleManager.OnPuzzleSolved -= OnPuzzleSolved;
            }
            
            // Move to next puzzle
            currentPuzzleIndex++;
            
            if (currentPuzzleIndex >= puzzleConfigs.Count)
            {
                Debug.Log("All puzzles completed!");
                OnAllPuzzlesCompleted();
            }
            else
            {
                Debug.Log($"Ready for next puzzle: {currentPuzzleIndex + 1}/{puzzleConfigs.Count}");
            }
        }
        
        /// <summary>
        /// Called when all puzzles are completed
        /// </summary>
        private void OnAllPuzzlesCompleted()
        {
            Debug.Log("🎉 All puzzles solved! Game complete!");
            // Add any end-game logic here
        }
        
        /// <summary>
        /// Validate that all required components are set up
        /// </summary>
        private void ValidateSetup()
        {
            bool isValid = true;
            
            if (puzzleManager == null)
            {
                Debug.LogError("PuzzleManager reference missing!");
                isValid = false;
            }
            
            if (puzzleUIController == null)
            {
                Debug.LogError("PuzzleUIController reference missing!");
                isValid = false;
            }
            
            if (maskRewardSystem == null)
            {
                Debug.LogError("MaskRewardSystem reference missing!");
                isValid = false;
            }
            
            if (puzzleConfigs.Count == 0)
            {
                Debug.LogWarning("No puzzle configurations added!");
            }
            
            if (isValid)
            {
                Debug.Log($"✓ Puzzle Game Manager initialized with {puzzleConfigs.Count} puzzles");
            }
        }
        
        /// <summary>
        /// Debug function to skip to next puzzle
        /// </summary>
        [ContextMenu("Skip Current Puzzle")]
        public void DebugSkipPuzzle()
        {
            if (isPuzzleActive)
            {
                OnPuzzleSolved();
            }
        }
        
        /// <summary>
        /// Debug function to reset all puzzles
        /// </summary>
        [ContextMenu("Reset All Puzzles")]
        public void DebugResetPuzzles()
        {
            currentPuzzleIndex = 0;
            if (isPuzzleActive)
            {
                ClosePuzzleUI();
            }
            Debug.Log("All puzzles reset");
        }
    }
}