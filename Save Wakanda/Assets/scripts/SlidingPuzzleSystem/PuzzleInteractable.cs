using UnityEngine;

namespace SlidingPuzzle
{
    /// <summary>
    /// Attach this to the puzzle table or any 3D object in the scene
    /// Handles player interaction to start the puzzle
    /// </summary>
    public class PuzzleInteractable : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("The puzzle game manager")]
        public PuzzleGameManager gameManager;
        
        [Header("Interaction Settings")]
        [Tooltip("Interaction key")]
        public KeyCode interactKey = KeyCode.E;
        
        [Tooltip("Max distance to interact")]
        public float interactionRange = 3f;
        
        [Tooltip("Layer mask for player detection (optional)")]
        public LayerMask playerLayer;
        
        [Header("UI Feedback")]
        [Tooltip("Optional UI text to show interaction prompt")]
        public GameObject interactionPrompt;
        
        [Tooltip("Text to display when player is in range")]
        public string promptText = "Press E to play puzzle";
        
        private Transform playerTransform;
        private bool playerInRange = false;
        
        void Start()
        {
            // Find player by tag (adjust as needed for your setup)
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
            
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }
        }
        
        void Update()
        {
            if (playerTransform == null) return;
            
            // Check distance to player
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            playerInRange = distance <= interactionRange;
            
            // Show/hide prompt
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(playerInRange && !gameManager.IsPuzzleActive);
            }
            
            // Handle interaction
            if (playerInRange && Input.GetKeyDown(interactKey) && !gameManager.IsPuzzleActive)
            {
                StartPuzzle();
            }
        }
        
        /// <summary>
        /// Start the puzzle interaction
        /// </summary>
        private void StartPuzzle()
        {
            if (gameManager != null)
            {
                gameManager.StartPuzzle(transform.position);
                Debug.Log("Puzzle started!");
            }
        }
        
        /// <summary>
        /// Alternative: Use trigger collider for interaction
        /// </summary>
        void OnTriggerEnter(Collider other)
        {
            if (playerLayer == (playerLayer | (1 << other.gameObject.layer)))
            {
                playerInRange = true;
            }
        }
        
        void OnTriggerExit(Collider other)
        {
            if (playerLayer == (playerLayer | (1 << other.gameObject.layer)))
            {
                playerInRange = false;
            }
        }
        
        /// <summary>
        /// Visual debug in editor
        /// </summary>
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactionRange);
        }
    }
}
