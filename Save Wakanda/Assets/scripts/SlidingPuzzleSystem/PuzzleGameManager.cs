using UnityEngine;
using System.Collections.Generic;

namespace SlidingPuzzle
{
    /// <summary>
    /// Main manager that orchestrates the entire puzzle game flow.
    /// Handles multiple puzzles, player state, progression, and end-game.
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
        public ChiefDialogue chiefDialogue;
        
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
            if (puzzleUIPanel != null)
            {
                puzzleUIPanel.SetActive(false);
            }
            
            mainCamera = Camera.main;
            
            if (ghostCounter != null)
            {
                ghostCounter.SetTotalGhosts(puzzleConfigs.Count);
            }
            
            // Spawn all ghosts at their spawn points at game start
            if (maskRewardSystem != null)
            {
                maskRewardSystem.SpawnAllGhosts(puzzleConfigs);
            }
            
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
            
            // Cursor
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
            puzzleManager.OnPuzzleSolved += OnPuzzleSolved;
        }
        
        /// <summary>
        /// Called when current puzzle is solved
        /// </summary>
        private void OnPuzzleSolved()
        {
            Debug.Log("Puzzle solved! Awarding mask...");
            
            // Award mask — ghost defeat sequence runs in background
            maskRewardSystem.AwardMask(currentPuzzleIndex);
            
            // Close UI after 1s so player gets back to the 3D scene to watch the ghost
            Invoke(nameof(ClosePuzzleUI), 1f);
        }
        
        /// <summary>
        /// Close the puzzle UI and return to gameplay
        /// </summary>
        private void ClosePuzzleUI()
        {
            Debug.Log("Closing puzzle UI and restoring player controls...");
            
            if (puzzleUIPanel != null)
            {
                puzzleUIPanel.SetActive(false);
            }
            
            // Unlock player
            if (lockPlayerDuringPuzzle && playerController != null)
            {
                playerController.enabled = true;
                Debug.Log("Player controller re-enabled");
            }
            
            // Restore cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
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
            
            // Advance to next puzzle
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
        /// Called when all puzzles are done — triggers chief dialogue
        /// </summary>
        private void OnAllPuzzlesCompleted()
        {
            Debug.Log("All puzzles solved! Triggering chief dialogue...");
            
            if (chiefDialogue != null)
            {
                chiefDialogue.TriggerEndDialogue();
            }
        }
        
        /// <summary>
        /// Restart the entire game from scratch.
        /// Called by ChiefDialogue "Start Over" button.
        /// </summary>
        public void RestartGame()
        {
            Debug.Log("Restarting game...");
            
            // Reset puzzle index
            currentPuzzleIndex = 0;
            isPuzzleActive = false;
            
            // Reset ghost counter
            if (ghostCounter != null)
            {
                ghostCounter.ResetCounter();
            }
            
            // Re-spawn all ghosts
            if (maskRewardSystem != null)
            {
                maskRewardSystem.SpawnAllGhosts(puzzleConfigs);
            }
            
            // Reset each puzzle so they can be solved again
            if (puzzleManager != null)
            {
                puzzleManager.ResetAllPuzzles();
            }
            
            Debug.Log("Game restarted. All ghosts re-spawned.");
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
            if (chiefDialogue == null)
            {
                Debug.LogWarning("ChiefDialogue not assigned — end-game dialogue won't trigger.");
            }
            
            if (isValid)
            {
                Debug.Log($"✓ Puzzle Game Manager initialized with {puzzleConfigs.Count} puzzles");
            }
        }
        
        [ContextMenu("Skip Current Puzzle")]
        public void DebugSkipPuzzle()
        {
            if (isPuzzleActive)
            {
                OnPuzzleSolved();
            }
        }
        
        [ContextMenu("Reset All Puzzles")]
        public void DebugResetPuzzles()
        {
            RestartGame();
        }
    }
}
