using UnityEngine;

namespace SlidingPuzzle
{
    /// <summary>
    /// Attach this to the puzzle table or any 3D object in the scene.
    /// Handles player proximity detection and starts the puzzle on interaction.
    /// Auto-creates a "Press E" prompt above the object — no manual UI setup needed.
    /// </summary>
    public class PuzzleInteractable : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("The puzzle game manager")]
        public PuzzleGameManager gameManager;
        
        [Header("Interaction Settings")]
        [Tooltip("Key to press to start the puzzle")]
        public KeyCode interactKey = KeyCode.E;
        
        [Tooltip("How close the player needs to be to interact")]
        public float interactionRange = 3f;
        
        [Header("Prompt Settings")]
        [Tooltip("Text shown when player is close enough")]
        public string promptText = "Press E to play";
        
        [Tooltip("Height the prompt floats above this object")]
        public float promptHeight = 1.5f;
        
        [Tooltip("Size of the prompt text")]
        public float promptFontSize = 0.3f;
        
        [Tooltip("Color of the prompt text")]
        public Color promptColor = Color.white;
        
        // Internal state
        private Transform playerTransform;
        private bool playerInRange = false;
        private bool previousPlayerInRange = false;
        
        // Auto-created prompt
        private GameObject promptObject;
        private TextMesh promptMesh;
        
        [Header("Audio")]
        [Tooltip("Audio source for playing sounds")]
        public AudioSource audioSource;
        
        [Tooltip("Sound to play when player enters range")]
        public AudioClip enterRangeSound;
        
        void Start()
        {
            // Find player
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                Debug.LogWarning("PuzzleInteractable: No Player found! Make sure your player has the 'Player' tag.");
            }
            
            // Auto-create the prompt text above this object
            CreatePrompt();
            
            // Get audio source
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }
        }
        
        /// <summary>
        /// Creates a 3D text prompt above the puzzle table automatically.
        /// No need to manually create any UI — it builds itself.
        /// </summary>
        private void CreatePrompt()
        {
            promptObject = new GameObject("InteractionPrompt");
            promptObject.transform.SetParent(transform);
            promptObject.transform.localPosition = Vector3.up * promptHeight;
            
            promptMesh = promptObject.AddComponent<TextMesh>();
            promptMesh.text = promptText;
            promptMesh.fontSize = 100;
            promptMesh.characterSize = promptFontSize / 100f;
            promptMesh.alignment = TextAlignment.Center;
            promptMesh.color = promptColor;
            
            // Hidden until player is close
            promptObject.SetActive(false);
        }
        
        void Update()
        {
            if (playerTransform == null) return;
            
            // Distance check
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            playerInRange = distance <= interactionRange;
            
            // Play/stop sound based on playerInRange
            if (!previousPlayerInRange && playerInRange && enterRangeSound != null && audioSource != null)
            {
                audioSource.clip = enterRangeSound;
                audioSource.loop = true;
                audioSource.Play();
            }
            else if (previousPlayerInRange && !playerInRange && audioSource != null)
            {
                audioSource.Stop();
            }
            previousPlayerInRange = playerInRange;
            
            // Show/hide prompt
            if (promptObject != null)
            {
                bool shouldShow = playerInRange && (gameManager == null || !gameManager.IsPuzzleActive);
                promptObject.SetActive(shouldShow);
                
                // Make prompt always face the player (billboard)
                if (shouldShow)
                {
                    promptObject.transform.LookAt(playerTransform);
                    promptObject.transform.Rotate(0f, 180f, 0f);
                }
            }
            
            // Interact on key press
            if (playerInRange && Input.GetKeyDown(interactKey))
            {
                if (gameManager != null && !gameManager.IsPuzzleActive)
                {
                    gameManager.StartPuzzle(transform.position);
                    Debug.Log("Puzzle started!");
                }
            }
        }
        
        /// <summary>
        /// Debug: draw the interaction range sphere in the editor
        /// </summary>
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactionRange);
        }
    }
}
