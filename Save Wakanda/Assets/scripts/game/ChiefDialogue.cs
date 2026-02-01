using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SlidingPuzzle
{
    /// <summary>
    /// End-game dialogue screen. The chief speaks to the player after all ghosts are defeated.
    /// Has a "Start Over" button that restarts the whole game.
    /// 
    /// Setup:
    ///   1. Create a Canvas > Panel. Name it "ChiefPanel".
    ///   2. Inside ChiefPanel: add a Text (name it "DialogueText") and a Button (name it "StartOverButton").
    ///   3. Set ChiefPanel inactive in the Inspector.
    ///   4. Attach this script to any GameObject.
    ///   5. Drag ChiefPanel into the chiefPanel field.
    ///   6. Drag your PuzzleGameManager into the gameManager field.
    ///   The script finds dialogueText and startOverButton automatically inside the panel.
    /// </summary>
    public class ChiefDialogue : MonoBehaviour
    {
        [Header("UI References")]
        [Tooltip("The panel GameObject that contains the dialogue and button — set it inactive in the Inspector at start")]
        public GameObject chiefPanel;
        
        [Header("References")]
        [Tooltip("The game manager — used to call RestartGame()")]
        public PuzzleGameManager gameManager;
        
        [Header("Dialogue Settings")]
        [Tooltip("The full text the chief says. Use \\n for new lines.")]
        [TextArea(3, 10)]
        public string chiefSpeech = "Thank you, brave warrior.\n\nYou have freed our village from the spirits that haunted us.\n\nAs a reward, I offer you one of my wives.";
        
        [Tooltip("How long to wait before showing the dialogue (lets the last ghost finish)")]
        public float delayBeforeDialogue = 4f;
        
        [Tooltip("How long each character takes to appear (typewriter effect)")]
        public float typewriterSpeed = 0.03f;
        
        // Found automatically
        private Text dialogueText;
        private Button startOverButton;
        
        void Start()
        {
            if (chiefPanel == null)
            {
                Debug.LogError("ChiefDialogue: chiefPanel is not assigned in the Inspector!");
                return;
            }
            
            // Auto-find the Text and Button inside the panel
            // We temporarily activate the panel so GetComponentInChildren can find them
            bool wasActive = chiefPanel.activeSelf;
            chiefPanel.SetActive(true);
            
            dialogueText = chiefPanel.GetComponentInChildren<Text>();
            startOverButton = chiefPanel.GetComponentInChildren<Button>();
            
            // Now hide it again
            chiefPanel.SetActive(wasActive ? true : false);
            
            // Log what we found so you can check the Console
            Debug.Log($"ChiefDialogue: dialogueText found = {dialogueText != null}, startOverButton found = {startOverButton != null}");
            
            if (dialogueText == null)
            {
                Debug.LogError("ChiefDialogue: Could not find a Text component inside chiefPanel!");
            }
            
            if (startOverButton == null)
            {
                Debug.LogError("ChiefDialogue: Could not find a Button component inside chiefPanel!");
            }
            else
            {
                startOverButton.onClick.AddListener(OnStartOverClicked);
            }
            
            // Make sure panel is hidden at start
            chiefPanel.SetActive(false);
        }
        
        /// <summary>
        /// Called by PuzzleGameManager when all puzzles are completed.
        /// </summary>
        public void TriggerEndDialogue()
        {
            Debug.Log("ChiefDialogue: TriggerEndDialogue called!");
            StartCoroutine(ShowDialogue());
        }
        
        private IEnumerator ShowDialogue()
        {
            Debug.Log($"ChiefDialogue: waiting {delayBeforeDialogue}s before showing...");
            yield return new WaitForSeconds(delayBeforeDialogue);
            
            // Show the panel
            Debug.Log("ChiefDialogue: showing panel now.");
            chiefPanel.SetActive(true);
            
            // Typewriter effect
            if (dialogueText != null)
            {
                dialogueText.text = "";
                
                foreach (char c in chiefSpeech)
                {
                    dialogueText.text += c;
                    yield return new WaitForSeconds(typewriterSpeed);
                }
                
                Debug.Log("ChiefDialogue: typewriter finished.");
            }
            else
            {
                Debug.LogError("ChiefDialogue: dialogueText is null, can't type the speech!");
            }
            
            // Make sure cursor is visible so they can click Start Over
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            Debug.Log("ChiefDialogue: complete. Waiting for Start Over click.");
        }
        
        private void OnStartOverClicked()
        {
            Debug.Log("ChiefDialogue: Start Over clicked!");
            
            chiefPanel.SetActive(false);
            
            if (gameManager != null)
            {
                gameManager.RestartGame();
            }
            else
            {
                Debug.LogError("ChiefDialogue: gameManager is not assigned! Can't restart.");
            }
        }
    }
}